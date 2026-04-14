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
        public DbSet<UserSession> UserSessions { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

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

            modelBuilder.Entity<OperationsOfItinerary>()
    .HasOne(o => o.Itinerary)
    .WithMany(i => i.Operations)
    .HasForeignKey(o => o.ItineraryID)
    .IsRequired() 
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>().HasData(
                new Role { ID = Role.BrigadirId, Name = Role.BrigadirName },
                new Role { ID = Role.ExecutorId, Name = Role.ExecutorName }
            ); 
        }
        public PostgresContext(DbContextOptions<PostgresContext> options): base(options) 
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Конфигурация будет установлена через DI в Program.cs
            }
        }

        
    }
}
