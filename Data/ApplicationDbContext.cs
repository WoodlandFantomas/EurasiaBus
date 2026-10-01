
using EurasiaBus.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EurasiaBus.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<BusStop> BusStops => Set<BusStop>();
    public DbSet<BusTrip> BusTrips => Set<BusTrip>();
    public DbSet<BusTripSeat> BusTripSeats => Set<BusTripSeat>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // BUS
        // =========================

        modelBuilder.Entity<Bus>()
            .HasIndex(x => x.RegistrationNumber)
            .IsUnique();

        // =========================
        // BUS TRIP
        // =========================    

        modelBuilder.Entity<BusTrip>()
            .HasOne(x => x.DepartureStop)
            .WithMany()
            .HasForeignKey(x => x.DepartureStopId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusTrip>()
            .HasOne(x => x.ArrivalStop)
            .WithMany()
            .HasForeignKey(x => x.ArrivalStopId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusTrip>()
            .Property(x => x.Price)
            .HasPrecision(10, 2);

        // =========================
        // BUS TRIP SEATS
        // =========================

        modelBuilder.Entity<BusTripSeat>()
            .HasIndex(x => new
            {
                x.BusTripId,
                x.SeatNumber
            })
            .IsUnique();

        modelBuilder.Entity<BusTripSeat>()
            .Property(x => x.Price)
            .HasPrecision(10, 2);

        // =========================
        // BOOKING -> USER
        // =========================

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.User)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // BOOKING -> PASSENGER
        // =========================

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Passenger)
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // BOOKING -> BUS TRIP
        // =========================

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.BusTrip)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.BusTripId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // BOOKING -> BUS TRIP SEAT
        // =========================

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.BusTripSeat)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.BusTripSeatId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // BOOKING
        // =========================

        modelBuilder.Entity<Booking>()
            .HasIndex(x => x.PnrCode)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .Property(x => x.TicketPrice)
            .HasPrecision(10, 2);

        // =========================
        // PAYMENT
        // =========================

        modelBuilder.Entity<Payment>()
            .Property(x => x.Amount)
            .HasPrecision(10, 2);
    }
}

