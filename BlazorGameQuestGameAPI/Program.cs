using Microsoft.EntityFrameworkCore;
using Database;      
using Service;       
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<DonjonService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});



var app = builder.Build();
//opération pré lancement, remplissage table

using (var scope = app.Services.CreateScope())
{
    var playerService = scope.ServiceProvider.GetRequiredService<PlayerService>();
    await playerService.InitializeAsync();
}

app.UseCors("AllowLocalhost");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
