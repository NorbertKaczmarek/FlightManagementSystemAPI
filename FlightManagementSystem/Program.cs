using FlightManagementSystem.Middleware;
using FlightManagementSystem.Models.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FlightManagementSystem.Entities;
using FlightManagementSystem.Models;
using FlightManagementSystem.Services;
using FlightManagementSystem;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policybuilder =>

        policybuilder
            .WithOrigins(builder.Configuration["AllowedOrigins"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("location")
        );
});

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
        ValidateIssuer = true,
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidateAudience = true,
        ValidAudience = authenticationSettings.JwtIssuer,
    };
});

builder.Services.AddAuthorization();

// Add services to the container.

// for enums:
builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Password Hasher:
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// validation
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddScoped<IValidator<CreateFlightDto>, CreateFlightDtoValidator>();

// seeder
builder.Services.AddScoped<FlightSeeder>();

// services
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

//Swagger Documentation Section
var info = new OpenApiInfo()
{
    Title = "System zarz¹dzania lotami", // Assembly.GetExecutingAssembly().GetName().Name
    Version = "v1",
    Description = "API do zarz¹dzania lotami, które pozwala przegl¹daæ, dodawaæ, aktualizowaæ i usuwaæ informacje o lotach."
};

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", info);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

    opt.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
});

builder.Services.AddDbContext<FlightManagementDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("FlightManagementDbConnection")));


var app = builder.Build();

app.UseCors("FrontEndClient");

// jwt
app.UseAuthentication();
app.UseAuthorization();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<FlightSeeder>();
seeder.Seed();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// for tests
public partial class Program { }