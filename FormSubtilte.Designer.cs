namespace NetVideoPlayer
{
    partial class FormSubtilte
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MetroStyleManager = new MetroFramework.Components.MetroStyleManager();
            this.TabControlSubtitle = new MetroFramework.Controls.MetroTabControl();
            this.TabPageSubtitle = new MetroFramework.Controls.MetroTabPage();
            this.Subtitle_Main_IsProcess = new MetroFramework.Controls.MetroProgressSpinner();
            this.TrackBar_Subtitle_Left = new MetroFramework.Controls.MetroTrackBar();
            this.TrackBar_Subtitle_Size = new MetroFramework.Controls.MetroTrackBar();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.TrackBar_Subtitle_Top = new MetroFramework.Controls.MetroTrackBar();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.ComboBox_Select_Subtitles = new MetroFramework.Controls.MetroComboBox();
            this.ToolTip = new MetroFramework.Components.MetroToolTip();
            this.Toggle_Subtitle_Enabled = new MetroFramework.Controls.MetroToggle();
            this.Link_Close = new MetroFramework.Controls.MetroLink();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.Subtitle_Button_NoAutoGet = new MetroFramework.Controls.MetroButton();
            this.Subtitle_Button_AutoGet = new MetroFramework.Controls.MetroButton();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.TabPageScreen = new MetroFramework.Controls.MetroTabPage();
            this.TabControlSubtitle.SuspendLayout();
            this.TabPageSubtitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // MetroStyleManager
            // 
            this.MetroStyleManager.OwnerForm = this;
            this.MetroStyleManager.Style = MetroFramework.MetroColorStyle.Green;
            this.MetroStyleManager.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // TabControlSubtitle
            // 
            this.TabControlSubtitle.Controls.Add(this.TabPageSubtitle);
            this.TabControlSubtitle.Controls.Add(this.TabPageScreen);
            this.TabControlSubtitle.CustomBackground = false;
            this.TabControlSubtitle.FontSize = MetroFramework.MetroTabControlSize.Medium;
            this.TabControlSubtitle.FontWeight = MetroFramework.MetroTabControlWeight.Light;
            this.TabControlSubtitle.Location = new System.Drawing.Point(0, 7);
            this.TabControlSubtitle.Name = "TabControlSubtitle";
            this.TabControlSubtitle.SelectedIndex = 0;
            this.TabControlSubtitle.Size = new System.Drawing.Size(339, 232);
            this.TabControlSubtitle.Style = MetroFramework.MetroColorStyle.Green;
            this.TabControlSubtitle.StyleManager = this.MetroStyleManager;
            this.TabControlSubtitle.TabIndex = 0;
            this.TabControlSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TabControlSubtitle.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TabControlSubtitle.UseStyleColors = false;
            // 
            // TabPageSubtitle
            // 
            this.TabPageSubtitle.Controls.Add(this.Subtitle_Main_IsProcess);
            this.TabPageSubtitle.Controls.Add(this.TrackBar_Subtitle_Left);
            this.TabPageSubtitle.Controls.Add(this.TrackBar_Subtitle_Size);
            this.TabPageSubtitle.Controls.Add(this.metroLabel5);
            this.TabPageSubtitle.Controls.Add(this.TrackBar_Subtitle_Top);
            this.TabPageSubtitle.Controls.Add(this.metroLabel4);
            this.TabPageSubtitle.Controls.Add(this.ComboBox_Select_Subtitles);
            this.TabPageSubtitle.Controls.Add(this.metroLabel3);
            this.TabPageSubtitle.Controls.Add(this.Subtitle_Button_NoAutoGet);
            this.TabPageSubtitle.Controls.Add(this.Subtitle_Button_AutoGet);
            this.TabPageSubtitle.Controls.Add(this.metroLabel2);
            this.TabPageSubtitle.Controls.Add(this.metroLabel1);
            this.TabPageSubtitle.Controls.Add(this.Toggle_Subtitle_Enabled);
            this.TabPageSubtitle.CustomBackground = false;
            this.TabPageSubtitle.HorizontalScrollbar = false;
            this.TabPageSubtitle.HorizontalScrollbarBarColor = true;
            this.TabPageSubtitle.HorizontalScrollbarHighlightOnWheel = false;
            this.TabPageSubtitle.HorizontalScrollbarSize = 10;
            this.TabPageSubtitle.Location = new System.Drawing.Point(4, 36);
            this.TabPageSubtitle.Name = "TabPageSubtitle";
            this.TabPageSubtitle.Size = new System.Drawing.Size(331, 192);
            this.TabPageSubtitle.Style = MetroFramework.MetroColorStyle.Green;
            this.TabPageSubtitle.StyleManager = this.MetroStyleManager;
            this.TabPageSubtitle.TabIndex = 0;
            this.TabPageSubtitle.Text = "  字幕";
            this.TabPageSubtitle.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TabPageSubtitle.VerticalScrollbar = false;
            this.TabPageSubtitle.VerticalScrollbarBarColor = true;
            this.TabPageSubtitle.VerticalScrollbarHighlightOnWheel = false;
            this.TabPageSubtitle.VerticalScrollbarSize = 10;
            // 
            // Subtitle_Main_IsProcess
            // 
            this.Subtitle_Main_IsProcess.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Subtitle_Main_IsProcess.CustomBackground = false;
            this.Subtitle_Main_IsProcess.Location = new System.Drawing.Point(8, 157);
            this.Subtitle_Main_IsProcess.Maximum = 100;
            this.Subtitle_Main_IsProcess.Name = "Subtitle_Main_IsProcess";
            this.Subtitle_Main_IsProcess.Size = new System.Drawing.Size(32, 32);
            this.Subtitle_Main_IsProcess.Speed = 2F;
            this.Subtitle_Main_IsProcess.Style = MetroFramework.MetroColorStyle.Green;
            this.Subtitle_Main_IsProcess.StyleManager = this.MetroStyleManager;
            this.Subtitle_Main_IsProcess.TabIndex = 14;
            this.Subtitle_Main_IsProcess.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ToolTip.SetToolTip(this.Subtitle_Main_IsProcess, "  操作执行中  ");
            this.Subtitle_Main_IsProcess.Value = 25;
            this.Subtitle_Main_IsProcess.Visible = false;
            // 
            // TrackBar_Subtitle_Left
            // 
            this.TrackBar_Subtitle_Left.BackColor = System.Drawing.Color.Transparent;
            this.TrackBar_Subtitle_Left.CustomBackground = false;
            this.TrackBar_Subtitle_Left.KeyEventEnable = false;
            this.TrackBar_Subtitle_Left.LargeChange = ((uint)(5u));
            this.TrackBar_Subtitle_Left.Location = new System.Drawing.Point(212, 111);
            this.TrackBar_Subtitle_Left.Maximum = 100;
            this.TrackBar_Subtitle_Left.Minimum = 0;
            this.TrackBar_Subtitle_Left.MouseWheelBarPartitions = 10;
            this.TrackBar_Subtitle_Left.MouseWheelEnable = false;
            this.TrackBar_Subtitle_Left.Name = "TrackBar_Subtitle_Left";
            this.TrackBar_Subtitle_Left.Size = new System.Drawing.Size(113, 23);
            this.TrackBar_Subtitle_Left.SmallChange = ((uint)(1u));
            this.TrackBar_Subtitle_Left.Style = MetroFramework.MetroColorStyle.Green;
            this.TrackBar_Subtitle_Left.StyleManager = this.MetroStyleManager;
            this.TrackBar_Subtitle_Left.TabIndex = 13;
            this.TrackBar_Subtitle_Left.Text = "水平位置";
            this.TrackBar_Subtitle_Left.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ToolTip.SetToolTip(this.TrackBar_Subtitle_Left, "水平位置");
            this.TrackBar_Subtitle_Left.Value = 50;
            this.TrackBar_Subtitle_Left.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TrackBar_Subtitle_Left_Scroll);
            // 
            // TrackBar_Subtitle_Size
            // 
            this.TrackBar_Subtitle_Size.BackColor = System.Drawing.Color.Transparent;
            this.TrackBar_Subtitle_Size.CustomBackground = false;
            this.TrackBar_Subtitle_Size.KeyEventEnable = false;
            this.TrackBar_Subtitle_Size.LargeChange = ((uint)(5u));
            this.TrackBar_Subtitle_Size.Location = new System.Drawing.Point(231, 6);
            this.TrackBar_Subtitle_Size.Maximum = 100;
            this.TrackBar_Subtitle_Size.Minimum = 3;
            this.TrackBar_Subtitle_Size.MouseWheelBarPartitions = 5;
            this.TrackBar_Subtitle_Size.MouseWheelEnable = false;
            this.TrackBar_Subtitle_Size.Name = "TrackBar_Subtitle_Size";
            this.TrackBar_Subtitle_Size.Size = new System.Drawing.Size(97, 23);
            this.TrackBar_Subtitle_Size.SmallChange = ((uint)(1u));
            this.TrackBar_Subtitle_Size.Style = MetroFramework.MetroColorStyle.Green;
            this.TrackBar_Subtitle_Size.StyleManager = this.MetroStyleManager;
            this.TrackBar_Subtitle_Size.TabIndex = 12;
            this.TrackBar_Subtitle_Size.Text = "字幕字号";
            this.TrackBar_Subtitle_Size.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ToolTip.SetToolTip(this.TrackBar_Subtitle_Size, "字幕字号");
            this.TrackBar_Subtitle_Size.Value = 50;
            this.TrackBar_Subtitle_Size.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TrackBar_Subtitle_Size_Scroll);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.CustomBackground = false;
            this.metroLabel5.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel5.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel5.LabelMode = MetroFramework.Controls.MetroLabelMode.Selectable;
            this.metroLabel5.Location = new System.Drawing.Point(155, 8);
            this.metroLabel5.Margin = new System.Windows.Forms.Padding(8);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(65, 19);
            this.metroLabel5.Style = MetroFramework.MetroColorStyle.Green;
            this.metroLabel5.StyleManager = this.MetroStyleManager;
            this.metroLabel5.TabIndex = 11;
            this.metroLabel5.Text = "字幕字号";
            this.metroLabel5.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel5.UseStyleColors = false;
            // 
            // TrackBar_Subtitle_Top
            // 
            this.TrackBar_Subtitle_Top.BackColor = System.Drawing.Color.Transparent;
            this.TrackBar_Subtitle_Top.CustomBackground = false;
            this.TrackBar_Subtitle_Top.KeyEventEnable = false;
            this.TrackBar_Subtitle_Top.LargeChange = ((uint)(5u));
            this.TrackBar_Subtitle_Top.Location = new System.Drawing.Point(89, 111);
            this.TrackBar_Subtitle_Top.Maximum = 100;
            this.TrackBar_Subtitle_Top.Minimum = 0;
            this.TrackBar_Subtitle_Top.MouseWheelBarPartitions = 10;
            this.TrackBar_Subtitle_Top.MouseWheelEnable = false;
            this.TrackBar_Subtitle_Top.Name = "TrackBar_Subtitle_Top";
            this.TrackBar_Subtitle_Top.Size = new System.Drawing.Size(113, 23);
            this.TrackBar_Subtitle_Top.SmallChange = ((uint)(1u));
            this.TrackBar_Subtitle_Top.Style = MetroFramework.MetroColorStyle.Green;
            this.TrackBar_Subtitle_Top.StyleManager = this.MetroStyleManager;
            this.TrackBar_Subtitle_Top.TabIndex = 10;
            this.TrackBar_Subtitle_Top.Text = "垂直位置";
            this.TrackBar_Subtitle_Top.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ToolTip.SetToolTip(this.TrackBar_Subtitle_Top, "垂直位置");
            this.TrackBar_Subtitle_Top.Value = 50;
            this.TrackBar_Subtitle_Top.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TrackBar_Subtitle_Top_Scroll);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.CustomBackground = false;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel4.LabelMode = MetroFramework.Controls.MetroLabelMode.Selectable;
            this.metroLabel4.Location = new System.Drawing.Point(8, 113);
            this.metroLabel4.Margin = new System.Windows.Forms.Padding(8);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(65, 19);
            this.metroLabel4.Style = MetroFramework.MetroColorStyle.Green;
            this.metroLabel4.StyleManager = this.MetroStyleManager;
            this.metroLabel4.TabIndex = 9;
            this.metroLabel4.Text = "字幕位置";
            this.metroLabel4.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel4.UseStyleColors = false;
            // 
            // ComboBox_Select_Subtitles
            // 
            this.ComboBox_Select_Subtitles._ToolTip = this.ToolTip;
            this.ComboBox_Select_Subtitles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ComboBox_Select_Subtitles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Select_Subtitles.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.ComboBox_Select_Subtitles.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.ComboBox_Select_Subtitles.FormattingEnabled = true;
            this.ComboBox_Select_Subtitles.ItemHeight = 23;
            this.ComboBox_Select_Subtitles.Location = new System.Drawing.Point(89, 73);
            this.ComboBox_Select_Subtitles.Margin = new System.Windows.Forms.Padding(8);
            this.ComboBox_Select_Subtitles.Name = "ComboBox_Select_Subtitles";
            this.ComboBox_Select_Subtitles.Size = new System.Drawing.Size(236, 29);
            this.ComboBox_Select_Subtitles.Style = MetroFramework.MetroColorStyle.Green;
            this.ComboBox_Select_Subtitles.StyleManager = this.MetroStyleManager;
            this.ComboBox_Select_Subtitles.TabIndex = 8;
            this.ComboBox_Select_Subtitles.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // ToolTip
            // 
            this.ToolTip.Style = MetroFramework.MetroColorStyle.Blue;
            this.ToolTip.StyleManager = null;
            this.ToolTip.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // Toggle_Subtitle_Enabled
            // 
            this.Toggle_Subtitle_Enabled.AutoSize = true;
            this.Toggle_Subtitle_Enabled.CustomBackground = false;
            this.Toggle_Subtitle_Enabled.DisplayStatus = false;
            this.Toggle_Subtitle_Enabled.FontSize = MetroFramework.MetroLinkSize.Small;
            this.Toggle_Subtitle_Enabled.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.Toggle_Subtitle_Enabled.Location = new System.Drawing.Point(89, 11);
            this.Toggle_Subtitle_Enabled.Margin = new System.Windows.Forms.Padding(8);
            this.Toggle_Subtitle_Enabled.Name = "Toggle_Subtitle_Enabled";
            this.Toggle_Subtitle_Enabled.Size = new System.Drawing.Size(50, 16);
            this.Toggle_Subtitle_Enabled.Style = MetroFramework.MetroColorStyle.Green;
            this.Toggle_Subtitle_Enabled.StyleManager = this.MetroStyleManager;
            this.Toggle_Subtitle_Enabled.TabIndex = 2;
            this.Toggle_Subtitle_Enabled.Text = "Off";
            this.Toggle_Subtitle_Enabled.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ToolTip.SetToolTip(this.Toggle_Subtitle_Enabled, "开启/关闭字幕");
            this.Toggle_Subtitle_Enabled.UseStyleColors = false;
            this.Toggle_Subtitle_Enabled.UseVisualStyleBackColor = true;
            this.Toggle_Subtitle_Enabled.CheckedChanged += new System.EventHandler(this.Toggle_Subtitle_Enabled_CheckedChanged);
            // 
            // Link_Close
            // 
            this.Link_Close.CustomBackground = false;
            this.Link_Close.FontSize = MetroFramework.MetroLinkSize.Small;
            this.Link_Close.FontWeight = MetroFramework.MetroLinkWeight.Bold;
            this.Link_Close.Location = new System.Drawing.Point(284, 12);
            this.Link_Close.Name = "Link_Close";
            this.Link_Close.Size = new System.Drawing.Size(45, 23);
            this.Link_Close.Style = MetroFramework.MetroColorStyle.Green;
            this.Link_Close.StyleManager = this.MetroStyleManager;
            this.Link_Close.TabIndex = 1;
            this.Link_Close.Text = "关闭";
            this.Link_Close.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ToolTip.SetToolTip(this.Link_Close, "关闭此面板");
            this.Link_Close.UseStyleColors = false;
            this.Link_Close.Click += new System.EventHandler(this.Link_Close_Click);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.CustomBackground = false;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel3.LabelMode = MetroFramework.Controls.MetroLabelMode.Selectable;
            this.metroLabel3.Location = new System.Drawing.Point(8, 78);
            this.metroLabel3.Margin = new System.Windows.Forms.Padding(8);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(65, 19);
            this.metroLabel3.Style = MetroFramework.MetroColorStyle.Green;
            this.metroLabel3.StyleManager = this.MetroStyleManager;
            this.metroLabel3.TabIndex = 7;
            this.metroLabel3.Text = "当前字幕";
            this.metroLabel3.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel3.UseStyleColors = false;
            // 
            // Subtitle_Button_NoAutoGet
            // 
            this.Subtitle_Button_NoAutoGet.Highlight = false;
            this.Subtitle_Button_NoAutoGet.Location = new System.Drawing.Point(212, 41);
            this.Subtitle_Button_NoAutoGet.Margin = new System.Windows.Forms.Padding(8);
            this.Subtitle_Button_NoAutoGet.Name = "Subtitle_Button_NoAutoGet";
            this.Subtitle_Button_NoAutoGet.Size = new System.Drawing.Size(113, 23);
            this.Subtitle_Button_NoAutoGet.Style = MetroFramework.MetroColorStyle.Green;
            this.Subtitle_Button_NoAutoGet.StyleManager = this.MetroStyleManager;
            this.Subtitle_Button_NoAutoGet.TabIndex = 6;
            this.Subtitle_Button_NoAutoGet.Text = "手动载入";
            this.Subtitle_Button_NoAutoGet.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // Subtitle_Button_AutoGet
            // 
            this.Subtitle_Button_AutoGet.Highlight = false;
            this.Subtitle_Button_AutoGet.Location = new System.Drawing.Point(89, 41);
            this.Subtitle_Button_AutoGet.Margin = new System.Windows.Forms.Padding(8);
            this.Subtitle_Button_AutoGet.Name = "Subtitle_Button_AutoGet";
            this.Subtitle_Button_AutoGet.Size = new System.Drawing.Size(113, 23);
            this.Subtitle_Button_AutoGet.Style = MetroFramework.MetroColorStyle.Green;
            this.Subtitle_Button_AutoGet.StyleManager = this.MetroStyleManager;
            this.Subtitle_Button_AutoGet.TabIndex = 5;
            this.Subtitle_Button_AutoGet.Text = "自动匹配";
            this.Subtitle_Button_AutoGet.Theme = MetroFramework.MetroThemeStyle.Light;
            this.Subtitle_Button_AutoGet.Click += new System.EventHandler(this.Subtitle_Button_AutoGet_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.CustomBackground = false;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel2.LabelMode = MetroFramework.Controls.MetroLabelMode.Selectable;
            this.metroLabel2.Location = new System.Drawing.Point(8, 43);
            this.metroLabel2.Margin = new System.Windows.Forms.Padding(8);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(65, 19);
            this.metroLabel2.Style = MetroFramework.MetroColorStyle.Green;
            this.metroLabel2.StyleManager = this.MetroStyleManager;
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "加载字幕";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel2.UseStyleColors = false;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.CustomBackground = false;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel1.LabelMode = MetroFramework.Controls.MetroLabelMode.Selectable;
            this.metroLabel1.Location = new System.Drawing.Point(8, 8);
            this.metroLabel1.Margin = new System.Windows.Forms.Padding(8);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(65, 19);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Green;
            this.metroLabel1.StyleManager = this.MetroStyleManager;
            this.metroLabel1.TabIndex = 3;
            this.metroLabel1.Text = "字幕开关";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel1.UseStyleColors = false;
            // 
            // TabPageScreen
            // 
            this.TabPageScreen.CustomBackground = false;
            this.TabPageScreen.HorizontalScrollbar = false;
            this.TabPageScreen.HorizontalScrollbarBarColor = true;
            this.TabPageScreen.HorizontalScrollbarHighlightOnWheel = false;
            this.TabPageScreen.HorizontalScrollbarSize = 10;
            this.TabPageScreen.Location = new System.Drawing.Point(4, 36);
            this.TabPageScreen.Name = "TabPageScreen";
            this.TabPageScreen.Size = new System.Drawing.Size(331, 192);
            this.TabPageScreen.Style = MetroFramework.MetroColorStyle.Green;
            this.TabPageScreen.StyleManager = this.MetroStyleManager;
            this.TabPageScreen.TabIndex = 1;
            this.TabPageScreen.Text = "   画面";
            this.TabPageScreen.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TabPageScreen.VerticalScrollbar = false;
            this.TabPageScreen.VerticalScrollbarBarColor = true;
            this.TabPageScreen.VerticalScrollbarHighlightOnWheel = false;
            this.TabPageScreen.VerticalScrollbarSize = 10;
            // 
            // FormSubtilte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 239);
            this.Controls.Add(this.Link_Close);
            this.Controls.Add(this.TabControlSubtitle);
            this.DisplayHeader = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSubtilte";
            this.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.StyleManager = this.MetroStyleManager;
            this.Text = "设置";
            this.Deactivate += new System.EventHandler(this.FormSubtilte_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSubtilte_FormClosing);
            this.Load += new System.EventHandler(this.FormSubtilte_Load);
            this.VisibleChanged += new System.EventHandler(this.FormSubtilte_VisibleChanged);
            this.TabControlSubtitle.ResumeLayout(false);
            this.TabPageSubtitle.ResumeLayout(false);
            this.TabPageSubtitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Components.MetroStyleManager MetroStyleManager;
        private MetroFramework.Controls.MetroTabControl TabControlSubtitle;
        private MetroFramework.Controls.MetroTabPage TabPageSubtitle;
        public MetroFramework.Controls.MetroToggle Toggle_Subtitle_Enabled;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTabPage TabPageScreen;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton Subtitle_Button_NoAutoGet;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroLink Link_Close;
        public MetroFramework.Controls.MetroTrackBar TrackBar_Subtitle_Top;
        public MetroFramework.Controls.MetroTrackBar TrackBar_Subtitle_Size;
        public MetroFramework.Controls.MetroTrackBar TrackBar_Subtitle_Left;
        public MetroFramework.Controls.MetroProgressSpinner Subtitle_Main_IsProcess;
        public MetroFramework.Controls.MetroComboBox ComboBox_Select_Subtitles;
        public MetroFramework.Components.MetroToolTip ToolTip;
        public MetroFramework.Controls.MetroButton Subtitle_Button_AutoGet;
    }
}