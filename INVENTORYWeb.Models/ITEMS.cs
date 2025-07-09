using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace INVENTORYWeb.Models
{
    public class ITEMS
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "Nama Item")]
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        //[Display(Name = "Stok")]
        //public int? STOCK { get; set; }
        //[Display(Name = "Qty")]
        //public int? QTY { get; set; }
        [Display(Name = "Keterangan")]
        public string? DESCRIPTION { get; set; }
        [MaxLength(255)]
        public string? CREATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        [Required]
        public long CATEGORY_ID { get; set; }
        [ForeignKey("CATEGORY_ID")]
        [ValidateNever]
        [Display(Name = "Satuan")]
        public CATEGORY CATEGORY { get; set; }
        [Display(Name = "Isi")]
        public int? PIECE { get; set; }

    }
}
