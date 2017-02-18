using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MtbBillCollectionLibrary.Utility.DatabaseUtility;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;

namespace BillCollectionUploadManager
{
    public class RealTimeBillCollManager
    {
        System.Configuration.Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private string _bankCode;
        private string _date = DateTime.Today.ToShortDateString(), _timeIntFileName = "", _dayEndFileName = "";
        private IDatabaseManager _databaseManager;
        int _serialNo = 1;
        int _previousdate = -1, _currentHour = -1, interval =0;
        DateTime _currentTime = DateTime.Now.Date;
        bool IsRun = false;
        string _customerCode = "", sSource = "MTB Bill Collection Module";
        Thread _thread;
        HistoricalInfo _prepaidHistoricalInfo, _postpaidHistoricalInfo;        
        MtbBillCollectionLibrary.Utility.XmlHandler _xmlHandler = new MtbBillCollectionLibrary.Utility.XmlHandler();
        int _bankStart = 0, _interval=0,_bankEnd = 6; 
        //Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["EndTime"].ToString());
        //string _historicalInfoPathLoc = "D:/dump/HistoricalInfo.XML";
        string _historicalInfoPathLoc = "";
        SqlConnection _sqlConnection;
        SqlCommand _sqlCommand = new SqlCommand();
        public enum ProductType
        {
            Prepaid,
            PostPaid
        }

        public RealTimeBillCollManager()
        {
            EventLog.WriteEntry(sSource, "Constructor code...", EventLogEntryType.Information);
            string temp = "";
            _bankStart = Convert.ToInt32(cfg.AppSettings.Settings["StartTime"].Value.ToString());            
            _bankEnd = Convert.ToInt32(cfg.AppSettings.Settings["EndTime"].Value.ToString());
            _historicalInfoPathLoc = cfg.AppSettings.Settings["HistoricalInfo"].Value.ToString();
            _interval = (short)Convert.ToUInt16(cfg.AppSettings.Settings["Interval"].Value.ToString());
            _xmlHandler = new MtbBillCollectionLibrary.Utility.XmlHandler();
            EventLog.WriteEntry(sSource, "_bankStart : " + _bankStart, EventLogEntryType.Information);
            //Console.WriteLine("Initializing.");
            Initialize();
            _thread = new Thread(RunBillCollInfoProcess);
            //Console.WriteLine("Thread Started.");
            _thread.Start(); 
            //UploadTimeIntervalFile();
            //FilterForTest();            
            EventLog.WriteEntry(sSource, "Thread Started", EventLogEntryType.Information);
        }

        public void RunBillCollInfoProcess()
        {
            while (true)
            {
                _currentTime = DateTime.Now;
                EventLog.WriteEntry(sSource, "Current Time: " + _currentTime, EventLogEntryType.Information);
                _currentHour = _currentTime.Hour;
                EventLog.WriteEntry(sSource, "Current Hour: " + _currentHour + " Bank End: " + _bankEnd, EventLogEntryType.Information);

                UploadTimeIntervalFileForPrePaid();                
                UploadTimeIntervalFileForPostPaid();

                EventLog.WriteEntry(sSource, "Checking Bank End : " + (_currentHour >= _bankEnd), EventLogEntryType.Information);
                if (_currentHour >= _bankEnd)
                {
                    EventLog.WriteEntry(sSource, "Start Day End file Uploading .... : " , EventLogEntryType.Information);
                    UploadDayEndFile(MtbBillCollection.Definitions.ProductType.Value.Prepaid);                   
                    UploadDayEndFile(MtbBillCollection.Definitions.ProductType.Value.Postpaid);
                    EventLog.WriteEntry(sSource, " *** Day End file Uploaded *** " , EventLogEntryType.Information);
                    EventLog.WriteEntry(sSource, " *** Thread Sleep for next day.. *** " , EventLogEntryType.Information);
                    Thread.Sleep((24 - (_bankEnd - _bankStart)) * 60 * 60 * 1000);
                    EventLog.WriteEntry(sSource, " *** Thread wake up on bank start time *** " , EventLogEntryType.Information);
                }
                else
                {
                    SaveHistoricalData();
                    EventLog.WriteEntry(sSource, " *** Thread Sleep for next interval.. *** ", EventLogEntryType.Information);
                    Thread.Sleep(_interval * 60 * 1000);
                    EventLog.WriteEntry(sSource, " *** Thread wake up after interval.. *** ", EventLogEntryType.Information);
                }                
            }
        }

        public void Initialize()
        {
            EventLog.WriteEntry(sSource, "Initialized Starting...", EventLogEntryType.Information);
            _bankCode = Convert.ToUInt32(cfg.AppSettings.Settings["BankCode"].Value.ToString()).ToString("000");
            _dayEndFileName = DateTime.Today.ToString("ddMMyyyy");
            _databaseManager = new MssqlDatabaseManager();
            _databaseManager.SetConnectionString = cfg.AppSettings.Settings["ConnectionString"].Value.ToString();
            GetHistoricalInformation();
            _interval = Convert.ToInt16(cfg.AppSettings.Settings["Interval"].Value.ToString());
            //_bankEnd = (_bankEnd < 12) ? _bankEnd + 12 : _bankEnd;
            EventLog.WriteEntry(sSource, "Initialized done...", EventLogEntryType.Information);            
        }       

        private bool Upload(string ftpServer, string userName, string password, string filename)
        {
            try
            {
                EventLog.WriteEntry(sSource, "Trying to upload...", EventLogEntryType.Information);

                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.UploadFile(ftpServer + "/" + new FileInfo(filename).Name, "STOR", filename);
                }

                EventLog.WriteEntry(sSource, "Uploaded...", EventLogEntryType.Information);
                return true;
            }
            catch (Exception excetion)
            {
                EventLog.WriteEntry(sSource, "Upload fail: "+excetion+" FileName: " +filename, EventLogEntryType.Error);
                return false;
            }
        }

        private void UploadTimeIntervalFileForPrePaid()
        {
            string ftpTimeIntPathLoc = cfg.AppSettings.Settings["FtpTimeIntPathLoc"].Value.ToString();
            string loacalTimeIntFileFullPathLoc = cfg.AppSettings.Settings["LocalTimeIntPathLoc"].Value.ToString();
            loacalTimeIntFileFullPathLoc += _bankCode + _dayEndFileName + _prepaidHistoricalInfo.LastFileSerialNo.ToString("000") + ".txt";
            //ftpTimeIntPathLoc += _bankCode + _dayEndFileName + _prepaidHistoricalInfo.LastFileSerialNo.ToString("000") + ".txt";
            FileStream fs = null;
            StreamWriter sw = null;
            DataTable dt = new DataTable();
            _customerCode = cfg.AppSettings.Settings["CustomerCode"].Value.ToString();
            try
            {
                EventLog.WriteEntry(sSource, "Getting Upload Data....", EventLogEntryType.Information);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = cfg.AppSettings.Settings["ConnectionString"].Value;
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFile: Before Opening Database Conn.", EventLogEntryType.Information);
                _databaseManager.OpenDatabaseConnection();
                
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFile: Before Excecuting Query.", EventLogEntryType.Information);

                string sqlQuery = "SELECT CollectionId,CollDate, BranchCode, CollFrom as PsCodeOrMobNo, it.InstTypeShortName ";
                sqlQuery += " ,InstNumber,InstDate,'' as Debit,CollAmount, blist.bankName,p.ProductName,Remarks ";
                sqlQuery += " FROM  Collection col Left join BankList blist on blist.BankId=col.InstBankCode ";
                sqlQuery += " Inner join Clients cl on cl.ClientId=col.ClientId ";
                sqlQuery += " Inner join Product p on p.ProductID=col.ProductId and p.ClientId=col.ClientId";
                sqlQuery += " Inner join InstrumentType it ";
                sqlQuery += " on it.InstTypeId=col.InstTypeId and it.Active=1 ";                               
                sqlQuery += " where uploaded=0 and cl.CustomerCode='" + _customerCode + "' and p.ProductID =" + MtbBillCollection.Definitions.ProductType.Value.Prepaid;
                sqlQuery += " and CollDate <='" + _currentTime.ToString() + "' ";

                
                string collectionStatus=cfg.AppSettings.Settings["CollectionStatus"].Value.ToString();
                if(collectionStatus!="")
                {
                    if (Convert.ToInt32(collectionStatus) == MtbBillCollection.Definitions.CollectionStatus.Value.Approved)
                    {
                        sqlQuery += " and  CollStatusId = " + MtbBillCollection.Definitions.CollectionStatus.Value.Approved;
                    }
                    else
                    {
                        sqlQuery += " and  CollStatusId <> " + MtbBillCollection.Definitions.CollectionStatus.Value.Cancelled;
                    }
                }
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPrepaid: Excecuting this Query \n " + sqlQuery, EventLogEntryType.Information);
                dt = _databaseManager.GetDataTable(sqlQuery);
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPrepaid: After Excecuting Query.", EventLogEntryType.Information);
                //DataTable dt = _databaseManager.GetDataTable("sp_deposit_cus_statement '"+accountNo+"', '"+accountNo+"','"+_currentTime.Subtract(new TimeSpan(0, interval, 0)).ToString()+"','"+ _currentTime.ToString()+"','N','N','N','N'");
                if (dt.Rows.Count > 0)
                {
                    EventLog.WriteEntry(sSource, "insert LogForFileUpload: " + _bankCode + _dayEndFileName + _prepaidHistoricalInfo.LastFileSerialNo.ToString("000"), EventLogEntryType.Information);
                    UpdateLogForFileUpload(_bankCode + _dayEndFileName + _prepaidHistoricalInfo.LastFileSerialNo.ToString("000"), false, DateTime.Now);
                    fs = new FileStream(loacalTimeIntFileFullPathLoc, FileMode.OpenOrCreate, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        string rowValue = "";

                        foreach (DataColumn dtColumn in dt.Columns)
                        {
                            if (dtColumn.ToString() == "CollDate" || dtColumn.ToString() == "InstDate")
                            {
                                rowValue += Convert.ToDateTime(dtRow[dtColumn].ToString()).Date.ToString("dd-MM-yyyy");
                            }
                            else if (dtColumn.ToString() == "CollectionId")
                            {
                                rowValue += _prepaidHistoricalInfo.LastTransSerialNo;
                            }
                            else if (dtColumn.ToString() == "CollAmount")
                            {
                                rowValue += (int)Convert.ToDecimal(dtRow[dtColumn].ToString());                               
                            }
                            else if (dtColumn.ToString() == "ProductName")
                            {
                                rowValue += "";
                            }
                            else
                            {
                                rowValue += dtRow[dtColumn];
                            }
                            rowValue += (dt.Columns.IndexOf(dtColumn) <= (dt.Columns.Count - 2)) ? ";" : "";
                        }
                        _prepaidHistoricalInfo.LastTransSerialNo++;
                        sw.WriteLine(rowValue);
                    }

                    EventLog.WriteEntry(sSource, "Local file created...For Prepaid", EventLogEntryType.Information);

                    _prepaidHistoricalInfo.LastFileSerialNo++;
                    SaveHistoricalData();

                }
                else
                {
                    EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPrepaid: No Data found...", EventLogEntryType.Information);
                }

            }
            catch (Exception exception)
            {

                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPrepaid: Error on geting data: " + exception.StackTrace, EventLogEntryType.Error);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    fs.Close();
                }

                _databaseManager.CloseDatabaseConnection();
            }
            
            if (dt.Rows.Count > 0 && Upload(ftpTimeIntPathLoc, cfg.AppSettings.Settings["UserName"].Value.ToString(), cfg.AppSettings.Settings["Password"].Value.ToString(), loacalTimeIntFileFullPathLoc))
            {
                EventLog.WriteEntry(sSource, "insert LogForFileUpload: " + _bankCode + _dayEndFileName + (_prepaidHistoricalInfo.LastFileSerialNo - 1).ToString("000"), EventLogEntryType.Information);
                UpdateLogForFileUpload(_bankCode + _dayEndFileName + (_prepaidHistoricalInfo.LastFileSerialNo-1).ToString("000"), true, DateTime.Now);
                EventLog.WriteEntry(sSource, "Data uploaded successfully For Prepaid...", EventLogEntryType.Information);
                UpdateCollectioninfo(dt);
                EventLog.WriteEntry(sSource, "Updated colection upload point For Prepaid..", EventLogEntryType.Information);
            }
            
        }

        private void UploadTimeIntervalFileForPostPaid()
        {
            string ftpTimeIntPathLoc = cfg.AppSettings.Settings["FtpTimeIntPathLoc"].Value.ToString();
            string loacalTimeIntFileFullPathLoc = cfg.AppSettings.Settings["LocalTimeIntPathLoc"].Value.ToString();
            loacalTimeIntFileFullPathLoc += _bankCode.Substring(0,2) + _dayEndFileName + _postpaidHistoricalInfo.LastFileSerialNo.ToString("000") + ".txt";
            //ftpTimeIntPathLoc += _bankCode + _dayEndFileName + _serialNo.ToString("000") + ".txt";
            FileStream fs = null;
            StreamWriter sw = null;
            DataTable dt = new DataTable();
            _customerCode = cfg.AppSettings.Settings["CustomerCode"].Value.ToString();
            try
            {

                EventLog.WriteEntry(sSource, "Getting Upload Data....", EventLogEntryType.Information);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = cfg.AppSettings.Settings["ConnectionString"].Value;
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid: Before DB Conn Open.", EventLogEntryType.Information);
                _databaseManager.OpenDatabaseConnection();

                decimal currentBalance = 0;
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid: Getting Current Balance ", EventLogEntryType.Information);

                currentBalance = GetCurrentBalance(MtbBillCollection.Definitions.ProductType.Value.Postpaid);

                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid: Before executing Query.", EventLogEntryType.Information);
                string sqlQuery = "SELECT CollectionId,CollDate, BranchCode, CollFrom as PsCodeOrMobNo,ct.CollectionType ";
                sqlQuery += " ,'',CollAmount";
                sqlQuery += " FROM  Collection col Left join BankList blist on blist.BankId=col.InstBankCode ";
                sqlQuery += " Inner join Clients cl on cl.ClientId=col.ClientId ";
                sqlQuery += " Inner join Product p on p.ProductID=col.ProductId and p.ClientId=col.ClientId";
                sqlQuery += " Inner join InstrumentType it ";
                sqlQuery += " on it.InstTypeId=col.InstTypeId and it.Active=1 ";
                sqlQuery += " Inner Join  CollectionMapping cm on cm.ClientId=col.ClientId "; 
                sqlQuery += "  Inner join CollectionType ct on ct.CollectionTypeId=col.CollectionTypeId and ct.isActive=1 and cm.CollectionTypeId=col.CollectionTypeId ";
                sqlQuery += " where uploaded=0 and cl.CustomerCode='" + _customerCode + "' and p.ProductID = " + MtbBillCollection.Definitions.ProductType.Value.Postpaid;
                sqlQuery += " and CollDate <= '" + _currentTime.ToString() + "'";

                string collectionStatus = cfg.AppSettings.Settings["CollectionStatus"].Value.ToString();
                if (collectionStatus != "")
                {
                    if (Convert.ToInt32(collectionStatus) == MtbBillCollection.Definitions.CollectionStatus.Value.Approved)
                    {
                        sqlQuery += " and  CollStatusId = " + MtbBillCollection.Definitions.CollectionStatus.Value.Approved;
                    }
                    else
                    {
                        sqlQuery += " and  CollStatusId <> " + MtbBillCollection.Definitions.CollectionStatus.Value.Cancelled;
                    }
                }

                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid: Excecuting this Query \n " + sqlQuery, EventLogEntryType.Information);

                dt = _databaseManager.GetDataTable(sqlQuery);
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid: After executing Query", EventLogEntryType.Information);
                //DataTable dt = _databaseManager.GetDataTable("sp_deposit_cus_statement '"+accountNo+"', '"+accountNo+"','"+_currentTime.Subtract(new TimeSpan(0, interval, 0)).ToString()+"','"+ _currentTime.ToString()+"','N','N','N','N'");
                if (dt.Rows.Count > 0)
                {
                    EventLog.WriteEntry(sSource, "Insert LogForFileUpload: " + _bankCode.Substring(0, 2) + _dayEndFileName + _postpaidHistoricalInfo.LastFileSerialNo.ToString("000"), EventLogEntryType.Information);
                    UpdateLogForFileUpload(_bankCode.Substring(0, 2) + _dayEndFileName + _postpaidHistoricalInfo.LastFileSerialNo.ToString("000"), false, DateTime.Now);
                    fs = new FileStream(loacalTimeIntFileFullPathLoc, FileMode.OpenOrCreate, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        string rowValue = "";


                        foreach (DataColumn dtColumn in dt.Columns)
                        {
                            if (dtColumn.ToString() == "CollDate")
                            {
                                rowValue += Convert.ToDateTime(dtRow[dtColumn].ToString()).Date.ToString("dd-MM-yyyy");
                            }
                            else if (dtColumn.ToString() == "CollectionId")
                            {
                                rowValue += _postpaidHistoricalInfo.LastTransSerialNo;
                            }
                            else if (dtColumn.ToString() == "CollAmount")
                            {
                                decimal collAmount = (int)Convert.ToDecimal(dtRow[dtColumn].ToString());
                                rowValue += collAmount;
                                currentBalance += collAmount;
                                rowValue += (";" + currentBalance);
                            }
                            else
                            {
                                rowValue += dtRow[dtColumn];
                            }
                            rowValue += (dt.Columns.IndexOf(dtColumn) <= (dt.Columns.Count - 2)) ? ";" : "";
                        }
                        _postpaidHistoricalInfo.LastTransSerialNo++;
                        sw.WriteLine(rowValue);
                    }

                    UpdateCurrentBalance(currentBalance, MtbBillCollection.Definitions.ProductType.Value.Postpaid, ref sqlQuery);

                    EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid:Local file created...", EventLogEntryType.Information);

                    _postpaidHistoricalInfo.LastFileSerialNo++;
                    SaveHistoricalData();
                }
                else
                {
                    EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid:No Data found...", EventLogEntryType.Information);
                }

            }
            catch (Exception exception)
            {

                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid: Error on geting data: " + exception.Message, EventLogEntryType.Error);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    fs.Close();
                }

                _databaseManager.CloseDatabaseConnection();
            }

            
            if (dt.Rows.Count > 0 && Upload(ftpTimeIntPathLoc, cfg.AppSettings.Settings["UserName"].Value.ToString(), cfg.AppSettings.Settings["Password"].Value.ToString(), loacalTimeIntFileFullPathLoc))
            {
                EventLog.WriteEntry(sSource, "UpdateLogForFileUpload: " + _bankCode.Substring(0, 2) + _dayEndFileName + (_postpaidHistoricalInfo.LastFileSerialNo - 1).ToString("000"), EventLogEntryType.Information);
                UpdateLogForFileUpload(_bankCode.Substring(0, 2) + _dayEndFileName + (_postpaidHistoricalInfo.LastFileSerialNo-1).ToString("000"), true, DateTime.Now);
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid:Data uploaded successfully...", EventLogEntryType.Information);
                UpdateCollectioninfo(dt);
                EventLog.WriteEntry(sSource, "UploadTimeIntervalFileForPostPaid:Updated colection upload point..", EventLogEntryType.Information);
            }
            

        }

        private void UploadDayEndFile(int productType)
        {            
            
            //-----------------------------------------------------------------------------------------------------
            string ftpDayEndPathLoc="", loacalDayEndFileFullPathLoc = "";
            FileStream fs = null;
            StreamWriter sw = null;

            ftpDayEndPathLoc = cfg.AppSettings.Settings["FtpDayEndPathLoc"].Value;            
            loacalDayEndFileFullPathLoc = cfg.AppSettings.Settings["LocalDayEndPathLoc"].Value.ToString();

            if (productType == MtbBillCollection.Definitions.ProductType.Value.Prepaid)
            { 
                loacalDayEndFileFullPathLoc += _bankCode + _dayEndFileName + ".txt";
                EventLog.WriteEntry(sSource, "Uploading Day End File For Prepaid: File Name -> " + loacalDayEndFileFullPathLoc, EventLogEntryType.Information);
            }
            else
            {
                loacalDayEndFileFullPathLoc += _bankCode.Substring(0, 2) + _dayEndFileName + ".txt";
                EventLog.WriteEntry(sSource, "Uploading Day End File For Postpaid: File Name -> " + loacalDayEndFileFullPathLoc, EventLogEntryType.Information);
                
            }
            
            string sqlQuery = "";
            try
            {   //BranchList b On a.BranchCode=b.branch_code
                EventLog.WriteEntry(sSource, "Uploading Pre Paid Day End File: Fetching database ", EventLogEntryType.Information);
                _databaseManager.OpenDatabaseConnection();  
                sqlQuery = " SELECT bl.branch_code as BranchCode,isNull(TotTrans,'')TotTrans "; 
                sqlQuery += "  ,isNull(Debit,'')Debit ,isNull(Credit,'') Credit ,isNull(Remarks,'') Remarks "  ;
                sqlQuery += "  FROM  (select Distinct(blist.branch_code) from BranchList blist where blist.status='OWN') bl   ";
                sqlQuery += "  INNER JOIN (  " ;
                sqlQuery += "  SELECT blist.branch_code,count(CollectionId) as TotTrans "   ;
                sqlQuery += " ,'' Debit ,Isnull(SUM(CollAmount),'') Credit  ,'' as Balance,'' as Remarks ";
                  
                 sqlQuery += " FROM Collection col   ";
                sqlQuery += "  Left join BranchList blist on blist.branch_code=col.BranchCode ";
                sqlQuery += "  Inner join Clients cl on cl.ClientId=col.ClientId   ";
                 sqlQuery += " Inner join Product p on p.ProductID=col.ProductId and p.ClientId=col.ClientId and p.ProductID=" + productType;
                 sqlQuery += " Inner join InstrumentType it  on it.InstTypeId=col.InstTypeId and it.Active=1 ";
                
                sqlQuery += " where uploaded=1 AND CollDate < '" + _currentTime.Date.AddDays(1).ToString("dd-MMM-yyyy") + "' and CollDate >= '" + _currentTime.Date.ToString("dd-MMM-yyyy") + "'";
                sqlQuery += " and cl.CustomerCode='" + _customerCode + "' ";
                string collectionStatus = cfg.AppSettings.Settings["CollectionStatus"].Value.ToString();
                if (collectionStatus != "")
                {
                    if (Convert.ToInt32(collectionStatus) == MtbBillCollection.Definitions.CollectionStatus.Value.Approved
                        || Convert.ToInt32(collectionStatus) == MtbBillCollection.Definitions.CollectionStatus.Value.Received
                        || Convert.ToInt32(collectionStatus) == MtbBillCollection.Definitions.CollectionStatus.Value.Cancelled)
                    {
                        sqlQuery += " and  CollStatusId = " + Convert.ToInt32(collectionStatus);
                    }
                    else if (Convert.ToInt32(collectionStatus) == MtbBillCollection.Definitions.CollectionStatus.Value.ReceivedAndApproved)
                    {
                        sqlQuery += " and  CollStatusId <> " + MtbBillCollection.Definitions.CollectionStatus.Value.Cancelled;
                    }
                }

                sqlQuery += " Group By blist.branch_code ) daySum on bl.branch_code=daySum.branch_code";
                
                EventLog.WriteEntry(sSource, "Uploading Day End File: Executing Day end query:\n " + sqlQuery, EventLogEntryType.Information);

                System.Data.DataTable dt = _databaseManager.GetDataTable(sqlQuery);

                EventLog.WriteEntry(sSource, "Uploading Day End File: Data rows: " + dt.Rows.Count, EventLogEntryType.Information);

                fs = new FileStream(loacalDayEndFileFullPathLoc, FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs);
                sw.WriteLine(_currentTime.Date.ToString("dd-MM-yyyy") + ";;;;;;Opening Balance");

                
                ////--------------Fetch Current Balance----------------------------
                decimal balance = 0;
                //---------------------------------------------------------------

                if (dt.Rows.Count > 0)
                {                    
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        balance += Convert.ToDecimal("0" + dataRow["Credit"].ToString());
                        sw.WriteLine(_currentTime.Date.ToString("dd-MM-yyyy") + ";" + dataRow["BranchCode"].ToString() + ";" + dataRow["TotTrans"].ToString() + ";" + dataRow["Debit"].ToString() + ";" + dataRow["Credit"].ToString() + ";" + balance + ";" + dataRow["Remarks"].ToString());
                    }
                }
                sw.WriteLine(_currentTime.Date.ToString("dd-MM-yyyy") + ";;;;;;Closing Balance");
                UpdateCurrentBalance(0,productType,ref sqlQuery);
           }
            catch (Exception exception)
            {

                EventLog.WriteEntry(sSource, "UploadDayEndFile: " + exception.Message + " \n " + sqlQuery, EventLogEntryType.Error);
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
                if (sw != null)
                {
                    sw.Close();
                    fs.Close();
                }
            }
            UpdateLogForFileUpload(_bankCode + _dayEndFileName, false, DateTime.Now);
            Upload(ftpDayEndPathLoc, cfg.AppSettings.Settings["UserName"].Value, cfg.AppSettings.Settings["Password"].Value, loacalDayEndFileFullPathLoc);
            UpdateLogForFileUpload(_bankCode + _dayEndFileName, true, DateTime.Now);
            if (productType == MtbBillCollection.Definitions.ProductType.Value.Prepaid)
                InitializeHistoricalInforamtion(ProductType.Prepaid, true);
            else
            {
                InitializeHistoricalInforamtion(ProductType.PostPaid, true);
                SaveHistoricalData();
                _dayEndFileName = _currentTime.ToString("ddMMyyyy");
            }
            
        }

        private void UpdateCurrentBalance(decimal balance,int productType,ref string sqlQuery)
        {
            try
            {
                sqlQuery = "UPDATE Product set CurrentBalance=" + balance + " where ClientId=(select c.ClientId from Clients c where c.CustomerCode='" + _customerCode + "') and ProductID=" + productType;
                _databaseManager.ExcecuteCommand(sqlQuery);
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sSource, "UpdateCurrentBalance: " + exception.Message + " \n " + sqlQuery, EventLogEntryType.Error);
            }
        }

        private decimal GetCurrentBalance(int productType)
        {
            string sqlQuery = " select CurrentBalance from Product p INNER JOIN  Clients c on  c.ClientId=p.ClientId where ProductID=";
            sqlQuery += productType + " and c.CustomerCode='" + _customerCode + "'";
            try
            {                
                decimal balance = Convert.ToDecimal("0" + _databaseManager.GetSingleString(sqlQuery));
                return balance;
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sSource, "GetCurrentBalance: " + exception.Message + " \n " + sqlQuery, EventLogEntryType.Error);
            }
            return 0;
        }

        //System.Configuration.ConfigurationSettings.AppSettings["LocalDayEndPathLoc"].ToString()
        private void SaveHistoricalData()
        {
            try
            {
                List<HistoricalInfo> listHistoricalInfo = new List<HistoricalInfo>();
                listHistoricalInfo.Add(_prepaidHistoricalInfo);
                listHistoricalInfo.Add(_postpaidHistoricalInfo);
                _xmlHandler.GenerateXml<HistoricalInfo>(listHistoricalInfo,_historicalInfoPathLoc);
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sSource, "SaveHistoricalData-->" + exception, EventLogEntryType.Error);
            }

        }

        private void GetHistoricalInformation()
        {
            try
            {

                List<HistoricalInfo> listOfhistoricalInfo = _xmlHandler.ReadXml<HistoricalInfo>(_historicalInfoPathLoc);

                foreach (HistoricalInfo historicalInfo in listOfhistoricalInfo)
                {
                    if (historicalInfo.LastRunDate != DateTime.Now.Date.ToString("dd-MMM-yyyy"))
                    {
                        historicalInfo.LastFileSerialNo = 0;
                        historicalInfo.LastTransSerialNo = 1;
                        historicalInfo.LastRunDate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                    }
                    if (historicalInfo.ProductType.ToString() == ProductType.Prepaid.ToString())
                    {
                        _prepaidHistoricalInfo = historicalInfo;

                    }
                    else
                    {
                        _postpaidHistoricalInfo = historicalInfo;
                    }

                }
               
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sSource, "GetHistoricalInformation Not Found...", EventLogEntryType.Error);
            }

            if (_prepaidHistoricalInfo == null)
            {
                InitializeHistoricalInforamtion(ProductType.Prepaid,false);               
            }
            if(_postpaidHistoricalInfo==null)
            {
                InitializeHistoricalInforamtion(ProductType.PostPaid,false);               
            }

        }

        private void InitializeHistoricalInforamtion(ProductType productType,bool IsForDayEnd)
        {
            _currentTime = (IsForDayEnd == true && productType == ProductType.PostPaid) ? _currentTime.AddDays(1) : _currentTime;
            if (productType == ProductType.Prepaid)
            {
                _prepaidHistoricalInfo = new HistoricalInfo();
                _prepaidHistoricalInfo.LastRunDate = _currentTime.ToString("dd-MMM-yyyy");
                _prepaidHistoricalInfo.LastFileSerialNo = 0;
                _prepaidHistoricalInfo.LastTransSerialNo = 1;
                _prepaidHistoricalInfo.ProductType = ProductType.Prepaid.ToString();
            }
            else
            {
                _postpaidHistoricalInfo = new HistoricalInfo();
                _postpaidHistoricalInfo.LastRunDate = _currentTime.ToString("dd-MMM-yyyy");
                _postpaidHistoricalInfo.LastFileSerialNo = 0;
                _postpaidHistoricalInfo.LastTransSerialNo = 1;
                _postpaidHistoricalInfo.ProductType = ProductType.PostPaid.ToString();
            }
        }

        private void FilterForTest()
        {
            try
            {
                //_sqlConnection = new SqlConnection("Initial Catalog=MTBBillCollection;Data Source=localhost;User Id=mtbdatawriter;Password=mtb123");
               
                EventLog.WriteEntry(sSource, "11111111111111111", EventLogEntryType.Information);
                //_sqlConnection = new SqlConnection(cfg.AppSettings.Settings["ConnectionString"].Value.ToString());
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = cfg.AppSettings.Settings["ConnectionString"].Value.ToString();
                EventLog.WriteEntry(sSource, "22222222222222222", EventLogEntryType.Information);
                //_sqlConnection.Open();
                _databaseManager.OpenDatabaseConnection();
                EventLog.WriteEntry(sSource, "3333333333333", EventLogEntryType.Information); 
                //_sqlCommand.CommandText = "Select count(*) from Clients";
                ;
                EventLog.WriteEntry(sSource, "4444444  ", EventLogEntryType.Information);
                //_sqlCommand.CommandType = CommandType.Text;
                //_sqlCommand.Connection = _sqlConnection;
                //string item = _sqlCommand.ExecuteScalar().ToString();
                string item=_databaseManager.GetDataTable("Select count(*) from Clients").Rows.Count.ToString();
                
                
                EventLog.WriteEntry(sSource, "Test Value: " + item, EventLogEntryType.Information);
            }
            catch (Exception ee)
            {
                EventLog.WriteEntry(sSource, "Test Exception: " + ee, EventLogEntryType.Error);
            }
            finally
            {
                //if(_sqlConnection!=null)
                //    _sqlConnection.Close();
                if (_databaseManager != null)
                    _databaseManager.CloseDatabaseConnection();
            }
        }

        private void UpdateCollectioninfo(DataTable dt)
        {
            string sqlCommand = "";
           
            try
            {
                _databaseManager.OpenDatabaseConnection();
                foreach(DataRow dtRow in dt.Rows)
                {
                    sqlCommand = " UPDATE Collection SET uploaded = 1 WHERE CollectionId= " + dtRow["CollectionId"];
                    _databaseManager.ExcecuteCommand(sqlCommand);
                }
            }
            catch (Exception exxception)
            {
                EventLog.WriteEntry(sSource, "UpdateCollectioninfo -> Exception: " + exxception, EventLogEntryType.Error);
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public bool UpdateLogForFileUpload(string fileName,bool IsUploaded,DateTime creationTime)
        {
            StringBuilder sb=new StringBuilder();
            if (IsUploaded == false)
                sb.Append(" INSERT INTO FileUploadLog  (FileId,CreationTime,IsUploaded) VALUES ('" + fileName + "' ,'" + creationTime.ToString("dd-MM-yyyy") + "',0)");
            else
                sb.Append(" UPDATE FileUploadLog SET IsUploaded = 1 WHERE FileId='"+fileName+"'");
           try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = cfg.AppSettings.Settings["ConnectionString"].Value.ToString();
                _databaseManager.OpenDatabaseConnection();
                _databaseManager.ExcecuteCommand(sb.ToString());
            }
           catch (Exception exception)
            {
                EventLog.WriteEntry(sSource, "UpdateLogForFileUpload -> Exception: " + exception + " \n Sql: \n" + sb.ToString(), EventLogEntryType.Error);
                return false;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
           return true;
        }
    }
}
