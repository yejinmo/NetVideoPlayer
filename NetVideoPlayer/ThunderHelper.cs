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

namespace NetVideoPlayer
{
    public class ThunderHelper
    {

        #region 迅雷JSON返回值结构

        public class ThunderJSONStuct
        {
            public Resp resp { get; set; }
        }

        public class Resp
        {
            public int userid { get; set; }
            public int ret { get; set; }
            public List<Subfile_List> subfile_list { get; set; }
            public string main_task_url_hash { get; set; }
            public string info_hash { get; set; }
            public int record_num { get; set; }
        }

        public class Subfile_List
        {
            public string gcid { get; set; }
            public string url_hash { get; set; }
            private string NAME;
            public string name
            {
                get
                {
                    return NAME;
                }
                set
                {
                    NAME = HttpUtility.UrlDecode(value).Replace("【失效链接】", "").Replace("【无效链接】", "").Replace("【处理中,请等待】", "");
                }
            }
            public int index { get; set; }
            public string cid { get; set; }
            public int file_size { get; set; }
            public int duration { get; set; }
        }

        #endregion

        /// <summary>
        /// 获取种子Hash值
        /// 若失败返回空字符串
        /// </summary>
        /// <param name="url">匹配到的种子Hash值</param>
        /// <returns></returns>
        public string MatchHash(string url)
        {
            Regex regex = new Regex("[0-9a-z]{40}");
            Match match = regex.Match(url.ToLower());
            if (match.Length > 0)
                return match.Groups[0].Value;
            else
                return "";
        }

        /// <summary>
        /// 获取迅雷JSON
        /// 若失败返回空字符串
        /// </summary>
        /// <param name="url">迅雷Hash值</param>
        /// <returns></returns>
        public string GetThunderJSON(string hash)
        {
            WebClient wc = new WebClient();
            string res = wc.DownloadString(string.Format("http://i.vod.xunlei.com/req_subBT/info_hash/{0}/req_num/1000/req_offset/0", hash));
            if (res == "" || res.Contains("\"record_num\": 0"))
                return "";
            else
                return res;
        }

        /// <summary>
        /// 获取一个ThunderJSONStuct类型的对象
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ThunderJSONStuct GetList(string url)
        {
            ThunderJSONStuct res = new ThunderJSONStuct();
            if (string.IsNullOrEmpty(url))
                return res;
            string hash = MatchHash(url);
            if (hash == "")
                return res;
            string json = GetThunderJSON(hash);
            if (json == "")
                return res;
            res = JSONParser.FromJson<ThunderJSONStuct>(json);
            return res;
        }

        /// <summary>
        /// QQ分享结构
        /// </summary>
        public class QQFenXiang
        {
            public List<QQDatum> data { get; set; }
            public string msg { get; set; }
            public int ret { get; set; }
            public string taskname { get; set; }
            public string torrent_hash { get; set; }
            public int uin { get; set; }
        }

        public class QQDatum
        {
            public string file_hash { get; set; }
            public int file_index { get; set; }
            public string file_name { get; set; }
            public string file_size { get; set; }
            public int isExist { get; set; }
        }

        public class XFPlay
        {
            public int ret { get; set; }
            public string msg { get; set; }
            public XFData data { get; set; }
        }

        public class XFData
        {
            public string type { get; set; }
            public string com_url { get; set; }
            public string com_cookie { get; set; }
            public string xf_url { get; set; }
        }

        /// <summary>
        /// 用于HTTP播放的返回值结构
        /// </summary>
        public struct URLData
        {
            public string url;
            public string cookie;
        }

        public URLData GetHTTPURL_V1(Subfile_List item, string info_hash)
        {
            URLData res = new URLData();
            WebClient web = new WebClient();

            #region 旧版接口
            /*
            #region QQ接口取File_hash
            string postString = "torrent_para={\"uin\":\"985189148\",\"hash\":\"" + info_hash + "\",\"taskname\":\"M\",\"data\":[{\"index\":\"" + item.index + "\",\"filesize\":\"1\",\"filename\":\"M.mkv\"}]}";
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            string url = "http://fenxiang.qq.com/upload/index.php/upload_c/checkExist";//地址
            web.Headers.Add("Referer", "http://fenxiang.qq.com/upload/index.php/upload_c/checkExist");
            web.Headers.Add("Accept", "*//*");//此行删除一个/
            web.Headers.Add("Accept-Language", "zh-cn");
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
            byte[] responseData = web.UploadData(url, "POST", postData);//得到返回字符流
            for (int i = 0; i < 10; ++i)
            {
                if (responseData.Length <= 0)
                    responseData = web.UploadData(url, "POST", postData);//得到返回字符流
                else
                    break;
            }
            string srcString = Encoding.UTF8.GetString(responseData);//解码
            if (string.IsNullOrEmpty(srcString) || srcString.Contains("\"ret\" : 2"))
                return res;
            QQFenXiang qqfx = JSONParser.FromJson<QQFenXiang>(srcString);
            if (qqfx.data[0].file_hash.Equals("0000000000000000000000000000000000000000"))
                return res;
            #endregion

            string File_hash = qqfx.data[0].file_hash;

            string File_hash = "e88137ca28580665330a2287b3c4f3288f946ffa";
            string postString;
            byte[] postData;
            string url;
            byte[] responseData;
            string srcString;
             */
            #endregion

            #region 获取File_hash

            string postString = "m=jx?hash=" + info_hash + "&index=" + item.index;
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            //string url = "http://1.jxbt.applinzi.com/jx.php?hash=" + info_hash + "&index=" + item.index;//地址
            string url = "http://mt520.xyz:8080/CPServer/cloudplayer/getlistopen?hash=" + info_hash + "&index=" + item.index;//地址
            web.Headers.Add("Accept", "*/*");
            web.Headers.Add("Referer", url);
            web.Headers.Add("Accept-Language", "zh-cn");
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)");
            web.Headers.Add("Host", "1.jxbt.applinzi.com");
            web.Headers.Add("Cache-Control", "no-cache");
            string srcString = web.DownloadString(url);
            for (int i = 0; i < 5; ++i)
            {
                if (srcString.Length <= 0)
                    srcString = web.DownloadString(url);
                else
                    break;
            }
            if (string.IsNullOrEmpty(srcString) || srcString.Contains("\"ret\" : 2"))
                return res;
            QQFenXiang qqfx = JSONParser.FromJson<QQFenXiang>(srcString);
            if (qqfx.data[0].file_hash.Equals("0000000000000000000000000000000000000000"))
                return res;

            string File_hash = qqfx.data[0].file_hash;

            #endregion

            #region 第三步：连线QQ取播放地址
            byte[] responseData;
            postString = string.Format("hash={0}&filename=movie", File_hash);
            postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            url = "http://lixian.qq.com/handler/lixian/get_http_url.php";//地址
            web.Headers.Clear();
            web.Headers.Add("Referer", "http://lixian.qq.com/handler/lixian/get_http_url.php");
            web.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            web.Headers.Add("Accept-Language", "zh-cn");
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
            responseData = web.UploadData(url, "POST", postData);//得到返回字符流
            //解码
            byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, responseData, 0, responseData.Length);
            srcString = Encoding.Default.GetString(buffer2, 0, buffer2.Length).TrimStart('?');
            if (string.IsNullOrEmpty(srcString) || srcString.Contains("\"ret\":-1"))
                return res;
            XFPlay xf = JSONParser.FromJson<XFPlay>(srcString);
            #endregion

            string com_url = xf.data.com_url;

            #region 分析真实地址调用播放
            string comkey = com_url.Substring(com_url.IndexOf("ftn_handler/"), com_url.IndexOf("/movie") - com_url.IndexOf("ftn_handler/")).Replace("ftn_handler/", "");
            string urlstart = "cd";
            try
            {
                urlstart = com_url.Substring(com_url.IndexOf("http://"), com_url.IndexOf(".qq.com") - com_url.IndexOf("http://")).Replace("http://", "").Replace("xflx", "").Replace("store", "").Replace("ctfs", "").Replace("btfs", "").Replace("ftn", "").Replace(".", "").Replace("src", "xa");
            }
            catch { }
            string com_cookie = xf.data.com_cookie;
            com_url = GetPlayUrl(comkey, urlstart, com_cookie);
            if (string.IsNullOrEmpty(com_url))
                return res;
            string VideoName = HttpUtility.UrlDecode(item.name).Replace("【失效链接】", "").Replace("【无效链接】", "").Replace("【处理中,请等待】", "");
            res.url = com_url;
            res.cookie = com_cookie;
            return res;
            #endregion
        }

        public URLData GetHTTPURL(Subfile_List item, string info_hash)
        {
            URLData res = new URLData();
            WebClient web = new WebClient();

            #region 获取File_hash

            string postString = "m=jx?hash=" + info_hash + "&index=" + item.index;
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            //string url = "http://1.jxbt.applinzi.com/jxcode.php?hash=" + info_hash + "&index=" + item.index;//地址
            string url = "http://mt520.xyz:8080/CPServer/cloudplayer/geturlopen?hash=" + info_hash + "&index=" + item.index;//地址
            web.Headers.Add("Accept", "*/*");
            web.Headers.Add("Referer", url);
            web.Headers.Add("Accept-Language", "zh-cn");
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)");
            web.Headers.Add("Host", "mt520.xyz:8080");
            web.Headers.Add("Cache-Control", "no-cache");
            string srcString = web.DownloadString(url);
            for (int i = 0; i < 2; ++i)
            {
                if (srcString.Length <= 0)
                    srcString = web.DownloadString(url);
                else
                    break;
            }

            Regex reg = new Regex("Success#(.+?)#(.+?)");
            MatchCollection mc = reg.Matches(srcString);
            if(mc.Count == 0)
                return res;

            res.url = mc[0].Groups[1].Value;
            res.cookie = mc[0].Groups[2].Value;
            return res;

            if (string.IsNullOrEmpty(srcString) || srcString.Contains("\"ret\" : 2"))
                return res;
            QQFenXiang qqfx = JSONParser.FromJson<QQFenXiang>(srcString);
            if (qqfx.data[0].file_hash.Equals("0000000000000000000000000000000000000000"))
                return res;

            string File_hash = qqfx.data[0].file_hash;

            #endregion

            #region 第三步：连线QQ取播放地址
            byte[] responseData;
            postString = string.Format("hash={0}&filename=movie", File_hash);
            postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            url = "http://lixian.qq.com/handler/lixian/get_http_url.php";//地址
            web.Headers.Clear();
            web.Headers.Add("Referer", "http://lixian.qq.com/handler/lixian/get_http_url.php");
            web.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            web.Headers.Add("Accept-Language", "zh-cn");
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
            responseData = web.UploadData(url, "POST", postData);//得到返回字符流
            //解码
            byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, responseData, 0, responseData.Length);
            srcString = Encoding.Default.GetString(buffer2, 0, buffer2.Length).TrimStart('?');
            if (string.IsNullOrEmpty(srcString) || srcString.Contains("\"ret\":-1"))
                return res;
            XFPlay xf = JSONParser.FromJson<XFPlay>(srcString);
            #endregion

            string com_url = xf.data.com_url;

            #region 分析真实地址调用播放
            string comkey = com_url.Substring(com_url.IndexOf("ftn_handler/"), com_url.IndexOf("/movie") - com_url.IndexOf("ftn_handler/")).Replace("ftn_handler/", "");
            string urlstart = "cd";
            try
            {
                urlstart = com_url.Substring(com_url.IndexOf("http://"), com_url.IndexOf(".qq.com") - com_url.IndexOf("http://")).Replace("http://", "").Replace("xflx", "").Replace("store", "").Replace("ctfs", "").Replace("btfs", "").Replace("ftn", "").Replace(".", "").Replace("src", "xa");
            }
            catch { }
            string com_cookie = xf.data.com_cookie;
            com_url = GetPlayUrl(comkey, urlstart, com_cookie);
            if (string.IsNullOrEmpty(com_url))
                return res;
            string VideoName = HttpUtility.UrlDecode(item.name).Replace("【失效链接】", "").Replace("【无效链接】", "").Replace("【处理中,请等待】", "");
            res.url = com_url;
            res.cookie = com_cookie;
            return res;
            #endregion
        }

        public string GetPlayUrl(string comkey, string urlstart, string cookie)
        {
            WebClient web = new WebClient();
            List<string> cs = new string[] { string.Format("xf{0}.ctfs.ftn", urlstart), string.Format("{0}.ctfs.ftn", urlstart), string.Format("{0}.ftn", urlstart), string.Format("{0}.btfs.ftn", urlstart), "xfcd.ctfs.ftn", "sh.ctfs.ftn", "xfsh.ctfs.ftn", "xfxa.ctfs.ftn", "xa.ctfs.ftn", "hz.ftn" }.Distinct().ToList();
            string url = string.Empty;
            bool ok = false;
            int i = 1;
            foreach (var item in cs)
            {
                string newurl = string.Format("http://{0}.qq.com/ftn_handler/{1}?compressed=0&dtype=1&fname=m.mkv", item, comkey);
                ok = GetBoolUrl(cookie, newurl);
                if (ok)
                {
                    url = newurl;
                    break;
                }
                i++;
            }
            return url;
        }

        private bool GetBoolUrl(string cookie, string url)
        {
            bool flag = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/4.0(compatible; MSIE 6.0; Windows NT 5.0; MyIE2; .NET CLR 1.1.4322)";
                request.Referer = url;
                request.Proxy = null;
                request.Headers.Add("Cookie", "FTN5K=" + cookie);
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                flag = (myResponse.StatusCode == HttpStatusCode.OK);
                return flag;
            }
            catch (Exception)
            {
                flag = false;
                return flag;
            }
        }

    }
}
