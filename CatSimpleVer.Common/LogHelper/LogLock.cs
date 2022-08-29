﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using System.IO;
using System.Threading;


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

        public static void OutSql2LogFile(string prefix, string[] dataParas, bool isHeader = false, bool isWrt = false)
        {
            try
            {




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

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
