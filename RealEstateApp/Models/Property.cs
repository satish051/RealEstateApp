using System.Collections.Generic; // This is required for List<>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public bool IsForSale { get; set; }

        // Agent Info
        public string AgentName { get; set; } = "General Inquiry";
        public string AgentEmail { get; set; } = "info@realestate.com";
        public string AgentPhone { get; set; } = "+977 9800000000";
        public List<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    }
}