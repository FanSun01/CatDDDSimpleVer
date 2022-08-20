using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatSimpleVer.Common.Helper;

namespace CatSimpleVer.Common.DB
{
    public class BaseDBConfig
    {
        //字段 声明主从数据库配置列表,简单组合避免创建新类
        public static (List<MutiDbConfig> allDbs, List<MutiDbConfig> slaveDbs) MutiDbConnString => MutiInitConn();

        public static (List<MutiDbConfig>, List<MutiDbConfig>) MutiInitConn()
        {
            List<MutiDbConfig> allDbList = Appsettings.app<MutiDbConfig>("DBS").Where(e => e.Enabled == true).ToList();
            //先读取本地conn string，没有的话再读取appsettings
            foreach (var db in allDbList)
            {
                SpecialDbString(db);
            }
            List<MutiDbConfig> listdatabaseSimpleDB = new List<MutiDbConfig>();//单库
            List<MutiDbConfig> listdatabaseSlaveDB = new List<MutiDbConfig>();//从库

            //只开启了单库,并且不开启读写分离,取第一个链接字符串的情况
            if (!Appsettings.app("MutiDBEnabled").ObjToBool() && !Appsettings.app("CQRSEnabled").ObjToBool())
            {
                if (allDbList.Count == 1)
                {
                    return (allDbList, listdatabaseSlaveDB);
                }
                else
                {
                    var filteredDb = allDbList.FirstOrDefault(d => d.ConnId == Appsettings.app("MainDB").ObjToString());
                    if (filteredDb == null)
                    {
                        filteredDb = allDbList.FirstOrDefault();
                    }
                    listdatabaseSimpleDB.Add(filteredDb);
                    return (listdatabaseSimpleDB, listdatabaseSlaveDB);
                }
            }

            //只开启了单库，并且必须开启读写分离，主库不动， 从库放listdatabaseSlaveDB
            if (!Appsettings.app("MutiDBEnabled").ObjToBool() && Appsettings.app("CQRSEnabled").ObjToBool())
            {
                if (allDbList.Count > 1)
                {
                    listdatabaseSlaveDB = allDbList.Where(db => db.ConnId != Appsettings.app("MainDB").ObjToString()).ToList();
                }
            }

            return (listdatabaseSimpleDB, listdatabaseSlaveDB);
        }

        /// <summary>
        /// 定制Db字符串
        /// 目的是保证安全：优先从本地txt文件获取，若没有文件则从appsettings.json中获取
        /// </summary>
        /// <param name="mutiDBOperate"></param>
        /// <returns></returns>
        private static MutiDbConfig SpecialDbString(MutiDbConfig mutiDBOperate)
        {
            if (mutiDBOperate.DbType == DataBaseType.Sqlite)
            {
                mutiDBOperate.Connection = $"DataSource=" + Path.Combine(Environment.CurrentDirectory, mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.SqlServer)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_SqlserverConn.txt", mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.MySql)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_MySqlConn.txt", mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.Oracle)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_OracleConn.txt", mutiDBOperate.Connection);
            }

            return mutiDBOperate;
        }

        public static string DifDBConnOfSecurity(params string[] path)
        {
            foreach (var item in path)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (Exception)
                {
                }

            }
            //connstr 放在最后一个参数，没读取到就直接返回connstring
            return path[path.Length - 1];
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
}
