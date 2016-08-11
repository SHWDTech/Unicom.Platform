using System;
using System.Collections.Generic;
using Unicom.Platform.Model.Service_References.UnicomPlatform;

namespace Unicom.DataProvider
{
    public interface IDataProvider
    {
        List<emsData> GetCurrentMinEmsDatas(string devCode);

        List<emsData> GetCurrentHourEmsDatas(string devCode);

        List<emsData> GetCurrentDayEmsDatas(string devCode);

        List<emsData> GetHistoryMinEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme);

        List<emsData> GetHistoryHourEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme);

        List<emsData> GetHistoryDayEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme);
    }
}
