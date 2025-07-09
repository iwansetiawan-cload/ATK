using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models
{
    public class MSUDC
    {
        [Key]
        public long ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string ENTRY_KEY { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? TEXT1 { get; set; }
        [MaxLength(255)]
        public string? TEXT2 { get; set; }
        [MaxLength(255)]
        public string? TEXT3 { get; set; }
        public int? INUM1 { get; set; }
        public int? INUM2 { get; set; }
        public double? MNUM1 { get; set; }
        public double? MNUM2 { get; set; }
        [MaxLength(100)]
        public string? CREATOR { get; set; }
        [MaxLength(100)]
        public string? LAST_MODIFY_USER { get; set; }
        public DateTime? LAST_MODIFY_DATE { get; set; }
    }
}
