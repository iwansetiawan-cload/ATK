using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class MasterProjectViewModel
    {
        public IEnumerable<MasterProject> ListData { get; set; }
    }
    public class MasterProject
    {
        public long? IDPROJECT { get; set; }
        public long? IDCUSTOMER { get; set; }
        public string? PROJECT_NAME { get; set; }
    }
}
