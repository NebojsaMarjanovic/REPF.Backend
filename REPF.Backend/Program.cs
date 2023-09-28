
using REPF.Grpc;
using REPF.Grpc.Protos;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
//.AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//})
;
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddGrpcClient<ForecastService.ForecastServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5001");
});

builder.Services.AddGrpcClient<CalculateService.CalculateServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5001");
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
