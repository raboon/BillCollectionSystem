namespace MtbBillCollectionLibrary.Utility.DatabaseUtility.EntityClass
{
    public class DatabaseInfo
    {
        public enum Authentication
        {
            Windows=0,
            SqlServer=1
        }
        private string _serVerName = "";
        public string ServerName
        {
            get { return _serVerName; }
            set { _serVerName = value; }
        }
        private string _databaseName = "";
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }
        private Authentication _authenticationMode;
        public Authentication AuthenticationMode
        {
            get {return _authenticationMode; }
            set { _authenticationMode = value; }
        }

        private User _user;

        public User UserInfo
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
