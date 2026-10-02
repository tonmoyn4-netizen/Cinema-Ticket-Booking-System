using CinemaTicketBookingSystem.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTicketBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly CinemaDbContext db = new CinemaDbContext();

        private SelectList ShowtimeList(int? id = null) =>
            new SelectList(db.Showtimes.Include("Movie")
                .Where(s => s.IsActive && s.StartTime >= DateTime.Now)
                .OrderBy(s => s.StartTime)
                .Select(s => new { s.Id, Label = s.Movie.Title + " - " + s.StartTime.ToString("dd MMM yyyy hh:mm tt") }),
                "Id", "Label", id);

        private SelectList CustomerList(int? id = null) =>
            new SelectList(db.Customers.OrderBy(c => c.Name), "Id", "Name", id);

        public ActionResult Index()
        {
            var data = db.Bookings.Include("Showtime.Movie").Include("Customer")
                .OrderByDescending(b => b.BookingDate).ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            var vm = new BookingVM
            {
                SeatCount = 1,
                ShowtimeList = ShowtimeList(),
                CustomerList = CustomerList()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(BookingVM vm)
        {
            var show = db.Showtimes.Find(vm.ShowtimeId);
            if (show == null || !show.IsActive)
                ModelState.AddModelError("ShowtimeId", "Please select a valid showtime.");

            if (show != null)
            {
                var booked = db.Bookings.Where(b => b.ShowtimeId == vm.ShowtimeId && b.Status == "Confirmed")
                    .Select(b => (int?)b.SeatCount).Sum() ?? 0;
                if (booked + vm.SeatCount > show.TotalSeats)
                    ModelState.AddModelError("SeatCount", "Not enough seats are available.");
            }

            if (!ModelState.IsValid)
            {
                vm.ShowtimeList = ShowtimeList(vm.ShowtimeId);
                vm.CustomerList = CustomerList(vm.CustomerId);
                return View(vm);
            }

            var booking = new Booking
            {
                BookingCode = "BK-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                BookingDate = DateTime.Now,
                SeatCount = vm.SeatCount,
                Status = "Confirmed",
                ShowtimeId = vm.ShowtimeId,
                CustomerId = vm.CustomerId,
                TotalAmount = show.TicketPrice * vm.SeatCount
            };

            db.Bookings.Add(booking);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = booking.Id });
        }

        public ActionResult Details(int id)
        {
            var booking = db.Bookings.Include("Showtime.Movie").Include("Customer").FirstOrDefault(b => b.Id == id);
            if (booking == null) return HttpNotFound();
            return View(booking);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Cancel(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null) return Json(false);
            booking.Status = "Cancelled";
            db.SaveChanges();
            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
