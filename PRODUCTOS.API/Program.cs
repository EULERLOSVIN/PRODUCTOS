using MassTransit;
using Microsoft.EntityFrameworkCore;
using PRODUCTOS.Application.Consumers;
using PRODUCTOS.Application.Interfaces;
using PRODUCTOS.Persistence.Context;
using PRODUCTOS.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(IApplicationDbContext).Assembly
    ));

// Configurar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => {
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    })
);

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
// 2. Registrar MassTransit y RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Registramos el consumidor que llama a tu UpdateStockHandler
    x.AddConsumer<ReduceStockConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("amqps://b-f19a7ab2-7079-4163-b0c6-854fe583469f.mq.us-east-1.on.aws:5671"), h =>
        {
            h.Username("admin");
            h.Password("martinez1234");

            h.UseSsl(s => s.Protocol = System.Security.Authentication.SslProtocols.Tls12);
        });

        // IMPORTANTE: Esto crea automáticamente las colas en Amazon MQ
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configurar CORS para API Gateway
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiGateway", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiTienda API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowApiGateway"); 
app.UseAuthorization();
app.MapControllers();

app.Run();
