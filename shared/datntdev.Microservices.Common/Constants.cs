namespace datntdev.Microservices.Common
{
    public static class Constants
    {
        public static class Application
        {
            public const string Name = "datntdev Microservices";
            public const string Version = "1.0.0";
            public const string DefaultTheme = "light";
            public const string AuthenticationScheme = "Cookies";
        }

        public static class MultiTenancy
        {
            public const int DefaultTenantId = 0;
            public const string DefaultTenantName = "Default";
        }

        public static class Authorization
        {
            public const string DefaultAdminRole = "Administrator";
        }

        public static class Endpoints
        {
            public const string Health = "/health";
            public const string Liveness = "/alive";
            public const string OAuth2Token = "/connect/token";
            public const string OAuth2Auth = "/connect/authorize";
            public const string OAuth2Logout = "/connect/logout";
            public const string AuthSignIn = "/auth/signin";
            public const string AuthSignUp = "/auth/signup";
        }

        public static class Enum
        {
            public enum ServiceType
            {
                Default,
                Gateway,
                Migrator,
                Microservice,
            }

            public enum AppPermission
            {
                None = 0,
            }
        }
    }
}
