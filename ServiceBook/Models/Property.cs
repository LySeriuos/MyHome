using System.ComponentModel.DataAnnotations;

namespace ServiceBook.Models
{
    public class Property
    {
       
        public int PropertyId { get; set; }        
                
        [Required(ErrorMessage = "Enter the property name")]        
        public string PropertyName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Enter the street name")]        
        public string StreetName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Enter the house number")]        
        public string HouseNumber { get; set; } = String.Empty;        
        
        public string? ApartmentNumber { get; set; } = String.Empty;

        [Required(ErrorMessage = "Enter the postal code")]       
        public string PostalCode { get; set; } = String.Empty;

        [Required(ErrorMessage = "Enter the city name")]        
        public string City { get; set; } = String.Empty;

        [Required(ErrorMessage = "Enter the Country name")]       
        public string Country { get; set; } = String.Empty;

        public List<PropertyItem> PropertyItems { get; set; } = new List<PropertyItem>();
        
        public override string ToString()
        {
            return $"{StreetName} {HouseNumber}, {PostalCode} {City}, {Country}";
        }

    }
}
