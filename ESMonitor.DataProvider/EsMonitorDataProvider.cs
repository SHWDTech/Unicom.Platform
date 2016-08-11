using System;
using System.Collections.Generic;
using System.Linq;
using ESMonitor.DataProvider.Models;
using Unicom.DataProvider;
using Unicom.Platform.Model.Service_References.UnicomPlatform;

namespace ESMonitor.DataProvider
{
    public class EsMonitorDataProvider : IDataProvider
    {
        public List<emsData> GetCurrentMinEmsDatas(string devCode)
        {
            using (var context = new EsMonitorModels())
            {
                var deviceId = int.Parse(devCode);
                var esMinDatas =
                    context.EsMin.Where(obj => obj.DevId == deviceId && obj.UpdateTime > DateTime.Now).ToList();

                return EsMinToEmsDatas(esMinDatas);
            }
        }

        public List<emsData> GetCurrentHourEmsDatas(string devCode)
        {
            throw new NotImplementedException();
        }

        public List<emsData> GetCurrentDayEmsDatas(string devCode)
        {
            throw new NotImplementedException();
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

        private List<emsData> EsMinToEmsDatas(List<T_ESMin> esMins)
        {
            var emsDatas = new List<emsData>();
            foreach (var esMin in esMins)
            {
                var emsData = new emsData
                {

                };
            }

            return emsDatas;
        }
    }
}
