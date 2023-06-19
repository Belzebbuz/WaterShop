using Microsoft.EntityFrameworkCore;

namespace WaterShop.Persistance;

public static class PersistanceExtensions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration, string contentRootPath)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if(connectionString == null) 
            throw new NullReferenceException(nameof(configuration));

        var updatedString = connectionString.Replace("%CONTENTROOTPATH%", contentRootPath);
		services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(updatedString);
        });
        services.AddTransient<DataSeeder>();
        return services;
    }

    public static async Task MigrateDbAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<DataSeeder>().SeedAsync();
	}
}