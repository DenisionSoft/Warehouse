using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Warehouse.Business;
using Warehouse.Data;
using Warehouse.Web.Infrastructure;

namespace Warehouse.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json");

        builder.Services.AddDb();
        builder.Services.AddRepositories();
        builder.Services.AddBusiness();
        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(IWebMarker));
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddAutoMapper(typeof(IWebMarker));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseExceptionHandler(opt => { });

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
            context.Database.Migrate();
        }

        app.Run();
    }
}
