using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Models
{
    public class Agent
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; } = "default-avatar.png";

        public string Bio { get; set; } // A short description (e.g. "Senior Consultant")
    }
}