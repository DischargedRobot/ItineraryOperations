using ItineraryOperations.Models.Executor;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ItineraryOperations.Models
{
    public class PostgresContext : DbContext
    {
        public DbSet<Divisions> Divisions { get; set; } = null!;
        public DbSet<Equipment> Equipment { get; set; } = null!;
        public DbSet<Executors> Executors { get; set; } = null!; 
        public DbSet<Itinerary> Itineraries { get; set; } = null!;
        public DbSet<OperationsOfItinerary> OperationsOfItinerary { get; set; } = null!;
        public DbSet<MainSubject> MainSubject { get; set; }
        public DbSet<OperationCategories> OperationCategories { get; set; } = null!;
        public DbSet<PlanPosition> PlanPositions { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<TaskOrders> TaskOrders { get; set; } = null!;
        public DbSet<TypesOperations> TypesOperations { get; set; } = null!;

        public PostgresContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
                        .HasOne(product => product.MainSubject)
                        .WithMany()
                        .HasForeignKey(product => product.AUDCode)
                        .HasPrincipalKey(mainSubject => mainSubject.AUDCode);

            modelBuilder.Entity<Itinerary>()
                        .HasOne(itinerary => itinerary.MainSubject)
                        .WithMany()
                        .HasForeignKey(itinerary => itinerary.AUDCode)
                        .HasPrincipalKey(mainSubject => mainSubject.AUDCode);

            modelBuilder.Entity<Itinerary>()
                        .Property(i => i.Date)
                        .HasDefaultValueSql("CURRENT_DATE");

        }
        public PostgresContext(DbContextOptions<PostgresContext> options): base(options) 
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=itineraryOperationDb;Username=postgres;Password=546915");
        }

        
    }
}
