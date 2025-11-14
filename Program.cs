using WepApiPrenumeranter.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddScoped<PrenumeranterMethods>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowReactDev",
        policy => policy.WithOrigins("http://localhost:5057")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json","WebApiPrenumeranter V1");
        c.RoutePrefix = "swagger";
    }
    );
}

app.UseCors("AllowReactDev");

app.UseAuthorization();

app.MapControllers();

app.Run();
