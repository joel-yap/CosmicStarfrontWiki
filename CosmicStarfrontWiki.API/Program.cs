using CosmicStarfrontWiki.API;
using CosmicStarfrontWiki.Data;
using CosmicStarfrontWiki.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Using Swagger for API documentation. No longer has official support, but is used by most APIs in production.
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Demo Api");
    });

    // Alternative: Scalar for API documentation. Better UI but more bloated, so can use later.
    //app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapWikiEndpoints();

app.Run();