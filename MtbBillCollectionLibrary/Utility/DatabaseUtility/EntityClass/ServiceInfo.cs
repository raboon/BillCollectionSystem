namespace MtbBillCollectionLibrary.Utility.DatabaseUtility.EntityClass
{
    public class ServiceInfo
    {
        public enum Service
        {
            ReportService=0,
            TransactionalService=1,
            InternetBanking=2,
            Log=3
        }

        private Service _serviceType;
        public Service ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }

        private DatabaseInfo _databaseInfo;
        public DatabaseInfo DatabaseInfo
        {
            get { return _databaseInfo; }
            set { _databaseInfo = value; }
        }

    }
}
