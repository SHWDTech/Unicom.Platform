using System;
using System.Linq;
using System.Web.Mvc;
using Unicom.Platform.Custom.RegisterWebSite.Models;
using Unicom.Platform.Custom.RegisterWebSite.Models.Bootstraptable;
using Unicom.Platform.Entities;
using Unicom.Platform.Model.UnicomPlatform;

namespace Unicom.Platform.Custom.RegisterWebSite.Controllers
{
    public class DeviceController : Controller
    {
        [HttpGet]
        public ActionResult Register(string code)
        {
            DeviceModel model;
            if (string.IsNullOrWhiteSpace(code))
            {
                model = new DeviceModel();
            }
            else
            {
                using (var ctx = new UnicomDbContext())
                {
                    var dev = ctx.EmsDevices.First(d => d.Code == code);
                    model = new DeviceModel
                    {
                        Code = dev.Code,
                        Name = dev.Name,
                        UnicomName = dev.UnicomName,
                        IpAddr = dev.IpAddr,
                        MacAddr = dev.MacAddr,
                        Port = dev.Port,
                        Version = dev.Version,
                        ProjectCode = dev.ProjectCode,
                        Longitude = dev.Longitude,
                        Latitude = dev.Latitude,
                        StartDate = dev.StartDate,
                        EndDate = dev.EndDate,
                        InstallDate = dev.InstallDate,
                        OnlineStatus = dev.OnlineStatus,
                        VideoUrl = dev.VideoUrl,
                        IsTransfer = dev.IsTransfer
                    };
                }
            }
            LoadInfomation(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(DeviceModel model)
        {
            var device = new emsDevice
            {
                code = model.Code,
                name = model.UnicomName,
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

            return string.IsNullOrWhiteSpace(model.Code) ? AddNewDevice(device, model) : UpdateDevice(device, model);
        }

        private ActionResult AddNewDevice(emsDevice emsDevice, DeviceModel model)
        {
            var result = WebConfig.UniComService.PushDevices(new[] { emsDevice });
            if (result.result.Length > 0 && !result.result[0].value.ToString().Contains("ERROR"))
            {
                try
                {
                    model.Code = result.result[0].key.ToString();
                    using (var ctx = new UnicomDbContext())
                    {
                        ctx.EmsDevices.Add(new EmsDevice
                        {
                            Code = model.Code,
                            Name = model.Name,
                            UnicomName = model.UnicomName,
                            IpAddr = model.IpAddr,
                            MacAddr = model.MacAddr,
                            Port = model.Port,
                            Version = model.Version,
                            ProjectCode = model.ProjectCode,
                            Longitude = model.Longitude,
                            Latitude = model.Latitude,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            InstallDate = model.InstallDate,
                            OnlineStatus = model.OnlineStatus,
                            VideoUrl = model.VideoUrl,
                            IsTransfer = model.IsTransfer,
                            IsHandlerValues = model.IsHandlerValues,
                            TpMax = model.TpMax,
                            TpMin = model.TpMin
                        });
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

        private ActionResult UpdateDevice(emsDevice emsDevice, DeviceModel model)
        {
            var result = WebConfig.UniComService.PushDeviceStatus(new[] { emsDevice });
            if (result.result.Length <= 0)
            {
                try
                {
                    using (var ctx = new UnicomDbContext())
                    {
                        var dev = ctx.EmsDevices.First(d => d.Code == model.Code);
                        dev.Code = model.Code;
                        dev.Name = model.Name;
                        dev.IpAddr = model.IpAddr;
                        dev.MacAddr = model.MacAddr;
                        dev.Port = model.Port;
                        dev.Version = model.Version;
                        dev.ProjectCode = model.ProjectCode;
                        dev.Longitude = model.Longitude;
                        dev.Latitude = model.Latitude;
                        dev.StartDate = model.StartDate;
                        dev.EndDate = model.EndDate;
                        dev.InstallDate = model.InstallDate;
                        dev.OnlineStatus = model.OnlineStatus;
                        dev.VideoUrl = model.VideoUrl;
                        dev.IsTransfer = model.IsTransfer;
                        dev.IsHandlerValues = model.IsHandlerValues;
                        dev.TpMax = model.TpMax;
                        dev.TpMin = model.TpMin;
                        ctx.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("RegisterError", "更新设备信息成功，但保存至服务器时遇到异常，请记录设备信息并提供给管理员手动添加。");
                    LoadInfomation(model);
                    return View(model);
                }
                return Redirect("/Device/DeviceManager");
            }

            foreach (var entry in result.result)
            {
                ModelState.AddModelError("RegisterError", entry.value.ToString());
            }
            LoadInfomation(model);
            return View(model);
        }

        public ActionResult DeviceManager() => View();

        public ActionResult UnicomDeviceTable(BootstraptablePost post)
        {
            using (var ctx = new UnicomDbContext())
            {
                var total = ctx.EmsDevices.Count();
                var rows = ctx.EmsDevices.OrderBy(d => d.Code).Skip(post.offset).Take(post.limit)
                    .Select(dev => new
                    {
                        dev.Code,
                        dev.Name,
                        dev.Version,
                        dev.ProjectCode,
                        dev.IsTransfer
                    }).ToList();
                return Json(new
                {
                    total,
                    rows
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Stop(string code)
        {
            using (var ctx = new UnicomDbContext())
            {
                var dev = ctx.EmsDevices.First(d => d.Code == code);
                try
                {
                    dev.IsTransfer = false;
                    ctx.SaveChanges();
                    return Json("停止上传成功！", JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json("停止上传操作失败！", JsonRequestBehavior.AllowGet);
                }
            }
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