using Animals.API.Animals.Commands;
using Animals.API.Animals.Models;
using Animals.API.Common.Exceptions;
using Animals.API.Configuration;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Animals.API.Animals.Queries;

public record GetAnimalByIdQuery(int Id)  : IRequest<Result<Animal, ValidationFailed>>;

public class GetAnimalByIdHandler(IOptions<AppConfig> config)
    : IRequestHandler<GetAnimalByIdQuery, Result<Animal, ValidationFailed>>
{
    private readonly AppConfig _config = config.Value;
    public async Task<Result<Animal, ValidationFailed>> Handle(GetAnimalByIdQuery request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_config.SQLServerConnectionString);
        await con.OpenAsync(cancellationToken);

        var sqlCmd = "SELECT * FROM [dbo].[Animals] WHERE IdAnimal = @IdAnimal";
        await using var cmd = new SqlCommand(sqlCmd, con);
        cmd.Parameters.Add(new SqlParameter("@IdAnimal", request.Id));

        Animal animal = null;
        var dr = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await dr.ReadAsync(cancellationToken))
        {
            animal = new Animal
            {
                IdAnimal = dr.GetInt32(dr.GetOrdinal("IdAnimal")),
                Name = dr.GetString(dr.GetOrdinal("Name")),
                Description = dr.IsDBNull(dr.GetOrdinal("Description")) ? null : dr.GetString(dr.GetOrdinal("Description")),
                Category = dr.GetString(dr.GetOrdinal("Category")),
                Area = dr.GetString(dr.GetOrdinal("Area"))
            };
        }
        
        return animal is null 
            ? Result.Failure<Animal, ValidationFailed>(new ValidationFailed("Animal not found")) 
            : Result.Success<Animal, ValidationFailed>(animal);
    }
}