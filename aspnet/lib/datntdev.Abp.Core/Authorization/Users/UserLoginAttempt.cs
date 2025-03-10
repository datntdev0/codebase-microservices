﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using datntdev.Abp.Domain.Entities;
using datntdev.Abp.Domain.Entities.Auditing;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Timing;

namespace datntdev.Abp.Authorization.Users
{
    /// <summary>
    /// Used to save a login attempt of a user.
    /// </summary>
    [Table("AbpUserLoginAttempts")]
    public class UserLoginAttempt : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        /// <summary>
        /// Max length of the <see cref="TenancyName"/> property.
        /// </summary>
        public const int MaxTenancyNameLength = AbpTenantBase.MaxTenancyNameLength;

        /// <summary>
        /// Max length of the <see cref="TenancyName"/> property.
        /// </summary>
        public const int MaxUserNameOrEmailAddressLength = AbpUserBase.MaxEmailAddressLength;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 512;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public const int MaxFailReasonLength = 1024;

        /// <summary>
        /// Tenant's Id, if <see cref="TenancyName"/> was a valid tenant name.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Tenancy name.
        /// </summary>
        [StringLength(MaxTenancyNameLength)]
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// User's Id, if <see cref="UserNameOrEmailAddress"/> was a valid username or email address.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// User name or email address
        /// </summary>
        [StringLength(MaxUserNameOrEmailAddressLength)]
        public virtual string UserNameOrEmailAddress { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        [StringLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        [StringLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// </summary>
        [StringLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// Login attempt result.
        /// </summary>
        public virtual AbpLoginResultType Result { get; set; }

        [StringLength(MaxFailReasonLength)] public virtual string FailReason { get; set; }

        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginAttempt"/> class.
        /// </summary>
        public UserLoginAttempt()
        {
            CreationTime = Clock.Now;
        }
    }
}