using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class ReportRequestItemsViewModel
    {        
        public string? request_number { get; set; }
        public string? project_name { get; set; }
        public string? status { get; set; }
        public int? year { get; set; }
        public int? month { get; set; }
        public IEnumerable<REQUEST_ITEM_HEADER>? listData { get; set; }
        public IEnumerable<MSUDC>? listStatus { get; set; }
    }
    public class ExportReportPR
    {
        public long? Id { get; set; }
        public long? Number { get; set; }
        public string? RequestNumber { get; set; }
        public string? ProjectName { get; set; }
        public string? RequestDate { get; set; }
        public string? ItemName { get; set; }
        public string? Catagory { get; set; }
        public string? Piece { get; set; }
        public string? Qty { get; set; }
        public string? Status { get; set; }

    }
}
