using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using datntdev.Abp.Extensions;
using datntdev.Abp.Runtime.Session;
using datntdev.Abp.Utils.Etc;
using Castle.Core;

namespace datntdev.Abp.Domain.Uow
{
    /// <summary>
    /// Base for all Unit Of Work classes.
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public string Id { get; }

        [DoNotWire]
        public IUnitOfWork Outer { get; set; }

        /// <inheritdoc/>
        public event EventHandler Completed;

        /// <inheritdoc/>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <inheritdoc/>
        public event EventHandler Disposed;

        /// <inheritdoc/>
        public UnitOfWorkOptions Options { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get { return _filters.ToImmutableList(); }
        }
        private readonly List<DataFilterConfiguration> _filters;
        
        /// <inheritdoc/>
        public IReadOnlyList<AuditFieldConfiguration> AuditFieldConfiguration
        {
            get { return _auditFieldConfiguration.ToImmutableList(); }
        }
        private readonly List<AuditFieldConfiguration> _auditFieldConfiguration;

        public Dictionary<string, object> Items { get; set; }

        /// <summary>
        /// Gets default UOW options.
        /// </summary>
        protected IUnitOfWorkDefaultOptions DefaultOptions { get; }

        /// <summary>
        /// Gets the connection string resolver.
        /// </summary>
        protected IConnectionStringResolver ConnectionStringResolver { get; }

        /// <summary>
        /// Gets a value indicates that this unit of work is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Reference to current ABP session.
        /// </summary>
        public IAbpSession AbpSession { protected get; set; }

        protected IUnitOfWorkFilterExecuter FilterExecuter { get; }

        /// <summary>
        /// Is <see cref="Begin"/> method called before?
        /// </summary>
        private bool _isBeginCalledBefore;

        /// <summary>
        /// Is <see cref="Complete"/> method called before?
        /// </summary>
        private bool _isCompleteCalledBefore;

        /// <summary>
        /// Is this unit of work successfully completed.
        /// </summary>
        private bool _succeed;

        /// <summary>
        /// A reference to the exception if this unit of work failed.
        /// </summary>
        private Exception _exception;

        private int? _tenantId;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected UnitOfWorkBase(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter)
        {
            FilterExecuter = filterExecuter;
            DefaultOptions = defaultOptions;
            ConnectionStringResolver = connectionStringResolver;

            Id = Guid.NewGuid().ToString("N");
            _filters = defaultOptions.Filters.ToList();
            _auditFieldConfiguration = defaultOptions.AuditFieldConfiguration.ToList();
            AbpSession = NullAbpSession.Instance;
            Items = new Dictionary<string, object>();
        }

        /// <inheritdoc/>
        public void Begin(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            PreventMultipleBegin();
            Options = options; //TODO: Do not set options like that, instead make a copy?

            SetFilters(options.FilterOverrides);

            SetTenantId(AbpSession.TenantId, false);

            BeginUow();
        }

        /// <inheritdoc/>
        public abstract void SaveChanges();

        /// <inheritdoc/>
        public abstract Task SaveChangesAsync();

        /// <inheritdoc/>
        public IDisposable DisableFilter(params string[] filterNames)
        {
            //TODO: Check if filters exists?

            var disabledFilters = new List<string>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = GetFilterIndex(filterName);
                if (_filters[filterIndex].IsEnabled)
                {
                    disabledFilters.Add(filterName);
                    _filters[filterIndex] = new DataFilterConfiguration(_filters[filterIndex], false);
                }
            }

            disabledFilters.ForEach(ApplyDisableFilter);

            return new DisposeAction(() => EnableFilter(disabledFilters.ToArray()));
        }

        /// <inheritdoc/>
        public IDisposable EnableFilter(params string[] filterNames)
        {
            //TODO: Check if filters exists?

            var enabledFilters = new List<string>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = GetFilterIndex(filterName);
                if (!_filters[filterIndex].IsEnabled)
                {
                    enabledFilters.Add(filterName);
                    _filters[filterIndex] = new DataFilterConfiguration(_filters[filterIndex], true);
                }
            }

            enabledFilters.ForEach(ApplyEnableFilter);

            return new DisposeAction(() => DisableFilter(enabledFilters.ToArray()));
        }
        
        /// <inheritdoc/>
        public IDisposable DisableAuditing(params string[] fieldNames)
        {
            var disabledAuditFields = new List<string>();

            foreach (var fieldName in fieldNames)
            {
                var fieldIndex = GetAuditFieldIndex(fieldName);
                if (_auditFieldConfiguration[fieldIndex].IsSavingEnabled)
                {
                    disabledAuditFields.Add(fieldName);
                    _auditFieldConfiguration[fieldIndex] = new AuditFieldConfiguration(_auditFieldConfiguration[fieldIndex].FieldName, false);
                }
            }
            
            return new DisposeAction(() => EnableAuditing(disabledAuditFields.ToArray()));
        }

        /// <inheritdoc/>
        public IDisposable EnableAuditing(params string[] fieldNames)
        {
            var enabledAuditFields = new List<string>();

            foreach (var fieldName in fieldNames)
            {
                var fieldIndex = GetAuditFieldIndex(fieldName);
                if (!_auditFieldConfiguration[fieldIndex].IsSavingEnabled)
                {
                    enabledAuditFields.Add(fieldName);
                    _auditFieldConfiguration[fieldIndex] = new AuditFieldConfiguration(_auditFieldConfiguration[fieldIndex].FieldName, true);
                }
            }

            return new DisposeAction(() => DisableAuditing(enabledAuditFields.ToArray()));
        }

        /// <inheritdoc/>
        public bool IsFilterEnabled(string filterName)
        {
            return GetFilter(filterName).IsEnabled;
        }

        /// <inheritdoc/>
        public IDisposable SetFilterParameter(string filterName, string parameterName, object value)
        {
            var filterIndex = GetFilterIndex(filterName);

            var newfilter = new DataFilterConfiguration(_filters[filterIndex]);

            //Store old value
            object oldValue = null;
            var hasOldValue = newfilter.FilterParameters.ContainsKey(parameterName);
            if (hasOldValue)
            {
                oldValue = newfilter.FilterParameters[parameterName];
            }

            newfilter.FilterParameters[parameterName] = value;

            _filters[filterIndex] = newfilter;

            ApplyFilterParameterValue(filterName, parameterName, value);

            return new DisposeAction(() =>
            {
                //Restore old value
                if (hasOldValue)
                {
                    SetFilterParameter(filterName, parameterName, oldValue);
                }
                else
                {
                    Completed += (s, e) => _filters[filterIndex].FilterParameters[parameterName] = default;
                }
            });
        }

        public virtual IDisposable SetTenantId(int? tenantId)
        {
            return SetTenantId(tenantId, true);
        }

        public virtual IDisposable SetTenantId(int? tenantId, bool switchMustHaveTenantEnableDisable)
        {
            var oldTenantId = _tenantId;
            _tenantId = tenantId;


            IDisposable mustHaveTenantEnableChange;
            if (switchMustHaveTenantEnableDisable)
            {
                mustHaveTenantEnableChange = tenantId == null
                    ? DisableFilter(AbpDataFilters.MustHaveTenant)
                    : EnableFilter(AbpDataFilters.MustHaveTenant);
            }
            else
            {
                mustHaveTenantEnableChange = NullDisposable.Instance;
            }

            var mayHaveTenantChange = SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId);
            var mustHaveTenantChange = SetFilterParameter(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId ?? 0);

            return new DisposeAction(() =>
            {
                mayHaveTenantChange.Dispose();
                mustHaveTenantChange.Dispose();
                mustHaveTenantEnableChange.Dispose();
                _tenantId = oldTenantId;
            });
        }

        public int? GetTenantId()
        {
            return _tenantId;
        }

        /// <inheritdoc/>
        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task CompleteAsync()
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_isBeginCalledBefore || IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected virtual void BeginUow()
        {

        }

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();

        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();

        protected virtual void ApplyDisableFilter(string filterName)
        {
            FilterExecuter.ApplyDisableFilter(this, filterName);
        }

        protected virtual void ApplyEnableFilter(string filterName)
        {
            FilterExecuter.ApplyEnableFilter(this, filterName);
        }

        protected virtual void ApplyFilterParameterValue(string filterName, string parameterName, object value)
        {
            FilterExecuter.ApplyFilterParameterValue(this, filterName, parameterName, value);
        }

        protected virtual string ResolveConnectionString(ConnectionStringResolveArgs args)
        {
            return ConnectionStringResolver.GetNameOrConnectionString(args);
        }

        protected virtual async Task<string> ResolveConnectionStringAsync(ConnectionStringResolveArgs args)
        {
            return await ConnectionStringResolver.GetNameOrConnectionStringAsync(args);
        }

        /// <summary>
        /// Called to trigger <see cref="Completed"/> event.
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        /// <summary>
        /// Called to trigger <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new AbpException("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new AbpException("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        private void SetFilters(List<DataFilterConfiguration> filterOverrides)
        {
            for (var i = 0; i < _filters.Count; i++)
            {
                var filterOverride = filterOverrides.FirstOrDefault(f => f.FilterName == _filters[i].FilterName);
                if (filterOverride != null)
                {
                    _filters[i] = filterOverride;
                }
            }

            if (AbpSession.TenantId == null)
            {
                ChangeFilterIsEnabledIfNotOverrided(filterOverrides, AbpDataFilters.MustHaveTenant, false);
            }
        }

        private void ChangeFilterIsEnabledIfNotOverrided(List<DataFilterConfiguration> filterOverrides, string filterName, bool isEnabled)
        {
            if (filterOverrides.Any(f => f.FilterName == filterName))
            {
                return;
            }

            var index = _filters.FindIndex(f => f.FilterName == filterName);
            if (index < 0)
            {
                return;
            }

            if (_filters[index].IsEnabled == isEnabled)
            {
                return;
            }

            _filters[index] = new DataFilterConfiguration(filterName, isEnabled);
        }

        private DataFilterConfiguration GetFilter(string filterName)
        {
            var filter = _filters.FirstOrDefault(f => f.FilterName == filterName);
            if (filter == null)
            {
                throw new AbpException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filter;
        }

        private int GetFilterIndex(string filterName)
        {
            var filterIndex = _filters.FindIndex(f => f.FilterName == filterName);
            if (filterIndex < 0)
            {
                throw new AbpException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filterIndex;
        }
        
        private int GetAuditFieldIndex(string filterName)
        {
            var filterIndex = _auditFieldConfiguration.FindIndex(f => f.FieldName == filterName);
            if (filterIndex < 0)
            {
                throw new AbpException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filterIndex;
        }

        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}
