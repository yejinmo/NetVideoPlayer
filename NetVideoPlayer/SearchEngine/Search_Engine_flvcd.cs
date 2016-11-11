using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TinyJson;
using System.Web;
using System.IO;
using System.Windows.Forms;

namespace NetVideoPlayer.SearchEngine
{
    public class Search_Engine_flvcd
    {
        public List<string> GetSrcURL(string http_url, int TimeOut = 10000)
        {
            try
            {
                List<string> res = new List<string>();
                List<Struct_BT_Search> List_BT_Search = new List<Struct_BT_Search>();
                WebClientFX web = new WebClientFX(TimeOut);
                web.Encoding = Encoding.Default;
                web.Headers.Add("Referer", "http://www.flvcd.com/parse.php");
                web.Headers.Add("Accept", "*/*");
                web.Headers.Add("Accept-Language", "zh-cn");
                web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                web.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
                string HTML = web.DownloadString(@"http://www.flvcd.com/parse.php?kw=" + @http_url);
                Regex regResult = new Regex("<a href=\"(.+?)\" target=\"_blank\" class=\"link\" onclick=");
                MatchCollection mcResult = regResult.Matches(HTML);
                
                Regex regName = new Regex("<input type=\"hidden\" name=\"name\" value=\"(.+?)\">");
                MatchCollection mcName = regName.Matches(HTML);
                string Name = mcName.Count > 0 ? mcName[0].Groups[1].Value : "";

                if (mcResult.Count > 0)
                {
                    foreach (Match mc in mcResult)
                    {
                        res.Add(mc.Groups[1].Value);
                    }
                    return res;
                }
                else
                {
                    throw new Exception("No Data");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
