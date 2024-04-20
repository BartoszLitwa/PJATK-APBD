using System.ComponentModel.DataAnnotations;
using Animals.API.Animals.Models;
using Animals.API.Common.Exceptions;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;

namespace Animals.API.Animals.Queries;

public record GetAllAnimalsQuery(
    [AllowedValues("name", "description", "category", "area")] string OrderBy) : IRequest<Result<IEnumerable<Animal>, ValidationFailed>>;

public class GetAllAnimalsHandler()
    : IRequestHandler<GetAllAnimalsQuery, Result<IEnumerable<Animal>, ValidationFailed>>
{
    public async Task<Result<IEnumerable<Animal>, ValidationFailed>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection();
        await con.OpenAsync(cancellationToken);

        var sqlCmd = "SELECT * FROM [dbo].[Animals] ORDER BY @OrderBy ASC";
        await using var cmd = new SqlCommand(sqlCmd, con);
        cmd.Parameters.Add(new SqlParameter("@OrderBy", request.OrderBy ?? "name"));

        var animals = new List<Animal>();
        var dr = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await dr.ReadAsync(cancellationToken))
        {
            animals.Add(new Animal
            {
                Id = dr.GetInt32(dr.GetOrdinal("Id")),
                Name = dr.GetString(dr.GetOrdinal("Name")),
                Description = dr.IsDBNull(dr.GetOrdinal("Description")) ? null : dr.GetString(dr.GetOrdinal("Description")),
                Category = dr.GetString(dr.GetOrdinal("Category")),
                Area = dr.GetString(dr.GetOrdinal("Area"))
            });
        }
        
        return Result.Success<IEnumerable<Animal>, ValidationFailed>(animals);
    }
}