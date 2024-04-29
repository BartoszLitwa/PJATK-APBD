namespace Warehouse.API;

public class AppConfig
{
    public const string Section = "AppConfig";
    
    public string ConnectionString { get; set; }
    public string ProcedureName { get; set; }
}