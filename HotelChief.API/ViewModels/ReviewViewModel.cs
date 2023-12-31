﻿namespace HotelChief.API.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using HotelChief.API.ViewModels.DTO;
    using HotelChief.Core.Entities;

    public class ReviewViewModel
    {
        public int ReviewId { get; set; }

        public int GuestId { get; set; }

        public string? GuestEmail { get; set; }

        [Required]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public IEnumerable<ReviewDTO>? Reviews { get; set; }
    }
}
