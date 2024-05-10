using System.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Warehouse.API.Warehouse.Commands;

public class AddProductToWarehouseInSQLProcedureCommand : IRequest<Result<int>>
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddProductToWarehouseInSQLProcedureHandler(IOptions<AppConfig> options) : IRequestHandler<AddProductToWarehouseInSQLProcedureCommand, Result<int>>
{
    private readonly AppConfig _appConfig = options.Value;
    
    public async Task<Result<int>> Handle(AddProductToWarehouseInSQLProcedureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new SqlConnection(_appConfig.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            var command = new SqlCommand(_appConfig.ProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
            command.Parameters.AddWithValue("@Amount", request.Amount);
            command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

            var result = (int)await command.ExecuteScalarAsync(cancellationToken);

            return result >= 0 
                ? Result.Success(result) 
                : Result.Failure<int>($"Failed with error code: {result}");
        }
        catch (Exception ex)
        {
            return Result.Failure<int>($"Exception: {ex.Message}");
        }
    }
}