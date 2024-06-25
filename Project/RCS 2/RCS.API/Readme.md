dotnet tool install --global dotnet-ef

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet ef migrations add InitialCreate --project Pharmacy.API

dotnet ef database update --project Pharmacy.API