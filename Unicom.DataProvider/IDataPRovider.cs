using System;
using System.Collections.Generic;
using Unicom.Platform.Model.UnicomPlatform;

namespace Unicom.DataProvider
{
    /// <summary>
    /// 联通平台数据提供容器
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 获取指定设备的当前分钟均值
        /// </summary>
        /// <param name="devCode"></param>
        /// <returns></returns>
        List<emsData> GetCurrentMinEmsDatas(string devCode);

        /// <summary>
        /// 获取指定设备的当前小时均值
        /// </summary>
        /// <param name="devCode"></param>
        /// <returns></returns>
        List<emsData> GetCurrentHourEmsDatas(string devCode);

        /// <summary>
        /// 获取指定设备的当前日均值
        /// </summary>
        /// <param name="devCode"></param>
        /// <returns></returns>
        List<emsData> GetCurrentDayEmsDatas(string devCode);

        /// <summary>
        /// 获取指定设备的历史分钟均值
        /// </summary>
        /// <param name="devCode"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTIme"></param>
        /// <returns></returns>
        List<emsData> GetHistoryMinEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme);

        /// <summary>
        /// 获取指定设备的历史小时均值
        /// </summary>
        /// <param name="devCode"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTIme"></param>
        /// <returns></returns>
        List<emsData> GetHistoryHourEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme);

        /// <summary>
        /// 获取指定设备的历史日均值
        /// </summary>
        /// <param name="devCode"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTIme"></param>
        /// <returns></returns>
        List<emsData> GetHistoryDayEmsDatas(string devCode, DateTime startDateTime, DateTime endDateTIme);

        List<emsData> GetValidHistoryData();

        void AddNewData(emsData data, int statId, int devId, string country, int? statCodeUp);

        void UpdateNewData(emsData data, int statId, int devId, string country, int? statCodeUp);
    }
}
