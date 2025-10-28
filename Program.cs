using System.Text.Json;
using System.Text.Json.Serialization;
using ElderConnectApi.Data;
using ElderConnectApi.Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions((config) =>
{
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.AllowTrailingCommas = true;
    config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
}).AddMvcOptions(options =>
{
    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider(JsonNamingPolicy.CamelCase));
});
builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});
builder.Services.AddDbContextPool<ElderConnectDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("ElderConnectDbContext"),
        o => o.UseNetTopologySuite()
                .MapEnum<UserAccountStatus>("user_account_status")
                .MapEnum<BookingStatus>("booking_status")
                .MapEnum<Gender>("gender")
                .MapEnum<NurseAccountStatus>("nurse_account_status")
    )
        .UseSnakeCaseNamingConvention()
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.DescribeAllParametersInCamelCase();
    opt.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = false;
});

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
