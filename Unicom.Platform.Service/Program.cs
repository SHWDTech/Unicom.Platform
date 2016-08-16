using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using ESMonitor.DataProvider;
using SHWDTech.Platform.Utility;
using SHWDTech.Platform.Utility.ExtensionMethod;
using Unicom.Platform.Model;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
using Unicom.Platform.SQLite;
using Unicom.Task;

namespace Unicom.Platform.Service
{
    internal class Program
    {
        private static readonly UnicomService Service = new UnicomService();

        private static string _sqliteConnectionString;

        private static void Main()
        {
            _sqliteConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            InitUnicomUpload();
            while (true)
            {
                Thread.Sleep(60000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void InitUnicomUpload()
        {
            var context = new UnicomContext(_sqliteConnectionString);
            foreach (var device  in context.Devices)
            {
                if (device.OnTransfer)
                {
                    AddMinuteTask(device.SystemCode);
                    AddHourTask(device.SystemCode);
                    AddDayTask(device.SystemCode);
                }
            }
        }

        private static void MinuteTimerCallBack(object taskState)
        {
            try
            {
                var dataProvider = new EsMonitorDataProvider();
                var emsDatas = dataProvider.GetCurrentMinEmsDatas(taskState.ToString());
                AddDeviceInfo(emsDatas, taskState.ToString());
                var result = Service.PushRealTimeData(emsDatas.ToArray());
                OutputError(result);
                AddMinuteTask(taskState);
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("发送数据失败！", ex);
            }
            
        }

        private static void HourTimerCallBack(object taskState)
        {
            try
            {
                var dataProvider = new EsMonitorDataProvider();
                var emsDatas = dataProvider.GetCurrentHourEmsDatas(taskState.ToString());
                AddDeviceInfo(emsDatas, taskState.ToString());
                var result = Service.PushRealTimeData(emsDatas.ToArray());
                OutputError(result);
                AddHourTask(taskState);
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("发送数据失败！", ex);
            }
        }

        private static void DayTimerCallBack(object taskState)
        {
            try
            {
                var dataProvider = new EsMonitorDataProvider();
                var emsDatas = dataProvider.GetCurrentDayEmsDatas(taskState.ToString());
                AddDeviceInfo(emsDatas, taskState.ToString());
                var result = Service.PushRealTimeData(emsDatas.ToArray());
                OutputError(result);
                AddDayTask(taskState);
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("发送数据失败！", ex);
            }
        }

        private static void AddMinuteTask(object taskState)
        {
            var runTime = DateTime.Now.GetCurrentMinute().AddMinutes(5);
            var task = new Task.Task(MinuteTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }

        private static void AddHourTask(object taskState)
        {
            var runTime = DateTime.Now.GetCurrentHour().AddHours(1);
            var task = new Task.Task(HourTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }

        private static void AddDayTask(object taskState)
        {
            var runTime = DateTime.Now.GetToday().AddDays(1);
            var task = new Task.Task(DayTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }

        private static void OutputError(resultData result)
        {
            if (result.result.Length > 0)
            {
                foreach (var dataEntry in result.result)
                {
                    Console.WriteLine($"Result Error=> key:{dataEntry.key},value:{dataEntry.value}");
                }
            }
            else
            {
                Console.WriteLine($"发送数据成功，时间：{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}。");
            }
        }

        private static void AddDeviceInfo(List<emsData> emsDatas, string systemDeviceCode)
        {
            var context = new UnicomContext(_sqliteConnectionString);

            var device = context.FirstOrDefault<EmsDevice>($"SystemCode = {systemDeviceCode}");

            var project = context.FirstOrDefault<EmsProject>($"UnicomCode == {device.ProjectUnicomCode}");

            foreach (var emsData in emsDatas)
            {
                emsData.devCode = device.UnicomCode;
                emsData.prjCode = project.UnicomCode;
                emsData.prjType = project.PrjType;
            }
        }
    }
}
