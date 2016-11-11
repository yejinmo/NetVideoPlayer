using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NetVideoPlayer.SubtitlesMatch
{
    public class ThunderSubtitles
    {
        public string GetSubtitles(string File_Name, string File_Length)
        {
            string url = string.Format(@"http://subtitle.kankan.xunlei.com:8000/search.json/mname={0}&videolength={1}", File_Name, File_Length);
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("UTF-8"));
            string res = sr.ReadToEnd();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return res;
        }

        public Struct_ThunderSubtitles GetSubtitlesStruct(string File_Name, string File_Length)
        {
            string json = GetSubtitles(File_Name, File_Length);
            Struct_ThunderSubtitles res = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Struct_ThunderSubtitles>(json);
            return res;
        }

    }
}
