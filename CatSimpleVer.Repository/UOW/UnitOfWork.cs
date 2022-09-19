using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using Microsoft.Extensions.Logging;
using CatSimpleVer.IRepository.UOW;

namespace CatSimpleVer.Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ILogger<UnitOfWork> _logger;
        private int _transCount { get; set; }
        public int TransCount => _transCount;

        public UnitOfWork(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWork> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            _transCount = 0;
        }

        public SqlSugarScope GetDbClient()
        {
            return _sqlSugarClient as SqlSugarScope;
        }

        //开始事务
        public void BeginTrans()
        {
            lock (this)
            {
                _transCount++;
                GetDbClient().BeginTran();
            }
        }

        public void CommitTrans()
        {
            lock (this)
            {
                _transCount--;
                if (_transCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        GetDbClient().RollbackTran();
                    }
                }
            }
        }

        public void RollBackTrans()
        {
            lock (this)
            {
                _transCount--;
                GetDbClient().RollbackTran();
            }
        }




    }
}
