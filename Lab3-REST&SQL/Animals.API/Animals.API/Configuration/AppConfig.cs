namespace Animals.API.Configuration;

public class AppConfig
{
    public const string Section = "AppConfig";

    public string SQLServerConnectionString { get; set; }
}