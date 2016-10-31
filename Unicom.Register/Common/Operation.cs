using Unicom.Platform;
using Unicom.Platform.Model;
using Unicom.Platform.SQLite;

namespace Unicom.Register.Common
{
    public class Operation
    {
        private static readonly UnicomService Service = new UnicomService();

        private static readonly UnicomContext Context = new UnicomContext(AppConfig.ConnectionString);
        /// <summary>
        /// 刷新基础信息
        /// </summary>
        public static void RefreashBaseInfo()
        {
            RefreashPrjType();

            RefreashPrjPeriod();

            RefreashPrjCategory();

            RefreashRegion();

            RefreashDistrict();
        }

        private static void RefreashPrjType()
        {
            var types = Service.PullProjectType();

            foreach (var emsPrjType in types)
            {
                var ret = Context.GetId<EmsPrjType>($"Code = {emsPrjType.Code}");
                if (ret != null)
                {
                    var id = (long) ret;
                    Context.AddOrUpdate(new EmsPrjType
                    {
                        Code = emsPrjType.Code.ToString(),
                        Id = id,
                        Name = emsPrjType.Name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsPrjType
                    {
                        Code = emsPrjType.Code.ToString(),
                        Id = -1,
                        Name = emsPrjType.Name

                    });
                }
            }
        }

        private static void RefreashPrjPeriod()
        {
            var periods = Service.PullProjectPeriod();

            foreach (var emsPrjPeriod in periods)
            {
                var ret = Context.GetId<EmsPrjPeriod>($"Code = {emsPrjPeriod.Code}");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsPrjPeriod
                    {
                        Code = emsPrjPeriod.Code.ToString(),
                        Id = id,
                        Name = emsPrjPeriod.Name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsPrjPeriod
                    {
                        Code = emsPrjPeriod.Code.ToString(),
                        Id = -1,
                        Name = emsPrjPeriod.Name

                    });
                }
            }
        }

        private static void RefreashPrjCategory()
        {
            var categories = Service.PullProjectCategory();

            foreach (var emsPrjCategory in categories)
            {
                var ret = Context.GetId<EmsPrjCategory>($"Code = {emsPrjCategory.Code}");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsPrjCategory
                    {
                        Code = emsPrjCategory.Code.ToString(),
                        Id = id,
                        Name = emsPrjCategory.Name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsPrjCategory
                    {
                        Code = emsPrjCategory.Code.ToString(),
                        Id = -1,
                        Name = emsPrjCategory.Name

                    });
                }
            }
        }

        private static void RefreashRegion()
        {
            var regions = Service.PullRegion();

            foreach (var emsRegion in regions)
            {
                var ret = Context.GetId<EmsRegion>($"Code = {emsRegion.Code}");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsRegion
                    {
                        Code = emsRegion.Code.ToString(),
                        Id = id,
                        Name = emsRegion.Name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsRegion
                    {
                        Code = emsRegion.Code.ToString(),
                        Id = -1,
                        Name = emsRegion.Name

                    });
                }
            }
        }

        private static void RefreashDistrict()
        {
            var districts = Service.PullDistrict(null);

            foreach (var emsDistrict in districts)
            {
                var ret = Context.GetId<EmsDistrict>($"Code = '{emsDistrict.Code}'");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsDistrict
                    {
                        Code = emsDistrict.Code,
                        Id = id,
                        Name = emsDistrict.Name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsDistrict
                    {
                        Code = emsDistrict.Code,
                        Id = -1,
                        Name = emsDistrict.Name

                    });
                }
            }
        }
    }
}
