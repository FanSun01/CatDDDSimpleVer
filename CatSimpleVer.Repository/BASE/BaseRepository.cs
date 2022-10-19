using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using CatSimpleVer.IRepository.UOW;
using Org.BouncyCastle.Crypto.Engines;

namespace CatSimpleVer.Repository.BASE
{
    public class BaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly SqlSugarScope _dbScope;
        private readonly IUnitOfWork _unitOfWork;

        private ISqlSugarClient _dbClient
        {
            get
            {
                //检查是否开启多库，判断是否要变更数据库

                return this._dbScope;
            }
        }

        public ISqlSugarClient Db { get { return this._dbClient; } }

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._dbScope = unitOfWork.GetDbClient();
        }

        public async Task<TEntity> QueryById(object objectId)
        {
            return await _dbClient.Queryable<TEntity>().In(objectId).SingleAsync();
        }

        public async Task<TEntity> QueryById(object objectId, bool useCache)
        {
            return await _dbClient.Queryable<TEntity>().WithCacheIF(useCache, 10).In(objectId).SingleAsync();
        }







    }
}
