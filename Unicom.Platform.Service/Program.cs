using System;
using System.Threading;
using ESMonitor.DataProvider;
using SHWDTech.Platform.Utility.ExtensionMethod;
using Unicom.Platform.SQLite;
using Unicom.Task;

namespace Unicom.Platform.Service
{
    internal class Program
    {
        private static readonly UnicomService Service = new UnicomService();

        private static void Main()
        {
            InitUnicomUpload();
            while (true)
            {
                Thread.Sleep(60000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void InitUnicomUpload()
        {
            var context = new UnicomContext("Data Source=d:\\App_Data\\Unicom_Platform.sqlite3;Version=3;");
            foreach (var device  in context.Devices)
            {
                AddMinuteTask(device.SystemCode);
                AddHourTask(device.SystemCode);
            }
        }

        private static void MinuteTimerCallBack(object taskState)
        {
            var dataProvider = new EsMonitorDataProvider();
            var emsDatas = dataProvider.GetCurrentMinEmsDatas(taskState.ToString());
            Service.PushRealTimeData(emsDatas.ToArray());
            AddMinuteTask(taskState);
        }

        private static void HourTimerCallBack(object taskState)
        {
            var dataProvider = new EsMonitorDataProvider();
            var emsDatas = dataProvider.GetCurrentHourEmsDatas(taskState.ToString());
            Service.PushRealTimeData(emsDatas.ToArray());
            AddHourTask(taskState);
        }

        private static void DayTimerCallBack(object taskState)
        {
            var dataProvider = new EsMonitorDataProvider();
            var emsDatas = dataProvider.GetCurrentDayEmsDatas(taskState.ToString());
            Service.PushRealTimeData(emsDatas.ToArray());
            AddDayTask(taskState);
        }

        private static void AddMinuteTask(object taskState)
        {
            var runTime = DateTime.Now.GetCurrentMinute().AddMinutes(1);
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
            var runTime = DateTime.Now.GetToday().AddHours(1);
            var task = new Task.Task(DayTimerCallBack, new ScheduleExecutionOnce(runTime));
            task.Start(taskState);
        }
    }
}
