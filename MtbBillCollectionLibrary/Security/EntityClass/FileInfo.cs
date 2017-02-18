using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtbBillCollectionLibrary.Security.EntityClass.FileType
{
    public sealed class Value
    {
        public const int Prepaid = 1;
        public const int PostPaid = 2;
        public const int DayEnd = 3;
        public const int All = 4;
    }
}

namespace MtbBillCollectionLibrary.Security.EntityClass
{    
  
   public class FileInfo
    {
        public string CreationDate{get;set;}
        public int FileType { get; set; }
        public string FileName { get; set; }
        public string FileSlnO { get; set; }
        public int NoOfTrans { get; set; }
        public string CreationTime { get; set; }
        public string FileInfoType { get; set; }
    }
}
