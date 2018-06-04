using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using ESMonitor.DataProvider.Models;
using SHWDTech.Platform.Utility.ExtensionMethod;
using Unicom.DataProvider;
using Unicom.Platform.Model.UnicomPlatform;

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
                var minute = DateTime.Now.GetCurrentMinute().AddMinutes(-1);
                var esMinDatas =
                    context.EsMin.Where(obj => obj.DevId == deviceId && obj.UpdateTime >= minute
                    && obj.TP > 0 && obj.DB > 0 && obj.Temperature > 0 && obj.Humidity > 0).Take(1).ToList();

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
                    context.EsHour.Where(obj => obj.DevId == deviceId && obj.UpdateTime >= hour
                                                && obj.TP > 0 && obj.DB > 0 && obj.Temperature > 0 && obj.Humidity > 0).ToList();

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
                    context.EsDay.Where(obj => obj.DevId == deviceId && obj.UpdateTime >= day
                                               && obj.TP > 0 && obj.DB > 0 && obj.Temperature > 0 && obj.Humidity > 0).ToList();

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

        public List<emsData> GetValidHistoryData()
        {
            using (var context = new EsMonitorModels())
            {
                var esDatas = context.EsMin.Where(obj => obj.TP > 100 && obj.TP < 1000)
                    .Take(10).ToList();

                return EsMinToEmsDatas(esDatas);
            }
        }

        public void AddNewData(emsData data, int statId, int devId, string country, int? statCodeUp)
        {
            using (var context = new EsMonitorModels())
            {
                var newData = new T_ESMin
                {
                    TP = data.dust,
                    DB = data.noise,
                    Temperature = data.temperature,
                    Humidity = data.humidity,
                    WindDirection = data.windDirection,
                    WindSpeed = data.windSpeed,
                    Airpressure = 0,
                    DevId = devId,
                    StatId = statId,
                    Country = country,
                    StatCode = statCodeUp
                };
                context.EsMin.Add(newData);
                context.SaveChanges();
            }
        }

        public void UpdateNewData(emsData data, int statId, int devId, string country, int? statCodeUp)
        {
            using (var context = new EsMonitorModels())
            {
                var last = context.EsMin.FirstOrDefault(d => d.DevId == devId);
                if (last == null) return;
                last.TP = data.dust;
                last.DB = data.noise;
                last.Temperature = data.temperature;
                last.Humidity = data.humidity;
                last.WindDirection = data.windDirection;
                last.WindSpeed = data.windSpeed;
                context.EsMin.AddOrUpdate(last);
                context.SaveChanges();
            }
        }

        public static T_Devs GetDevs(int devId) => new EsMonitorModels().T_Devs.First(s => s.Id == devId);

        public static T_Stats GetStatss(int statId) => new EsMonitorModels().T_Stats.First(s => s.Id == statId);

        private static List<emsData> EsMinToEmsDatas(IEnumerable<T_ESMin> esMins) => esMins.Select(esMin => new emsData
        {
            dust = ((float)esMin.TP) / 1000,
            temperature = (float)esMin.Temperature,
            humidity = (float)esMin.Humidity,
            noise = (int)esMin.DB,
            windSpeed = (float)esMin.WindSpeed,
            windDirection = (int)esMin.WindDirection,
            dateTime = ConvertToUnixTime(esMin.UpdateTime.Value),
            dustFlag = "N",
            humiFlag = "N",
            noiseFlag = "N"
        }).ToList();

        private static List<emsData> EsHourToEmsDatas(IEnumerable<T_ESHour> esHours) => esHours.Select(esHour => new emsData
        {
            dust = ((float)esHour.TP) / 1000,
            noise = (int)esHour.DB,
            temperature = (float)esHour.Temperature,
            humidity = (float)esHour.Humidity,
            windSpeed = (float)esHour.WindSpeed,
            windDirection = (int)esHour.WindDirection,
            dateTime = ConvertToUnixTime(esHour.UpdateTime),
            dustFlag = "N",
            humiFlag = "N",
            noiseFlag = "N"
        }).ToList();

        private static List<emsData> EsDayToEmsDatas(IEnumerable<T_ESDay> esDays) => esDays.Select(esDay => new emsData
        {
            dust = ((float)esDay.TP) / 1000,
            noise = (int)esDay.DB,
            temperature = (float)esDay.Temperature,
            humidity = (float)esDay.Humidity,
            windSpeed = (float)esDay.WindSpeed,
            windDirection = (int)esDay.WindDirection,
            dateTime = ConvertToUnixTime(esDay.UpdateTime),
            dustFlag = "N",
            humiFlag = "N",
            noiseFlag = "N"
        }).ToList();

        private static long ConvertToUnixTime(DateTime dateTime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(dateTime.ToUniversalTime() - sTime).TotalMilliseconds;
        }
    }
}
