using System.ComponentModel.DataAnnotations;
using MediatR;
using Animals.API.Configuration;
using CSharpFunctionalExtensions;
using Animals.API.Common.Exceptions;
using Microsoft.Data.SqlClient;
using Animals.API.Animals.Models;
using Microsoft.Extensions.Options;

namespace Animals.API.Animals.Commands;

public record CreateAnimalRequestDto(
    [MaxLength(200)] string Name, 
    [MaxLength(200)] string? Description, 
    [MaxLength(200)] string Category, 
    [MaxLength(200)] string Area);

public record CreateAnimalCommand(string Name, string? Description, string Category, string Area) 
    : IRequest<Result<Animal, ValidationFailed>>;

public class CreateAnimalCommandHandler(IOptions<AppConfig> config)
    : IRequestHandler<CreateAnimalCommand, Result<Animal, ValidationFailed>>
{
    private readonly AppConfig _config = config.Value;
    public async Task<Result<Animal, ValidationFailed>> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_config.SQLServerConnectionString);
        await con.OpenAsync(cancellationToken);
    
        var sqlCmd = "INSERT INTO [dbo].[Animals] (Name, Description, Category, Area) " +
                     "VALUES (@Name, @Description, @Category, @Area);" +
                     "SELECT SCOPE_IDENTITY();";
        await using var cmd = new SqlCommand(sqlCmd, con);
        cmd.Parameters.Add(new SqlParameter("@Name", request.Name));
        cmd.Parameters.Add(new SqlParameter("@Description", request.Description ?? (object)DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@Category", request.Category));
        cmd.Parameters.Add(new SqlParameter("@Area", request.Area));
    
        // Execute the command and retrieve the newly generated ID
        var id = await cmd.ExecuteScalarAsync(cancellationToken);
        if (id != null && id != DBNull.Value)
        {
            return Result.Success<Animal, ValidationFailed>(new Animal
            {
                IdAnimal = Convert.ToInt32(id),
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Area = request.Area
            });
        }
    
        return Result.Failure<Animal, ValidationFailed>(new ValidationFailed("Failed to create animal."));
    }
}