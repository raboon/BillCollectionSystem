using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MtbBillCollectionLibrary.Security.EntityClass;
using MtbBillCollectionLibrary.Utility.DatabaseUtility;
using System.Data;
namespace MtbBillCollection.Utility
{
    public static class Screen
    {
        public static bool IsUserPermitedToAccessScreen(int userType,string pageName)
        {
            string[] pageList=GetPageList(userType);           

            foreach(string pgName in pageList.ToArray<string>())
            {
                if ((pgName).ToLower() == pageName.ToLower())
                    return true;                
            }

            return false;
        }

        public static string[] GetPageList(int userType)
        {
            string[] pageList;
            if (userType == MtbBillCollection.Global.Definitions.UserType.Value.SuperAdmin)
            {
                pageList = MtbBillCollection.Global.Definitions.MappedScreen.Value.SuperAdmin.Split((','));
            }
            else if (userType == MtbBillCollection.Global.Definitions.UserType.Value.Manager)
            {
                pageList = MtbBillCollection.Global.Definitions.MappedScreen.Value.Manager.Split((','));
            }
            else if (userType == MtbBillCollection.Global.Definitions.UserType.Value.Issuer)
            {
                pageList = MtbBillCollection.Global.Definitions.MappedScreen.Value.Issuer.Split((','));
            }
            else
            {
                pageList = MtbBillCollection.Global.Definitions.MappedScreen.Value.Reviewer.Split((','));
            }
            return pageList;
        }

        public static System.Collections.Hashtable LoadAllMenu()
        {
            System.Collections.Hashtable hs = new System.Collections.Hashtable();
            hs.Add("Register", "Register.aspx");
            hs.Add("BillCollection", "BillCollection.aspx");
            hs.Add("AuthorizeBillCollection", "AuthorizeBillCollection.aspx");
            hs.Add("RecoverPassword", "RecoverPassword.aspx");
            hs.Add("ViewCollections", "ViewCollections.aspx");
            hs.Add("CollectionLogReport", "CollectionLogReport.aspx");
            hs.Add("Clearing", "Clearing.aspx");
            hs.Add("PasswordChange", "ChangePassword.aspx");
            hs.Add("MarkForSpeacial", "MarkForSpeacial.aspx");
            return hs;
        }

        public static string GetLinkAddres(System.Collections.Hashtable hs, string pageName)
        {
            return hs[pageName.Trim()].ToString();
        }

        public static string[] GetAllLinkAddress(System.Collections.Hashtable hs, string[] pageList)
        {
            string[] menuList=new string[pageList.Length];
            int i = 0;
            foreach (string page in pageList)
                menuList[i++]=GetLinkAddres(hs, page);
            return menuList;
        }

    }
}
