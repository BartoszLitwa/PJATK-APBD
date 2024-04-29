using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Warehouse.API.Warehouse.Commands;

public class AddProductToWarehouseInSQLProcedureCommand : IRequest<Result<int>>
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddProductToWarehouseInSQLProcedureHandler(IOptions<AppConfig> options) : IRequestHandler<AddProductToWarehouseInSQLProcedureCommand, Result<int>>
{
    private readonly AppConfig _appConfig = options.Value;
    
    public async Task<Result<int>> Handle(AddProductToWarehouseInSQLProcedureCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            return Result.Failure<int>("Amount must be greater than 0");
        
        await using var connection = new SqlConnection(_appConfig.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            // Check if product exists
            var productCheckCmd = new SqlCommand("SELECT COUNT(*) FROM Product WHERE ID = @ProductId", connection, transaction);
            productCheckCmd.Parameters.AddWithValue("@ProductId", request.ProductId);
                
            if ((int)await productCheckCmd.ExecuteScalarAsync(cancellationToken) == 0)
                return Result.Failure<int>("Product does not exist");

            // Check if warehouse exists
            var warehouseCheckCmd = new SqlCommand("SELECT COUNT(*) FROM Warehouse WHERE ID = @WarehouseId", connection, transaction);
            warehouseCheckCmd.Parameters.AddWithValue("@WarehouseId", request.WarehouseId);
                
            if ((int)await warehouseCheckCmd.ExecuteScalarAsync(cancellationToken) == 0)
                return Result.Failure<int>("Warehouse does not exist");

            // Check for a valid order
            var orderCheckCmd = new SqlCommand("SELECT ID, Price FROM [Order] WHERE ProductId = @ProductId AND Amount = @Amount AND CreatedAt < @CreatedAt AND FulfilledAt IS NULL", connection, transaction);
            orderCheckCmd.Parameters.AddWithValue("@ProductId", request.ProductId);
            orderCheckCmd.Parameters.AddWithValue("@Amount", request.Amount);
            orderCheckCmd.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

            await using var orderReader = await orderCheckCmd.ExecuteReaderAsync(cancellationToken);
            if (!orderReader.HasRows)
                return Result.Failure<int>("No matching order found or order already fulfilled");

            await orderReader.ReadAsync(cancellationToken);
            var orderId = orderReader.GetInt32(0);
            var productPrice = orderReader.GetDecimal(1);
            await orderReader.CloseAsync();

            // Update the order as fulfilled
            var updateOrderCmd = new SqlCommand("UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE ID = @OrderId", connection, transaction);
            updateOrderCmd.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
            updateOrderCmd.Parameters.AddWithValue("@OrderId", orderId);
            await updateOrderCmd.ExecuteNonQueryAsync(cancellationToken);

            // Insert the record into Product_Warehouse
            var insertProductWarehouseCmd = new SqlCommand("INSERT INTO Product_Warehouse (ProductId, WarehouseId, OrderId, Amount, Price, CreatedAt) OUTPUT INSERTED.ID VALUES (@ProductId, @WarehouseId, @OrderId, @Amount, @Price, @CreatedAt)", connection, transaction);
            insertProductWarehouseCmd.Parameters.AddWithValue("@ProductId", request.ProductId);
            insertProductWarehouseCmd.Parameters.AddWithValue("@WarehouseId", request.WarehouseId);
            insertProductWarehouseCmd.Parameters.AddWithValue("@OrderId", orderId);
            insertProductWarehouseCmd.Parameters.AddWithValue("@Amount", request.Amount);
            insertProductWarehouseCmd.Parameters.AddWithValue("@Price", productPrice * request.Amount);
            insertProductWarehouseCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

            var insertedId = (int)(await insertProductWarehouseCmd.ExecuteScalarAsync(cancellationToken))!;

            transaction.Commit();

            return Result.Success(insertedId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<int>($"An error occurred: {ex.Message}");
        }
    }
}