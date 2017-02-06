using System.Configuration;

namespace Unicom.Register.Common
{
    public static class AppConfig
    {
        public static string ConnectionString { get; private set; }

        public static string ShortTitle { get; private set; }

        public static string VendorCode { get; private set; }

        static AppConfig()
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            ShortTitle = ConfigurationManager.AppSettings["ShortTitle"];
            VendorCode = ConfigurationManager.AppSettings["vendorCode"];
        }
    }
}
