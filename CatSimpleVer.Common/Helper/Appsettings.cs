using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;


namespace CatSimpleVer.Common.Helper
{
    public class Appsettings
    {
        //声明为静态属性，为了可以静态访问config file, 而不用实例化Appsettings
        public static IConfiguration Configuration { get; set; }

        //默认读 /bin 下的 appsettings.json
        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //也可以不读 /bin下的, 那需要提供配置文件路径
        public Appsettings(string configFileDir)
        {
            string fileName = "appsettings.json";
            Configuration = new ConfigurationBuilder().SetBasePath(configFileDir)
                .Add(new JsonConfigurationSource() { Path = fileName, Optional = false, ReloadOnChange = true })
                .Build();
        }

        public static string app(params string[] sections)
        {
            try
            {
                //找多层级，例如Setting: Db: ConnId...
                return Configuration[string.Join(":", sections)];
            }
            catch (Exception) { }
            return "";
        }

        public static List<T> app<T>(params T[] sections)
        {
            var list = new List<T>();
            //没绑定上就返回空list实例，不用catch
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }

        //直接提供配置好的 settingName1:settingName2
        public static string GetValue(string configPath)
        {
            try
            {
                return Configuration[configPath];
            }
            catch (Exception) { }
            return "";
        }
    }
}
