using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CinemaTicketBookingSystem.Models
{
    public class BookingVM
    {
        public int Id { get; set; }

        [Required]
        public int ShowtimeId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Range(1, 20)]
        public int SeatCount { get; set; } = 1;

        public string Status { get; set; } = "Confirmed";

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public IEnumerable<SelectListItem> ShowtimeList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> CustomerList { get; set; } = new List<SelectListItem>();
    }
}
