using System;
using System.Collections.Generic;
using System.Linq;
using MTWESensorData.DataProvider.Models;
using Unicom.DataProvider;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
using SHWDTech.Platform.Utility.ExtensionMethod;

// ReSharper disable InconsistentNaming

namespace MTWESensorData.DataProvider
{
    public class MTWEDataProvider : IDataProvider
    {
        public List<emsData> GetCurrentMinEmsDatas(string devCode)
        {
            using (var context = new MTWEModels())
            {
                var minute = DateTime.Now.GetCurrentMinute().AddMinutes(-1);
                var mtweMinDatas = context.SensorDataMin
                    .Where(d => d.StatCode == devCode && d.DataTime >= minute && d.TP > 0 && d.DB > 0 && d.Humidity > 0)
                    .OrderByDescending(data => data.DataTime)
                    .Take(1).ToList();

                return MtweMinToEmsDatas(mtweMinDatas);
            }
        }

        public List<emsData> GetCurrentHourEmsDatas(string devCode)
        {
            return new List<emsData>();
        }

        public List<emsData> GetCurrentDayEmsDatas(string devCode)
        {
            return new List<emsData>();
        }

        public List<emsData> GetHistoryMinEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme)
        {
            throw new NotImplementedException();
        }

        public List<emsData> GetHistoryHourEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme)
        {
            throw new NotImplementedException();
        }

        public List<emsData> GetHistoryDayEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme)
        {
            throw new NotImplementedException();
        }

        public List<emsData> GetValidHistoryData()
        {
            using (var context = new MTWEModels())
            {
                var mtweMinDatas = context.SensorDataMin
                    .Where(d => d.TP > 100 && d.TP < 1000)
                    .OrderByDescending(data => data.DataTime)
                    .Take(10).ToList();

                return MtweMinToEmsDatas(mtweMinDatas);
            }
        }

        private static List<emsData> MtweMinToEmsDatas(IEnumerable<sensor_data_min> mtweMins) => mtweMins.Select(
            min => new emsData
            {
                dust = min.TP,
                temperature = min.Temperature,
                humidity = min.Humidity,
                noise = (int)min.DB,
                windSpeed = min.WindSpeed,
                windDirection = (int)min.WindDirection,
                dateTime = ConvertToUnixTime(min.DataTime),
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
