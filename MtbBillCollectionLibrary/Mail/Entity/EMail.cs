using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtbBillCollectionLibrary.Mail.Entity
{
   public class EMail
    {
        public string SenderName { get; set; }
        public string ReciverName { get; set; }
        public string SMTPServerName{get;set;}
        public bool IsDefaultSecurityEnable{get;set;}
        public string NetworkUserName{get;set;}
        public string NetworkUserPassword{get;set;}        
        public EMessage EmailMessege{get;set;}
    }
}
