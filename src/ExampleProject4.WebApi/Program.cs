
using ExampleProject4.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace ExampleProject4.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Pøíprava pro autentizaci pomocí JWT
            var secret = builder.Configuration.GetValue<string>("Jwt:Secret");
            var audience = builder.Configuration.GetValue<string>("Jwt:Audience");
            var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
            byte[] jwtSecret = Encoding.UTF8.GetBytes(secret);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                        ValidateLifetime = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["Authorization"];;
                            return Task.CompletedTask;
                        }
                    };
                });

            //Registrace databázového kontextu
            builder.Services.AddDbContext<ExampleDbContext>();

            //Povolení requestù z jiné aplikace KROK 1.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            builder.Services.AddControllers();

            //Registrace služeb pro Swagger (API dokumentace)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

            });

            var app = builder.Build();

            //Swagger povolíme pouze pøi vývoji (lze zmìnit)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Povolení requestù z jiné aplikace KROK 2.
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            //Zapneme autentizaci (kdo jsme?) a potom autorizaci (jaké máme práva/role?)
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
