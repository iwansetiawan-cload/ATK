using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class PurchaseRequestDetailViewModel
    {
        public string? Id { get; set; }
        public DateTime? timestamp { get; set; }
        public string? actor { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public int? ItemQuantity { get; set; }
        public string? ItemUnit { get; set; }
        public string? Brand { get; set; }
        public string? Spesification { get; set; }
        public string? PurchaseRequestHeaderId { get; set; }
    }
}
