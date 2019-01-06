using System.ComponentModel.DataAnnotations;

namespace PortalApp.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(80, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 4)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(400)]
        public string Message { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}