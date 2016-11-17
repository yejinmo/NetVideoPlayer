using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetVideoPlayer.SearchEngine
{
    public class Search_Engine_CI_LI_BA
    {

        public List<Struct_BT_Search> Get_Search_CI_LI_BA(string KeyWord, int PageIndex, int TimeOut = 10000)
        {
            try
            {
                List<Struct_BT_Search> List_BT_Search = new List<Struct_BT_Search>();
                WebClientFX web = new WebClientFX(TimeOut);
                web.Encoding = Encoding.UTF8;
                string HTML = web.DownloadString(string.Format(@"http://www.cili8.org/s/{0}_rel_{1}.html", KeyWord, PageIndex)).Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("&nbsp;", "").Replace("<em>", "").Replace("</em>", "").Replace("class=\"cpillyellow-pill\"", "");
                Regex regResult = new Regex("<ahref=\"http://www.cili8.net/detail/(.+?).html\"target=\"_blank\">(.+?)</a></h3></div><divclass=\"item-list\">(.+?)</div><divclass=\"item-bar\"><spanclass=\"cpillfileType1\">视频</span><span>创建时间：<b>(.+?)</b></span><span>文件大小：<b>(.+?)</b></span>");
                MatchCollection mcResult = regResult.Matches(HTML);
                if (mcResult.Count > 0)
                {
                    foreach (Match mc in mcResult)
                    {
                        string ToUrl = mc.Groups[1].Value;
                        string Name = mc.Groups[2].Value;
                        string UpTime = mc.Groups[4].Value;
                        string VideoLength = mc.Groups[5].Value;
                        Struct_BT_Search item = new Struct_BT_Search()
                        {
                            ToUrl = ToUrl,
                            Name = Name.Replace("【失效链接】", "").Replace("【无效链接】", "").Replace("【处理中,请等待】", ""),
                            UpTime = UpTime,
                            VideoLength = VideoLength,
                            Source = "CI_LI_BA"
                        };
                        List_BT_Search.Add(item);
                    }
                    //检测所有磁链连通性
                    //TestingHash();
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
