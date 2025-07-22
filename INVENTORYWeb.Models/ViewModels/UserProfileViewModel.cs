using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string? Id { get; set; }
        public DateTime? timestamp { get; set; }
        public string? actor { get; set; }
        public string? NIK { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ShortCode { get; set; }
    }
}
