namespace RCS.API.Auth.Models.Requests;

public record CreateEmployeeRequest(string Login, string Password, bool IsAdmin);