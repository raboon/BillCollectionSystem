using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MtbBillCollectionLibrary.Utility.DatabaseUtility;
using System.Configuration;
using System.Data;
using System.Text;
using System.Globalization;

namespace MtbBillCollection
{
    public class BillCollectionManager
    {
        IDatabaseManager _databaseManager;
        
        public BillCollectionManager()
        {
            _databaseManager = new MssqlDatabaseManager();
            _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
        }
        public DataTable GetProductList(int clientId)
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();
                return _databaseManager.GetDataTable(" SELECT ProductID,ProductName FROM Product where ClientId=" + clientId);
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
           //return null;
        }

        public DataTable GetTransactionTypeList(int clientId,int productId)
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();
                string sqlQuery = "SELECT InstTypeName ,InstTypeId FROM InstrumentType as it";
                sqlQuery+=" Inner Join Product as p on it.InstrSet=p.InstrTypeSet ";
                sqlQuery += " where  p.ClientId= " + clientId + " and Active=1 and p.ProductID=" + productId;
                return _databaseManager.GetDataTable(sqlQuery);
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetCollectionTypeList(int ClientId)
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();

                string sqlQuery = "Select c.CollectionTypeId, c.CollectionType from Clients a";
                sqlQuery += " Inner Join CollectionMapping b On a.ClientId = b.ClientId ";
                sqlQuery += " Inner Join CollectionType c On b.CollectionTypeId = c.CollectionTypeId ";
                sqlQuery += " where a.ClientId=" + ClientId;

                return _databaseManager.GetDataTable(sqlQuery);
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        
        }

        public DataTable GetCollectionStatusType()
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();
                return _databaseManager.GetDataTable("SELECT CollStatusID, CollStatus FROM CollectionStatus where isActive=1");
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetBankList()
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();
                return _databaseManager.GetDataTable("SELECT BankId,(cast(BankId as varchar)+':'+bankName)bankName FROM BankList where Active='1'");
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetClientist()
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();
                return _databaseManager.GetDataTable("SELECT ClientId, ClientName FROM Clients where isActive=1");
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetInstrumentTypeist()
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();
                return _databaseManager.GetDataTable("SELECT InstTypeId, InstTypeName, InstTypeShortName FROM InstrumentType where Active=1");
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public void SaveCollectionInformation(BillCollectionInfo billCollection)
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString = "INSERT INTO Collection  (ClientId,CollDate,ProductId,BranchCode,CollectionTypeId,CollFrom,InstTypeId ,InstNumber,InstRecvdBy";
                commandString += "  ,InstDate,CollAmount ";
                if (billCollection.InstBankCode != 0)
                    commandString += ",InstBankCode ";
                commandString += " ,Remarks,CollStatusId) VALUES (";
                commandString += billCollection.ClientId + ",'" + billCollection.CollDate + "'," + billCollection.ProductId + ",'";
                commandString += billCollection.BranchCode + "'," + billCollection.CollectionTypeId + ",'" + billCollection.CollFrom + "',";
                commandString += billCollection.InstTypeId + ",'" + billCollection.InstNumber + "'," + billCollection.InstRecvdBy + ",'" ;
                commandString += billCollection.InstDate + "'," + billCollection.CollAmount ;
                if (billCollection.InstBankCode != 0)
                    commandString +=  ","+ billCollection.InstBankCode;
                commandString += " ,'" + billCollection.Remarks + "'," + billCollection.CollStatus + " )";
          
                _databaseManager.ExcecuteCommand(commandString);
                             
                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public DataTable GetCollectionListForSpeacilEdit(int clientId, string fromDate, string toDate, string collStatusSrchCond, string instTypeSrchCond, string productTypeSrchCond,bool isBranchWise, string branchCode)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();

                //Change date format
                //We have read the date as dd/MM/yyyy. need to convert them in MM/dd/yyyy for sql date comparison

                DateTime CollFromDate, CollToDate;
                if (!DateTime.TryParseExact(fromDate, "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out CollFromDate))
                {
                    // Parse failed
                }
                if (!DateTime.TryParseExact(toDate, "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out CollToDate))
                {
                    // Parse failed
                }

                sb.Append("SELECT a.CollectionId,a.CollDate, a.ProductID, c.ProductName, b.branch_name,");
                sb.Append("a.CollFrom, d.InstTypeName,a.InstNumber, e.BankName, CollAmount FROM Collection a ");
                sb.Append("Left  Join BranchList b On a.BranchCode=b.branch_code ");
                sb.Append("Left  Join Product c on a.ProductId=c.ProductID ");
                sb.Append("Left Join InstrumentType d On a.InstTypeId = d.InstTypeId ");
                sb.Append("Left Join BankList e On a.InstBankCode = e.BankId ");
                sb.Append("Where a.clientId=");
                sb.Append(clientId);

                if (isBranchWise==true)
                {
                    sb.Append(" and b.branch_code='" + branchCode + "' ");
                }

                sb.Append(" and DATEDIFF(day, '");
                sb.Append(CollFromDate.ToString("MM/dd/yyyy"));
                sb.Append("', colldate)>=0");


                sb.Append(" and DATEDIFF(day, '");
                sb.Append(CollToDate.ToString("MM/dd/yyyy"));
                sb.Append("', colldate)<=0");

                if (!collStatusSrchCond.Equals("0"))
                {
                    if (collStatusSrchCond.Equals("6"))
                    {
                        sb.Append(" and (a.CollStatusId in (");
                        sb.Append(2);
                        sb.Append(")");
                        sb.Append(" OR a.CollStatusId in (");
                        sb.Append(5);
                        sb.Append("))");
                    }
                    else
                    {
                        sb.Append(" and a.CollStatusId in (");
                        sb.Append(collStatusSrchCond);
                        sb.Append(")");
                    }
                }

                if (!instTypeSrchCond.Equals(""))
                {
                    sb.Append(" and a.InstTypeId in (");
                    sb.Append(instTypeSrchCond);
                    sb.Append(")");
                }

                if (!productTypeSrchCond.Equals(""))
                {
                    sb.Append(" and a.ProductID in (");
                    sb.Append(productTypeSrchCond);
                    sb.Append(")");
                }



                return _databaseManager.GetDataTable(sb.ToString());
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetCollectionList(int clientId, string fromDate, string toDate, string collStatusSrchCond, string instTypeSrchCond, string productTypeSrchCond, int userType, string branchCode)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                _databaseManager.OpenDatabaseConnection();

                //Change date format
                //We have read the date as dd/MM/yyyy. need to convert them in MM/dd/yyyy for sql date comparison

                DateTime CollFromDate, CollToDate;
                if (!DateTime.TryParseExact(fromDate, "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out CollFromDate))
                {
                    // Parse failed
                }
                if (!DateTime.TryParseExact(toDate, "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out CollToDate))
                {
                    // Parse failed
                }

                sb.Append("SELECT cl.ClientName,a.CollectionId,a.CollDate, a.ProductID, c.ProductName, b.branch_name,");
                sb.Append(" a.CollFrom, d.InstTypeName,a.InstNumber, e.BankName,cs.CollStatus,a.InstClearingDate, CollAmount FROM Collection a ");
                sb.Append(" INNER JOIN Clients cl on cl.clientId=a.clientId ");
                sb.Append(" INNER JOIN CollectionStatus cs on cs.CollStatusID=a.CollStatusId ");
                sb.Append(" Left  Join BranchList b On a.BranchCode=b.branch_code ");
                sb.Append(" Left  Join Product c on a.ProductId=c.ProductID ");
                sb.Append(" Left Join InstrumentType d On a.InstTypeId = d.InstTypeId ");
                sb.Append(" Left Join BankList e On a.InstBankCode = e.BankId ");
                sb.Append(" Where a.clientId=");
                sb.Append(clientId);

                if (userType != MtbBillCollection.Global.Definitions.UserType.Value.Reviewer)
                {
                    sb.Append(" and b.branch_code='" + branchCode + "' ");
                }
                else if (branchCode!="")
                {
                    sb.Append(" and b.branch_code='" + branchCode + "' ");
                }

                sb.Append(" and DATEDIFF(day, '");
                sb.Append(CollFromDate.ToString("MM/dd/yyyy"));
                sb.Append("', colldate)>=0");


                sb.Append(" and DATEDIFF(day, '");
                sb.Append(CollToDate.ToString("MM/dd/yyyy"));
                sb.Append("', colldate)<=0");

                if (!collStatusSrchCond.Equals(""))
                {
                    sb.Append(" and a.CollStatusId in (");
                    sb.Append(collStatusSrchCond);
                    sb.Append(")");
                }

                if (!instTypeSrchCond.Equals(""))
                {
                    sb.Append(" and a.InstTypeId in (");
                    sb.Append(instTypeSrchCond);
                    sb.Append(")");
                }

                if (!productTypeSrchCond.Equals(""))
                {
                    sb.Append(" and a.ProductID in (");
                    sb.Append(productTypeSrchCond);
                    sb.Append(")");
                }

                

                return _databaseManager.GetDataTable(sb.ToString());
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetCollectionListByStatus(int clientId, int collStatusId, string fromDate, string toDate,int userType,string branchCode)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();

                //Change date format
                //We have read the date as dd/MM/yyyy. need to convert them in MM/dd/yyyy for sql date comparison

                sb.Append("SELECT a.CollectionId,a.CollDate, a.ProductId, c.ProductName, b.branch_name,");
                sb.Append(" a.CollFrom, d.InstTypeName,a.InstNumber, e.BankName, a.CollAmount,a.ApproveBy,a.ApproveDate FROM Collection a ");

                if (collStatusId==MtbBillCollection.Global.Definitions.CollectionStatus.Value.SpeacialEdit)
                    sb.Append(" INNER JOIN SpecialEdit sp on sp.CollectionId=a.CollectionId AND Active=1");

                sb.Append(" Left  Join BranchList b On a.BranchCode=b.branch_code ");
                sb.Append(" Left  Join Product c on a.ProductId=c.ProductID ");
                sb.Append(" Left Join InstrumentType d On a.InstTypeId = d.InstTypeId ");
                sb.Append(" Left Join BankList e On a.InstBankCode = e.BankId ");
                sb.Append(" Where a.clientId=");
                sb.Append(clientId);

                sb.Append(" and CollStatusId=");
                sb.Append(collStatusId);


                if (userType != MtbBillCollection.Global.Definitions.UserType.Value.Reviewer)
                {
                    sb.Append(" and b.branch_code='" +branchCode+"' ");                    
                }
                
                if (!fromDate.Equals("") && !toDate.Equals(""))
                {
                    DateTime CollFromDate, CollToDate;

                    if (!DateTime.TryParseExact(fromDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out CollFromDate))
                    {
                        // Parse failed
                    }
                    if (!DateTime.TryParseExact(toDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out CollToDate))
                    {
                        // Parse failed
                    }

                    sb.Append(" and DATEDIFF(day, '");
                    sb.Append(CollFromDate.ToString("MM/dd/yyyy"));
                    sb.Append("', colldate)>=0");

                    sb.Append(" and DATEDIFF(day, '");
                    sb.Append(CollToDate.ToString("MM/dd/yyyy"));
                    sb.Append("', colldate)<=0");
                }

                
                return _databaseManager.GetDataTable(sb.ToString());
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public DataTable GetNonCashColllectionList(int collStatusId, int clientId, bool isCleared,int userType,string branchCode, string fromDate, string toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();

                //Change date format
                //We have read the date as dd/MM/yyyy. need to convert them in MM/dd/yyyy for sql date comparison

                sb.Append("SELECT CollectionId,a.CollDate, c.ProductName, b.branch_name,InstRecvdBy,");
                sb.Append(" a.CollFrom, d.InstTypeName,a.InstNumber, e.BankName, CONVERT(VARCHAR,[CollAmount],1) as 'CollAmount' FROM Collection a ");
                sb.Append(" Left  Join BranchList b On a.BranchCode=b.branch_code ");
                sb.Append(" Left  Join Product c on a.ProductId=c.ProductID ");
                sb.Append(" Left Join InstrumentType d On a.InstTypeId = d.InstTypeId ");
                sb.Append(" Left Join BankList e On a.InstBankCode = e.BankId ");
                sb.Append("Left join  Users u on u.UserId=a.InstRecvdBy");
                sb.Append(" Where a.clientId=");
                sb.Append(clientId);

                sb.Append(" and CollStatusId=");
                sb.Append(collStatusId);

                sb.Append(" and a.InstTypeID <> ");
                sb.Append(MtbBillCollection.Global.Definitions.InstType.Value.CASH);

                sb.Append(" and a.InstrCLeared = ");
                sb.Append((isCleared==true)?1:0);

                if (userType != MtbBillCollection.Global.Definitions.UserType.Value.Reviewer)
                {
                    sb.Append(" and b.branch_code='" + branchCode + "' ");
                }
                 if (!fromDate.Equals("") && !toDate.Equals(""))
                {
                    DateTime CollFromDate, CollToDate;

                    if (!DateTime.TryParseExact(fromDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out CollFromDate))
                    {
                        // Parse failed
                    }
                    if (!DateTime.TryParseExact(toDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out CollToDate))
                    {
                        // Parse failed
                    }

                    sb.Append(" and DATEDIFF(day, '");
                    sb.Append(CollFromDate.ToString("MM/dd/yyyy"));
                    sb.Append("', colldate)>=0");

                    sb.Append(" and DATEDIFF(day, '");
                    sb.Append(CollToDate.ToString("MM/dd/yyyy"));
                    sb.Append("', colldate)<=0");
                }
                return _databaseManager.GetDataTable(sb.ToString());
            }
            catch
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public BillCollectionInfo GetCollectionInformtion(string collectionId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                _databaseManager.OpenDatabaseConnection();

                //Change date format
                //We have read the date as dd/MM/yyyy. need to convert them in MM/dd/yyyy for sql date comparison

                sb.Append("SELECT ClientId , CollDate , ProductId, BranchCode , CollectionTypeId , CollFrom ,InstTypeId , InstNumber");
                sb.Append(" , InstDate , CollAmount, InstBankCode , Remarks, CollStatusId,ApproveBy,ApproveDate FROM Collection ");                
                sb.Append("Where CollectionId=");
                sb.Append(collectionId);
                DataTable dt= _databaseManager.GetDataTable(sb.ToString());
                BillCollectionInfo billColectionInfo = new BillCollectionInfo();
                billColectionInfo.ClientId= Convert.ToInt32(dt.Rows[0]["ClientId"].ToString());
                billColectionInfo.CollDate = dt.Rows[0]["CollDate"].ToString();
                billColectionInfo.ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"].ToString());
                billColectionInfo.BranchCode = dt.Rows[0]["BranchCode"].ToString();
                billColectionInfo.CollectionTypeId = Convert.ToInt32(dt.Rows[0]["CollectionTypeId"].ToString());
                billColectionInfo.CollFrom = dt.Rows[0]["CollFrom"].ToString();
                billColectionInfo.InstTypeId = Convert.ToInt32(dt.Rows[0]["InstTypeId"].ToString());
                billColectionInfo.InstNumber = dt.Rows[0]["InstNumber"].ToString();
                billColectionInfo.InstDate = dt.Rows[0]["InstDate"].ToString();
                billColectionInfo.CollAmount = Convert.ToDecimal(dt.Rows[0]["CollAmount"].ToString());
                billColectionInfo.InstBankCode = Convert.ToInt32((dt.Rows[0]["InstBankCode"].ToString() == "" ? "0" : dt.Rows[0]["InstBankCode"].ToString()));                
                billColectionInfo.Remarks = dt.Rows[0]["Remarks"].ToString();
                billColectionInfo.CollStatus = Convert.ToInt32(dt.Rows[0]["CollStatusId"].ToString());
                billColectionInfo.InstApprovedBy = Convert.ToInt32("0"+dt.Rows[0]["ApproveBy"].ToString());
                billColectionInfo.InstApprovedDate = dt.Rows[0]["ApproveDate"].ToString();

                return billColectionInfo;
            }
            catch(Exception e)
            {
                return null;
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            //return null;
        }

        public void UpdateCollectionInformation(BillCollectionInfo billCollection)
        {
            try
            {
                UpadteCollectionAudit(billCollection.ColectionId);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString ="UPDATE Collection SET  ClientId  = "+billCollection.ClientId+" , CollDate  = '"+billCollection.CollDate+"'";
                commandString += ", ProductId  = "+billCollection.ProductId+", BranchCode  = '"+billCollection.BranchCode+"', CollectionTypeId  = "+ billCollection.CollectionTypeId;
                commandString += ", CollFrom  = '"+billCollection.CollFrom+"', InstTypeId  = "+billCollection.InstTypeId+", InstNumber  = '"+billCollection.InstNumber+"'";
                commandString += ", InstDate  ='"+billCollection.InstDate+"', CollAmount  = "+billCollection.CollAmount+", InstBankCode  = "+billCollection.InstBankCode;
                commandString += ", Remarks  = '" + billCollection.Remarks + "',ApproveBy=" + billCollection.InstApprovedBy + ",ApproveDate='"+billCollection.InstApprovedDate+"'";
                commandString += " WHERE CollectionId=" + billCollection.ColectionId;

                _databaseManager.ExcecuteCommand(commandString);
                
                //_databaseManager.Commit();
            }
            catch
            {
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public void ApproveCollection(BillCollectionInfo billCollection)
        {
          try
            {
                UpadteCollectionAudit(billCollection.ColectionId);
                
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();  
                
                string commandString = "UPDATE Collection SET  CollStatusId= " + MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved ;
                commandString += ", ApproveBy=" + billCollection.InstApprovedBy + ",ApproveDate='" + billCollection.InstApprovedDate + "' ";
                if (billCollection.InstTypeId == MtbBillCollection.Global.Definitions.InstType.Value.CASH)
                {
                    //UpadteCollectionAudit(billCollection.ColectionId);
                    billCollection.Cleared = 1; // means Cleared;
                    billCollection.InstClearedBy = billCollection.InstApprovedBy;
                    billCollection.InstClearingDate = billCollection.InstApprovedDate;
                    commandString += ",InstrCleared = " + billCollection.Cleared + " ,InstClearedBy = " + billCollection.InstClearedBy + " ,InstClearingDate ='" + billCollection.InstClearingDate + "' ";
                }

              commandString += " WHERE CollectionId=" + billCollection.ColectionId;
                _databaseManager.ExcecuteCommand(commandString);
                
                //_databaseManager.Commit();
            }
          catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            
        }

        public void ApproveCollection(string collectionId,int appproveBy,string approveDate)
        {
          try
            {
                UpadteCollectionAudit(int.Parse(collectionId)); 
                int instrumentTypeId=GetCollectionInsType(collectionId); 
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();                
                string commandString = "UPDATE Collection SET  CollStatusId= " + MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved ;
                commandString += ", ApproveBy=" + appproveBy + ",ApproveDate='" + approveDate + "' ";

                if (instrumentTypeId == MtbBillCollection.Global.Definitions.InstType.Value.CASH)
                {
                   // UpadteCollectionAudit(int.Parse(collectionId));                    
                    commandString += ",InstrCleared = " + 1 + " ,InstClearedBy = " + appproveBy + " ,InstClearingDate ='" + approveDate + "' ";
                }
                commandString += " WHERE CollectionId=" + collectionId;
                _databaseManager.ExcecuteCommand(commandString);
                
                
               
                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            
        }

        public void ApproveSpeacialColection(string collectionId, int appproveBy, string approveDate)
        {
            try
            {

                UpadteCollectionAudit(int.Parse(collectionId));
                int instrumentTypeId = GetCollectionInsType(collectionId);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString = "UPDATE Collection SET  CollStatusId= " + MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved;                

                if (instrumentTypeId == MtbBillCollection.Global.Definitions.InstType.Value.CASH)
                {
                    // UpadteCollectionAudit(int.Parse(collectionId));                    
                    commandString += ",InstrCleared = " + 1 + " ,InstClearedBy = " + appproveBy + " ,InstClearingDate ='" + approveDate + "' ";
                }
                commandString += " WHERE CollectionId=" + collectionId;
                _databaseManager.ExcecuteCommand(commandString);

                InactiveSpeacilEntry(collectionId, appproveBy, approveDate);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void CancelCollection(BillCollectionInfo billCollection)
        {
            try
            {
                UpadteCollectionAudit(billCollection.ColectionId);

                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();

                string commandString = "UPDATE Collection SET  CollStatusId= " + MtbBillCollection.Global.Definitions.CollectionStatus.Value.Cancelled;
                commandString += ", ApproveBy=" + billCollection.InstApprovedBy + ",ApproveDate='" + billCollection.InstApprovedDate + "' ";              

                commandString += " WHERE CollectionId=" + billCollection.ColectionId;
                _databaseManager.ExcecuteCommand(commandString);

                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }

        }

        public void CancelCollection(string collectionId, int appproveBy, string approveDate)
        {
            try
            {
                UpadteCollectionAudit(int.Parse(collectionId));
                int instrumentTypeId = GetCollectionInsType(collectionId);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString = "UPDATE Collection SET  CollStatusId= " + MtbBillCollection.Global.Definitions.CollectionStatus.Value.Cancelled;
                commandString += ", ApproveBy=" + appproveBy + ",ApproveDate='" + approveDate + "' ";
                               
                commandString += " WHERE CollectionId=" + collectionId;
                _databaseManager.ExcecuteCommand(commandString);


                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }

        }

        public void UpadteCollectionAudit(int collectionId)
        {
            try
            {                
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString = "INSERT INTO CollectionAudit (CollectionId,ClientId,CollDate,ProductId,BranchCode,CollectionTypeId,CollFrom,InstTypeId ";
		        commandString +=" ,InstNumber,InstRecvdBy,InstDate,CollAmount,InstBankCode,Remarks,CollStatusId ,ApproveBy,ApproveDate,AuditDate) ";
                commandString +=" ( select CollectionId,ClientId,CollDate,ProductId,BranchCode,CollectionTypeId,CollFrom,InstTypeId ,InstNumber, ";
                commandString +=" InstRecvdBy,InstDate,CollAmount,InstBankCode,Remarks,CollStatusId	,ApproveBy,ApproveDate,GETDATE() ";
                commandString += " from dbo.Collection Where CollectionId=" + collectionId + ")";

                _databaseManager.ExcecuteCommand(commandString);                
                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public void MarkAsSpeacialEdit(int collectionId,string brachCode, string editedBy, string editedDate, string reason)
        {
            try
            {
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();                 
                string commandString = "INSERT INTO SpecialEdit ";
                commandString += " ( CollectionId,EditedBy,EditedDate,Description,Active) VALUES   ";
                commandString += "(" + collectionId + "," + editedBy + ",'" + editedDate + "','" + reason + "',1)";

                _databaseManager.ExcecuteCommand(commandString);

                commandString = "UPDATE Collection SET  CollStatusId="+MtbBillCollection.Global.Definitions.CollectionStatus.Value.SpeacialEdit;
                commandString += " WHERE CollectionId=" + collectionId;
                _databaseManager.ExcecuteCommand(commandString);
                
                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public void ApproveSpeacialEdit(int collectionId, string approveBy, string approveDate)
        {
            try
            {
                UpadteCollectionAudit(collectionId);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString = "UPDATE MarkForSpeacialEdit SET Active = 0 , ReAppoveBy = " + approveBy + " ,ReApproveDate = '" + approveDate + "' WHERE CollId="+collectionId;
               

                _databaseManager.ExcecuteCommand(commandString);
                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public void ClearCollection(BillCollectionInfo billCollInfo)
        {
            try
            {
                UpadteCollectionAudit(billCollInfo.ColectionId);
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();

                string commandString = "UPDATE Collection   SET InstrCleared = 1 ,InstClearedBy = "+billCollInfo.InstClearedBy+" ,InstClearingDate ='"+billCollInfo.InstClearingDate+"' WHERE CollectionId="+billCollInfo.ColectionId;

                _databaseManager.ExcecuteCommand(commandString);
                
                //_databaseManager.Commit();
            }
            catch
            {
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public void ClearCollection(string collectionId,int clearedBy,string clearingDate)
        {
            try
            {
                UpadteCollectionAudit(int.Parse(collectionId));
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();

                string commandString = "UPDATE Collection   SET InstrCleared = 1 ,InstClearedBy = " + clearedBy + " ,InstClearingDate ='" + clearingDate + "' WHERE CollectionId=" + collectionId;

                _databaseManager.ExcecuteCommand(commandString);                
               
                //_databaseManager.Commit();
            }
            catch (Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
        }

        public int GetCollectionInsType(string collectionId)
        {
            try
            {                
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();

                string quereyString = "SELECT InstTypeId FROM Collection WHERE CollectionId=" + collectionId;

                DataTable dt=_databaseManager.GetDataTable(quereyString);
                if (dt.Rows.Count > 0)
                    return int.Parse(dt.Rows[0][0].ToString());
                //_databaseManager.Commit();
            }
            catch(Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            return 0;
        }
        
        public bool IsSpeacialEntryExist(string collectionId)
        {
            try
            {                
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string quereyString = "if Exists(select CollectionId from SpecialEdit where CollectionId=" + collectionId + " and Active=1) Begin   select 1 End else begin select 0   end ";

               string value=_databaseManager.GetSingleString(quereyString);
               return (int.Parse(value)==1)?true:false;
                //_databaseManager.Commit();
            }
            catch(Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }
            return false;
        }

        public void InactiveSpeacilEntry(string collectionId, int appproveBy, string approveDate)
        {
           try
            {                
                _databaseManager = new MssqlDatabaseManager();
                _databaseManager.SetConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();;
                //_databaseManager.BeginTransaction();
                _databaseManager.OpenDatabaseConnection();
                string commandString = "UPDATE SpecialEdit SET Active=0, ApproveBy=" + appproveBy + ",ApproveDate='" + approveDate + "' where CollectionId=" + collectionId;

               _databaseManager.ExcecuteCommand(commandString); 
                //_databaseManager.Commit();
            }
            catch(Exception exception)
            {
                throw exception;
                //_databaseManager.RollBack();
            }
            finally
            {
                _databaseManager.CloseDatabaseConnection();
            }            
        }

    }
}