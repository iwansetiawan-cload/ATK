using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class ItemsViewModel
    {
        public ITEMS Items { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SatuanList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> IsiList { get; set; }
    }
}
