using System;
using System.Linq;
using System.Web.Mvc;
using Unicom.Platform.Custom.RegisterWebSite.Entities;
using Unicom.Platform.Custom.RegisterWebSite.Models;
using Unicom.Platform.Model.Service_References.UnicomPlatform;

namespace Unicom.Platform.Custom.RegisterWebSite.Controllers
{
    public class ProjectController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            var model = new ProjectModel();
            LoadInfomation(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(ProjectModel model)
        {
            var project = new emsProject
            {
                code = model.RegisterCode,
                name = model.Name,
                district = model.District,
                prjType = model.ProjectType,
                prjTypeSpecified = true,
                prjCategory = model.ProjectCategory,
                prjCategorySpecified = true,
                prjPeriod = model.ProjectPeriod,
                prjPeriodSpecified = true,
                region = model.Region,
                regionSpecified = true,
                street = model.Street,
                longitude = model.Longitude,
                latitude = model.Latitude,
                contractors = model.Contractors,
                superintendent = model.Superintendent,
                telephone = model.Telephone,
                address = model.Address,
                siteArea = model.SiteArea,
                siteAreaSpecified = true,
                buildingArea = model.BuildingArea,
                buildingAreaSpecified = true,
                startDate = model.StartDate,
                startDateSpecified = true,
                endDate = model.EndDate,
                endDateSpecified = true,
                stage = model.Stage,
                status = true,
                statusSpecified = true,
                isCompleted = model.IsCompleted,
                isCompletedSpecified = true
            };

            var result = WebConfig.UniComService.PushProjects(new[] { project });
            if (result.result.Length > 0 && !result.result[0].value.ToString().Contains("ERROR"))
            {
                try
                {
                    model.Code = result.result[0].key.ToString();
                    using (var ctx = new UnicomDbContext())
                    {
                        ctx.EmsProjects.Add(model);
                        ctx.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("RegisterError", "注册工程信息成功，但保存至服务器时遇到异常，请记录工程信息并提供给管理员手动添加。");
                    LoadInfomation(model);
                    return View(model);
                }
                return View("Success");
            }

            foreach (var entry in result.result)
            {
                ModelState.AddModelError("RegisterError", entry.value.ToString());
            }
            LoadInfomation(model);
            return View(model);
        }

        private void LoadInfomation(ProjectModel model)
        {
            using (var ctx = new UnicomDbContext())
            {
                model.Districts = ctx.EmsDistricts.ToList();
                model.ProjectCategories = ctx.EmsProjectCategories.ToList();
                model.ProjectPeriods = ctx.EmsProjectPeriods.ToList();
                model.ProjectTypes = ctx.EmsProjectTypes.ToList();
                model.Regions = ctx.EmsRegions.ToList();
            }
        }
    }
}
