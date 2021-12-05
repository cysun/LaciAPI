using Laci.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5011");

var configuration = builder.Configuration;
var environment = builder.Environment;

// Configure Services
var services = builder.Services;

services.AddCors(options =>
{
    if (environment.IsDevelopment())
    {
        options.AddPolicy("MyAllowAll", builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    }
    else
    {
        options.AddPolicy("MyAllowSome", builder =>
        {
            builder.WithOrigins(configuration["AllowedCorsOrigin"]);
        });
    }
});

services.AddRouting(options => options.LowercaseUrls = true);

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Laci", Version = "v1" });
});

services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = configuration["OIDC:Authority"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});

services.AddAuthorization(options =>
{
    options.AddPolicy("HasApiScope",
        policy => policy.RequireClaim("scope", configuration["OIDC:ApiScope"]));
});

services.AddScoped<CityService>();
services.AddScoped<RecordService>();

// Build App
var app = builder.Build();

// Configure Middleware Pipeline
if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

if (environment.IsDevelopment())
    app.UseCors("MyAllowAll");
else
    app.UseCors("MyAllowSome");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run App
app.Run();
