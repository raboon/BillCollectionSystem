using MtbBillCollectionLibrary.Security;

namespace MtbBillCollection
{
    public static class WebConfigManager
    {

        public static string GetCollDBConnString()
        {
            return MtbBillCollection.Global.Utility.DecryptString(System.Configuration.ConfigurationManager.AppSettings["CollDBConnectionString"]);
        }

        public static string GetSMTPServername()
        {            
            return System.Configuration.ConfigurationManager.AppSettings["SMTP"];
        }

        public static string GetSenderEmailAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Sender"];
        }

        public static string GetNetWorkUserName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["NetworkUser"];
        }

        public static string GetNetWorkUserPass()
        {            
            return System.Configuration.ConfigurationManager.AppSettings["NetworkUserPass"];
        }

        public static string GetActivationLink()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ActivationLink"];
        }
    }
}