namespace RealEstateApp.Models
{
        public class SavedProperty
        {
            public int Id { get; set; }
            public string UserEmail { get; set; } // The User
            public int PropertyId { get; set; }   // The House
            public Property? Property { get; set; }
        }
    
}
