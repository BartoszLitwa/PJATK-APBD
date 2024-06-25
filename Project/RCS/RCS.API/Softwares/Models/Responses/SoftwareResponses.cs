namespace RCS.API.Softwares.Models.Responses;

public record SoftwareResponse(int Id, string Name, string Description, string CurrentVersion, string Category, decimal Price);
