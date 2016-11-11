using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetVideoPlayer
{
    public partial class MessageBoxFX : MetroForm
    {

        public MessageBoxFX(string content , string title = "" , MetroColorStyle style = MetroColorStyle.Red , MetroThemeStyle theme = MetroThemeStyle.Dark)
        {
            InitializeComponent();
            Label_Content.Text = content;
            Label_Title.Text = title;
            MetroStyleManager.Style = style;
            MetroStyleManager.Theme = theme;
            Text = string.IsNullOrEmpty(title) ? "提示" : title;
            Button_Confirm.Left = (Width - Button_Confirm.Width) / 2;
        }

        private void Button_Confirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
