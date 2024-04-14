using Animals.API.Animals.Models;
using Animals.API.Common.Exceptions;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;

namespace Animals.API.Animals.Queries;

public record GetAllAnimalsQuery() : IRequest<Result<IEnumerable<Animal>, ValidationFailed>>;

public class GetAllAnimalsHandler()
    : IRequestHandler<GetAllAnimalsQuery, Result<IEnumerable<Animal>, ValidationFailed>>
{
    public async Task<Result<IEnumerable<Animal>, ValidationFailed>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection();
        await con.OpenAsync(cancellationToken);

        var sqlCmd = "";
        await using var cmd = new SqlCommand(sqlCmd, con);

        var animals = new List<Animal>();
        var dr = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await dr.ReadAsync(cancellationToken))
        {
            animals.Add(new Animal()
            {

            });
        }
        
        return Result.Success<IEnumerable<Animal>, ValidationFailed>(animals);
    }
}