using Microsoft.EntityFrameworkCore;
using Trips.API;
using Trips.API.Data;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(AppSettings.SectionName).Get<AppSettings>();

builder.Services.AddDbContext<TripsDbContext>(options =>
{
    options.UseSqlServer(appSettings!.SQLConnectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();