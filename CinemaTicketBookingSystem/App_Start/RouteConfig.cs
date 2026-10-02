using System.Web.Mvc;
using System.Web.Routing;

namespace CinemaTicketBookingSystem
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "MovieList",
                url: "movie-list",
                defaults: new { controller = "Movies", action = "Index" });

            routes.MapRoute(
                name: "BookingList",
                url: "booking-list",
                defaults: new { controller = "Bookings", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
