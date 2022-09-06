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
        static int WritedCount = 0;
        static int FailedCount = 0;

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
                var folderPath = Path.Combine(_contentPath, "Log");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string logFilePath = FileHelper.GetAvailableFileWithPrefixOrderSize(folderPath, prefix);
                switch (prefix)
                {
                    case "AOPLog":
                        ApiLogAopInfo apiLogAopInfo = JsonConvert.DeserializeObject<ApiLogAopInfo>(dataParas[0]);
                        var dataIntercept = "" +
                            $"【操作时间】：{apiLogAopInfo.RequestTime}\r\n" +
                            $"【当前操作用户】：{apiLogAopInfo.OpUserName} \r\n" +
                            $"【当前执行方法】：{apiLogAopInfo.RequestMethodName} \r\n" +
                            $"【携带的参数有】： {apiLogAopInfo.RequestParamsName} \r\n" +
                            $"【携带的参数JSON】： {apiLogAopInfo.RequestParamsData} \r\n" +
                            $"【响应时间】：{apiLogAopInfo.ResponseIntervalTime}\r\n" +
                            $"【执行完成时间】：{apiLogAopInfo.ResponseTime}\r\n" +
                            $"【执行完成结果】：{apiLogAopInfo.ResponseJsonData}\r\n";
                        dataParas = new string[] { dataIntercept };
                        break;
                    //发生异常
                    case "AOPLogEx":
                        ApiLogAopExInfo apiLogAopExInfo = JsonConvert.DeserializeObject<ApiLogAopExInfo>(dataParas[0]);
                        var dataInterceptEx = "" +
                            $"【操作时间】：{apiLogAopExInfo.ApiLogAopInfo.RequestTime}\r\n" +
                            $"【当前操作用户】：{ apiLogAopExInfo.ApiLogAopInfo.OpUserName} \r\n" +
                            $"【当前执行方法】：{ apiLogAopExInfo.ApiLogAopInfo.RequestMethodName} \r\n" +
                            $"【携带的参数有】： {apiLogAopExInfo.ApiLogAopInfo.RequestParamsName} \r\n" +
                            $"【携带的参数JSON】： {apiLogAopExInfo.ApiLogAopInfo.RequestParamsData} \r\n" +
                            $"【响应时间】：{apiLogAopExInfo.ApiLogAopInfo.ResponseIntervalTime}\r\n" +
                            $"【执行完成时间】：{apiLogAopExInfo.ApiLogAopInfo.ResponseTime}\r\n" +
                            $"【执行完成结果】：{apiLogAopExInfo.ApiLogAopInfo.ResponseJsonData}\r\n" +
                            $"【执行完成异常信息】：方法中出现异常：{apiLogAopExInfo.ExMessage}\r\n" +
                            $"【执行完成结果】: {apiLogAopExInfo.InnerException}\r\n";
                        dataParas = new string[] { dataInterceptEx };
                        break;
                    default:
                        break;
                }
                string logContent = String.Join("\r\n", dataParas);
                if (isHeader)
                {
                    logContent = "--------------------------------\r\n" + DateTime.Now + "|\r\n" + String.Join("\r\n", dataParas) + "\r\n";
                }
                if (isWrt)
                {
                    File.WriteAllText(logFilePath, logContent);
                }
                else
                {
                    File.AppendAllText(logFilePath, logContent);
                }
                WritedCount++;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                FailedCount++;
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
