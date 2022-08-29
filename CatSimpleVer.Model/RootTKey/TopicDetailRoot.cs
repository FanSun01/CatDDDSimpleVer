using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatSimpleVer.Model
{
    public class TopicDetailRoot<TKey> : RootEntityTKey<TKey> where TKey : IEquatable<TKey>
    {
        public TKey TopicId { get; set; }
    }
}
