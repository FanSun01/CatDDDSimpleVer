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


        }
    }
}
