﻿using System;
using datntdev.Abp.Authorization.Users;

namespace datntdev.Abp.Runtime.Session
{
    public static class AbpSessionExtensions
    {
        public static bool IsUser(this IAbpSession session, AbpUserBase user)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return session.TenantId == user.TenantId && 
                session.UserId.HasValue && 
                session.UserId.Value == user.Id;
        }
    }
}
