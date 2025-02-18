using datntdev.Microservice.Debugging;

namespace datntdev.Microservice;

public class MicroserviceConsts
{
    public const string LocalizationSourceName = "Microservice";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "fdb9f1f6318c489fb98280ff2c52bf5b";
}
