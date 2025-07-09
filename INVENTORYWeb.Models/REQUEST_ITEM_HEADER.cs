using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models
{
    public class REQUEST_ITEM_HEADER
    {
        [Key]
        public long ID { get; set; }
        [Required]
        [Display(Name = "Nama Project")]
        public string PROJECT_NAME { get; set; }
        [Display(Name = "Keterangan")]
        public string? NOTES { get; set; }
        public string? REQUESTER { get; set; }
        [Display(Name = "Tanggal Permintaan")]
        public DateTime? REQUEST_DATE { get; set; }
        public double? TOTAL_QTY { get; set; }
        public int? STATUS_ID { get; set; }
        public string? STATUS { get; set; }
        [MaxLength(255)]
        public string? CREATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }           
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
