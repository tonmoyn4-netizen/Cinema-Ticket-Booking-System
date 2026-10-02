using CinemaTicketBookingSystem.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTicketBookingSystem.Controllers
{
    public class ShowtimesController : Controller
    {
        private readonly CinemaDbContext db = new CinemaDbContext();

        private SelectList MovieList(int? id = null) =>
            new SelectList(db.Movies.Where(m => m.IsActive).OrderBy(m => m.Title), "Id", "Title", id);

        public ActionResult Index()
        {
            return View(db.Showtimes.Include("Movie").OrderBy(s => s.StartTime).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.MovieList = MovieList();
            return View(new Showtime { StartTime = DateTime.Now.AddHours(1), TicketPrice = 300, TotalSeats = 80, IsActive = true });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Showtime showtime)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovieList = MovieList(showtime.MovieId);
                return View(showtime);
            }

            db.Showtimes.Add(showtime);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var showtime = db.Showtimes.Find(id);
            if (showtime == null) return HttpNotFound();
            ViewBag.MovieList = MovieList(showtime.MovieId);
            return View(showtime);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Showtime showtime)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovieList = MovieList(showtime.MovieId);
                return View(showtime);
            }

            var existing = db.Showtimes.Find(showtime.Id);
            if (existing == null) return HttpNotFound();

            existing.MovieId = showtime.MovieId;
            existing.StartTime = showtime.StartTime;
            existing.TicketPrice = showtime.TicketPrice;
            existing.TotalSeats = showtime.TotalSeats;
            existing.IsActive = showtime.IsActive;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AjaxDelete(int id)
        {
            var showtime = db.Showtimes.Find(id);
            if (showtime == null || db.Bookings.Any(b => b.ShowtimeId == id)) return Json(false);

            db.Showtimes.Remove(showtime);
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
