using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;


namespace CatSimpleVer.Model
{
    public class RootEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsNullable = false)]
        public int Id { get; set; }
    }
}
