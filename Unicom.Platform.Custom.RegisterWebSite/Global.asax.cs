using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unicom.Platform.Entities;

namespace Unicom.Platform.Custom.RegisterWebSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var ctx = new UnicomDbContext())
            {
                if (!ctx.EmsDistricts.Any())
                {
                    var server = WebConfig.UniComService;
                    foreach (var emsDistrict in server.PullDistrict(null))
                    {
                        ctx.EmsDistricts.Add(new EmsDistrict
                        {
                            Code = emsDistrict.code,
                            Name = emsDistrict.name
                        });
                    }
                    foreach (var emsPrjType in server.PullProjectType())
                    {
                        ctx.EmsProjectTypes.Add(new EmsProjectType
                        {
                            Code = emsPrjType.code,
                            Name = emsPrjType.name
                        });
                    }
                    foreach (var emsPrjPeriod in server.PullProjectPeriod())
                    {
                        ctx.EmsProjectPeriods.Add(new EmsProjectPeriod
                        {
                            Code = emsPrjPeriod.code,
                            Name = emsPrjPeriod.name
                        });
                    }
                    foreach (var emsPrjCategory in server.PullProjectCategory())
                    {
                        ctx.EmsProjectCategories.Add(new EmsProjectCategory
                        {
                            Code = emsPrjCategory.code,
                            Name = emsPrjCategory.name
                        });
                    }
                    foreach (var emsRegion in server.PullRegion())
                    {
                        ctx.EmsRegions.Add(new EmsRegion
                        {
                            Code = emsRegion.code,
                            Name = emsRegion.name
                        });
                    }
                    ctx.SaveChanges();
                }
            }
        }
    }
}
