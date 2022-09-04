using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CatSimpleVer.Common.Helper
{
    public class FileHelper : IDisposable
    {
        #region 初始化

        private bool _alreadyDispose = false;
        public FileHelper()
        {
        }

        ~FileHelper()
        {
            Dispose();
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose)
            {
                return;
            }
            _alreadyDispose = true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 根据文件大小获取指定前缀的可用文件名
        /// <summary>
        /// 根据文件大小获取指定前缀的可用文件名
        /// </summary>
        /// <param name="folderPath">文件夹</param>
        /// <param name="prefix">文件前缀</param>
        /// <param name="size">文件大小(1m)</param>
        /// <param name="ext">文件后缀(.log)</param>
        /// <returns>可用文件名</returns>
        public static string GetAvailableFileWithPrefixOrderSize(string folderPath, string prefix, int size = 1 * 1024 * 1024, string ext = ".log")
        {
            //根据 prefix， 找到了就返回旧的log文件路径
            var allFiles = new DirectoryInfo(folderPath);
            var selectedFiles = allFiles.GetFiles().Where(f => f.Name.Contains(prefix) && f.Extension.ToLower() == ext && f.Length <= size).OrderByDescending(o => o.Name).ToList();
            if (selectedFiles.Count > 0)
            {
                return selectedFiles.FirstOrDefault().FullName;
            }
            //没找到就返回新创建的路径
            return Path.Combine(folderPath, $@"{prefix}_{DateTime.Now.DateToTimeStamp()}.log");
        }
        #endregion
    }
}
