using System;
using MtbBillCollectionLibrary.Utility.DatabaseUtility;
using MtbBillCollectionLibrary.Security.EntityClass;
using MtbBillCollectionLibrary.Security.EntityClass.FileType;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MtbBillCollectionLibrary.DAL;
using System.Linq;
namespace MtbBillCollectionLibrary.Security
{
    public class LogManager
    {
        private IDatabaseManager _databaseManager;
        private string _connectionString = "";
        private BillCollDataContext _billCollDataContext = null;
        public string ConnectionString
        {
            set{ 
                _connectionString = value;
                _databaseManager.SetConnectionString = value;
               }
        }
        
        public LogManager()
        {
            _databaseManager = new MssqlDatabaseManager();
        }
        
        public bool UpdateLog(string userId)
        {
            try
            {

                int logId = (from trLogs in _billCollDataContext.TraceLogs
                                where trLogs.UserId == int.Parse(userId)
                                select trLogs.LogId).Max();

                TraceLog trLog = _billCollDataContext.TraceLogs.Single(t=>t.LogId==logId);
                trLog.LogoutTime = DateTime.Now;
                _billCollDataContext.SubmitChanges();
                return true;
                //if(_connectionString.Equals(string.Empty)!=true)
                //{
                //    _databaseManager.OpenDatabaseConnection();
                //    string sqlQuerey="select MAX(LogId) from TraceLog where userId="+userId;
                //    string logId=_databaseManager.GetSingleString(sqlQuerey);
                //    string sqlCommand = " Update TraceLog set LogoutTime =GETDATE() where LogId="+logId;
                //    _databaseManager.ExcecuteCommand(sqlCommand);
                //    sqlCommand = " Update Users set isLoggedIn=0 where UserId="+userId;
                //    _databaseManager.ExcecuteCommand(sqlCommand); 

                //    return true;
                //}

            }catch(Exception exception)
            {
                throw exception;
            }
            finally
            {
                //_databaseManager.CloseDatabaseConnection();
            }

            return false;
        }

        public void InsertLog(string userId)
        {
            try
            {
                _billCollDataContext = new BillCollDataContext();
                TraceLog trLog = new TraceLog();
                trLog.UserId = int.Parse(userId );
                trLog.LoginTime = DateTime.Now;
                _billCollDataContext.TraceLogs.InsertOnSubmit(trLog);
                User user = _billCollDataContext.Users.Single(u=>u.UserId==int.Parse(userId));
                user.isLoggedIn = true;
                _billCollDataContext.SubmitChanges();
                //if (_connectionString.Equals(string.Empty) != true)
                //{
                    //_databaseManager.OpenDatabaseConnection();

                    //string sqlCommand = " INSERT INTO TraceLog (UserId,LoginTime) VALUES ("+userId+",GETDATE())";
                    //_databaseManager.ExcecuteCommand(sqlCommand);
                    //sqlCommand = " Update Users set isLoggedIn=1 where UserId="+userId;
                    //_databaseManager.ExcecuteCommand(sqlCommand);                  
                //}
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                //_databaseManager.CloseDatabaseConnection();
            }
           
        }

        public List<MtbBillCollectionLibrary.Security.EntityClass.FileInfo> GetLogInformation(string path, DateTime fromDate, DateTime toDate, int fileType)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            List<MtbBillCollectionLibrary.Security.EntityClass.FileInfo> listOfFileInfo = new List<MtbBillCollectionLibrary.Security.EntityClass.FileInfo>();
            foreach (System.IO.FileInfo f in dir.GetFiles("*.*"))
            {
                StreamReader sr = null;
                DateTime creationTime = DateTime.Now.Date;
                try
                {
                    if (!DateTime.TryParseExact(f.CreationTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out creationTime))
                        {
                            // Parse failed
                        }
                   
                   if (creationTime >= fromDate && creationTime <= toDate)
                    {
                        MtbBillCollectionLibrary.Security.EntityClass.FileInfo fileInfo = new MtbBillCollectionLibrary.Security.EntityClass.FileInfo();
                        fileInfo.CreationDate = f.CreationTime.ToString("dd/MM/yyyy");
                        fileInfo.CreationTime = f.CreationTime.Hour + " : " + f.CreationTime.Minute;
                        fileInfo.FileName = f.Name;
                        

                        string filePrefix = "100" + f.CreationTime.ToString("ddMMyyyy");
                        if (f.Name.StartsWith(filePrefix))
                        {
                            if (f.Name.Length == 18 && (fileType == MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.Prepaid ||
                                    fileType == MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.All))
                            {
                                sr = new StreamReader(new FileStream(path + "//" + f.Name, FileMode.Open, FileAccess.Read));

                                fileInfo.FileType = MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.Prepaid;
                                fileInfo.FileSlnO = f.Name.Substring(filePrefix.Length, 3);
                                while (!sr.EndOfStream)
                                {
                                    string transactionInfo = sr.ReadLine();
                                    fileInfo.NoOfTrans++;
                                }
                                fileInfo.FileInfoType = "Prepaid";                               
                                listOfFileInfo.Add(fileInfo);
                                
                            }
                            else if (f.Name.Length == 15 && (fileType == MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.DayEnd ||
                                    fileType == MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.All))
                            {
                                sr = new StreamReader(new FileStream(path + "//" + f.Name, FileMode.Open, FileAccess.Read));
                                fileInfo.FileType = MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.DayEnd;
                                fileInfo.FileSlnO = "0";
                                while (!sr.EndOfStream)
                                {
                                    string transactionInfo = sr.ReadLine();
                                    string[] arrayInfo = transactionInfo.Split((';'));
                                    fileInfo.NoOfTrans += Convert.ToInt32("0" + arrayInfo[2]);
                                }
                                fileInfo.FileInfoType = "Day End";
                                listOfFileInfo.Add(fileInfo);
                            }

                        }
                        else 
                        {
                            filePrefix = "10" + f.CreationTime.ToString("ddMMyyyy");
                            if (f.Name.StartsWith(filePrefix) && f.Name.Length == 17 && (fileType == MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.PostPaid ||
                                    fileType == MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.All))
                            {
                                sr = new StreamReader(new FileStream(path + "//" + f.Name, FileMode.Open, FileAccess.Read));

                                fileInfo.FileType = MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.PostPaid;
                                
                                fileInfo.FileSlnO = f.Name.Substring(filePrefix.Length, 3);
                                while (!sr.EndOfStream)
                                {
                                    string transactionInfo = sr.ReadLine();
                                    fileInfo.NoOfTrans++;
                                }
                                fileInfo.FileInfoType = "PostPaid";
                                listOfFileInfo.Add(fileInfo);
                            }
                        }                        

                    }
                }
                catch (Exception exception)
                {
                    EventLog.WriteEntry("*** MTB*** From Log: ", exception.ToString(), EventLogEntryType.Error);
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                }
            }

            return listOfFileInfo;

        }

    }
}
