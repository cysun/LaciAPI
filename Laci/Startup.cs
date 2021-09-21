using Laci.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                if (Environment.IsDevelopment()) {
                    options.AddPolicy("MyAllowAll", builder => {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
                }
                else {
                    options.AddPolicy("MyAllowSome", builder => {
                        builder.WithOrigins(Configuration["AllowedCorsOrigin"]);
                    });
                }
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Laci", Version = "v1" });
            });

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options => {
                options.Authority = Configuration["OIDC:Authority"];
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options => {
                options.AddPolicy("HasApiScope",
                    policy => policy.RequireClaim("scope", Configuration["OIDC:ApiScope"]));
            });

            services.AddScoped<CityService>();
            services.AddScoped<RecordService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laci v1"));
            }

            app.UseRouting();

            if (Environment.IsDevelopment())
                app.UseCors("MyAllowAll");
            else
                app.UseCors("MyAllowSome");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
