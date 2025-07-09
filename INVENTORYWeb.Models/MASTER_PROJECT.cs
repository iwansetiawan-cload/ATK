using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models
{
    public class MASTER_PROJECT
    {
        [Key]
        public long ID { get; set; }
        [Display(Name = "Nama Proyek")]
        [Required]
        [MaxLength(250)]
        public string PROJECT_NAME { get; set; }
        [MaxLength(50)]
        public string? ENTRYBY { get; set; }
        public DateTime? ENTRYDATE { get; set; }
    }
}
