using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models
{
    public class CATEGORY
    {
        [Key]
        public long ID { get; set; }
        [Display(Name = "Satuan")]
        [Required]
        [MaxLength(50)]
        public string NAME { get; set; }
        [Display(Name = "Keterangan")]
        public string? DESCRIPTION { get; set; }
    }
}
