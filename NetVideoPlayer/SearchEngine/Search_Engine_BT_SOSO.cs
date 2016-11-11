using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetVideoPlayer.SearchEngine
{

    public class Search_Engine_BT_SOSO
    {

        public List<Struct_BT_Search> Get_Search_BT_SOSO(string KeyWord,int PageIndex,int TimeOut = 10000)
        {
            try
            {
                List<Struct_BT_Search> List_BT_Search = new List<Struct_BT_Search>();
                WebClientFX web = new WebClientFX(TimeOut);
                web.Encoding = Encoding.UTF8;
                string HTML = FromUnicodeString(web.DownloadString(string.Format(@"http://api.xhub.cn/api.php?op=search_list&key={0}&page={1}", KeyWord, PageIndex)).Replace("{", "").Replace("}", "").Replace("\"", ""));
                Regex regResult = new Regex("(.+?):title:(.+?),size:(.+?),day:(.+?),hits:(.+?)");
                MatchCollection mcResult = regResult.Matches(HTML);
                if (mcResult.Count > 0)
                {
                    foreach (Match mc in mcResult)
                    {
                        string ToUrl = mc.Groups[1].Value;
                        string Name = mc.Groups[2].Value;
                        string VideoLength = mc.Groups[3].Value;
                        string UpTime = mc.Groups[4].Value;
                        Struct_BT_Search item = new Struct_BT_Search()
                        {
                            ToUrl = ToUrl,
                            Name = Name.Replace("【失效链接】", "").Replace("【无效链接】", "").Replace("【处理中,请等待】", ""),
                            UpTime = UpTime,
                            VideoLength = VideoLength,
                            Source = "BT_SOSO"
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

        private string FromUnicodeString(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i].TrimStart('-'), 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException ex)
                {
                    return Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }

    }

}
