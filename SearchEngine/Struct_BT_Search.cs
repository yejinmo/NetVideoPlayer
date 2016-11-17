using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetVideoPlayer.SearchEngine
{
    /// <summary>
    /// BT搜索结构体
    /// </summary>
    public class Struct_BT_Search
    {
        /// <summary>
        /// 详细地址
        /// </summary>
        public string ToUrl { get; set; }
        /// <summary>
        /// 名称 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpTime { get; set; }
        /// <summary>
        /// 资源大小
        /// </summary>
        public string VideoLength { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        private string testing = "检测中";
        /// <summary>
        /// 是否有效
        /// </summary>
        public string Testing
        {
            get { return testing; }
            set { testing = value; }
        }

    }
}
