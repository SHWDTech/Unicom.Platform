using System.Linq;
using System.Web.Mvc;
using Unicom.Platform.Custom.RegisterWebSite.Entities;

namespace Unicom.Platform.Custom.RegisterWebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var projects = new UnicomDbContext().EmsProjects.ToList();

            return View(projects);
        }
    }
}