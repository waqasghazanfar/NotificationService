using Microsoft.AspNetCore.Authentication;
using NotificationService.Api.Authentication;
using NotificationService.Infrastructure;
using System.Reflection;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Bind SecuritySettings from appsettings.json
builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("SecuritySettings"));
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register Application services
builder.Services.AddAutoMapper((IMapperConfigurationExpression cfg) => cfg.AddMaps(new[]
{
    Assembly.GetExecutingAssembly(),
    typeof(NotificationService.Application.Mappings.MappingProfile).Assembly
}));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NotificationService.Application.Contracts.Persistence.IAsyncRepository<>).Assembly));

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotificationService API", Version = "v1" });

//    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "basic",
//        In = ParameterLocation.Header,
//        Description = "Basic Authorization header using ClientId and ClientSecret"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Basic"
//                }
//            },
//            new string[] {}
//        }
//    });
//});

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