using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku2Xml.Model
{
    public class Danmaku
    {
        public DateTime Time { get; set; }
        public String User { get; set; }
        public String Content { get; set; }
    }
}
