using MediatR;
using Animals.API.Configuration;
using CSharpFunctionalExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Animals.API.Animals.Commands;

public record DeleteAnimalCommand(int IdAnimal) : IRequest<Result>;

public class DeleteAnimalCommandHandler(IOptions<AppConfig> config)
    : IRequestHandler<DeleteAnimalCommand, Result>
{
    private readonly AppConfig _config = config.Value;
    public async Task<Result> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_config.SQLServerConnectionString);
        await con.OpenAsync(cancellationToken);

        var sqlCmd = "DELETE FROM [dbo].[Animals] WHERE IdAnimal = @IdAnimal";
        await using var cmd = new SqlCommand(sqlCmd, con);
        cmd.Parameters.Add(new SqlParameter("@IdAnimal", request.IdAnimal));

        var rowsAffected = await cmd.ExecuteNonQueryAsync(cancellationToken);
        // No rows affected means no animal was found with the given ID
        return rowsAffected == 0 
            ? Result.Failure($"No animal found with ID {request.IdAnimal}") 
            : Result.Success();
    }
}