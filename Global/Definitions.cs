using System;


namespace MtbBillCollection.Global.Definitions.ProductType
{
    public sealed class Value
    {
        public const int Prepaid = 1;
        public const int Postpaid = 2;
        public const int Fees = 3;
    }

    public sealed class Name
    {
        public const string Prepaid = "Prepaid";
        public const string Postpaid = "Postpaid";
        public const string Fees = "Fees";
    }
}


namespace MtbBillCollection.Global.Definitions.UserType
{
    public sealed class Value
    {
        public const int SuperAdmin = 1;
        public const int Manager = 2;
        public const int Issuer = 3;
        public const int Reviewer = 4;
    }
}

namespace MtbBillCollection.Global.Definitions.ScreenList
{
    public sealed class Value
    {
        public const int LogIn = 1;
        public const int PasswordChange = 2;
        public const int Register = 3;
        public const int RoleWiseScreenMapping = 4;
        public const int BillCollection= 5;
        public const int AuthorizeBillCollection = 6;
        public const int CollectionClearing = 7;
        public const int CollectionLogReport = 8;        
        public const int ViewCollections = 9;

    }
}

namespace MtbBillCollection.Global.Definitions.CollectionStatus
{
    public sealed class Value
    {
        public const int Received = 1;
        public const int Approved = 2;
        public const int Cancelled = 3;
        public const int SpeacialEdit = 4;
        public const int Cleared = 5;
        public const int All = 6;
    }
}

namespace MtbBillCollection.Global.Definitions.CollectionType
{
    public sealed class Value
    {
        public const int Bill = 1;
        public const int SDP = 2;
        public const int ApplicationFee = 3;
        public const int RenewalFee = 4;
        public const int LateFee = 5;
    }
}

namespace MtbBillCollection.Global.Definitions.InstType
{
    public sealed class Value
    {
        public const int CASH = 1;
        public const int Cheque = 2;
        public const int Po = 3;
        public const int DD = 4;
        public const int ACT = 5;
    }

    public sealed class Name
    {
        public const string CASH = "CASH";
        public const string Cheque = "Cheque";
        public const string Po = "PO";
        public const string DD = "DD";
        public const string ACT = "ACt";
    }
}
namespace MtbBillCollection.Global.Definitions.MatchingCriteria
{
    public sealed class Value
    {
        public const int Equal = 0;
        public const int StartsWith = 1;
        public const int EndsWith = 2;
        public const int Contains = 3;
        public const int NotEqual = 4;
    }
}
namespace MtbBillCollection.Global.Definitions.MappedScreen
{
    public sealed class Value
    {
        public const string SuperAdmin = "Register,PasswordChange,CollectionLogReport,ViewCollections,BillCollection,RecoverPassword,AuthorizeBillCollection";
        public const string Manager = "AuthorizeBillCollection,BillCollection,CollectionLogReport,ViewCollections,Clearing,PasswordChange";
        public const string Issuer = "BillCollection,ViewCollections,PasswordChange";
        public const string Reviewer = "ViewCollections,Clearing,MarkForSpeacial,PasswordChange";
    }   
   
}

namespace MtbBillCollection.Global.Definitions.SessionVariable
{
    public sealed class Value
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string UserTypeId = "UserTypeId";
        public const string IsAdmin = "IsAdmin";
        public const string BranchCode = "BranchCode";
        public const string BranchName = "BranchName";
        public const string IsLoggedIn = "IsLoggedIn";
        public const string ClientId = "ClientId";
        public const string CollectionId = "CollectionId";
        public const string FullName = "FullName";     
        public const string SaveMode = "Mode";  
        public const string PreviousPage = "PreviousPage";    
        public const string CollectionInfo = "CollectionInfo";
        public const string UserList = "UserList";
        public const string CollDBConnectionString = "CollDBConnectionString";
        public const string Screenlist = "Screenlist";
        public const string CollectionReport = "CollectionReport";
        public const string PageList = "PageList";
        public const string MenuList = "MenuList";
          
    }
}
