using System;
using System.Linq;
using System.Threading.Tasks;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Dependency;
using datntdev.Abp.Localization;
using datntdev.Abp.Threading;

namespace datntdev.Abp.Authorization
{
    /// <summary>
    /// Extension methods for <see cref="IPermissionChecker"/>
    /// </summary>
    public static class PermissionCheckerExtensions
    {
        /// <summary>
        /// Checks if given user is granted for given permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="user">User</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more.</param>
        /// <param name="permissionNames">Name of the permissions</param>
        public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, UserIdentifier user, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(await permissionChecker.IsGrantedAsync(user, permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (await permissionChecker.IsGrantedAsync(user, permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if given user is granted for given permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="user">User</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more.</param>
        /// <param name="permissionNames">Name of the permissions</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, UserIdentifier user, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(permissionChecker.IsGranted(user, permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (permissionChecker.IsGranted(user, permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        
        /// <summary>
        /// Checks if current user is granted for given permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more.</param>
        /// <param name="permissionNames">Name of the permissions</param>
        public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(await permissionChecker.IsGrantedAsync(permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (await permissionChecker.IsGrantedAsync(permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if current user is granted for given permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more.</param>
        /// <param name="permissionNames">Name of the permissions</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(permissionChecker.IsGranted(permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (permissionChecker.IsGranted(permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        
        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// User it authorized if any of the <paramref name="permissionNames"/> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static Task AuthorizeAsync(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            return AuthorizeAsync(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// User it authorized if any of the <paramref name="permissionNames"/> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static void Authorize(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            Authorize(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <paramref name="permissionNames"/> must be granted.
        /// If it's false, at least one of the <paramref name="permissionNames"/> must be granted.
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static async Task AuthorizeAsync(this IPermissionChecker permissionChecker, bool requireAll, params string[] permissionNames)
        {
            if (await IsGrantedAsync(permissionChecker, requireAll, permissionNames))
            {
                return;
            }

            var localizedPermissionNames = LocalizePermissionNames(permissionChecker, permissionNames);

            if (requireAll)
            {
                throw new AbpAuthorizationException(
                    string.Format(
                        L(
                            permissionChecker,
                            "AllOfThesePermissionsMustBeGranted",
                            "Required permissions are not granted. All of these permissions must be granted: {0}"
                        ),
                        string.Join(", ", localizedPermissionNames)
                    )
                );
            }
            else
            {
                throw new AbpAuthorizationException(
                    string.Format(
                        L(
                            permissionChecker,
                            "AtLeastOneOfThesePermissionsMustBeGranted",
                            "Required permissions are not granted. At least one of these permissions must be granted: {0}"
                        ),
                        string.Join(", ", localizedPermissionNames)
                    )
                );
            }
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <paramref name="permissionNames"/> must be granted.
        /// If it's false, at least one of the <paramref name="permissionNames"/> must be granted.
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static void Authorize(this IPermissionChecker permissionChecker, bool requireAll, params string[] permissionNames)
        {
            if (IsGranted(permissionChecker, requireAll, permissionNames))
            {
                return;
            }

            var localizedPermissionNames = LocalizePermissionNames(permissionChecker, permissionNames);

            if (requireAll)
            {
                throw new AbpAuthorizationException(
                    string.Format(
                        L(
                            permissionChecker,
                            "AllOfThesePermissionsMustBeGranted",
                            "Required permissions are not granted. All of these permissions must be granted: {0}"
                        ),
                        string.Join(", ", localizedPermissionNames)
                    )
                );
            }
            else
            {
                throw new AbpAuthorizationException(
                    string.Format(
                        L(
                            permissionChecker,
                            "AtLeastOneOfThesePermissionsMustBeGranted",
                            "Required permissions are not granted. At least one of these permissions must be granted: {0}"
                        ),
                        string.Join(", ", localizedPermissionNames)
                    )
                );
            }
        }

        public static string L(IPermissionChecker permissionChecker, string name, string defaultValue)
        {
            if (!(permissionChecker is IIocManagerAccessor))
            {
                return defaultValue;
            }

            var iocManager = (permissionChecker as IIocManagerAccessor).IocManager;
            using (var localizationManager = iocManager.ResolveAsDisposable<ILocalizationManager>())
            {
                return localizationManager.Object.GetString(AbpConsts.LocalizationSourceName, name);
            }
        }

        public static string[] LocalizePermissionNames(IPermissionChecker permissionChecker, string[] permissionNames)
        {
            if (!(permissionChecker is IIocManagerAccessor))
            {
                return permissionNames;
            }

            var iocManager = (permissionChecker as IIocManagerAccessor).IocManager;
            using (var localizationContext = iocManager.ResolveAsDisposable<ILocalizationContext>())
            {
                using (var permissionManager = iocManager.ResolveAsDisposable<IPermissionManager>())
                {
                    return permissionNames.Select(permissionName =>
                    {
                        var permission = permissionManager.Object.GetPermissionOrNull(permissionName);
                        return permission?.DisplayName == null
                            ? permissionName
                            : permission.DisplayName.Localize(localizationContext.Object);
                    }).ToArray();
                }
            }
        }
    }
}