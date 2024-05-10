using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Warehouse.API.Controllers.Commands;

public class AddProductToWarehouseInCodeCommand : IRequest<Result<int>>
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddProductToWarehouseInCodeHandler(IOptions<AppConfig> options) : IRequestHandler<AddProductToWarehouseInCodeCommand, Result<int>>
{
    private readonly AppConfig _appConfig = options.Value;
    
    public async Task<Result<int>> Handle(AddProductToWarehouseInCodeCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            return Result.Failure<int>("Amount must be greater than 0");
        
        await using var connection = new SqlConnection(_appConfig.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            // Check if product exists
            var productCheckCmd = new SqlCommand("SELECT COUNT(*) FROM [Product] WHERE IdProduct = @IdProduct", connection, transaction);
            productCheckCmd.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            if ((int)await productCheckCmd.ExecuteScalarAsync(cancellationToken) == 0)
                return Result.Failure<int>("Product does not exist");

            // Check if warehouse exists
            var warehouseCheckCmd = new SqlCommand("SELECT COUNT(*) FROM [Warehouse] WHERE IdWarehouse = @IdWarehouse", connection, transaction);
            warehouseCheckCmd.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
            if ((int)await warehouseCheckCmd.ExecuteScalarAsync(cancellationToken) == 0)
                return Result.Failure<int>("Warehouse does not exist");

            // Check for a valid order
            var orderCheckCmd = new SqlCommand("SELECT TOP 1 o.IdOrder, p.Price FROM [Order] o " +
                                               "INNER JOIN [Product] p ON o.IdProduct = p.IdProduct " +
                                               "WHERE o.IdProduct = @IdProduct AND o.Amount = @Amount AND o.CreatedAt < @CreatedAt " +
                                               "AND NOT EXISTS (SELECT 1 FROM [Product_Warehouse] pw WHERE pw.IdOrder = o.IdOrder)", connection, transaction);
            orderCheckCmd.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            orderCheckCmd.Parameters.AddWithValue("@Amount", request.Amount);
            orderCheckCmd.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);
            await using var orderReader = await orderCheckCmd.ExecuteReaderAsync(cancellationToken);
            if (!orderReader.HasRows)
                return Result.Failure<int>("No matching order found in Order table");
            
            await orderReader.ReadAsync(cancellationToken);
            var orderId = orderReader.GetInt32(0);
            var productPrice = orderReader.GetDecimal(1);
            await orderReader.CloseAsync();
            
            var productWarehouseCmd = new SqlCommand("SELECT COUNT(*) FROM [Product_Warehouse] " +
                                                     "WHERE OrderId = @OrderId", connection, transaction);
            productWarehouseCmd.Parameters.AddWithValue("@OrderId", request.IdProduct);
            await using var productWarehouseReader = await orderCheckCmd.ExecuteReaderAsync(cancellationToken);
            if (!productWarehouseReader.HasRows)
                return Result.Failure<int>("No matching order found in ProductWarehouse table");

            // Update the order as fulfilled
            var updateOrderCmd = new SqlCommand("UPDATE [Product_Warehouse] SET FulfilledAt = @FulfilledAt WHERE ID = @OrderId", connection, transaction);
            updateOrderCmd.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
            updateOrderCmd.Parameters.AddWithValue("@OrderId", orderId);
            await updateOrderCmd.ExecuteNonQueryAsync(cancellationToken);

            // Insert the record into Product_Warehouse
            var insertProductWarehouseCmd = new SqlCommand("INSERT INTO Product_Warehouse " +
                                                           "(ProductId, WarehouseId, OrderId, Amount, Price, CreatedAt) OUTPUT INSERTED.ID " +
                                                           "VALUES (@ProductId, @WarehouseId, @OrderId, @Amount, @Price, @CreatedAt)", connection, transaction);
            insertProductWarehouseCmd.Parameters.AddWithValue("@ProductId", request.IdProduct);
            insertProductWarehouseCmd.Parameters.AddWithValue("@WarehouseId", request.IdWarehouse);
            insertProductWarehouseCmd.Parameters.AddWithValue("@OrderId", orderId);
            insertProductWarehouseCmd.Parameters.AddWithValue("@Amount", request.Amount);
            insertProductWarehouseCmd.Parameters.AddWithValue("@Price", productPrice * request.Amount);
            insertProductWarehouseCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

            var insertedId = (int)await insertProductWarehouseCmd.ExecuteScalarAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Result.Success(insertedId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<int>($"Rollback - An error occurred: {ex.Message}");
        }
    }
}