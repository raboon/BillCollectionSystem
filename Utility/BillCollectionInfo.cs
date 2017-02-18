using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MtbBillCollection
{
    public class BillCollectionInfo
    {
        public int ColectionId { get; set; }
        public int ClientId { get; set; }
        public string CollDate { get; set; }
        public int ProductId { get; set; }
        public string BranchCode { get; set; }
        public int CollectionTypeId { get; set; }
        public string CollFrom { get; set; }
        public int InstTypeId { get; set; }
        public string InstNumber { get; set; }
        public string InstDate { get; set; }
        public decimal CollAmount { get; set; }
        public int InstBankCode { get; set; }        
        public string Remarks { get; set; }
        public int CollStatus { get; set; }
        public int InstClearedBy { get; set; }
        public int InstRecvdBy { get; set; }
        public int InstApprovedBy { get; set; }
        public string InstApprovedDate { get; set; }
        public string InstClearingDate { get; set; }
        public int Cleared { get; set; }

    }
}