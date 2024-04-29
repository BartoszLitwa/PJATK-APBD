CREATE PROCEDURE AddProductToWarehouse
    @IdProduct INT, 
    @IdWarehouse INT, 
    @Amount INT, 
    @CreatedAt DATETIME 
AS
BEGIN 
 SET NOCOUNT ON;  
 SET XACT_ABORT ON;
BEGIN TRAN;  
  
 DECLARE @IdProductFromDb INT, @IdOrder INT, @Price DECIMAL(5,2);  

 IF @Amount <= 0
BEGIN  
  RAISERROR('Invalid parameter: Amount must be greater than 0', 16, 1);  
  RETURN;
END;

SELECT TOP 1 @IdOrder = o.IdOrder, @Price = p.Price
    FROM "Order" o
    INNER JOIN Product p ON o.IdProduct = p.IdProduct
    WHERE o.IdProduct = @IdProduct
      AND o.Amount = @Amount
      AND o.CreatedAt < @CreatedAt
      AND NOT EXISTS(SELECT 1 FROM Product_Warehouse pw WHERE pw.IdOrder = o.IdOrder);

IF @IdOrder IS NULL
BEGIN  
  RAISERROR('Invalid parameter: There is no order to fulfill or it has already been processed', 16, 1);
ROLLBACK;
RETURN;
END;  

 IF NOT EXISTS(SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse)
BEGIN  
  RAISERROR('Invalid parameter: Provided IdWarehouse does not exist', 16, 1);
ROLLBACK;
RETURN;
END;

UPDATE "Order" SET
    FulfilledAt = @CreatedAt
WHERE IdOrder = @IdOrder;

INSERT INTO Product_Warehouse(IdWarehouse,
                              IdProduct, IdOrder, Amount, Price, CreatedAt)
VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price * @Amount, @CreatedAt);

SELECT SCOPE_IDENTITY() AS NewId;

COMMIT;
END
