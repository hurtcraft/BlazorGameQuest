using Microsoft.EntityFrameworkCore;
using Database;      
using Service;       
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<DonjonService>();

// Configuration de l'authentification JWT avec Keycloak
// Utiliser host.docker.internal pour accéder à Keycloak depuis Docker, ou localhost sinon
var keycloakUrl = builder.Configuration["Keycloak:Authority"] 
    ?? Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY") 
    ?? "http://host.docker.internal:8080"; // Depuis Docker, utiliser host.docker.internal pour accéder à l'hôte

var keycloakAuthority = $"{keycloakUrl}/realms/blazorgamequest";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = keycloakAuthority;
    options.Audience = "blazor-client";
    options.RequireHttpsMetadata = false; // Pour le développement uniquement
    options.MetadataAddress = $"{keycloakAuthority}/.well-known/openid-configuration";
    
    // Configuration pour récupérer les clés de signature
    options.BackchannelHttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Spécifier l'issuer valide (Keycloak) - accepter localhost (ce qui est dans le token)
        ValidIssuer = "http://localhost:8080/realms/blazorgamequest",
        // Accepter les tokens avec l'audience "blazor-client" ou "account"
        ValidAudiences = new[] { "blazor-client", "account" }
    };
    
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    
    // Configuration Swagger pour JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("http://localhost:5000", "http://client:80")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
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

// Désactiver HTTPS redirection car on utilise HTTP en développement
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Route de test pour vérifier que l'API fonctionne
app.MapGet("/", () => "API BlazorGameQuest is running! Go to /swagger for API documentation.");

app.MapControllers();


app.Run();
