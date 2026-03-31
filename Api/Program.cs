using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api.Core.Application.utils;
using Microsoft.AspNetCore.RateLimiting;
using auth.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace Api

{
    class Program
    {
        
        
        public static async Task Main()
        {
            Load.LoadEnv();
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.Services.AddRateLimiter(option =>
            {
                option.AddFixedWindowLimiter("fixed", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(1);
                });
                
            });
            AddScope scope = new();
            scope.AddScopeFuncion(builder);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {

                        var key = Environment.GetEnvironmentVariable("JWT_KEY"); // Ou o nome exato no seu appsettings.json

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                            ValidateIssuer = true, 
                            ValidateAudience = true, 
                            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                        };
                    });
                
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",policy=>policy.RequireRole("Admin"));
                options.AddPolicy("User",policy=>policy.RequireRole("User"));
            });
            
            //builder.Services.
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira apenas o token JWT abaixo."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRateLimiter();
            await new Routers.Routers().InitRouters(app);
            app.UseHttpsRedirection();
           
            app.Run();
        }
    }
}


