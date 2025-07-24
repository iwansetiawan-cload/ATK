using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class ReportRequestItemsViewModel
    {
        public string? searchRequestNumber { get; set; }
        public string? searchProjectName { get; set; }
        public string? searchStatus { get; set; }
        public IEnumerable<REQUEST_ITEM_HEADER>? listData { get; set; }
        public IEnumerable<MSUDC>? listStatus { get; set; }
    }
}
