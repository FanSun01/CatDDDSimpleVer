using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CatSimpleVer.Model
{
    public class TopicDetail : TopicDetailRoot<int>
    {

        public TopicDetail()
        {
            this.tdUpdatetime = DateTime.Now;
        }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tdLogo { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tdName { get; set; }

        [SugarColumn(Length = 2000, IsNullable = true)]
        public string tdContent { get; set; }

        [SugarColumn(Length = 2000, IsNullable = true)]
        public string tdDetail { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tdSectendDetail { get; set; }

        public bool tdIsDelete { get; set; } = false;
        public int tdRead { get; set; }
        public int tdCommend { get; set; }
        public int tdGood { get; set; }
        public DateTime tdCreatetime { get; set; }
        public DateTime tdUpdatetime { get; set; }
        public int tdTop { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tdAuthor { get; set; }

        [SugarColumn(IsIgnore = true)]
        public virtual Topic topic { get; set; }




    }

}
