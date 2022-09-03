using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using CatSimpleVer.Common.DB;
using CatSimpleVer.Common.Helper;

namespace CatSimpleVer.Common.LogHelper
{
    public class LogLock
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LogLock));
        static string _contentPath = string.Empty;
        static ReaderWriterLockSlim _ReaderWriterLockSlim = new ReaderWriterLockSlim();

        public LogLock(string contentPath)
        {
            _contentPath = contentPath;
        }

        public static void OutSql2Log(string prefix, string[] dataParas, bool IsHeader = true, bool isWrt = false)
        {

            if (Appsettings.app(new string[] { "AppSettings", "LogToDb", "Enabled" }).ObjToBool())
            {
                OutSql2LogToDB(prefix, dataParas, IsHeader);
            }
            else
            {
                OutSql2LogFile(prefix, dataParas, IsHeader, isWrt);
            }
        }

        public static void OutSql2LogFile(string prefix, string[] dataParas, bool isHeader = false, bool isWrt = false)
        {
            try
            {
                _ReaderWriterLockSlim.EnterWriteLock();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //退出写入模式，释放资源占用
                //注意：一次请求对应一次释放
                //      若释放次数大于请求次数将会触发异常[写入锁定未经保持即被释放]
                //      若请求处理完成后未释放将会触发异常[此模式不下允许以递归方式获取写入锁定]
                _ReaderWriterLockSlim.ExitWriteLock();
            }
        }

        public static void OutSql2LogToDB(string prefix, string[] dataParas, bool IsHeader = true)
        {

            string logContent = String.Join("", dataParas);
            if (IsHeader)
            {
                logContent = (String.Join("", dataParas));
            }
            switch (prefix)
            {
                case "AOPLog":
                    _log.Info(logContent);
                    break;
                case "AOPLogEx":
                    _log.Error(logContent);
                    break;
                case "RequestIpInfoLog":
                    _log.Debug(logContent);
                    break;
                case "RecordAccessLogs":
                    _log.Debug(logContent);
                    break;
                case "SqlLog":
                    _log.Info(logContent);
                    break;
                case "RequestResponseLog":
                    _log.Debug(logContent);
                    break;
                default:
                    break;
            }

        }

    }


    public enum ReadType
    {
        /// <summary>
        /// 精确查找一个
        /// </summary>
        Accurate,
        /// <summary>
        /// 指定前缀，模糊查找全部
        /// </summary>
        Prefix,
        /// <summary>
        /// 指定前缀，最新一个文件
        /// </summary>
        PrefixLatest
    }
}
