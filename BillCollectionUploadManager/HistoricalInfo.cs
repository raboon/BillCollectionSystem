using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillCollectionUploadManager
{
   public class HistoricalInfo
    {
       public string ProductType { get; set; }
        public string LastRunDate   {  get;    set; }
        public int LastFileSerialNo { get; set; }
        public int LastTransSerialNo { get; set; }

    }
}
