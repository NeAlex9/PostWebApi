using Api.Reddit;
using Application;
using Caching.InMemory;
using Infrastructure.SqlLite;
using PostsApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration.GetSection("PostCachingOptions"));
builder.Services.AddRedditApiService(builder.Configuration.GetSection("RedditConfiguration"));
builder.Services.AddSqLiteInfrastructure(builder.Configuration.GetSection("SqlConnectionString"));
builder.Services.AddCachingService();

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
