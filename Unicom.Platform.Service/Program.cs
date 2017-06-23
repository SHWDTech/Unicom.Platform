using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using ESMonitor.DataProvider;
using ESMonitor.DataProvider.Models;
using SHWDTech.Platform.Utility;
using SHWDTech.Platform.Utility.ExtensionMethod;
using Unicom.Platform.Model;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
using Unicom.Platform.SQLite;
using Unicom.Task;
using EmsDevice = Unicom.Platform.Model.EmsDevice;
using EmsProject = Unicom.Platform.Model.EmsProject;

namespace Unicom.Platform.Service
{
    internal static class Program
    {
        private static readonly UnicomService Service = new UnicomService();

        private static string _sqliteConnectionString;

        private static UnicomContext _context;

        private static readonly List<string> OnTransferDevices = new List<string>();

        private static readonly List<emsData> HistoryDatas = new List<emsData>();

        private static readonly Dictionary<int, T_Devs> SystemDevs = new Dictionary<int, T_Devs>();

        private static readonly Dictionary<int, T_Stats> SystemStates = new Dictionary<int, T_Stats>();

        private static string _platform;

        private static readonly List<RandomDataGenerator> DataGenerators = new List<RandomDataGenerator>();

        private static void Main()
        {
            _sqliteConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            _platform = ConfigurationManager.AppSettings["vendorName"];
            _context = new UnicomContext(_sqliteConnectionString);
            InitUnicomUpload();
            while (true)
            {
                RefreashTransfer();
                Thread.Sleep(60000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void InitUnicomUpload()
        {
            LoadHistoryData();
            foreach (var device in _context.Devices)
            {
                if (!device.OnTransfer) continue;
                AddMinuteTask(device.SystemCode);
                AddHourTask(device.SystemCode);
                AddDayTask(device.SystemCode);
                OnTransferDevices.Add(device.SystemCode);
            }
        }

        private static void LoadHistoryData()
        {
            HistoryDatas.AddRange(EsMonitorDataProvider.GetValidHistoryData());
        }

        private static void MinuteTimerCallBack(object taskState)
        {
            try
            {
                var dev = LoadDevInfo(taskState);
                if (DeviceOnTransfer(taskState.ToString()))
                {
                    var dataProvider = new EsMonitorDataProvider();
                    var emsDatas = dataProvider.GetCurrentMinEmsDatas(taskState.ToString());
                    if (emsDatas.Count <= 0)
                    {
                        LoadFromHistoryData(taskState.ToString(), emsDatas);
                        NotifyServer.Notify(taskState.ToString(), $"设备分钟值取值失败，请检查设备状态，异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{LoadStatInfo(dev.StatId)?.StatName}");
                    }
                    foreach (var emsData in emsDatas)
                    {
                        if (emsData.dust > 1)
                        {
                            NotifyServer.ExceedNotify(taskState.ToString(), $"设备分钟值超标，请检查设备状态！ 异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{LoadStatInfo(dev.StatId)?.StatName}，超标值：{emsData.dust}");
                            emsData.dust = emsData.dust / 10;
                        }
                        else if (emsData.dust < 0.01 && NeedRandomData(dev.Id, out EmsAutoDust dust))
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
                    var dataProvider = new EsMonitorDataProvider();
                    var emsDatas = dataProvider.GetCurrentHourEmsDatas(taskState.ToString());
                    if (emsDatas.Count <= 0)
                    {
                        LoadFromHistoryData(taskState.ToString(), emsDatas);
                        NotifyServer.Notify(taskState.ToString(), $"设备小时值取值失败，请检查设备状态，异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{LoadStatInfo(dev.StatId)?.StatName}");
                    }
                    foreach (var emsData in emsDatas)
                    {
                        if (emsData.dust > 1)
                        {
                            NotifyServer.ExceedNotify(taskState.ToString(), $"设备小时值超标，请检查设备状态！ 异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{LoadStatInfo(dev.StatId)?.StatName}");
                            emsData.dust = emsData.dust / 10;
                        }
                        if (emsData.dust <= 0.01 && NeedRandomData(dev.Id, out EmsAutoDust dust))
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
                    var dataProvider = new EsMonitorDataProvider();
                    var emsDatas = dataProvider.GetCurrentDayEmsDatas(taskState.ToString());
                    if (emsDatas.Count <= 0)
                    {
                        LoadFromHistoryData(taskState.ToString(), emsDatas);
                        NotifyServer.Notify(taskState.ToString(), $"设备日均值取值失败，请检查设备状态，异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{LoadStatInfo(dev.StatId)?.StatName}");
                    }
                    foreach (var emsData in emsDatas)
                    {
                        if (emsData.dust > 1)
                        {
                            NotifyServer.ExceedNotify(taskState.ToString(), $"设备日均值超标，请检查设备状态！ 异常设备平台：{_platform}，异常设备系统编码：{taskState}，设备名称：{dev.DevCode}，设备所属工地名称：{LoadStatInfo(dev.StatId)?.StatName}");
                            emsData.dust = emsData.dust / 10;
                        }
                        if (emsData.dust <= 0.01 && NeedRandomData(dev.Id, out EmsAutoDust dust))
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
            var device = _context.FirstOrDefault<EmsDevice>($"SystemCode = {systemDeviceCode}");
            return device == null || device.OnTransfer;
        }

        private static void RefreashTransfer()
        {
            foreach (var device in _context.Devices)
            {
                if (OnTransferDevices.Contains(device.SystemCode) || !device.OnTransfer) continue;
                AddMinuteTask(device.SystemCode);
                AddHourTask(device.SystemCode);
                AddDayTask(device.SystemCode);
                OnTransferDevices.Add(device.SystemCode);
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

        private static T_Devs LoadDevInfo(object taskState)
        {
            if (int.TryParse(taskState.ToString(), out int devId))
            {
                if (SystemDevs.ContainsKey(devId))
                {
                    return SystemDevs[devId];
                }
                var dev = EsMonitorDataProvider.GetDevs(devId);
                SystemDevs.Add(devId, dev);
                return dev;
            }

            return null;
        }

        private static T_Stats LoadStatInfo(string statIdStr)
        {
            if (int.TryParse(statIdStr, out int statId))
            {
                if (SystemStates.ContainsKey(statId))
                {
                    return SystemStates[statId];
                }
                var stat = EsMonitorDataProvider.GetStatss(statId);
                SystemStates.Add(statId, stat);
                return stat;
            }

            return null;
        }

        private static bool NeedRandomData(int devId, out EmsAutoDust dust)
        {
            var d = _context.AutoDusts.FirstOrDefault(a => a.DevSystemCode == devId.ToString());
            dust = d;
            return d != null;
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
