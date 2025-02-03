
using ExampleProject4.Infrastructure;

namespace ExampleProject4.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Registrace databázového kontextu
            builder.Services.AddDbContext<ExampleDbContext>();

            //Povolení requestù z jiné aplikace KROK 1.
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

            //Registrace služeb pro Swagger (API dokumentace)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
