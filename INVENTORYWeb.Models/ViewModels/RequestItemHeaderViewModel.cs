using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class RequestItemHeaderViewModel
    {
        public REQUEST_ITEM_HEADER REQUEST_ITEM_HEADER { get; set; }
        public REQUEST_ITEM_DETAIL REQUEST_ITEM_DETAIL { get; set; }
        public List<REQUEST_ITEM_DETAIL>? ListItems { get; set; }
        public int? GetIdTemp { get; set; }
    }
}
