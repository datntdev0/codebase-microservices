﻿using System.Threading.Tasks;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;

namespace datntdev.Abp.Auditing
{
    /// <summary>
    /// Implements <see cref="IAuditingStore"/> to save auditing informations to database.
    /// </summary>
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;

        /// <summary>
        /// Creates  a new <see cref="AuditingStore"/>.
        /// </summary>
        public AuditingStore(IRepository<AuditLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public virtual Task SaveAsync(AuditInfo auditInfo)
        {
            return _auditLogRepository.InsertAsync(AuditLog.CreateFromAuditInfo(auditInfo));
        }

        public virtual void Save(AuditInfo auditInfo)
        {
            _auditLogRepository.Insert(AuditLog.CreateFromAuditInfo(auditInfo));
        }
    }
}