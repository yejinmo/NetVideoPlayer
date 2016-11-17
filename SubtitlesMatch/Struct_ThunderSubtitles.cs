using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetVideoPlayer.SubtitlesMatch
{
    public class Struct_ThunderSubtitles
    {
        public List<Sublist> sublist = new List<Sublist>();
        public class Sublist
        {
            public string scid { get; set; }
            public string sname { get; set; }
            public string sext { get; set; }
            public string language { get; set; }
            public string simility { get; set; }
            public string surl { get; set; }
        }
    }
}
