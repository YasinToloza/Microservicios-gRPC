using BasketSpa.API.grpServices;
using BasketSpa.API.Protos;
using BasketSpa.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyeccion de dependencias para Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("RedisSettings:Server");
});

// Inyeccion de Dependencia para IbasketRepository
builder.Services.AddScoped<IBasketRepository, basketRepository>();

// Inyeccion de Dependencia para gRPC Client
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
{
   o.Address = new Uri(builder.Configuration["gRPCSettings:DiscountURL"]);
});
builder.Services.AddScoped<DiscountgRPCService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
