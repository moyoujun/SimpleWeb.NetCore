using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Models
{
    public class ZhuanlanDO
    {
        public string ID { get; set; }

        public string AuthorID { get; set; }

        public List<ZhuanlanTagDO> Tags { get; set; }
    }
}
