using CosmicStarfrontWiki.API;
using CosmicStarfrontWiki.Data;
using CosmicStarfrontWiki.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS services to the DI container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:7243") // Replace with your client-side URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Apply the CORS policy
app.UseCors("AllowSpecificOrigins");


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