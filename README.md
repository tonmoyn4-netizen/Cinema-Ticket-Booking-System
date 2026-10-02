# Cinema Ticket Booking System

An **ASP.NET MVC 5 + Entity Framework 6 Code First** cinema ticket booking project designed in the same practical CRUD style as the supplied MVC Code First reference.

## Features

- Movie CRUD
- Poster/image upload
- Showtime CRUD
- Customer records
- Ticket booking
- Automatic booking code
- Seat-capacity validation
- Automatic total price calculation
- Booking details / printable-style ticket view
- Booking cancellation
- AJAX movie/showtime deletion
- Bootstrap UI
- Code First database initialization with demo data
- Friendly routes such as `/movie-list` and `/booking-list`

## Technology

- ASP.NET MVC 5
- .NET Framework 4.8
- Entity Framework 6.5.1
- SQL Server LocalDB
- Razor Views
- Bootstrap 3
- jQuery

## Project Structure

```text
CinemaTicketBookingSystem/
├── App_Start/
├── Content/
├── Controllers/
│   ├── HomeController.cs
│   ├── MoviesController.cs
│   ├── ShowtimesController.cs
│   └── BookingsController.cs
├── Images/
├── Models/
│   ├── CinemaModels.cs
│   └── BookingVM.cs
├── Views/
│   ├── Bookings/
│   ├── Home/
│   ├── Movies/
│   ├── Shared/
│   └── Showtimes/
├── Global.asax
├── Web.config
└── packages.config
```

## Run Locally

1. Install Visual Studio 2022 with **ASP.NET and web development** and .NET Framework 4.8 developer tools.
2. Open `CinemaTicketBookingSystem.sln`.
3. Restore NuGet packages.
4. Build the solution.
5. Run with IIS Express.
6. The default database uses SQL Server LocalDB and is created automatically.
7. Demo movies, showtimes, and customers are seeded on first database creation.

### Database

Connection string:

```text
(localdb)\MSSQLLocalDB
Database: CinemaTicketBookingSystemDb
```

If you want to use a normal SQL Server instance, update `CinemaDbContext` in `Web.config`.

## Important

This project intentionally follows the **MVC 5 / Code First** style of the supplied reference rather than ASP.NET Core. It is therefore intended for Visual Studio on Windows with .NET Framework 4.8.

## License

Use and modify this project for learning, coursework, and portfolio purposes.
