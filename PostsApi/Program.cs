using Api.Reddit;
using Application;
using Caching.InMemory;
using Infrastructure.SqlLite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddRedditApiService(builder.Configuration.GetSection("RedditConfiguration"));
builder.Services.AddRepository(builder.Configuration.GetSection("SqlConnectionString"));
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
