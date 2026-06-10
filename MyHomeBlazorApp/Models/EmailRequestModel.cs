using System.ComponentModel.DataAnnotations;

namespace MyHomeBlazorApp.Models
{
    public class EmailRequestModel
    {
        [Required(ErrorMessage = "The 'To' field is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "The 'Subject' field is required.")]
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
