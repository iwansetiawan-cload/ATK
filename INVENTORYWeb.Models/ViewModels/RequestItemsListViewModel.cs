using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class RequestItemsListViewModel
    {
        public IEnumerable<REQUEST_ITEM_HEADER>? listData { get; set; }
    }
}
