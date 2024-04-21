using Animals.API.Animals.Models;
using Animals.API.Common.Exceptions;
using Animals.API.Configuration;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Animals.API.Animals.Queries;

public record GetAllAnimalsQuery(string? OrderBy) : IRequest<Result<IEnumerable<Animal>, ValidationFailed>>;

public class GetAllAnimalsHandler(IOptions<AppConfig> config)
    : IRequestHandler<GetAllAnimalsQuery, Result<IEnumerable<Animal>, ValidationFailed>>
{
    private readonly AppConfig _config = config.Value;
    private readonly string[] _allowedOrderByValues = ["name", "description", "category", "area"];

    public async Task<Result<IEnumerable<Animal>, ValidationFailed>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
    {
        if(request.OrderBy is not null && !_allowedOrderByValues.Contains(request.OrderBy))
            return Result.Failure<IEnumerable<Animal>, ValidationFailed>(new ValidationFailed("Invalid OrderBy value"));
        
        await using var con = new SqlConnection(_config.SQLServerConnectionString);
        await con.OpenAsync(cancellationToken);

        var orderBy = (request.OrderBy ?? "name");
        orderBy = char.ToUpper(orderBy[0]) + orderBy[1..];
        // SQL injection vulnerability - SQL Parameter does not allow it
        var sqlCmd = $"SELECT * FROM [dbo].[Animals] ORDER BY {orderBy} ASC";
        await using var cmd = new SqlCommand(sqlCmd, con);

        var animals = new List<Animal>();
        var dr = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await dr.ReadAsync(cancellationToken))
        {
            animals.Add(new Animal
            {
                IdAnimal = dr.GetInt32(dr.GetOrdinal("IdAnimal")),
                Name = dr.GetString(dr.GetOrdinal("Name")),
                Description = dr.IsDBNull(dr.GetOrdinal("Description")) ? null : dr.GetString(dr.GetOrdinal("Description")),
                Category = dr.GetString(dr.GetOrdinal("Category")),
                Area = dr.GetString(dr.GetOrdinal("Area"))
            });
        }
        
        return Result.Success<IEnumerable<Animal>, ValidationFailed>(animals);
    }
}