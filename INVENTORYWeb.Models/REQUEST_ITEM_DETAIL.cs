using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models
{
    public class REQUEST_ITEM_DETAIL
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public long HEADER_ID { get; set; }
        [ForeignKey("HEADER_ID")]
        [ValidateNever]
        public REQUEST_ITEM_HEADER REQUEST_ITEM_HEADER { get; set; }
        [Required]
        public long ITEM_ID { get; set; }
        [ForeignKey("ITEM_ID")]
        [ValidateNever]
        public ITEMS ITEMS { get; set; }      
        public string? ITEM_NAME { get; set; }    
        public string? CATEGORY { get; set; }
        public int? PIECE { get; set; }
        public int? STOCK { get; set; }
        public int? QTY { get; set; }
        public int? QTY_ADJUST { get; set; }
        public string? REMARK_ADJUST { get; set; }
        public int? STATUS_ID { get; set; }
        public string? STATUS { get; set; }
        public string? APPROVE_BY { get; set; }
        public DateTime? APPROVE_DATE { get; set; }
        public string? REJECTED_BY { get; set; }
        public DateTime? REJECTED_DATE { get; set; }
        public string? REJECTED_NOTES { get; set; }
        public string? COMPLETED_BY { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        public string? COMPLETED_NOTES { get; set; }
    }
}
