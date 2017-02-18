using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtbBillCollectionLibrary.Utility.DatabaseUtility.EntityClass
{
    public class User
    {
        private string _userName="";

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _userPassword="";

        public string Password
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }
    }
}
