using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CatSimpleVer.IRepository.UOW
{
    public interface IUnitOfWork
    {
        SqlSugarScope GetDbClient();
        int TransCount { get; }
        void BeginTrans();
        void CommitTrans();
        void RollBackTrans();
    }
}
