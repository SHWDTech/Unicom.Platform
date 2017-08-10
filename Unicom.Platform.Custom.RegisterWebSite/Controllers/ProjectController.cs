using System;
using System.Linq;
using System.Web.Mvc;
using Unicom.Platform.Custom.RegisterWebSite.Models;
using Unicom.Platform.Entities;
using Unicom.Platform.Model.Service_References.UnicomPlatform;

namespace Unicom.Platform.Custom.RegisterWebSite.Controllers
{
    public class ProjectController : Controller
    {
        [HttpGet]
        public ActionResult Register(string code)
        {
            ProjectModel model;
            if (string.IsNullOrWhiteSpace(code))
            {
                model = new ProjectModel();
            }
            else
            {
                using (var ctx = new UnicomDbContext())
                {
                    var prj = ctx.EmsProjects.First(d => d.Code == code);
                    model = new ProjectModel
                    {
                        Code = prj.Code,
                        Name = prj.Name,
                        RegisterCode = prj.RegisterCode,
                        District = prj.District,
                        ProjectType = prj.ProjectType,
                        ProjectCategory = prj.ProjectCategory,
                        ProjectPeriod = prj.ProjectPeriod,
                        Longitude = prj.Longitude,
                        Latitude = prj.Latitude,
                        StartDate = prj.StartDate,
                        EndDate = prj.EndDate,
                        Region = prj.Region,
                        Street = prj.Street,
                        Contractors = prj.Contractors,
                        Superintendent = prj.Superintendent,
                        Telephone = prj.Telephone,
                        Address = prj.Address,
                        SiteArea = prj.SiteArea,
                        BuildingArea = prj.BuildingArea,
                        Stage = prj.Stage,
                        IsCompleted = prj.IsCompleted,
                        Status = prj.Status
                    };
                }
            }
            LoadInfomation(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(ProjectModel model)
        {
            var project = new emsProject
            {
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

            return string.IsNullOrWhiteSpace(model.Code) ? AddNewProject(project, model) : UpdateProject(project, model);
        }

        public ActionResult ProjectManager() => View();

        public ActionResult AddNewProject(emsProject project, ProjectModel model)
        {
            project.code = model.RegisterCode;
            var result = WebConfig.UniComService.PushProjects(new[] { project });
            if (result.result.Length > 0 && !result.result[0].value.ToString().Contains("ERROR"))
            {
                try
                {
                    model.Code = result.result[0].key.ToString();
                    using (var ctx = new UnicomDbContext())
                    {
                        ctx.EmsProjects.Add(new EmsProject
                        {
                            Code = model.Code,
                            Name = model.Name,
                            RegisterCode = model.RegisterCode,
                            District = model.District,
                            ProjectType = model.ProjectType,
                            ProjectCategory = model.ProjectCategory,
                            ProjectPeriod = model.ProjectPeriod,
                            Region = model.Region,
                            Street = model.Street,
                            Longitude = model.Longitude,
                            Latitude = model.Latitude,
                            Contractors = model.Contractors,
                            Superintendent = model.Superintendent,
                            Telephone = model.Telephone,
                            Address = model.Address,
                            SiteArea = model.SiteArea,
                            BuildingArea = model.BuildingArea,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            Stage = model.Stage,
                            IsCompleted = model.IsCompleted,
                            Status = model.Status
                        });
                        ctx.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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

        public ActionResult UpdateProject(emsProject project, ProjectModel model)
        {
            project.code = model.Code;
            var result = WebConfig.UniComService.PushProjectStatus(new[] { project });
            if (result.result.Length <= 0)
            {
                try
                {
                    using (var ctx = new UnicomDbContext())
                    {
                        var prj = ctx.EmsProjects.First(p => p.Code == model.Code);
                        prj.Code = model.Code;
                        prj.Name = model.Name;
                        prj.RegisterCode = model.RegisterCode;
                        prj.District = model.District;
                        prj.ProjectType = model.ProjectType;
                        prj.ProjectCategory = model.ProjectCategory;
                        prj.ProjectPeriod = model.ProjectPeriod;
                        prj.Region = model.Region;
                        prj.Street = model.Street;
                        prj.Longitude = model.Longitude;
                        prj.Latitude = model.Latitude;
                        prj.Contractors = model.Contractors;
                        prj.Superintendent = model.Superintendent;
                        prj.Telephone = model.Telephone;
                        prj.Address = model.Address;
                        prj.SiteArea = model.SiteArea;
                        prj.BuildingArea = model.BuildingArea;
                        prj.StartDate = model.StartDate;
                        prj.EndDate = model.EndDate;
                        prj.Stage = model.Stage;
                        prj.IsCompleted = model.IsCompleted;
                        prj.Status = model.Status;
                        ctx.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("RegisterError", "更新工程信息成功，但保存至服务器时遇到异常，请记录工程信息并提供给管理员手动添加。");
                    LoadInfomation(model);
                    return View(model);
                }
                return Redirect("/Project/ProjectManager");
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
