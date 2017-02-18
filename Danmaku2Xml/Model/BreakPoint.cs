using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku2Xml.Model
{
    public class BreakPoint
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime LastTime { get { return this.EndTime.Subtract(this.StartTime.TimeOfDay); } }
    }
}
