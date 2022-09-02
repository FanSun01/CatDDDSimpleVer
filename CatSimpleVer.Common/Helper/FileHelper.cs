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




    }
}
