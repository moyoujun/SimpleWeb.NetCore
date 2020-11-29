using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Dto
{
    public class ZhuanlanDto
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public List<string> Tags { get; set; }
    }
}
