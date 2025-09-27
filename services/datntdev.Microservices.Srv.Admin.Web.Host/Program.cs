namespace datntdev.Microservices.Srv.Admin.Web.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        var app = builder.Build();
        app.MapDefaultEndpoints();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
