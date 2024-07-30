using Amazon.S3;
using Amazon.Util;
using FileStorageMicroService.Middleware;
using FileStorageMicroService.Models;
using FileStorageMicroService.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Config S3
if (!builder.Configuration.GetSection("S3").Exists()) throw new Exception("Missing value config: S3Config");

var s3Config = builder.Configuration.GetSection("S3").Get<S3Options>();
builder.Services.AddSingleton<IAmazonS3>(provider =>
{
    var client = new AmazonS3Client(s3Config.AccessKey, s3Config.SecretKey, new AmazonS3Config
    {
        ServiceURL = s3Config.RegionEndpoint,
    });
    return client;
});


builder.Services.AddSingleton<IS3Service, S3Service>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Storage API", Version = "v1" });
});

// Config Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();


    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Storage API v1");
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

// Config Middleware
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();

