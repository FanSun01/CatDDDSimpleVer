using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatSimpleVer.Common.Helper;
using CatSimpleVer.Common.DB;



namespace CatSimpleVer.Extensions.ServiceSetup
{
    public static class SqlsugarSetup
    {

        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            //config存在静态属性
            MainDb.CurrentDbConnId = Appsettings.app("MainDB");

            //SqlSugarScope用单例AddSingleton 单例; SqlSugarClient用 AddScoped 每次请求一个实例 ; 2选1只能用一种方式
            //SqlSugarScope是线程安全的，这边用单例注入
            services.AddSingleton<ISqlSugarClient>(o =>
            {
                var configList = new List<ConnectionConfig>();
                var configList_Slave = new List<SlaveConnectionConfig>();
                var memCache = o.GetRequiredService<MemoryCache>();

                BaseDBConfig.MutiDbConnString.slaveDbs.ForEach(sld => configList_Slave.Add(new SlaveConnectionConfig()
                {
                    HitRate = sld.HitRate,
                    ConnectionString = sld.Connection
                }));

                BaseDBConfig.MutiDbConnString.allDbs.ForEach(md =>
                {
                    configList.Add(new ConnectionConfig()
                    {
                        ConnectionString = md.Connection,
                        DbType = (DbType)md.DbType,
                        ConfigId = md.ConnId.ObjToString().ToLower(),
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.Attribute,
                        SlaveConnectionConfigs = configList_Slave,

                        MoreSettings = new ConnMoreSettings()
                        {
                            //增，删，改，的时候自动清除二级缓存
                            IsAutoRemoveDataCache = true
                        },

                        AopEvents = new AopEvents()
                        {
                            OnLogExecuting = (sql, param) =>
                            {
                                //ToDo:判断并写出sql到控制台或者Log
                                if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                                {
                                    if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "OutToLogFile", "Enabled" }).ObjToBool())
                                    {
                                        Parallel.For(0, 1, body =>
                                        {
                                            MiniProfiler.Current.CustomTiming("SQL：", GetParas(param) + "【SQL语句】：" + sql);

                                        });

                                    }
                                    if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "OutToConsole", "Enabled" }).ObjToBool())
                                    {

                                    }
                                }

                            }
                        },

                        ConfigureExternalServices = new ConfigureExternalServices()
                        {
                            //增加二级缓存
                            //ToDo: https://www.donet5.com/home/doc?masterId=1&typeId=1214
                            //cache实现接口，SqlSugarCache : ICacheService
                            DataInfoCacheService = new SqlSugarMemoryCacheService(memCache),

                            //https://www.donet5.com/home/doc?masterId=1&typeId=1182
                            //实体使用自定义特性
                            //当列是int类型的主键，把列设置为自增长标志
                            EntityService = (property, colunm) =>
                            {
                                if (colunm.IsPrimarykey && property.PropertyType == typeof(int))
                                {
                                    colunm.IsIdentity = true;
                                }
                            }
                        }

                    });
                });

                return new SqlSugarScope(configList);
            });

        }



        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }
            return key;
        }
    }
}
