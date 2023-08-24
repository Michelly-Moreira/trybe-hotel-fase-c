using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;

namespace TrybeHotel.Repository;
public class TrybeHotelContext : DbContext, ITrybeHotelContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Hotel> Hotels { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public TrybeHotelContext(DbContextOptions<TrybeHotelContext> options) : base(options) {
        Seeder.SeedUserAdmin(this);
    }
    public TrybeHotelContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    // Verificando se o banco de dados já foi configurado:
    if (!optionsBuilder.IsConfigured)
    {
    // Buscando o valor da connection string armazenada na variável de ambiente:
        var connectionString = "Server=localhost;Database=TrybeHotel;User=SA;Password=TrybeHotel12!;TrustServerCertificate=True";
    // Executando o método UseSqlServer e passando a connection string a ele:
        optionsBuilder.UseSqlServer(connectionString);
    }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   // definição das relações
        modelBuilder.Entity<City>()
        .HasMany(c => c.Hotels)
        .WithOne(h => h.City)
        .HasForeignKey(c => c.CityId);

        modelBuilder.Entity<Hotel>()
        .HasMany(h => h.Rooms)
        .WithOne(r => r.Hotel)
        .HasForeignKey(h => h.RoomId);

        modelBuilder.Entity<Room>()
        .HasMany(r => r.Bookings)
        .WithOne(b => b.Room)
        .HasForeignKey(r => r.RoomId);

        modelBuilder.Entity<User>()
        .HasMany(u => u.Bookings)
        .WithOne(b => b.User)
        .HasForeignKey(u => u.UserId);
    }
}