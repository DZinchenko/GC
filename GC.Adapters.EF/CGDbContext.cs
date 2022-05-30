using System;
using Microsoft.EntityFrameworkCore;
using GC.Data.Objects;

namespace GC.Adapters.EF
{
    public class CGDbContext : DbContext
    {
        private readonly IEFConfiguration configuration;

        public DbSet<Bin> Bins { get; set; }
        public DbSet<Dump> Dumps { get; set; }
        public DbSet<Station> Stations { get; set; }

        public DbSet<BinSensor> BinSensors { get; set; }
        public DbSet<BinSensorReading> BinSensorReadings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarAssignment> CarAssignments { get; set; }
        public DbSet<CarSensor> CarSensors { get; set; }
        public DbSet<CarSensorReading> CarSensorReadings { get; set; }
        public DbSet<Path> Paths { get; set; }
        public DbSet<PathPart> PathParts { get; set; }
        public DbSet<User> Users { get; set; }


        public CGDbContext(IEFConfiguration configuration): base()
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Place>().ToTable("Places");

            modelBuilder.Entity<CarAssignment>().HasOne<Car>().WithMany().HasForeignKey(x => x.CarId);
            modelBuilder.Entity<CarAssignment>().HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
            modelBuilder.Entity<CarSensor>().HasOne<Car>().WithOne().HasForeignKey<CarSensor>(x => x.CarId);
            modelBuilder.Entity<CarSensorReading>().HasOne<CarSensor>().WithMany().HasForeignKey(x => x.CarSensorId);
            modelBuilder.Entity<Path>().HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
            modelBuilder.Entity<PathPart>().HasOne<Path>().WithMany().HasForeignKey(x => x.PathId);
            modelBuilder.Entity<PathPart>().HasOne<Place>().WithMany().HasForeignKey(x => x.PlaceId);
            modelBuilder.Entity<Distance>().HasOne<Place>().WithMany().HasForeignKey(x => x.Place1Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Distance>().HasOne<Place>().WithMany().HasForeignKey(x => x.Place2Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BinSensor>().HasOne<Bin>().WithOne().HasForeignKey<BinSensor>(x => x.BinId);
            modelBuilder.Entity<BinSensorReading>().HasOne<BinSensor>().WithMany().HasForeignKey(x => x.BinSensorId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.ConnectionString);
        }
    }
}
