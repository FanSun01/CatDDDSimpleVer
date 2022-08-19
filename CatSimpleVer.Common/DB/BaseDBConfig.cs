using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatSimpleVer.Common.DB
{
    public class BaseDBConfig
    {
        //字段 声明主从数据库配置列表,简单组合避免创建新类
        public static (List<MutiDbConfig> allDbs, List<MutiDbConfig> slaveDbs) MutiDbConnString;

        public (List<MutiDbConfig>, List<MutiDbConfig>) MutiInitConn()
        {
            List<MutiDbConfig> listdatabaseSimpleDB = new List<MutiDbConfig>();//单库
            List<MutiDbConfig> listdatabaseSlaveDB = new List<MutiDbConfig>();//从库





            return (listdatabaseSimpleDB, listdatabaseSlaveDB);
        }
    }


    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4,
        Dm = 5,
        Kdbndp = 6,
    }

    public class MutiDbConfig
    {
        /// <summary>
        /// 连接启用开关
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }
        /// <summary>
        /// 从库执行级别，越大越先执行
        /// </summary>
        public int HitRate { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }
    }
}
