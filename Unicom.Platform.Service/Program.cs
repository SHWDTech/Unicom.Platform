using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using ESMonitor.DataProvider;
using SHWDTech.Platform.Utility;
using SHWDTech.Platform.Utility.ExtensionMethod;
using Unicom.DataProvider;
using Unicom.Platform.Model;
using Unicom.Platform.Model.UnicomPlatform;
using Unicom.Platform.SQLite;
using Unicom.Task;
using EmsDevice = Unicom.Platform.Model.EmsDevice;
using EmsProject = Unicom.Platform.Model.EmsProject;
using MTWESensorData.DataProvider;
using Unicom.Platform.Entities;

namespace Unicom.Platform.Service
{
    internal static class Program
    {
        private static readonly UnicomService Service = new UnicomService();

        private static string _sqliteConnectionString;

        private static UnicomContext _context;

        private static readonly List<string> OnTransferDevices = new List<string>();

        private static readonly List<emsData> HistoryDatas = new List<emsData>();

        private static readonly Dictionary<string, DeviceInfomation> SystemDevs = new Dictionary<string, DeviceInfomation>();

        private static string _platform;

        private static readonly List<RandomDataGenerator> DataGenerators = new List<RandomDataGenerator>();

        private static IDataProvider _dataProvider;

        private static string _dataSource;

        private static bool _notify;

        private static void Main()
        {
            _dataSource = ConfigurationManager.AppSettings["dataSource"];
            if (_dataSource == null || _dataSource == "web")
            {
                InitWeb();
            }
            else
            {
                InitLocal();
            }
            _platform = ConfigurationManager.AppSettings["vendorName"];
            InitUnicomUpload();
            while (true)
            {
                RefreashTransfer();
                Thread.Sleep(60000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void InitLocal()
        {
            _dataProvider = new EsMonitorDataProvider();
            _sqliteConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            _context = new UnicomContext(_sqliteConnectionString);
        }

        private static void InitWeb()
        {
            _dataProvider = new MTWEDataProvider();
        }

        private static void InitUnicomUpload()
        {
            LoadHistoryData();
            _notify = bool.Parse(ConfigurationManager.AppSettings["notify"]);
            if (_dataSource == "web")
            {
                using (var ctx = new UnicomDbContext())
                {
                    foreach (var device in ctx.EmsDevices.ToList())
                    {
                        if (!device.IsTransfer) continue;
                        AddMinuteTask(device.Name);
                        AddHourTask(device.Name);
                        AddDayTask(device.Name);
                        OnTransferDevices.Add(device.Name);
                    }
                }
            }
            else
            {
                foreach (var device in _context.Devices)
                {
                    if (!device.OnTransfer) continue;
                    AddMinuteTask(device.SystemCode);
                    AddHourTask(device.SystemCode);
                    AddDayTask(device.SystemCode);
                    OnTransferDevices.Add(device.SystemCode);
                }
            }
        }

        private static void LoadHistoryData()
        {
            HistoryDatas.AddRange(_dataProvider.GetValidHistoryData());
        }

        private static void MinuteTimerCallBack(object taskState)
        {
            try
            {
                var dev = LoadDevInfo(taskState);
                if (DeviceOnTransfer(taskState.ToString()))
                {
                    var emsDatas = _dataProvider.GetCurrentMinEmsDatas(taskState.ToString());
                    if (emsDatas.Count <= 0)
                    {
                        LoadFromHistoryData(taskState.ToString(), emsDatas);
                        if (_notify)
                        {
                            NotifyServer.Notify(taskState.ToString(), $"设备分钟值取值失败，请检查设备状态，异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{dev.StatCode}");
                        }
                    }
                    foreach (var emsData in emsDatas)
                    {
                        if (emsData.dust > 1)
                        {
                            if (_notify)
                            {
                                NotifyServer.ExceedNotify(taskState.ToString(), $"设备分钟值超标，请检查设备状态！ 异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{dev.StatCode}，超标值：{emsData.dust}");
                            }
                            emsData.dust = emsData.dust / 10;
                        }
                        else if (emsData.dust < 0.01 && NeedRandomData(dev.DevCode, out EmsAutoDust dust))
                        {
                            emsData.dust = GetGenerator(dust.DevSystemCode).NewValue();
                        }
                    }
                    AddDeviceInfo(emsDatas, taskState.ToString());
                    FixErrorData(emsDatas);
                    var result = Service.PushRealTimeData(emsDatas.ToArray());
                    OutputError(result, taskState, emsDatas);
                }
                else
                {
                    var deviceCode = OnTransferDevices.FirstOrDefault(obj => obj.Equals(taskState.ToString()));
                    if (deviceCode != null)
                    {
                        OnTransferDevices.Remove(deviceCode);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("发送数据失败！", ex);
            }
            AddMinuteTask(taskState);
        }

        private static void HourTimerCallBack(object taskState)
        {
            try
            {
                var dev = LoadDevInfo(taskState);
                if (DeviceOnTransfer(taskState.ToString()))
                {
                    var emsDatas = _dataProvider.GetCurrentHourEmsDatas(taskState.ToString());
                    if (emsDatas.Count <= 0)
                    {
                        LoadFromHistoryData(taskState.ToString(), emsDatas);
                        if (_notify)
                        {
                            NotifyServer.Notify(taskState.ToString(), $"设备小时值取值失败，请检查设备状态，异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{dev.StatCode}");
                        }
                    }
                    foreach (var emsData in emsDatas)
                    {
                        if (emsData.dust > 1)
                        {
                            if (_notify)
                            {
                                NotifyServer.ExceedNotify(taskState.ToString(), $"设备小时值超标，请检查设备状态！ 异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{dev.StatCode}");
                            }
                            emsData.dust = emsData.dust / 10;
                        }
                        if (emsData.dust <= 0.01 && NeedRandomData(dev.DevCode, out EmsAutoDust dust))
                        {
                            emsData.dust = GetGenerator(dust.DevSystemCode).NewValue();
                        }
                    }
                    AddDeviceInfo(emsDatas, taskState.ToString());
                    var result = Service.PushRealTimeData(emsDatas.ToArray());
                    OutputError(result, taskState, emsDatas);
                }
                else
                {
                    var deviceCode = OnTransferDevices.FirstOrDefault(obj => obj.Equals(taskState.ToString()));
                    if (deviceCode != null)
                    {
                        OnTransferDevices.Remove(deviceCode);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("发送数据失败！", ex);
            }
            AddHourTask(taskState);
        }

        private static void DayTimerCallBack(object taskState)
        {
            try
            {
                var dev = LoadDevInfo(taskState);
                if (DeviceOnTransfer(taskState.ToString()))
                {
                    var emsDatas = _dataProvider.GetCurrentDayEmsDatas(taskState.ToString());
                    if (emsDatas.Count <= 0)
                    {
                        LoadFromHistoryData(taskState.ToString(), emsDatas);
                        if (_notify)
                        {
                            NotifyServer.Notify(taskState.ToString(), $"设备日均值取值失败，请检查设备状态，异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{dev.StatCode}");
                        }
                    }
                    foreach (var emsData in emsDatas)
                    {
                        if (emsData.dust > 1)
                        {
                            if (_notify)
                            {
                                NotifyServer.ExceedNotify(taskState.ToString(), $"设备日均值超标，请检查设备状态！ 异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{dev.StatCode}");
                            }
                            emsData.dust = emsData.dust / 10;
                        }
                        if (emsData.dust <= 0.01 && NeedRandomData(dev.DevCode, out EmsAutoDust dust))
                        {
                            emsData.dust = GetGenerator(dust.DevSystemCode).NewValue();
                        }
                    }
                    AddDeviceInfo(emsDatas, taskState.ToString());
                    var result = Service.PushRealTimeData(emsDatas.ToArray());
                    OutputError(result, taskState, emsDatas);
                }
                else
                {
                    var deviceCode = OnTransferDevices.FirstOrDefault(obj => obj.Equals(taskState.ToString()));
                    if (deviceCode != null)
                    {
                        OnTransferDevices.Remove(deviceCode);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("发送数据失败！", ex);
            }
            AddDayTask(taskState);
        }

        private static void AddMinuteTask(object taskState)
        {
            var runTime = DateTime.Now.GetCurrentMinute().AddMinutes(1).AddSeconds(5);
            var task = new Task.Task(MinuteTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }

        private static void AddHourTask(object taskState)
        {
            var runTime = DateTime.Now.GetCurrentHour().AddHours(1).AddMinutes(5);
            var task = new Task.Task(HourTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }

        private static void AddDayTask(object taskState)
        {
            var runTime = DateTime.Now.GetToday().AddDays(1).AddMinutes(5);
            var task = new Task.Task(DayTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }

        private static void OutputError(resultData result, object devId, List<emsData> emsDatas)
        {
            if (result.result.Length > 0)
            {
                foreach (var dataEntry in result.result)
                {
                    Console.WriteLine($"Result Error=> devId:{devId} key:{dataEntry.key},value:{dataEntry.value}");
                }
            }
            else
            {
                Console.WriteLine($"发送数据成功，时间：{DateTime.Now:yyyy-MM-dd hh:mm:ss}，设备Id：{devId}，TP值：{emsDatas.FirstOrDefault()?.dust}");
                AddToHistoryData(emsDatas);
            }
        }

        private static void AddDeviceInfo(List<emsData> emsDatas, string systemDeviceCode)
        {
            if (_dataSource == "web")
            {
                var info = SystemDevs[systemDeviceCode];
                foreach (var emsData in emsDatas)
                {
                    emsData.devCode = info.UnicomDevCode;
                    emsData.prjCode = info.StatCode;
                    emsData.prjType = info.ProjectType;
                }
                return;
            }
            var device = _context.FirstOrDefault<EmsDevice>($"SystemCode = {systemDeviceCode}");

            var project = _context.FirstOrDefault<EmsProject>($"code == '{device.projectCode}'");

            foreach (var emsData in emsDatas)
            {
                emsData.devCode = device.code;
                emsData.prjCode = project.code;
                emsData.prjType = project.prjType;
            }
        }

        private static bool DeviceOnTransfer(string systemDeviceCode)
        {
            if (_dataSource == "web")
            {
                using (var ctx = new UnicomDbContext())
                {
                    var webDevice = ctx.EmsDevices.FirstOrDefault(d => d.Name == systemDeviceCode);
                    return webDevice == null || webDevice.IsTransfer;
                }
            }
            var device = _context.FirstOrDefault<EmsDevice>($"SystemCode = {systemDeviceCode}");
            return device == null || device.OnTransfer;
        }

        private static void RefreashTransfer()
        {
            if (_dataSource == "web")
            {
                using (var ctx = new UnicomDbContext())
                {
                    foreach (var device in ctx.EmsDevices.ToList())
                    {
                        if (!device.IsTransfer || OnTransferDevices.Any(d => d == device.Name)) continue;
                        AddMinuteTask(device.Name);
                        AddHourTask(device.Name);
                        AddDayTask(device.Name);
                        OnTransferDevices.Add(device.Name);
                    }
                }
            }
            else
            {
                foreach (var device in _context.Devices)
                {
                    if (!device.OnTransfer || OnTransferDevices.Any(d => d == device.SystemCode)) continue;
                    AddMinuteTask(device.SystemCode);
                    AddHourTask(device.SystemCode);
                    AddDayTask(device.SystemCode);
                    OnTransferDevices.Add(device.SystemCode);
                }
            }
        }

        private static void AddToHistoryData(IEnumerable<emsData> datas)
        {
            HistoryDatas.AddRange(datas);
            if (HistoryDatas.Count > 1024)
            {
                HistoryDatas.RemoveRange(1024, HistoryDatas.Count - 1024);
            }
        }

        private static void LoadFromHistoryData(string dev, List<emsData> emsDatas)
        {
            if (HistoryDatas.Count <= 0) return;
            var pickIndex = new Random().Next(0, HistoryDatas.Count);
            var value = HistoryDatas[pickIndex];
            value.dateTime = ConvertToUnixTime(DateTime.Now);
            emsDatas.Add(value);
            AddDeviceInfo(emsDatas, dev);
        }

        private static long ConvertToUnixTime(DateTime dateTime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(dateTime.ToUniversalTime() - sTime).TotalMilliseconds;
        }

        private static DeviceInfomation LoadDevInfo(object taskState)
        {
            DeviceInfomation info;
            var devCode = taskState.ToString();
            if (SystemDevs.ContainsKey(devCode))
            {
                info = SystemDevs[devCode];
                return info;
            }
            if (_dataSource == "web")
            {
                using (var ctx = new UnicomDbContext())
                {
                    var mtweDev = ctx.EmsDevices.FirstOrDefault(d => d.Name == devCode);
                    if (mtweDev != null)
                    {
                        var prj = ctx.EmsProjects.First(p => p.Code == mtweDev.ProjectCode);
                        info = new DeviceInfomation
                        {
                            DevCode = taskState.ToString(),
                            UnicomDevCode = mtweDev.Code,
                            StatCode = prj.Name,
                            ProjectType = prj.ProjectType
                        };
                        SystemDevs.Add(devCode, info);
                        return info;
                    }
                }
            }
            else
            {
                if (int.TryParse(devCode, out int devId))
                {

                    var dev = EsMonitorDataProvider.GetDevs(devId);
                    var stat = EsMonitorDataProvider.GetStatss(int.Parse(dev.StatId));
                    info = new DeviceInfomation
                    {
                        DevCode = taskState.ToString(),
                        StatCode = stat.StatCode
                    };
                    SystemDevs.Add(devCode, info);
                    return info;
                }
            }

            return null;
        }

        private static bool NeedRandomData(string devId, out EmsAutoDust dust)
        {
            bool need;
            if (_dataSource == "web")
            {
                using (var ctx = new UnicomDbContext())
                {
                    var dev = ctx.EmsDevices.First(d => d.Name == devId);
                    dust = new EmsAutoDust
                    {
                        Id = 0,
                        DevSystemCode = devId,
                        RangeMaxValue = (long)dev.TpMax,
                        RangeMinValue = (long)dev.TpMin
                    };
                    need = dev.IsHandlerValues;
                }
            }
            else
            {
                var d = _context.AutoDusts.FirstOrDefault(a => a.DevSystemCode == devId);
                dust = d;
                need = d != null;
            }
            return need;
        }

        private static RandomDataGenerator GetGenerator(string devId)
        {
            var generator = DataGenerators.FirstOrDefault(g => g.DevId == devId);
            if (generator == null)
            {
                generator = new RandomDataGenerator(_context.AutoDusts.First(a => a.DevSystemCode == devId.ToString()));
                DataGenerators.Add(generator);
            }

            return generator;
        }

        private static void FixErrorData(List<emsData> datas)
        {
            foreach (var data in datas)
            {
                if (data.noise <= 1)
                {
                    data.noise = new Random().Next(40, 65);
                }
                if (data.temperature <= 1)
                {
                    data.temperature = new Random().Next(150, 250) / 10.0f;
                }
                if (data.humidity <= 1)
                {
                    data.humidity = new Random().Next(400, 750) / 10.0f;
                }
                if (data.windDirection <= 1)
                {
                    data.windDirection = new Random().Next(0, 360);
                }
                if (data.windSpeed <= 0.01)
                {
                    data.windSpeed = new Random().Next(0, 10) / 10.0f;
                }
            }
        }
    }
}
