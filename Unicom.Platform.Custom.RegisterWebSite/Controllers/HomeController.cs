using System.Linq;
using System.Web.Mvc;
using Unicom.Platform.Custom.RegisterWebSite.Entities;
using Unicom.Platform.Custom.RegisterWebSite.Models.Bootstraptable;

namespace Unicom.Platform.Custom.RegisterWebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var projects = new UnicomDbContext().EmsProjects.ToList();

            return View(projects);
        }

        public ActionResult UnicomProjectTable(BootstraptablePost post)
        {
            var ctx = new UnicomDbContext();
            var total = ctx.EmsProjects.Count();
            var rows = ctx.EmsProjects.OrderBy(p => p.Code).Skip(post.offset).Take(post.limit)
                .Select(prj => new
                {
                    prj.Code,
                    prj.RegisterCode,
                    prj.Name,
                    prj.Telephone,
                    prj.Superintendent
                }).ToList();

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }
    }
}