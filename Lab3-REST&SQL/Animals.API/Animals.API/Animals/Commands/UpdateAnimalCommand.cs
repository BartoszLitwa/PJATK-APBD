using System.ComponentModel.DataAnnotations;
using Animals.API.Configuration;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Animals.API.Animals.Commands;

public record UpdateAnimalRequestDto(
    [MaxLength(200)] string Name,
    [MaxLength(200)] string? Description,
    [MaxLength(200)] string Category,
    [MaxLength(200)] string Area);

public record UpdateAnimalCommand(
    int IdAnimal,
    UpdateAnimalRequestDto Dto) : IRequest<Result>;

public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, Result>
{
    private readonly AppConfig _config;

    public UpdateAnimalCommandHandler(IOptions<AppConfig> config)
    {
        _config = config.Value;
    }

    public async Task<Result> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_config.SQLServerConnectionString);
        await con.OpenAsync(cancellationToken);

        var sqlCmd = "UPDATE [dbo].[Animals] SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
        await using var cmd = new SqlCommand(sqlCmd, con);
        cmd.Parameters.Add(new SqlParameter("@IdAnimal", request.IdAnimal));
        cmd.Parameters.Add(new SqlParameter("@Name", request.Dto.Name));
        cmd.Parameters.Add(new SqlParameter("@Description", request.Dto.Description ?? (object)DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@Category", request.Dto.Category));
        cmd.Parameters.Add(new SqlParameter("@Area", request.Dto.Area));

        var affectedRows = await cmd.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 0 
            ? Result.Failure("No animal was updated; the specified ID may not exist.")
            : Result.Success();
    }
}

