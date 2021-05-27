using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratory9.Entities
{
    public class News
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public virtual UserInfo Author { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
