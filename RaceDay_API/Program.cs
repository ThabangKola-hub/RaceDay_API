using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RaceDay_API.Data;
using RaceDay_API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Database Context
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Register Custom Services
builder.Services.AddScoped<TokenService>();

// 4. Swagger Configuration (Adds JWT Auth button)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RaceDay API", Version = "v1" });
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
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// 5. JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// --- SEEDING BLOCK: Only runs if NOT in "Testing" environment ---
if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<RaceDayDbContext>();

        if (!context.Users.Any(u => u.Role == RaceDay_API.Models.UserRole.Organiser))
        {
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<RaceDay_API.Models.User>();

            var org = new RaceDay_API.Models.User
            {
                Name = "Admin Organiser",
                Email = "organiser@raceday.com",
                Role = RaceDay_API.Models.UserRole.Organiser
            };
            org.PasswordHash = hasher.HashPassword(org, "Password123!");

            var participant = new RaceDay_API.Models.User
            {
                Name = "Test Participant",
                Email = "participant@raceday.com",
                Role = RaceDay_API.Models.UserRole.Participant
            };
            participant.PasswordHash = hasher.HashPassword(participant, "Password123!");

            context.Users.Add(org);
            context.Users.Add(participant);
            context.SaveChanges();
        }
    }
}
// -------------------------------------------------------------

// 6. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // MUST be before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();

// Required for WebApplicationFactory to find the entry point
public partial class Program { }