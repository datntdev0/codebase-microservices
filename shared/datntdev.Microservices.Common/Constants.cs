namespace datntdev.Microservices.Common
{
    public static class Constants
    {
        public static class Application
        {
            public const string Name = "datntdev Microservices";
            public const string Version = "1.0.0";
        }
        public static class Endpoints
        {
            public const string Health = "/health";
            public const string Liveness = "/alive";
        }
    }
}
