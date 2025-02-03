
using ExampleProject4.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExampleProject4.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //P��prava pro autentizaci pomoc� JWT
            byte[] jwtSecret = Encoding.UTF8.GetBytes("sald�kfjasl�kfjasl�dkfjl�asjf�laskdjfl�kasjfl�sakjflksadjfl�sakjfsl�dakjfa�l");
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = "authenticated",
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                        ValidateLifetime = true
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
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();

            //Registrace slu�eb pro Swagger (API dokumentace)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
