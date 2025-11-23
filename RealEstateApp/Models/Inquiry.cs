using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Models
{
    public class Inquiry
    {
        public int Id { get; set; }

        [Required]
        public string UserEmail { get; set; } // Who sent it?

        [Required]
        public string Message { get; set; } // What did they say?

        public int PropertyId { get; set; } // Which house is this for?

        // Navigation property: allows access to the related Property (inquiry.Property.Title)
        public Property? Property { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool IsArchived { get; set; } = false;
    }
}