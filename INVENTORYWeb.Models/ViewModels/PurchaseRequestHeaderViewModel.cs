using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class PurchaseRequestHeaderViewModel
    {      

        public string Id { get; set; }
        public DateTime? timestamp { get; set; }
        public string? actor { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public string ProjectName { get; set; }
        public string Company { get; set; }
        public string? Description { get; set; }
        public Int16? IsCancel { get; set; }
        public string? UserProfileId { get; set; }
        public string? ApproverId { get; set; }
    }
}
