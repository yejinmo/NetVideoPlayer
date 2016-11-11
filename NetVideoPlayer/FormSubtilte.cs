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
    public partial class FormSubtilte : MetroForm
    {
        public FormSubtilte()
        {
            InitializeComponent();
        }

        public event C_ChangeSubtitleEnable _C_ChangeSubtitleEnable;
        public event C_ChangeSubtitleSize _C_ChangeSubtitleSize;
        public event C_ChangeSubtitleTop _C_ChangeSubtitleTop;
        public event C_SearchSubtitle _C_SearchSubtitle;

        private void FormSubtilte_Load(object sender, EventArgs e)
        {
            Visible = false;
            TopMost = true;
            Resizable = false;
            LostFocus += FormSubtilte_LostFocus;
        }

        private void FormSubtilte_LostFocus(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void FormSubtilte_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void Toggle_Subtitle_Enabled_CheckedChanged(object sender, EventArgs e)
        {
            _C_ChangeSubtitleEnable(Toggle_Subtitle_Enabled.Checked);
        }

        private void TrackBar_Subtitle_Size_Scroll(object sender, ScrollEventArgs e)
        {
            _C_ChangeSubtitleSize(TrackBar_Subtitle_Size.Value);
        }

        private void Link_Close_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void TrackBar_Subtitle_Top_Scroll(object sender, ScrollEventArgs e)
        {
            _C_ChangeSubtitleTop(TrackBar_Subtitle_Top.Value, TrackBar_Subtitle_Left.Value);
        }

        private void TrackBar_Subtitle_Left_Scroll(object sender, ScrollEventArgs e)
        {
            _C_ChangeSubtitleTop(TrackBar_Subtitle_Top.Value, TrackBar_Subtitle_Left.Value);
        }

        private void FormSubtilte_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void FormSubtilte_Deactivate(object sender, EventArgs e)
        {

        }

        private void Subtitle_Button_AutoGet_Click(object sender, EventArgs e)
        {
            _C_SearchSubtitle();
        }

    }

    public delegate void C_ChangeSubtitleEnable(bool IsEnable);
    public delegate void C_ChangeSubtitleSize(int Size);
    public delegate void C_ChangeSubtitleTop(int Top,int Left);
    public delegate void C_SearchSubtitle();

}
