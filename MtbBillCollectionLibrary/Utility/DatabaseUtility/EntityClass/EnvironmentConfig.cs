using System.Xml.Serialization;

namespace MtbBillCollectionLibrary.Utility.DatabaseUtility.EntityClass
{
    public class EnvironmentConfig
    {
        public enum EnvironmentType
        {
            Development=0,
            Testing=1,
            Release=2
        }

        private EnvironmentType _environment;
        public EnvironmentType Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

        private ServiceInfo _serviceInfo;
        public ServiceInfo ServiceInfo
        {
            get { return _serviceInfo; }
            set { _serviceInfo = value; }
        }
        
    }
}
