using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using TinyJson;

namespace NetVideoPlayer
{
    public class Subtitles
    {

        #region 字幕结构
        /*
        /// <summary>
        /// 字幕类
        /// </summary>
        public class SubtitlesClass
        {
            public int Count = -1;      //字幕数量
            public List<SubtitlesInfo> SubtitlesContent;    //字幕详细信息List
            /// <summary>
            /// 是否成功获取字幕
            /// </summary>
            /// <returns></returns>
            public bool IsNull()        
            {
                if (Count <= 0)
                    return true;
                else
                    return false;
            }
            /// <summary>
            /// 获取指定索引的字幕
            /// </summary>
            /// <param name="index">索引号</param>
            /// <returns>指定索引的字幕</returns>
            public SubtitlesInfo GetSubtitlesContent(int index)
            {
                if (IsNull() || index > Count)
                    return null;
                return SubtitlesContent[index];
            }
            /// <summary>
            /// 下载字幕
            /// </summary>
            /// <param name="index">索引号</param>
            /// <param name="path">字幕存储路径</param>
            /// <param name="filename">字幕文件名，若为空则为[FileName].[Subtype]</param>
            /// <returns>是否成功下载</returns>
            public bool DownloadSubtitles(int index, string path, string filename = "")
            {
                if (IsNull() || index > Count)
                    return false;
                return SubtitlesContent[index].Download(path, filename);
            }
        }

        /// <summary>
        /// 字幕详细信息类
        /// </summary>
        public class SubtitlesInfo
        {
            public string FileName = "";    //字幕文件名
            public int Size = -1;           //字幕大小
            public string URL = "";         //字幕下载地址
            public int ViewCount = -1;      //浏览次数
            public int DownCount = -1;      //下载次数
            public string Title = "";       //字幕标题
            public string Source = "";      //字幕来源
            public double VoteScore = -1;   //用户评分
            public string Subtype = "";     //字幕后缀名
            public string LocalURL = "";    //存储地址
            public string Language = "";    //字幕语言
            /// <summary>
            /// 获取字幕信息
            /// </summary>
            /// <returns>字幕信息</returns>
            new public string ToString()
            {
                string res = "";
                res += "文件名： " + (FileName == "" ? "未知" : FileName);
                res += '\n' + "文件大小： " + (Size == -1 ? "未知" : Size.ToString()) + " kb";
                res += '\n' + "下载地址： " + (URL == "" ? "未知" : URL);
                res += '\n' + "浏览次数： " + (ViewCount == -1 ? "未知" : ViewCount.ToString()) + " 次";
                res += '\n' + "下载次数： " + (DownCount == -1 ? "未知" : DownCount.ToString()) + " 次";
                res += '\n' + "字幕标题： " + (Title == "" ? "未知" : Title);
                res += '\n' + "字幕来源： " + (Source == "" ? "未知" : Source);
                res += '\n' + "用户评分： " + (VoteScore == -1 ? "未知" : VoteScore.ToString()) + " 分";
                res += '\n' + "文件格式： " + (Subtype == "" ? "未知" : Subtype);
                res += '\n' + "存储地址： " + (LocalURL == "" ? "未知" : LocalURL);
                res += '\n' + "字幕语言： " + (Language == "" ? "未知" : Language);
                return res;
            }
            /// <summary>
            /// 下载字幕文件
            /// </summary>
            /// <param name="path">字幕存储路径</param>
            /// <param name="filename">字幕文件名，若为空则为[FileName].[Subtype]</param>
            /// <returns>是否成功下载</returns>
            public bool Download(string path, string filename = "")
            {
                try
                {
                    if (path.EndsWith("\\"))
                        path += "\\";
                    if (!string.IsNullOrEmpty(filename) && !filename.EndsWith(Subtype))
                        filename += Subtype;
                    else
                        filename = FileName + "." + Subtype;
                    LocalURL = path + filename;
                    HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();
                    Stream stream = new FileStream(LocalURL, FileMode.Create);
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    stream.Close();
                    responseStream.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        */
        #endregion

        #region 字幕搜索引擎
        /*
        private int EngineIndex = 0;

        public bool SetEngine(int index)
        {
            EngineIndex = index;
            return true;
        }
        */
        #endregion

        /*
        /// <summary>
        /// 获取一个SubtitlesClass的字幕类型
        /// </summary>
        /// <param name="KeyWord">关键词</param>
        /// <param name="FileHash">文件hash值</param>
        /// <returns></returns>
        public SubtitlesClass GetSubtitles(string KeyWord, string FileHash = "")
        {
            SubtitlesClass res = new SubtitlesClass();
            switch (EngineIndex)
            {
                case 0:
                    {
                        //MessageBox.Show(GetShooterAPI(KeyWord));
                        return res;
                        break;
                    }
                case 1:
                    {
                        return res;
                        break;
                    }
                default:
                    {
                        return res;
                    }
            }
        }
        */

        #region 射手网（伪）API

        #region 射手网（伪）API JSON 结构

        public struct Struct_ShooterAPI_root
        {
            public int status;
            public Struct_ShooterAPI_sub sub;
        }

        public struct Struct_ShooterAPI_sub
        {
            public string result;
            public string action;
            public string keyword;
            public List<Struct_ShooterAPI_subs> subs;
        }

        public struct Struct_ShooterAPI_subs
        {
            public string native_name;
            public string videoname;
            public int revision;
            public string release_site;
            public string upload_time;
            public int vote_score;
            public int id;
            public string subtype;
            public Struct_ShooterAPI_lang lang;
        }

        public struct Struct_ShooterAPI_lang
        {
            public string desc;
            public Struct_ShooterAPI_langlist langlist;
        }

        public struct Struct_ShooterAPI_langlist
        {
            public List<string> langlist;
        }

        #endregion

        #region 射手网（伪）API JSON 结构 （详细信息）

        public struct Struct_ShooterAPI_info_root
        {
            public int status;
            public Struct_ShooterAPI_info_sub sub;
        }

        public struct Struct_ShooterAPI_info_sub
        {
            public string result;
            public string action;
            public List<Struct_ShooterAPI_info_subs> subs;
        }

        public struct Struct_ShooterAPI_info_subs
        {
            public string filename;
            public string native_name;
            public int id;
            public int down_count;
            public int revision;
            public string upload_time;
            public string url;
            public int size;
            public Struct_ShooterAPI_info_producer producer;
            public List<Struct_ShooterAPI_info_filelist> filelist;
            public string subtype;
            public string title;
            public int vote_score;
            public string release_site;
            public string videoname;
            public int view_count;
            public Struct_ShooterAPI_info_lang lang;
        }

        public struct Struct_ShooterAPI_info_producer
        {
            public string producer;
            public string verifier;
            public string source;
            public string uploader;
        }

        public struct Struct_ShooterAPI_info_filelist
        {
            public string url;
            public string f;
            public string s;
        }

        public struct Struct_ShooterAPI_info_lang
        {
            public string desc;
        }

        #endregion

        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        private string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        private string GetShooterAPIJSON(string KeyWord, string token = "LIMvLD5xfvce3cAmmDuf76Mcg0Dd90iV")
        {
            string postString = "http://api.makedie.me/v1/sub/search?token=" + token + "&q=" + KeyWord + "&cnt=15&pos=0";
            HttpWebRequest request = null;
            request = WebRequest.Create(postString) as HttpWebRequest;
            request.Method = "GET";
            using (Stream s = (request.GetResponse() as HttpWebResponse).GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return  reader.ReadToEnd();
            }
        }

        public Struct_ShooterAPI_root GetShooterAPI(string KeyWord)
        {
            return JSONParser.FromJson<Struct_ShooterAPI_root>(GetShooterAPIJSON(KeyWord));
        }

        public bool DownloadSubtitles(Struct_ShooterAPI_subs subs, string path, string target_movie = "", string token = "LIMvLD5xfvce3cAmmDuf76Mcg0Dd90iV")
        {
            //try
            {
                string temp_path = Application.StartupPath + "\\temp\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                                   DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                if (Directory.Exists(temp_path))
                {
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(temp_path);
                    directoryInfo.Create();
                }
                if (subs.id <= 0)
                    return false;
                string postString = "http://api.makedie.me/v1/sub/detail?token=" + token + "&id=" + subs.id.ToString();
                HttpWebRequest request = null;
                request = WebRequest.Create(postString) as HttpWebRequest;
                request.Method = "GET";
                string json;
                using (Stream s = (request.GetResponse() as HttpWebResponse).GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    json = reader.ReadToEnd();
                }
                Struct_ShooterAPI_info_root node = JSONParser.FromJson<Struct_ShooterAPI_info_root>(json);
                if (node.status != 0)
                    return false;
                bool flag = false;
                foreach (Struct_ShooterAPI_info_subs i in node.sub.subs)
                {
                    if (string.IsNullOrEmpty(i.url) || string.IsNullOrEmpty(i.filename))
                        continue;
                    HttpDownloadFile(i.url, temp_path + (temp_path.EndsWith("\\") ? "" : "\\") + i.filename);
                    flag = true;
                }
                return flag;
            }
            //catch
            {
                return false;
            }
        }

        private string HttpDownloadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return path;
        }

        #endregion

    }
    
}
