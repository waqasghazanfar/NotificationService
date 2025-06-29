using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using NotificationService.Api.Authentication;
using NotificationService.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration);

// Register Application services
builder.Services.AddAutoMapper((IMapperConfigurationExpression cfg) => cfg.AddMaps(new[]
{
    Assembly.GetExecutingAssembly(),
    typeof(NotificationService.Application.Mappings.MappingProfile).Assembly
}));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NotificationService.Application.Contracts.Persistence.IAsyncRepository<>).Assembly));

var apiKeyAuthConfig = builder.Configuration.GetSection("ApiKeyAuthentication");

builder.Services.AddAuthentication()
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApiKeyAuthenticationOptions.PolicyName, policy =>
    {
        if (apiKeyAuthConfig.GetValue<bool>("Enabled"))
        {
            policy.AddAuthenticationSchemes(ApiKeyAuthenticationOptions.DefaultScheme);
            policy.RequireAuthenticatedUser();
        }
        else
        {
            // If API key auth is disabled, the policy still exists but allows all requests.
            policy.RequireAssertion(context => true);
        }
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Keycloak Admin API", Version = "v1" });

    // **CORRECTED:** Define two separate security schemes for ClientId and ClientSecret.
    c.AddSecurityDefinition("ClientId", new OpenApiSecurityScheme
    {
        Name = ApiKeyAuthenticationHandler.ClientIdHeaderName,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "The Client ID for this API."
    });
    c.AddSecurityDefinition("ClientSecret", new OpenApiSecurityScheme
    {
        Name = ApiKeyAuthenticationHandler.ClientSecretHeaderName,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "The Client Secret for this API."
    });

    // **CORRECTED:** Require both schemes to be present.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ClientId" }
            },
            new string[] {}
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ClientSecret" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();