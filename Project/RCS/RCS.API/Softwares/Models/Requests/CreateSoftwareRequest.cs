namespace RCS.API.Softwares.Models.Requests;

public record CreateSoftwareRequest(string Name, string Description, string CurrentVersion, string Category, decimal Price);