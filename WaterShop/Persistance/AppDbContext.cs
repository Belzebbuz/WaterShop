using Microsoft.EntityFrameworkCore;
using WaterShop.Domain;

namespace WaterShop.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Water> Waters => Set<Water>();
}
