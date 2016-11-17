using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetVideoPlayer
{
    public class PlayList
    {
        public List<info> Info = new List<info>();
        public int Total;
        public class info
        {
            /// <summary>
            /// 文件名称
            /// </summary>
            private string name = "";
            public string Name
            { get { return name; } set { name = value; } }
            /// <summary>
            /// 是否为本地文件
            /// </summary>
            private int isLocal = 1;
            public int IsLocal
            { get { return isLocal; } set { isLocal = value; } }
            /// <summary>
            /// cookie设置
            /// </summary>
            private string cookie = "";
            public string Cookie
            { get { return cookie; } set { cookie = value; } }
            /// <summary>
            /// 缓存全路径
            /// </summary>
            private string path = "";
            public string Path
            { get { return path; } set { path = value; } }
            /// <summary>
            /// 文件路径
            /// 若为本地文件则为本地全路径
            /// 若为网络文件则为网络全路径
            /// </summary>
            private string url = "";
            public string URL
            { get { return url; } set { url = value; } }
            /// <summary>
            /// 上次播放时间
            /// </summary>
            private int lastTime = 0;
            public int LastTime
            { get { return lastTime; } set { lastTime = value; } }
        }
    }
}
