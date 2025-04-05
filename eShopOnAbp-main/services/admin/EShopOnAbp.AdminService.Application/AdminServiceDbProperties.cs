namespace EShopOnAbp.AdminService
{
    public static class AdminServiceDbProperties
    {
        public static string DbTablePrefix { get; set; } = string.Empty;

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "AdminService";
    }
}
