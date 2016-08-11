using SHWDTech.Platform.Utility;

namespace Unicom.DataProvider
{
    public class UnicomDatas
    {
        /// <summary>
        /// 获取数据提供容器
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IDataProvider DataProvider(string providerName)
            => UnityFactory.Resolve<IDataProvider>(providerName);
    }
}
