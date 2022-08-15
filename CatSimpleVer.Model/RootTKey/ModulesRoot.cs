using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CatSimpleVer.Model.RootTKey
{
    public class ModulesRoot<TKey> : RootEntityTKey<TKey> where TKey : IEquatable<TKey>
    {
        [SugarColumn(IsNullable = false)]
        public TKey ParentId { get; set; }
    }
}
