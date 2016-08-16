using System;
using System.Collections.Generic;
using System.Linq;
using ESMonitor.DataProvider.Models;
using SHWDTech.Platform.Utility.ExtensionMethod;
using Unicom.DataProvider;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
// ReSharper disable PossibleInvalidOperationException

namespace ESMonitor.DataProvider
{
    public class EsMonitorDataProvider : IDataProvider
    {
        public List<emsData> GetCurrentMinEmsDatas(string devCode)
        {
            using (var context = new EsMonitorModels())
            {
                var deviceId = int.Parse(devCode);
                var minute = DateTime.Now.AddMinutes(-1);
                var esMinDatas =
                    context.EsMin.Where(obj => obj.DevId == deviceId && obj.UpdateTime > minute).ToList();

                return EsMinToEmsDatas(esMinDatas);
            }
        }

        public List<emsData> GetCurrentHourEmsDatas(string devCode)
        {
            using (var context = new EsMonitorModels())
            {
                var hour = DateTime.Now.GetPreviousHour();
                var deviceId = int.Parse(devCode);
                var esHourDatas =
                    context.EsHour.Where(obj => obj.DevId == deviceId && obj.UpdateTime > hour).ToList();

                return EsHourToEmsDatas(esHourDatas);
            }
        }

        public List<emsData> GetCurrentDayEmsDatas(string devCode)
        {
            using (var context = new EsMonitorModels())
            {
                var day = DateTime.Now.GetPrevioudDay();
                var deviceId = int.Parse(devCode);
                var esDayDatas =
                    context.EsDay.Where(obj => obj.DevId == deviceId && obj.UpdateTime > day).ToList();

                return EsDayToEmsDatas(esDayDatas);
            }
        }

        public List<emsData> GetHistoryMinEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme)
        {
            using (var context = new EsMonitorModels())
            {
                var deviceId = int.Parse(devCode);
                var esMinDatas =
                    context.EsMin.Where(obj => obj.DevId == deviceId && obj.UpdateTime > startDateTime && obj.UpdateTime < endDateTIme).ToList();

                return EsMinToEmsDatas(esMinDatas);
            }
        }

        public List<emsData> GetHistoryHourEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme)
        {
            using (var context = new EsMonitorModels())
            {
                var deviceId = int.Parse(devCode);
                var esHourDatas =
                    context.EsHour.Where(obj => obj.DevId == deviceId && obj.UpdateTime > startDateTime && obj.UpdateTime < endDateTIme).ToList();

                return EsHourToEmsDatas(esHourDatas);
            }
        }

        public List<emsData> GetHistoryDayEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme)
        {
            using (var context = new EsMonitorModels())
            {
                var deviceId = int.Parse(devCode);
                var esDayDatas =
                    context.EsDay.Where(obj => obj.DevId == deviceId && obj.UpdateTime > startDateTime && obj.UpdateTime < endDateTIme).ToList();

                return EsDayToEmsDatas(esDayDatas);
            }
        }

        private List<emsData> EsMinToEmsDatas(List<T_ESMin> esMins)
        {
            var emsDatas = new List<emsData>();
            foreach (var esMin in esMins)
            {
                var emsData = new emsData
                {
                    dust = ((float)esMin.TP) / 1000,
                    temperature = (float)esMin.Temperature,
                    humidity = (float)esMin.Humidity,
                    noise = (int)esMin.DB,
                    windSpeed = (float)esMin.WindSpeed,
                    windDirection = (int)esMin.WindDirection,
                    dateTime = ConvertToUnixTime(esMin.UpdateTime.Value)
                };

                emsDatas.Add(emsData);
            }

            return emsDatas;
        }

        private List<emsData> EsHourToEmsDatas(List<T_ESHour> esHours)
        {
            var emsDatas = new List<emsData>();
            foreach (var esHour in esHours)
            {
                var emsData = new emsData
                {
                    dust = ((float)esHour.TP) / 1000,
                    noise = (int)esHour.DB,
                    dateTime = ConvertToUnixTime(esHour.UpdateTime)
                };

                emsDatas.Add(emsData);
            }

            return emsDatas;
        }

        private List<emsData> EsDayToEmsDatas(List<T_ESDay> esDays)
        {
            var emsDatas = new List<emsData>();
            foreach (var esDay in esDays)
            {
                var emsData = new emsData
                {
                    dust = ((float)esDay.TP) / 1000,
                    noise = (int)esDay.DB,
                    dateTime = ConvertToUnixTime(esDay.UpdateTime)
                };

                emsDatas.Add(emsData);
            }

            return emsDatas;
        }

        private static long ConvertToUnixTime(DateTime dateTime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long) (dateTime.ToUniversalTime() - sTime).TotalMilliseconds;
        }
    }
}
