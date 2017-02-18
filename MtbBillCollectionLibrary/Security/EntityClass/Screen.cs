using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtbBillCollectionLibrary.Security.EntityClass
{
    public class Screen
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public bool IsManagerPermited { get; set; }
        public bool IsIssuerPermited { get; set; }
        public bool IsReviewerPermited { get; set; }
    }
}
