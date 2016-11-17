using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetVideoPlayer.SearchEngine
{
    class Search_Engine_BT_TianTang
    {
        public List<Struct_BT_Search> Get_Search_BT_TianTang(string KeyWord, int PageIndex, int TimeOut = 10000)
        {
            try
            {
                List<Struct_BT_Search> List_BT_Search = new List<Struct_BT_Search>();
                WebClientFX web = new WebClientFX(TimeOut);
                web.Encoding = Encoding.UTF8;
                string HTML = web.DownloadString(string.Format(@"http://www.bttiantang.com/s.php?q={0}", KeyWord)).Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("&nbsp;", "").Replace("<em>", "").Replace("</em>", "").Replace("class=\"cpillyellow-pill\"", "");
                Clipboard.SetText(HTML);
                Regex regResult = new Regex("</span><ahref=\"/subject/(.+?)\"target=\"_blank\">");
                MatchCollection mcResult = regResult.Matches(HTML);
                if (mcResult.Count > 0)
                {
                    foreach (Match mc in mcResult)
                    {
                        MessageBox.Show(mc.Groups[1].Value);
                        continue;
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
