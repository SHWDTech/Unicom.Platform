using System;
using System.Linq;
using System.Web.Mvc;
using Unicom.Platform.Custom.RegisterWebSite.Models;
using Unicom.Platform.Entities;
using Unicom.Platform.Model.Service_References.UnicomPlatform;

namespace Unicom.Platform.Custom.RegisterWebSite.Controllers
{
    public class DeviceController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            var model = new DeviceModel();
            LoadInfomation(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(DeviceModel model)
        {
            var device = new emsDevice
            {
                name = model.Name,
                ipAddr = model.IpAddr,
                macAddr = model.MacAddr,
                port = model.Port,
                version = model.Version,
                projectCode = model.ProjectCode,
                longitude = model.Longitude,
                latitude = model.Latitude,
                startDate = model.StartDate,
                startDateSpecified = true,
                endDate = model.EndDate,
                endDateSpecified = true,
                installDate = model.InstallDate,
                installDateSpecified = true,
                onlineStatus = model.OnlineStatus,
                onlineStatusSpecified = true,
                videoUrl = model.VideoUrl
            };

            var result = WebConfig.UniComService.PushDevices(new[] {device});
            if (result.result.Length > 0 && !result.result[0].value.ToString().Contains("ERROR"))
            {
                try
                {
                    model.Code = result.result[0].key.ToString();
                    using (var ctx = new UnicomDbContext())
                    {
                        ctx.EmsDevices.Add(model);
                        ctx.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("RegisterError", "注册设备信息成功，但保存至服务器时遇到异常，请记录工程信息并提供给管理员手动添加。");
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

        private void LoadInfomation(DeviceModel model)
        {
            using (var ctx = new UnicomDbContext())
            {
                model.Projects = ctx.EmsProjects.ToList();
            }
        }
    }
}