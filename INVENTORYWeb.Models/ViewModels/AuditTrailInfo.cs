﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Models.ViewModels
{
    public class AuditTrailInfo
    {
        public string UserName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public long TransactionId { get; set; } = 0;
        public string ActionName { get; set; } = string.Empty;
        public string OtherInfo { get; set; } = string.Empty;
        public int AuditTrailType { get; set; } = 0;
        public string ApplicationId { get; set; } = string.Empty;
        public Guid? AuditTrailId { get; set; } = Guid.Empty;
    }
    public class DataHistoryList
    {
        public IEnumerable<HistoryApproval>? ListData { get; set; }
    }
    public class HistoryApproval
    {
        public string? Status { get; set; }
        public string? EntryBy { get; set; }
        public string? EntryDate { get; set; }
        public string? Notes { get; set; }
    }
}
