using Microsoft.WindowsAzure.Storage.Table;

namespace PortalApp.Models
{
    public class ContactModel:TableEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}