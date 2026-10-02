using CinemaTicketBookingSystem.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicketBookingSystem.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext db = new CinemaDbContext();

        public ActionResult Index()
        {
            return View(db.Movies.OrderByDescending(m => m.Id).ToList());
        }

        public ActionResult Details(int id)
        {
            var movie = db.Movies.Include("Showtimes").FirstOrDefault(m => m.Id == id);
            if (movie == null) return HttpNotFound();
            return View(movie);
        }

        public ActionResult Create() => View(new Movie { ReleaseDate = System.DateTime.Today, DurationMinutes = 120, IsActive = true });

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie, HttpPostedFileBase Poster)
        {
            if (!ModelState.IsValid) return View(movie);

            SavePoster(movie, Poster);
            db.Movies.Add(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var movie = db.Movies.Find(id);
            return movie == null ? (ActionResult)HttpNotFound() : View(movie);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie, HttpPostedFileBase Poster)
        {
            if (!ModelState.IsValid) return View(movie);

            var existing = db.Movies.Find(movie.Id);
            if (existing == null) return HttpNotFound();

            existing.Title = movie.Title;
            existing.Genre = movie.Genre;
            existing.DurationMinutes = movie.DurationMinutes;
            existing.ReleaseDate = movie.ReleaseDate;
            existing.Rating = movie.Rating;
            existing.IsActive = movie.IsActive;
            SavePoster(existing, Poster);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AjaxDelete(int id)
        {
            var movie = db.Movies.Find(id);
            if (movie == null) return Json(false);

            if (db.Showtimes.Any(s => s.MovieId == id))
                return Json(false);

            db.Movies.Remove(movie);
            db.SaveChanges();
            return Json(true);
        }

        private void SavePoster(Movie movie, HttpPostedFileBase poster)
        {
            if (poster == null || poster.ContentLength <= 0) return;

            var ext = Path.GetExtension(poster.FileName);
            var name = Path.GetFileNameWithoutExtension(poster.FileName) + "_" + System.DateTime.Now.Ticks + ext;
            var folder = Server.MapPath("~/Images");
            Directory.CreateDirectory(folder);
            poster.SaveAs(Path.Combine(folder, name));
            movie.PosterImage = "/Images/" + name;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
