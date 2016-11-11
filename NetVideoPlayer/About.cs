using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Threading;
using MetroFramework;
using System.Runtime.InteropServices;
using TinyJson;

namespace NetVideoPlayer
{
    public partial class About : MetroForm
    {
        public About(MetroColorStyle style = MetroColorStyle.Red, MetroThemeStyle theme = MetroThemeStyle.Light)
        {
            InitializeComponent();
            MetroStyleManager.Style = style;
            MetroStyleManager.Theme = theme;
        }
    }
}
