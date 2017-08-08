using System.Web;
using System.Web.Mvc;

namespace Unicom.Platform.Custom.RegisterWebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
