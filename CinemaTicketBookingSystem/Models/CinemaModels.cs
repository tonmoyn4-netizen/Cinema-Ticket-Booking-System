using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace CinemaTicketBookingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie title is required")]
        [StringLength(120)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(60)]
        public string Genre { get; set; }

        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
        public int DurationMinutes { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [StringLength(20)]
        public string Rating { get; set; }

        public string PosterImage { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }

    public class Showtime
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Range(1, 1000)]
        public decimal TicketPrice { get; set; }

        [Range(1, 1000)]
        public int TotalSeats { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(30)]
        public string Phone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string BookingCode { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Range(1, 20)]
        public int SeatCount { get; set; }

        [Range(0, 1000000)]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Confirmed";

        [Required]
        public int ShowtimeId { get; set; }
        public virtual Showtime Showtime { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext() : base("CinemaDbContext") { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }

    public class CinemaDbInitializer : CreateDatabaseIfNotExists<CinemaDbContext>
    {
        protected override void Seed(CinemaDbContext db)
        {
            var movies = new[]
            {
                new Movie { Title = "The Last Horizon", Genre = "Adventure", DurationMinutes = 128, ReleaseDate = new DateTime(2026, 5, 15), Rating = "PG-13", IsActive = true },
                new Movie { Title = "Midnight Detective", Genre = "Mystery", DurationMinutes = 112, ReleaseDate = new DateTime(2026, 6, 1), Rating = "PG-13", IsActive = true },
                new Movie { Title = "Galaxy Rangers", Genre = "Sci-Fi", DurationMinutes = 141, ReleaseDate = new DateTime(2026, 7, 10), Rating = "12A", IsActive = true }
            };
            db.Movies.AddRange(movies);
            db.SaveChanges();

            db.Showtimes.AddRange(new[]
            {
                new Showtime { MovieId = movies[0].Id, StartTime = DateTime.Today.AddHours(15), TicketPrice = 350, TotalSeats = 80 },
                new Showtime { MovieId = movies[0].Id, StartTime = DateTime.Today.AddHours(19), TicketPrice = 450, TotalSeats = 80 },
                new Showtime { MovieId = movies[1].Id, StartTime = DateTime.Today.AddHours(17), TicketPrice = 300, TotalSeats = 60 },
                new Showtime { MovieId = movies[2].Id, StartTime = DateTime.Today.AddHours(20), TicketPrice = 500, TotalSeats = 100 }
            });

            db.Customers.AddRange(new[]
            {
                new Customer { Name = "Demo Customer", Phone = "01700000000", Email = "demo@example.com" },
                new Customer { Name = "Sample Guest", Phone = "01800000000", Email = "guest@example.com" }
            });
            db.SaveChanges();
        }
    }
}
