using CinemaTicketBookingSystem.Models;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTicketBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly CinemaDbContext db = new CinemaDbContext();

        public ActionResult Index()
        {
            ViewBag.MovieCount = db.Movies.Count();
            ViewBag.ShowtimeCount = db.Showtimes.Count(s => s.StartTime >= System.DateTime.Now);
            ViewBag.BookingCount = db.Bookings.Count();
            ViewBag.Revenue = db.Bookings.Where(b => b.Status == "Confirmed").Select(b => (decimal?)b.TotalAmount).Sum() ?? 0;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
