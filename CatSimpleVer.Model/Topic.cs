using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;
using System.Threading.Tasks;


namespace CatSimpleVer.Model
{
    public class Topic : RootEntityTKey<int>
    {
        public Topic()
        {
            this.tUpdatetime = DateTime.Now;
            this.topicDetail = new List<TopicDetail>();
        }
        [SugarColumn(Length = 200, IsNullable = true)]
        public string tLogo { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tName { get; set; }

        [SugarColumn(Length = 400, IsNullable = true)]
        public string tDetail { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tAuthor { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string tSectendDetail { get; set; }

        public bool tIsDelete { get; set; }
        public int tRead { get; set; }
        public int tCommend { get; set; }
        public int tGood { get; set; }
        public DateTime tCreatetime { get; set; }
        public DateTime tUpdatetime { get; set; }


        [SugarColumn(IsIgnore = true)]
        public virtual ICollection<TopicDetail> topicDetail { get; set; }

    }
}
