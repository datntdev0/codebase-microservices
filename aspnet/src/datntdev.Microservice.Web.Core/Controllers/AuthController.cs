﻿using datntdev.Abp.Authorization;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Configuration;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Runtime.Security;
using datntdev.Abp.Zero.Configuration;
using datntdev.Microservice.Authentication.JwtBearer;
using datntdev.Microservice.Authorization;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.Identity;
using datntdev.Microservice.Models.Auth;
using datntdev.Microservice.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace datntdev.Microservice.Controllers
{
    [Route("api/auth")]
    public class AuthController(
        LogInManager logInManager,
        ITenantCache tenantCache,
        AbpLoginResultTypeHelper abpLoginResultTypeHelper,
        TokenAuthConfiguration configuration,
        UserRegistrationManager userRegistrationManager,
        TenantManager tenantManager
    ) : MicroserviceControllerBase
    {
        private readonly LogInManager _logInManager = logInManager;
        private readonly ITenantCache _tenantCache = tenantCache;
        private readonly TenantManager _tenantManager = tenantManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration = configuration;
        private readonly UserRegistrationManager _userRegistrationManager = userRegistrationManager;

        [HttpGet("tenant-status")]
        public async Task<GetTenantStatusOutput> GetTenantStatusAsync([FromQuery] string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            if (tenant == null)
            {
                return new GetTenantStatusOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new GetTenantStatusOutput(TenantAvailabilityState.InActive);
            }

            return new GetTenantStatusOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        [HttpPost("login")]
        public async Task<LoginOutput> LoginAsync([FromBody] LoginInput model)
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            return new LoginOutput
            {
                AccessToken = accessToken,
                EncryptedAccessToken = SimpleStringCipher.Instance.Encrypt(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id
            };
        }

        [HttpPost("register")]
        public async Task<RegisterOutput> RegisterAsync([FromBody] RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue) return null;

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(
            string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(
            [
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            ]);

            return claims;
        }
    }
}
