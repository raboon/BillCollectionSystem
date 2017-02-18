using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MtbBillCollectionLibrary.Security.EntityClass;
using MtbBillCollectionLibrary.Utility.DatabaseUtility;
using System.Data;
namespace MtbBillCollectionLibrary.Security
{
    public class ScreenManager
    {
        private IDatabaseManager _databaseManager = new MssqlDatabaseManager();        
        private string _connectionString = "";

        public string ConnectionString
        {
            set
            {
                _connectionString = value;
            }
        }

        public bool AddNewScreen(Screen screen)
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
                    string sqlquery = "INSERT INTO ScreenList( ScreenName, IsSuperAdmin , IsManager , IsIssuer , IsReviewer ) ";
                    sqlquery += " VALUES ('" + screen.ScreenName + "',1," + ((screen.IsManagerPermited == true) ? 1 : 0) + "," ;
                    sqlquery += ((screen.IsIssuerPermited == true) ? 1 : 0) + "," + ((screen.IsReviewerPermited == true) ? 1 : 0) + ")";

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

        public bool UpdateScreenInfo(Screen screen)
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
                    string sqlquery = "UPDATE ScreenList SET  ScreenName  = ";
                    sqlquery += " '" + screen.ScreenName + "', IsManager = " + ((screen.IsManagerPermited == true) ? 1 : 0);
                    sqlquery += ", IsIssuer = " + ((screen.IsIssuerPermited == true) ? 1 : 0) + ", IsReviewer = " + ((screen.IsReviewerPermited == true) ? 1 : 0) + "";
                    sqlquery += " WHERE ScreenId= " + screen.ScreenId;

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

        public bool DeleteScreenInfo(int screenId)
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
                    string sqlquery = "DELETE FROM ScreenList WHERE ScreenId= " + screenId;

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

        public IList<Screen> GetAllScreenInformation()
        {

            MtbBillCollectionLibrary.Utility.ListHandler listHandler = new Utility.ListHandler();
            IList<Screen> listOfScreen ;
             try
            {
                _databaseManager.SetConnectionString = _connectionString; 
                _databaseManager.OpenDatabaseConnection();

                if (_connectionString.Equals(String.Empty))
                {
                    return null;
                }
                else
                {                   
                    string sqlquery = "select ScreenId, ScreenName, IsSuperAdmin, IsManager, IsIssuer, IsReviewer from ScreenList ";
                    listOfScreen=listHandler.ConvertTo<Screen>(_databaseManager.GetDataTable(sqlquery));
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

             return listOfScreen;
        }

        public Screen GetScreenInformation(int screenId)
        {

            MtbBillCollectionLibrary.Utility.ListHandler listHandler = new Utility.ListHandler();
            IList<Screen> listOfScreen;
            try
            {
                _databaseManager.SetConnectionString = _connectionString;
                _databaseManager.OpenDatabaseConnection();

                if (_connectionString.Equals(String.Empty))
                {
                    return null;
                }
                else
                {
                    string sqlquery = "select ScreenId, ScreenName, IsSuperAdmin, IsManager, IsIssuer, IsReviewer from ScreenList where ScreenId=  " + screenId;
                    listOfScreen = listHandler.ConvertTo<Screen>(_databaseManager.GetDataTable(sqlquery));
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

            return listOfScreen.First<Screen>();
        }


        public static object GetPageList()
        {
            throw new NotImplementedException();
        }
    }
}
