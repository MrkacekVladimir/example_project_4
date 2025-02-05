
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

            //P��prava pro autentizaci pomoc� JWT
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

            //Registrace datab�zov�ho kontextu
            builder.Services.AddDbContext<ExampleDbContext>();

            //Povolen� request� z jin� aplikace KROK 1.
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

            //Registrace slu�eb pro Swagger (API dokumentace)
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

            //Swagger povol�me pouze p�i v�voji (lze zm�nit)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Povolen� request� z jin� aplikace KROK 2.
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            //Zapneme autentizaci (kdo jsme?) a potom autorizaci (jak� m�me pr�va/role?)
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
