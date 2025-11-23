using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }

        public string Url { get; set; } // The filename (e.g., "kitchen.jpg")

        public int PropertyId { get; set; } // Link to the house
        public Property Property { get; set; }

        // Keep this for the "Main/Cover" photo shown on the home page
        public string ImageUrl { get; set; } = string.Empty;

        // ADD THIS: The list of gallery images
        public List<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    }
}