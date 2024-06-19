using Carter;
using Microsoft.EntityFrameworkCore;
using RCS.API;
using RCS.API.Clients.Repository;
using RCS.API.Contracts.Repository;
using RCS.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
var appSettings = builder.Configuration.GetSection(AppSettings.SectionName).Get<AppSettings>();

builder.Services.AddDbContext<RcsDbContext>(options =>
{
    options.UseSqlServer(appSettings!.SQLConnectionString);
});

// builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddCarter();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();

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
