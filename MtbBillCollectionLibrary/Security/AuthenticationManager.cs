using System;
using System.Collections.Generic;
using MtbBillCollectionLibrary.Utility.DatabaseUtility;
using System.Data;
using MtbBillCollectionLibrary.DAL;
using System.Linq;
namespace MtbBillCollectionLibrary.Global.Definitions.UserType
{
    public sealed class Value
    {
        public const int SuperAdmin = 1;
        public const int Manager = 2;
        public const int Issuer = 3;
        public const int Reviewer = 4;
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
namespace MtbBillCollectionLibrary.Security
{
    public class AuthenticationManager
    {        

        private IDatabaseManager _databaseManager = new MssqlDatabaseManager();
        private EncryptionDecryptionManager encryptionDecriptionManager = new EncryptionDecryptionManager();
        private BillCollDataContext _billCollDataContext=null;
        private string _connectionString = "";
        
        public string ConnectionString
        {
            set
            {
                _connectionString = value;
            }
        }

        public bool IsValidUser(ref string userId,ref string userName, ref bool isActive, ref bool isAdmin, ref int userTypeId, ref string branchCode, ref string branchName, string loginName, string password)
        {
            try
            {
                _billCollDataContext = new BillCollDataContext();
                User user = new User();              
                password = encryptionDecriptionManager.ComputeHash(password, new byte[password.Length]);
                isAdmin = false;
                if (_connectionString.Equals(String.Empty))
                {
                    return false;
                }
                else
                {
                    var reqUser = _billCollDataContext.Users.Single(u => u.LoginName == loginName && u.Password == password);
                    
                    //var reqUser = from user in _billCollDataContext.Users
                    //                join branch in _billCollDataContext.BranchLists on user.BranchCode equals branch.branch_code
                    //                where user.LoginName == loginName && user.Password == password
                    //                select new
                    //                {
                    //                    user,branch.branch_name                                        
                    //                };                                             
                   if(reqUser!=null)
                   {
                        userId = ""+reqUser.UserId;
                        userName = reqUser.UserName;
                        isAdmin =reqUser.isAdmin;
                        isActive = reqUser.isActive;
                        userTypeId = reqUser.UserTypeId;
                        branchCode = reqUser.BranchCode;
                        branchName = reqUser.BranchList.branch_name;
                        return true;
                   }
                   //foreach (var v in reqUser)
                   // {
                        
                   //     userId = v.user.UserId;
                   //     userName = v.user..UserName;
                   //     isAdmin = v.user..isAdmin;
                   //     isActive = v.user..isActive;
                   //     userTypeId = v.user..UserTypeId;
                   //     branchCode = v.user.BranchCode;
                   //     branchName = v.branch_name;
                   //     return true;
                   // }
                                   
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }           
            return false;
        }

        public bool IsUserAllowedToPerformOperation(int userTypeId ,int screenId)
        {
            ScreenManager screenManager = new ScreenManager();
            screenManager.ConnectionString = _connectionString;
            MtbBillCollectionLibrary.Security.EntityClass.Screen screen = screenManager.GetScreenInformation(screenId);
            if((MtbBillCollectionLibrary.Global.Definitions.UserType.Value.SuperAdmin==userTypeId)
                || (MtbBillCollectionLibrary.Global.Definitions.UserType.Value.Manager==userTypeId && screen.IsManagerPermited==true)
                || (MtbBillCollectionLibrary.Global.Definitions.UserType.Value.Issuer==userTypeId && screen.IsIssuerPermited==true)
                || (MtbBillCollectionLibrary.Global.Definitions.UserType.Value.Reviewer==userTypeId && screen.IsReviewerPermited==true))
                return true;           
            return false;
        }

        public void ChangePassword(string userId,string oldPassword,string newPassword)
        {
            try
            {               
                _billCollDataContext = new BillCollDataContext();
                if (!_connectionString.Equals(String.Empty))
                {
                    newPassword = encryptionDecriptionManager.ComputeHash(newPassword, new byte[newPassword.Length]);
                    oldPassword = encryptionDecriptionManager.ComputeHash(oldPassword, new byte[oldPassword.Length]);
                    //List<User> listUser = (from user in _billCollDataContext.Users
                    //                         where user.LoginName == userId && user.Password == oldPassword
                    //                         select user).ToList();

                    var user=_billCollDataContext.Users.Single(u=> u.LoginName==userId && u.Password==oldPassword);
                    user.Password=newPassword;
                    _billCollDataContext.SubmitChanges();

                   
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
                      
        }

        public bool IsActiveUser(string userId, string password)
        {
            try
            {             
                _billCollDataContext = new BillCollDataContext();
                if (_connectionString.Equals(String.Empty))
                {
                    return false;
                }
                else
                {
                    
                    var user = _billCollDataContext.Users.Single(u => u.LoginName == userId && u.Password == password);
                    return user.isActive;
                }
            }
            catch
            {
                return false;
            }
            
        }

        public void CreteNewUser(User user)
        {
            try
            {
                _databaseManager.SetConnectionString = _connectionString;
                _databaseManager.OpenDatabaseConnection();
                user.Password = encryptionDecriptionManager.ComputeHash(user.Password, new byte[user.Password.Length]);             
                _billCollDataContext = new BillCollDataContext();
                _billCollDataContext.Users.InsertOnSubmit(user);
                _billCollDataContext.SubmitChanges();
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }         
        }

        public bool getCustomerDetails(string custCode, ref string custName, ref string custAddress)
        {
            try
            {
                _databaseManager.SetConnectionString = _connectionString;
                _databaseManager.OpenDatabaseConnection();

                if (_connectionString.Equals(String.Empty))
                {
                    return false;
                }
                else
                {
                    string sqlquery = "select acname,pre_add from cus_ac_1 "
                                + " where customer='" + custCode + "'";

                    System.Data.DataTable dt = _databaseManager.GetDataTable(sqlquery);
                    if (dt.Rows.Count > 0)
                    {
                        custName = dt.Rows[0]["acname"].ToString();
                        custAddress = dt.Rows[0]["pre_add"].ToString(); ;

                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            return false;
        }

        public List<BranchList> GetBranchList()
        {
            try
            {         
                _billCollDataContext = new BillCollDataContext();
                return _billCollDataContext.BranchLists.OrderBy(b => b.branch_name).ToList();            
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return null;
        }

        public List<UserType> GetUserTypeList()
        {
            try
            {
                _billCollDataContext = new BillCollDataContext();
                return _billCollDataContext.UserTypes.OrderBy(ut => ut.UserTypeId).ToList();
            }
            catch
            {
                return null;
            }
            
        }

        public List<User> GetUserDetails(string userId, string branchCode, string userType, string machingCriteria)
        {
            _billCollDataContext=new BillCollDataContext();
            List<User> listUser = (from user in _billCollDataContext.Users
                          where ((((userId != "" && userId.ToLower() != "all")
                                      && (
                                          (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.Equal.ToString() && user.LoginName == userId)
                                          || (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.EndsWith.ToString() && user.LoginName.StartsWith(userId))
                                          || (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.StartsWith.ToString() && user.LoginName.EndsWith(userId))
                                          || (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.Equal.ToString() && user.LoginName.Contains(userId))
                                         )
                                     ) || (userId == "" || userId.ToLower() == "all")
                                    )
                                    && ((branchCode.ToLower() != "all" && user.BranchCode == branchCode) || (branchCode.ToLower() == "all"))
                                    && ((userType.ToLower() != "all" && user.UserType.TypeName == userType) || (userType.ToLower() == "all"))
                                 )
                          select user ).ToList();
            return listUser;
            //string searchCriteria=" where ";
            //if (userId != "" && userId.ToLower() != "all")
            //{
            //    if(machingCriteria==MtbBillCollection.Global.Definitions.MatchingCriteria.Value.Equal.ToString())
            //    {
            //        searchCriteria +=" LoginName ='"+userId+"'";
            //    }
            //    else if (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.EndsWith.ToString())
            //    {
            //        searchCriteria +=" LoginName Like '%"+userId+"'";
            //    }
            //    else if (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.StartsWith.ToString())
            //    {
            //        searchCriteria += " LoginName Like '" + userId + "%'";
            //    }
            //    else if (machingCriteria == MtbBillCollection.Global.Definitions.MatchingCriteria.Value.Contains.ToString())
            //    {
            //        searchCriteria += " LoginName Like '%" + userId + "%'";
            //    }
            //}
            //if(branchCode.ToLower() != "all")
            //{
            //    if(searchCriteria.Trim().ToLower()!="where")
            //        searchCriteria += " AND BranchCode ='" + branchCode + "'";
            //    else
            //        searchCriteria += " BranchCode ='" + branchCode + "'";
            //}
            //if(userType.ToLower() != "all")
            //{
            //    if(searchCriteria.Trim().ToLower()!="where")
            //        searchCriteria += " AND u.UserTypeId =" + userType;
            //    else
            //        searchCriteria += " u.UserTypeId =" + userType;
            //}
            //try
            //{
            //    _databaseManager = new MssqlDatabaseManager();
            //    _databaseManager.SetConnectionString = _connectionString;
            //    _databaseManager.OpenDatabaseConnection();
            //    string sqlQuery = "SELECT UserId,LoginName,UserName,UserDetails,BranchCode,ut.UserType ";      
            //    sqlQuery +=" ,(case when isActive=1 Then 'Active' else 'InActive' end) as Active ";
            //    sqlQuery += "  FROM Users u INNER JOIN UserType ut on u.UserTypeId=ut.UserTypeId ";
            //    sqlQuery += (searchCriteria.Trim().ToLower()!="where")?searchCriteria:"";
            //    return _databaseManager.GetDataTable(sqlQuery);
            //}
            //catch
            //{
            //    return null;
            //}
            //finally
            //{
            //    _databaseManager.CloseDatabaseConnection();
            //}
            
        }

        public bool ChangeUserpassword(string loginName,string newPassword)
        {           
          try
            {
                _databaseManager.SetConnectionString = _connectionString;
                _databaseManager.OpenDatabaseConnection();

                if (_connectionString.Equals(String.Empty))
                {
                    return false;
                }
                else
                {
                    string newPass = encryptionDecriptionManager.ComputeHash(newPassword, new byte[newPassword.Length]);
                    string sqlquery = "update Users set Password='" + newPass + "' where LoginName='" + loginName + "'";
                    _databaseManager.ExcecuteCommand(sqlquery);
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            return false;
        }

    }
}
