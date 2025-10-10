namespace datntdev.Microservices.Common
{
    public static class Constants
    {
        public static class Application
        {
            public const string Name = "datntdev Microservices";
            public const string Version = "1.0.0";
        }

        public static class MultiTenancy
        {
            public const int DefaultTenantId = 0;
            public const string DefaultTenantName = "Default";
        }

        public static class Endpoints
        {
            public const string Health = "/health";
            public const string Liveness = "/alive";
            public const string OAuth2Token = "/connect/token";
            public const string OAuth2Auth = "/connect/authorize";
        }
    }
}
