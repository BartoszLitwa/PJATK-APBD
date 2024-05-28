using Carter;
using Microsoft.EntityFrameworkCore;
using Pharmacy.API;
using Pharmacy.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
var appSettings = builder.Configuration.GetSection(AppSettings.SectionName).Get<AppSettings>();

builder.Services.AddDbContext<PharmacyDbContext>(options =>
{
    options.UseSqlServer(appSettings!.SQLConnectionString);
});

// builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddCarter();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();

app.Run();