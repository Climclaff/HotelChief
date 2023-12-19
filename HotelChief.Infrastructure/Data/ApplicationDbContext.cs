namespace HotelChief.Infrastructure.Data
{
    using HotelChief.Infrastructure.Config;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<Guest, IdentityRole<int>, int>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HotelService>? HotelServices { get; set; }

        public DbSet<HotelServiceOrder>? HotelServiceOrders { get; set; }

        public DbSet<Employee>? Employees { get; set; }

        public DbSet<Room>? Rooms { get; set; }

        public DbSet<Reservation>? Reservations { get; set; }

        public DbSet<Review>? Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new HotelServiceConfiguration());
            modelBuilder.ApplyConfiguration(new HotelServiceOrderConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=CLIMCLAFF\\SQLEXPRESS;Initial Catalog=HotelChiefdb;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }
    }
}