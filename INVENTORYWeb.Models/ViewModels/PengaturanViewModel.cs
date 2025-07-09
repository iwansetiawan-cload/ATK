using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class PengaturanViewModel
    {
        public string? nama_lengkap { get; set; }
        public string? user_name { get; set; }
        public IEnumerable<ApplicationUser> ListData { get; set; }
    }
}
