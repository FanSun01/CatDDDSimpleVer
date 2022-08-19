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



                return new SqlSugarScope(configList);
            });

        }
    }
}
