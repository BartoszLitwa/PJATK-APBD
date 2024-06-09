using Kolos.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolos.API.Data;

public class KolosDbContext : DbContext
{
    public KolosDbContext(DbContextOptions<KolosDbContext> options) : base(options)
    {
    }
    
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Payment> Payments { get; set; }
}