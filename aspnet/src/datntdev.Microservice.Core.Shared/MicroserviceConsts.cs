namespace datntdev.Microservice;

public class MicroserviceConsts
{
    public const string LocalizationSourceName = "Microservice";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;

    /// <summary>
    /// Gets current version of the application.
    /// It's also shown in the web page.
    /// </summary>
    public const string Version = "9.3.0";

    /// <summary>
    /// Gets release (last build) date of the application.
    /// It's shown in the web page.
    /// </summary>
    public static DateTime ReleaseDate => new(2025, 1, 1);

    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase = "fdb9f1f6318c489fb98280ff2c52bf5b";
}
