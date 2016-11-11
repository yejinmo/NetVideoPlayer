using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetVideoPlayer.SearchEngine
{
    public class Search_Engine_360
    {
        public List<Struct_BT_Search> Get_Search_360(string KeyWord, int PageIndex, int TimeOut = 10000)
        {
            try
            {
                List<Struct_BT_Search> List_BT_Search = new List<Struct_BT_Search>();
                WebClientFX web = new WebClientFX(TimeOut);
                web.Encoding = Encoding.UTF8;
                string HTML = string.Empty;
                HTML = web.DownloadString(string.Format(@"http://video.so.com/ugc?kw={0}&from=&du=100&fr=100&pb=100&st=101&pageno={1}", KeyWord, PageIndex)).Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("&nbsp;", "").Replace("<em>", "").Replace("</em>", "").Replace("class=\"cpillyellow-pill\"", "");
                int _i = 0;
                while (HTML.Length <= 100 && _i <= 10)
                {
                    Thread.Sleep(1000);
                    HTML = web.DownloadString(string.Format(@"http://video.so.com/ugc?kw={0}&from=&du=100&fr=100&pb=100&st=101&pageno={1}", KeyWord, PageIndex)).Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("&nbsp;", "").Replace("<em>", "").Replace("</em>", "").Replace("class=\"cpillyellow-pill\"", "");
                    _i++;
                }
                Regex regResult = new Regex("<spanclass=\"w-figure-lefthint\">(.+?)</span><spanclass=\"w-figure-righthint\">(.+?)</span></span></a><h4><aclass=\'w-figure-title\'href=\'(.+?)\'title=\"(.+?)\">");
                MatchCollection mcResult = regResult.Matches(HTML);
                if (mcResult.Count > 0)
                {
                    foreach (Match mc in mcResult)
                    {
                        string ToUrl = mc.Groups[3].Value;
                        string Name = mc.Groups[4].Value;
                        string UpTime = mc.Groups[2].Value;
                        string VideoLength = mc.Groups[1].Value;
                        Struct_BT_Search item = new Struct_BT_Search()
                        {
                            ToUrl = ToUrl,
                            Name = Name,
                            UpTime = UpTime,
                            VideoLength = VideoLength,
                            Source = "vedio.so.com"
                        };
                        List_BT_Search.Add(item);
                    }
                    return List_BT_Search;
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
