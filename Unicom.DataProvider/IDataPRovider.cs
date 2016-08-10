using System.Collections.Generic;
using Unicom.Platform.Model.UnicomPlatform;

namespace Unicom.DataProvider
{
    public interface IDataPRovider
    {
        List<emsData> GetCurrentEmsDatas(string devCode);

        List<emsData> GetHourEmsDatas(string devCode);

        List<emsData> GetDayEmsDatas(string devCode);
    }
}
