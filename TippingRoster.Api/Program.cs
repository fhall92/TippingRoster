using TippingRoster.Application.Interfaces;
using TippingRoster.Application.Services;
using TippingRoster.Infrastructure.Data;
using TippingRoster.Infrastructure.Repositories;
using TippingRoster.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddSingleton<InMemoryDataContext>();

builder.Services.AddSingleton<IHostedService, DataSeedHostedService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<ITipsRepository, TipsRepository>();

builder.Services.AddScoped<RosterService>();
builder.Services.AddScoped<TipCalculationService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");


app.MapControllers();

app.Run();

public class DataSeedHostedService : IHostedService
{
    private readonly InMemoryDataContext _context;

    public DataSeedHostedService(InMemoryDataContext context)
    {
        _context = context;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        DataSeeder.Seed(_context);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
