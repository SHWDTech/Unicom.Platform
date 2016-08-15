using Unicom.Platform;
using Unicom.Platform.Model;
using Unicom.Platform.SQLite;

namespace Unicom.Register.Common
{
    public class Operation
    {
        private static readonly Service Service = new Service();

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
                var ret = Context.GetId<EmsPrjType>($"Code = {emsPrjType.code}");
                if (ret != null)
                {
                    var id = (long) ret;
                    Context.AddOrUpdate(new EmsPrjType
                    {
                        Code = emsPrjType.code.ToString(),
                        Id = id,
                        Name = emsPrjType.name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsPrjType
                    {
                        Code = emsPrjType.code.ToString(),
                        Id = -1,
                        Name = emsPrjType.name

                    });
                }
            }
        }

        private static void RefreashPrjPeriod()
        {
            var periods = Service.PullProjectPeriod();

            foreach (var emsPrjPeriod in periods)
            {
                var ret = Context.GetId<EmsPrjPeriod>($"Code = {emsPrjPeriod.code}");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsPrjPeriod
                    {
                        Code = emsPrjPeriod.code.ToString(),
                        Id = id,
                        Name = emsPrjPeriod.name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsPrjPeriod
                    {
                        Code = emsPrjPeriod.code.ToString(),
                        Id = -1,
                        Name = emsPrjPeriod.name

                    });
                }
            }
        }

        private static void RefreashPrjCategory()
        {
            var categories = Service.PullProjectCategory();

            foreach (var emsPrjCategory in categories)
            {
                var ret = Context.GetId<EmsPrjCategory>($"Code = {emsPrjCategory.code}");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsPrjCategory
                    {
                        Code = emsPrjCategory.code.ToString(),
                        Id = id,
                        Name = emsPrjCategory.name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsPrjCategory
                    {
                        Code = emsPrjCategory.code.ToString(),
                        Id = -1,
                        Name = emsPrjCategory.name

                    });
                }
            }
        }

        private static void RefreashRegion()
        {
            var regions = Service.PullRegion();

            foreach (var emsRegion in regions)
            {
                var ret = Context.GetId<EmsRegion>($"Code = {emsRegion.code}");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsRegion
                    {
                        Code = emsRegion.code.ToString(),
                        Id = id,
                        Name = emsRegion.name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsRegion
                    {
                        Code = emsRegion.code.ToString(),
                        Id = -1,
                        Name = emsRegion.name

                    });
                }
            }
        }

        private static void RefreashDistrict()
        {
            var districts = Service.PullDistrict(null);

            foreach (var emsDistrict in districts)
            {
                var ret = Context.GetId<EmsDistrict>($"Code = '{emsDistrict.code}'");
                if (ret != null)
                {
                    var id = (long)ret;
                    Context.AddOrUpdate(new EmsDistrict
                    {
                        Code = emsDistrict.code,
                        Id = id,
                        Name = emsDistrict.name
                    });
                }
                else
                {
                    Context.AddOrUpdate(new EmsDistrict
                    {
                        Code = emsDistrict.code,
                        Id = -1,
                        Name = emsDistrict.name

                    });
                }
            }
        }
    }
}
