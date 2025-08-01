using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int? Status_NewRequest { get; set; }        
        public int? Status_ApproveAndReject { get; set; }
        public int? Status_Process { get; set; }
        public int? Status_Complete { get; set; }

        public int? Status_NewRequest_Current { get; set; }
        public int? Status_Approve_Current { get; set; }
        public int? Status_Reject_Current { get; set; }        
        public int? Status_Process_Current { get; set; }
        public int? Status_Complete_Current { get; set; }

        public int TotalRequestTahunan { get; set; }
        public double? Persen_NewRequest_Current { get; set; }
        public double? Persen_Approve_Current { get; set; }
        public double? Persen_Reject_Current { get; set; }
        public double? Persen_Process_Current { get; set; }
        public double? Persen_Complete_Current { get; set; }

        public int? Persen_NewRequest { get; set; }
        public int? Persen_Approve { get; set; }
        public int? Persen_Reject { get; set; }
        public int? Persen_Process { get; set; }
        public int? Persen_Complete { get; set; }


        #region Pergerakan Barang Tahunan
        public string? Item1 { get; set; }  
        public int Jan_Item1 { get; set; }
        public int Feb_Item1 { get; set; }
        public int Mar_Item1 { get; set; }
        public int Apr_Item1 { get; set; }
        public int May_Item1 { get; set; }
        public int Jun_Item1 { get; set; }
        public int Jul_Item1 { get; set; }
        public int Aug_Item1 { get; set; }
        public int Sep_Item1 { get; set; }
        public int Okt_Item1 { get; set; }
        public int Nov_Item1 { get; set; }
        public int Des_Item1 { get; set; }

        public string? Item2 { get; set; }
        public int Jan_Item2 { get; set; }
        public int Feb_Item2 { get; set; }
        public int Mar_Item2 { get; set; }
        public int Apr_Item2 { get; set; }
        public int May_Item2 { get; set; }
        public int Jun_Item2 { get; set; }
        public int Jul_Item2 { get; set; }
        public int Aug_Item2 { get; set; }
        public int Sep_Item2 { get; set; }
        public int Okt_Item2 { get; set; }
        public int Nov_Item2 { get; set; }
        public int Des_Item2 { get; set; }

        public string? Item3 { get; set; }
        public int Jan_Item3 { get; set; }
        public int Feb_Item3 { get; set; }
        public int Mar_Item3 { get; set; }
        public int Apr_Item3 { get; set; }
        public int May_Item3 { get; set; }
        public int Jun_Item3 { get; set; }
        public int Jul_Item3 { get; set; }
        public int Aug_Item3 { get; set; }
        public int Sep_Item3 { get; set; }
        public int Okt_Item3 { get; set; }
        public int Nov_Item3 { get; set; }
        public int Des_Item3 { get; set; }

        public string? Item4 { get; set; }
        public int Jan_Item4 { get; set; }
        public int Feb_Item4 { get; set; }
        public int Mar_Item4 { get; set; }
        public int Apr_Item4 { get; set; }
        public int May_Item4 { get; set; }
        public int Jun_Item4 { get; set; }
        public int Jul_Item4 { get; set; }
        public int Aug_Item4 { get; set; }
        public int Sep_Item4 { get; set; }
        public int Okt_Item4 { get; set; }
        public int Nov_Item4 { get; set; }
        public int Des_Item4 { get; set; }

        public string? Item5 { get; set; }
        public int Jan_Item5 { get; set; }
        public int Feb_Item5 { get; set; }
        public int Mar_Item5 { get; set; }
        public int Apr_Item5 { get; set; }
        public int May_Item5 { get; set; }
        public int Jun_Item5 { get; set; }
        public int Jul_Item5 { get; set; }
        public int Aug_Item5 { get; set; }
        public int Sep_Item5 { get; set; }
        public int Okt_Item5 { get; set; }
        public int Nov_Item5 { get; set; }
        public int Des_Item5 { get; set; }


        #endregion

        public List<REQUEST_ITEM_HEADER>? MonthlyTransactionList { get; set; }
        public List<MSUDC>? mSUDCList { get; set; }
        //public List<Proc_GetDataDashboard>? GetDataDashboard { get; set; }
    }

    public class Proc_GetDataDashboard
    {
        public string? ITEM_NAME { get; set; }
        public int QTY { get; set; }
        public string? MONTH { get; set; }
        public int FLAG { get; set; } = 0;
    }
}
