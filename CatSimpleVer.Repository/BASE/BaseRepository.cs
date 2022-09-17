using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CatSimpleVer.Repository.BASE
{
    public class BaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly SqlSugarScope _db;

        



    }
}
