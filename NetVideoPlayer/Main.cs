using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Threading;
using MetroFramework;
using System.Runtime.InteropServices;
using NetVideoPlayer.Properties;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using NetVideoPlayer.SearchEngine;
using NetVideoPlayer.SubtitlesMatch;

/*
http://www.bthave.org/
http://www.btfuli.net/
http://www.isoupian.com/
http://www.btgongcang.com/
http://www.bt177.net/
*/

namespace NetVideoPlayer
{
    public partial class Main : MetroForm
    {

        #region 全局

        //窗体移动
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private ThunderHelper th = new ThunderHelper();

        private FormSubtilte fs;

        public static Player player;

        private string config_path = Application.StartupPath + @"\config.json";

        private string version = "Beta 0.8.1";

        #endregion

        #region 初始化

        public Main()
        {
            InitializeComponent();
            PlayList_Load();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Thread ThreadLoad = new Thread(new ThreadStart(LoadThread));
            ThreadLoad.Start();
        }

        private void LoadThread()
        {
            try
            {
                Invoke((EventHandler)delegate
                {

                    #region UI初始化及设置

                    Panel_Search_Panel.Left = 42;

                    MinimumSize = new Size(900, 613);

                    MakeButtonImage(Color.FromArgb(153, 153, 153), Color.FromArgb(204, 204, 204), Color.FromArgb(51, 51, 51));

                    Picture_Search_Logo.Image = ReduceImage(Resources.Logo, Picture_Search_Logo.Width, Picture_Search_Logo.Height);

                    LoadSubtitleForm();

                    Panel_Config_Save_Command.Location = new Point(92, 41);

                    Panel_Main_Download.Dock = DockStyle.Fill;

                    PanelConfig.Location = new Point(8, 27);

                    Label_Config_About_Version.Text = "当前版本       " + version;

                    Text_Search_SearchBox.BaseTextBoxLostFocus(null, null);

                    #endregion

                    #region 播放器初始化及设置

                    player = new Player();

                    TabPageMain.Controls.Add(player);

                    player.OnMessage += Player_OnMessage;
                    player.PreviewKeyDown += Player_PreviewKeyDown;
                    player.OnStateChanged += Player_OnStateChanged;
                    player.OnOpenSucceeded += Player_OnOpenSucceeded;
                    player.OnDownloadCodec += Player_OnDownloadCodec;
                    player.OnBuffer += Player_OnBuffer;
                    player.OnSeekCompleted += Player_OnSeekCompleted;
                    TabControlMain.SelectedTab = TabPageMain;

                    LoadConfig(LoadConfigFile());

                    player._SetLogo(new Bitmap(Resources.Logo));
                    player.SetConfig(2207, "1");

                    player.Left = 0;
                    player.Top = 0;
                    player.Width = TabPageMain.Width;
                    player.Height = ControlPanel.Top;
                    player.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

                    LoadSupptorList(player.GetSubtitleExtnameList());

                    #endregion

                    #region 播放列表初始化

                    PlayList_Load();

                    #endregion

                });
                Invoke((EventHandler)delegate
                {
                    ShowPictureText("准备就绪");
                });

            }
            catch
            {

            }
            finally
            {
            }
        }

        #endregion

        #region 配置设置

        /// <summary>
        /// 配置设置根节点
        /// </summary>
        public struct StructConfig_root
        {
            public StructConfig_player player;
            public StructConfig_UI UI;
            public string CheckCode;
        }

        /// <summary>
        /// 配置设置播放器设置节点
        /// </summary>
        public struct StructConfig_player
        {
            public string DecodingPath;
            public int SpeedupEnable;
            public int RenderMode;
            public int ImageNormalizeEnable;
            public int Volume;
            public int SoundSilent;
            public int SubtitleShow;
            public int SubtitleTopPercent;
            public int SubtitleLeftPercent;
            public int SubtitleFontSize;
            public string SubtitleFontName;
            public int SubtitleFontColor;
            public int SubtitleFontShadow;
            public int CacheEnable;
            public string CachePath;
        }

        /// <summary>
        /// 配置设置UI设置节点
        /// </summary>
        public struct StructConfig_UI
        {
            public int MetroColorStyle;
            public int MetroThemeStyle;
            public int AutoPause;
        }

        /// <summary>
        /// 获取初始化配置
        /// </summary>
        /// <returns>初始化配置</returns>
        private StructConfig_root GetDefaultConfig()
        {
            StructConfig_root res = new StructConfig_root();
            res.player.DecodingPath = Application.StartupPath + "\\codecs";
            res.player.SpeedupEnable = 1;
            res.player.RenderMode = 4;
            res.player.ImageNormalizeEnable = 0;
            res.player.Volume = 50;
            res.player.SoundSilent = 0;
            res.player.SubtitleShow = 1;
            res.player.SubtitleTopPercent = 80;
            res.player.SubtitleLeftPercent = 50;
            res.player.SubtitleFontSize = 20;

            res.player.SubtitleFontName = "黑体";
            res.player.SubtitleFontColor = _ParseRGB(Color.FromArgb(255, 255, 255));
            res.player.SubtitleFontShadow = 1;

            res.player.CacheEnable = 1;
            res.player.CachePath = Application.StartupPath + "\\download";

            res.UI.MetroColorStyle = (int)MetroColorStyle.Green;
            res.UI.MetroThemeStyle = (int)MetroThemeStyle.Dark;
            res.UI.AutoPause = 1;

            return res;
        }

        /// <summary>
        /// 将颜色转换为十进制数
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private int _ParseRGB(Color color)
        {
            return (int)(((uint)color.B << 16) | (ushort)(((ushort)color.G << 8) | color.R));
        }

        /// <summary>
        /// 将十进制数转换为颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Color _ParseColor(int color)
        {
            int red = color & 255;
            int green = color >> 8 & 255;
            int blue = color >> 16 & 255;
            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// 初始化配置设置处理过程
        /// </summary>
        /// <returns></returns>
        private StructConfig_root LoadConfigFile()
        {
            StructConfig_root res = new StructConfig_root();
            if (File.Exists(config_path))
            {           //文件存在
                res = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<StructConfig_root>(ReadConfigFile());
                if (EncodeAndDecode(res.CheckCode) == GetDateTimeString(new FileInfo(config_path).LastWriteTime))
                    return res;   //文件合法
                else
                {       //文件不合法
                    res = GetDefaultConfig();
                    res.CheckCode = EncodeAndDecode(GetDateTimeString(DateTime.Now));
                    SaveConfigFile(MakeConfigFile(res));
                }
            }
            else        //文件不存在
            {
                res = GetDefaultConfig();
                res.CheckCode = EncodeAndDecode(GetDateTimeString(DateTime.Now));
                SaveConfigFile(MakeConfigFile(res));
            }
            return res;
        }

        /// <summary>
        /// 加载配置设置
        /// </summary>
        /// <param name="config"></param>
        private void LoadConfig(StructConfig_root config)
        {

            #region 播放器配置初始化

            player.SetDecodingPath(config.player.DecodingPath);
            player.SetSpeedupEnable(config.player.SpeedupEnable != 0 ? true : false);
            player.SetRenderModeConfig((Player.EnumRenderMode)config.player.RenderMode);
            player.SetVolume(config.player.Volume);
            player.SetSoundSilent((Player.EnumSilent)config.player.SoundSilent);
            player.SetSubtitlePlacement(true, config.player.SubtitleLeftPercent, config.player.SubtitleTopPercent);
            player.SetSubtitleFont(config.player.SubtitleFontName, config.player.SubtitleFontSize, _ParseColor(config.player.SubtitleFontColor), config.player.SubtitleFontShadow);

            #endregion

            #region 主窗体配置初始化

            TrackBar_Main_Volume.Value = config.player.Volume <= 100 && config.player.Volume >= 0 ? config.player.Volume : 100;
            Picture_Main_Volume.Image = config.player.SoundSilent != 0 ? _Image_Main_Silent_Normal : _Image_Main_Volume_Normal;
            Picture_Main_Volume_IsSilent = config.player.SoundSilent != 0 ? true : false;

            MetroStyleManager.Theme = (MetroThemeStyle)config.UI.MetroThemeStyle;
            MetroStyleManager.Style = (MetroColorStyle)config.UI.MetroColorStyle;
            Toggle_Config_UI_AutoPause.Checked = config.UI.AutoPause != 0 ? true : false;

            #endregion

            #region 子窗体配置初始化

            fs.Toggle_Subtitle_Enabled.Checked = config.player.SubtitleShow != 0 ? true : false;
            fs.TrackBar_Subtitle_Top.Value = (config.player.SubtitleTopPercent <= 100 && config.player.SubtitleTopPercent >= 0) ? config.player.SubtitleTopPercent : 100;
            fs.TrackBar_Subtitle_Left.Value = (config.player.SubtitleLeftPercent <= 100 && config.player.SubtitleLeftPercent >= 0) ? config.player.SubtitleLeftPercent : 100;
            fs.TrackBar_Subtitle_Size.Value = (config.player.SubtitleFontSize <= 100 && config.player.SubtitleFontSize >= 3) ? config.player.SubtitleFontSize : 100;

            #endregion

            #region 设置界面初始化

            Toggle_Config_Video_SpeedUp.Checked = config.player.SpeedupEnable == 1 ? true : false;

            if (config.player.RenderMode == 1)
                RadioButton_Config_Video_Render_Mode_Overlay.Checked = true;
            else if (config.player.RenderMode == 2)
                RadioButton_Config_Video_Render_Mode_Renderless.Checked = true;
            else if (config.player.RenderMode == 3)
                RadioButton_Config_Video_Render_Mode_EVR.Checked = true;
            else if (config.player.RenderMode == 4)
                RadioButton_Config_Video_Render_Mode_EVRCP.Checked = true;

            Toggle_Config_Video_Image_Nomallize.Checked = config.player.ImageNormalizeEnable == 1 ? true : false;

            Toggle_Config_Download_Cache_Enabled.Checked = config.player.CacheEnable == 1 ? true : false;
            Text_Config_Download_Cache_Path.Text = config.player.CachePath;

            if (config.UI.MetroThemeStyle == 1)
                RadioButton_Config_UI_Theme_Dark.Checked = true;
            else
                RadioButton_Config_UI_Theme_Light.Checked = true;

            switch (config.UI.MetroColorStyle)
            {
                case 0:
                    {
                        RadioButton_Config_UI_Style_Black.Checked = true;
                        break;
                    }
                case 1:
                    {
                        RadioButton_Config_UI_Style_White.Checked = true;
                        break;
                    }
                case 2:
                    {
                        RadioButton_Config_UI_Style_Silver.Checked = true;
                        break;
                    }
                case 3:
                    {
                        RadioButton_Config_UI_Style_Blue.Checked = true;
                        break;
                    }
                case 4:
                    {
                        RadioButton_Config_UI_Style_Green.Checked = true;
                        break;
                    }
                case 5:
                    {
                        RadioButton_Config_UI_Style_Lime.Checked = true;
                        break;
                    }
                case 6:
                    {
                        RadioButton_Config_UI_Style_Teal.Checked = true;
                        break;
                    }
                case 7:
                    {
                        RadioButton_Config_UI_Style_Orange.Checked = true;
                        break;
                    }
                case 8:
                    {
                        RadioButton_Config_UI_Style_Brown.Checked = true;
                        break;
                    }
                case 9:
                    {
                        RadioButton_Config_UI_Style_Pink.Checked = true;
                        break;
                    }
                case 10:
                    {
                        RadioButton_Config_UI_Style_Magenta.Checked = true;
                        break;
                    }
                case 11:
                    {
                        RadioButton_Config_UI_Style_Purple.Checked = true;
                        break;
                    }
                case 12:
                    {
                        RadioButton_Config_UI_Style_Red.Checked = true;
                        break;
                    }
                case 13:
                    {
                        RadioButton_Config_UI_Style_Yellow.Checked = true;
                        break;
                    }
            }

            #endregion

        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="json">配置文件字符串</param>
        private void SaveConfigFile(string json)
        {
            FileStream fs = new FileStream(config_path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns>配置文件字符串</returns>
        private string ReadConfigFile()
        {
            StreamReader sr = new StreamReader(config_path);
            string json = "", temp;
            while ((temp = sr.ReadLine()) != null)
                json += temp;
            sr.Close();
            return json;
        }

        /// <summary>
        /// 生成配置文件
        /// </summary>
        /// <param name="JSON">待序列化的结构体</param>
        /// <returns>配置文件字符串</returns>
        private string MakeConfigFile(StructConfig_root JSON)
        {
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            return (jss.Serialize(JSON));
        }

        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <returns>校验码</returns>
        private string GetCheckCode()
        {
            return EncodeAndDecode(GetDateTimeString(DateTime.Now));
        }

        /// <summary>
        /// 加密解密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        private string EncodeAndDecode(string str, byte key = 123)
        {
            byte[] bs = Encoding.Default.GetBytes(str);
            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (byte)(bs[i] ^ key);
            }
            return Encoding.Default.GetString(bs);
        }

        /// <summary>
        /// 获取DateTime生成的字符串
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>DateTime生成的字符串</returns>
        private string GetDateTimeString(DateTime dt)
        {
            return dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString();
        }

        #endregion

        #region 播放器事件

        #region 鼠标事件

        /// <summary>
        /// 鼠标事件
        /// </summary>
        private enum WinMsg
        {
            WM_MOUSEMOVE = 0x0200, //鼠标移动
            WM_LBUTTONDOWN = 0x201, //Left mousebutton down
            WM_LBUTTONUP = 0x202,  //Left mousebutton up
            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
            WM_RBUTTONDOWN = 0x204, //Right mousebutton down
            WM_RBUTTONUP = 0x205,   //Right mousebutton up
            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
            WM_KEYDOWN = 0x100,  //Key down
            WM_KEYUP = 0x101   //Key up
        }

        private void Player_OnMessage(object sender, AxAPlayer3Lib._IPlayerEvents_OnMessageEvent e)
        {
            int code = e.nMessage;
            if (code == Convert.ToInt32(WinMsg.WM_LBUTTONDBLCLK))
            {
                //全屏控制
                IsPlayerDBClick = true;
                Action_FullScreen();
            }
            else if (code == Convert.ToInt32(WinMsg.WM_LBUTTONUP))
            {
                if (IsCanPlayerClick)
                {
                    IsCanPlayerClick = false;
                    Timer_Main_Player_DBClick.Enabled = true;
                }
            }
            else if (code == Convert.ToInt32(WinMsg.WM_LBUTTONDOWN))
            {
                IsCanPlayerClick = true;
                Timer_Main_Player_Click.Enabled = true;
                IsPlayerDBClick = false;
            }
            else if (code == Convert.ToInt32(WinMsg.WM_MOUSEMOVE))
            {
                if (!IsFullScreen)
                {
                    ReleaseCapture(); //释放鼠标捕捉
                                      //发送左键点击的消息至该窗体(标题栏)
                    SendMessage(Handle, 0xA1, 0x02, 0);
                }
                else if (IsFullScreen)
                {
                    if (!ControlPanel.Visible)
                        ControlPanel.Visible = true;
                    Timer_Main_Show_ControlPanel.Enabled = false;
                    Timer_Main_Show_ControlPanel.Enabled = true;
                }
            }
        }

        private bool IsCanPlayerClick = false;

        private bool IsPlayerDBClick = false;

        private void Timer_Main_Player_Click_Tick(object sender, EventArgs e)
        {
            IsCanPlayerClick = false;
            Timer_Main_Player_Click.Enabled = false;
        }

        private void Timer_Main_Player_DBClick_Tick(object sender, EventArgs e)
        {
            if (!IsPlayerDBClick)
            {
                IsPlayerDBClick = false;
                if (player.GetState() == Player.EnumPlayState.IsPlay)
                {
                    ShowPictureText("已暂停");
                    Thread.Sleep(100);
                    player.Pause();
                }
                else if (player.GetState() == Player.EnumPlayState.IsPause)
                {
                    player.Play();
                    ShowPictureText("恢复播放");
                }
            }
            Timer_Main_Player_DBClick.Enabled = false;
        }

        #endregion

        #region 键盘事件

        private void Player_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (TabControlMain.SelectedTab == TabPageMain)
            {
                if (e.KeyData == (Keys.Enter | Keys.Control))
                {
                    Action_FullScreen();
                }
                else if (e.KeyData == (Keys.Escape))
                {
                    if (IsFullScreen)
                        Action_FullScreen();
                }
                else if (e.KeyData == (Keys.Left))
                {
                    if (player.GetPosition() - 5000 <= 0)
                        player.SetPosition(0);
                    else
                        player.SetPosition(player.GetPosition() - 5000);
                    ShowPictureText("跳转播放：" + player._ConvertTime(player.GetPosition()));
                    if (player.GetState() == Player.EnumPlayState.IsPause)
                        TrackBar_Main.Value = player.GetPosition() / 1000;
                }
                else if (e.KeyData == (Keys.Right))
                {
                    player.SetPosition(player.GetPosition() + 5000);
                    ShowPictureText("跳转播放：" + player._ConvertTime(player.GetPosition()));
                    if (player.GetState() == Player.EnumPlayState.IsPause)
                        TrackBar_Main.Value = player.GetPosition() / 1000;
                }
                else if (e.KeyData == (Keys.Space))
                {
                    if (player.GetState() == Player.EnumPlayState.IsPlay)
                    {
                        ShowPictureText("已暂停");
                        Thread.Sleep(100);
                        player.Pause();
                    }
                    else if (player.GetState() == Player.EnumPlayState.IsPause)
                    {
                        player.Play();
                        ShowPictureText("恢复播放");
                    }
                }
                else if (e.KeyData == (Keys.Up))
                {
                    int Volume = player.GetVolume();
                    if (Volume <= 99)
                    {
                        Volume++;
                        TrackBar_Main_Volume.Value = Volume;
                    }
                    else
                        Volume += 10;
                    if (Volume > 1000 || Volume < 0)
                        return;
                    player.SetVolume(Volume);
                    ShowPictureText("音量：" + player.GetVolume() + "%");
                }
                else if (e.KeyData == (Keys.Down))
                {
                    int Volume = player.GetVolume();
                    if (Volume <= 100)
                    {
                        if (Volume > 0)
                        {
                            Volume--;
                            TrackBar_Main_Volume.Value = Volume;
                        }
                    }
                    else
                        Volume -= 10;
                    if (Volume > 1000 || Volume < 0)
                        return;
                    player.SetVolume(Volume);
                    ShowPictureText("音量：" + player.GetVolume() + "%");
                }
            }
        }

        #endregion

        #region 播放状态改变事件

        private enum PLAY_STATE
        {
            PS_READY = 0,  // 准备就绪
            PS_OPENING = 1,  // 正在打开
            PS_PAUSING = 2,  // 正在暂停
            PS_PAUSED = 3,  // 暂停中
            PS_PLAYING = 4,  // 正在开始播放
            PS_PLAY = 5,  // 播放中
            PS_CLOSING = 6,  // 正在开始关闭
        };

        private void Player_OnStateChanged(object sender, AxAPlayer3Lib._IPlayerEvents_OnStateChangedEvent e)
        {
            PLAY_STATE OldState = (PLAY_STATE)e.nOldState;
            switch ((PLAY_STATE)e.nNewState)
            {
                case PLAY_STATE.PS_READY:
                    {
                        TrackBar_Main.Enabled = false;
                        Picture_Main_PlayOrPause_IsPlay = false;
                        Picture_Main_PlayOrPause.Image = _Image_Main_Play_Normal;
                        Label_Main_TimeTip.Text = "00:00:00 / 00:00:00";
                        TrackBar_Main.Value = 0;
                        GetCurrentSpeedByKB_Old_Size = 0;
                        GetCurrentSpeedByKB_Current_Size = 0;
                        if (Label_Main_OnBuffer.Visible)
                            Label_Main_OnBuffer.Visible = false;
                        if (Panel_Main_Download.Visible)
                            Panel_Main_Download.Visible = false;
                        ShowPictureText("准备就绪");
                        if (OldState == PLAY_STATE.PS_OPENING)
                            ShowPictureText("文件打开失败");
                        break;
                    }
                case PLAY_STATE.PS_OPENING:
                    {
                        break;
                    }
                case PLAY_STATE.PS_PAUSING:
                    {
                        Picture_Main_PlayOrPause_IsPlay = false;
                        Picture_Main_PlayOrPause.Image = _Image_Main_Play_Normal;
                        break;
                    }
                case PLAY_STATE.PS_PAUSED:
                    {

                        break;
                    }
                case PLAY_STATE.PS_PLAYING:
                    {
                        break;
                    }
                case PLAY_STATE.PS_PLAY:
                    {
                        if (!TrackBar_Main.Enabled)
                            TrackBar_Main.Enabled = true;
                        Picture_Main_PlayOrPause_IsPlay = true;
                        Picture_Main_PlayOrPause.Image = _Image_Main_Pause_Normal;
                        Panel_Main_Download.Visible = false;
                        break;
                    }
                case PLAY_STATE.PS_CLOSING:
                    {
                        if (Label_Main_OnBuffer.Visible)
                            Label_Main_OnBuffer.Visible = false;
                        break;
                    }
                default:
                    break;
            }
        }

        #endregion

        private void Player_OnOpenSucceeded(object sender, EventArgs e)
        {
            ShowPictureText("正在播放 - " + player.GetCurrentURL());
            PlayList_AddCurrentMedia();
        }

        private void Player_OnBuffer(object sender, AxAPlayer3Lib._IPlayerEvents_OnBufferEvent e)
        {
            if (e.nPercent == 100)
            {
                Label_Main_OnBuffer.Visible = false;
                return;
            }
            Label_Main_OnBuffer.Text = "缓冲中 " + e.nPercent.ToString() + "% - " + GetCurrentSpeedByKB_CurrentSpeedByKB.ToString() + "KB/s";
            if (!Label_Main_OnBuffer.Visible)
                Label_Main_OnBuffer.Visible = true;
        }

        private void Player_OnDownloadCodec(object sender, AxAPlayer3Lib._IPlayerEvents_OnDownloadCodecEvent e)
        {
            MessageBoxFX_Show(e.strCodecPath);
        }

        private void Player_OnSeekCompleted(object sender, AxAPlayer3Lib._IPlayerEvents_OnSeekCompletedEvent e)
        {
            GetCurrentSpeedByKB_Old_Size = player.GetReadSize();
            GetCurrentSpeedByKB_Current_Size = GetCurrentSpeedByKB_Old_Size;
        }

        #endregion

        #region 控制面板按钮事件

        private bool Picture_Main_PlayOrPause_IsPlay = false;

        private void Picture_Main_PlayOrPause_Click(object sender, EventArgs e)
        {
            if (player.GetState() == Player.EnumPlayState.IsPlay)
            {
                Picture_Main_PlayOrPause_IsPlay = false;
                ShowPictureText("已暂停");
                Thread.Sleep(100);
                player.Pause();
            }
            else if (player.GetState() == Player.EnumPlayState.IsPause)
            {
                Picture_Main_PlayOrPause_IsPlay = true;
                player.Play();
                ShowPictureText("恢复播放");
            }
        }

        private void Picture_Main_Last_Click(object sender, EventArgs e)
        {

        }

        private void Picture_Main_Next_Click(object sender, EventArgs e)
        {

        }

        private void Picture_Main_Stop_Click(object sender, EventArgs e)
        {
            player.Close();
        }

        private bool Picture_Main_Volume_IsSilent = false;

        private void Picture_Main_Volume_Click(object sender, EventArgs e)
        {
            if (player.GetSoundSilent() == Player.EnumSilent.Enabled)
            {
                player.SetSoundSilent(Player.EnumSilent.Disabled);
                Picture_Main_Volume_IsSilent = false;
                ShowPictureText("静音关闭");
            }
            else
            {
                player.SetSoundSilent(Player.EnumSilent.Enabled);
                Picture_Main_Volume_IsSilent = true;
                ShowPictureText("静音开启");
            }
        }

        private void Picture_Main_Subtilte_Click(object sender, EventArgs e)
        {
            if (fs == null)
                fs = new FormSubtilte();
            if (!fs.Visible)
                fs.Location = new Point(Left + Width - Picture_Main_Subtilte.Width - fs.Width + 17, Top + Height - Picture_Main_Subtilte.Height - fs.Height - 50);
            fs.Visible = !fs.Visible;
        }

        private void Picture_Main_List_Click(object sender, EventArgs e)
        {
            Panel_Main_Play_List.Visible = !Panel_Main_Play_List.Visible;
        }

        #endregion 

        #region 播放进度控制

        private bool TrackBar_Main_Process_IsMouseDown = false;

        private void TrackBar_Main_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void TrackBar_Main_MouseDown(object sender, MouseEventArgs e)
        {
            TrackBar_Main_Process_IsMouseDown = true;
            Label_Main_TrackBar_Tip.Left = (int)((((double)TrackBar_Main.Value / (double)TrackBar_Main.Maximum) * (double)TrackBar_Main.Width) - ((double)Label_Main_TrackBar_Tip.Width / 2)) + 7;
            Label_Main_TrackBar_Tip.Text = player._ConvertTime(player.GetPosition());
            Label_Main_TrackBar_Tip.Visible = true;
        }

        private void TrackBar_Main_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBar_Main_Process_IsMouseDown = false;
            Label_Main_TrackBar_Tip.Visible = false;
            player.SetPosition(TrackBar_Main.Value * 1000);
            ShowPictureText("跳转播放：" + player._ConvertTime(player.GetPosition()));
        }

        private int TrackBar_Main_Old = 0;

        private void TrackBar_Main_ValueChanged(object sender, EventArgs e)
        {
            if (TrackBar_Main_Process_IsMouseDown && TrackBar_Main.Value != TrackBar_Main_Old)
            {
                TrackBar_Main_Old = TrackBar_Main.Value;
                player.SetPosition(TrackBar_Main.Value * 1000);
                ShowPictureText("跳转播放：" + player._ConvertTime(player.GetPosition()));
                Label_Main_TrackBar_Tip.Text = player._ConvertTime(player.GetPosition());
                Label_Main_TrackBar_Tip.Left = (int)((((double)TrackBar_Main.Value / (double)TrackBar_Main.Maximum) * (double)TrackBar_Main.Width) - ((double)Label_Main_TrackBar_Tip.Width / 2)) + 7;
            }
        }

        private void TrackBar_Main_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Timer_Main_TimeTip_Tick(object sender, EventArgs e)
        {
            if (player.GetState() != Player.EnumPlayState.IsPlay)
                return;
            Label_Main_TimeTip.Text = player._GetPositionString();
            if (!TrackBar_Main_Process_IsMouseDown)
            {
                TrackBar_Main.Maximum = player.GetDuration() / 1000;
                TrackBar_Main.Value = player.GetPosition() / 1000;
            }
        }

        #endregion

        #region 音量控制

        private bool TrackBar_Main_Volume_IsMouseDown = false;

        private void TrackBar_Main_Volume_MouseDown(object sender, MouseEventArgs e)
        {
            TrackBar_Main_Volume_IsMouseDown = true;
        }

        private void TrackBar_Main_Volume_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void TrackBar_Main_Volume_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBar_Main_Volume_IsMouseDown = false;
            player.SetVolume(TrackBar_Main_Volume.Value);
        }

        private void TrackBar_Main_Volume_ValueChanged(object sender, EventArgs e)
        {
            if (TrackBar_Main_Volume_IsMouseDown)
            {
                player.SetVolume(TrackBar_Main_Volume.Value);
                ShowPictureText("音量：" + player.GetVolume() + "%");
            }
        }

        #endregion

        #region UI控制

        private void ControlPanel_Resize(object sender, EventArgs e)
        {
            ControlPanel_ActionButton.Left = (ControlPanel.Width - ControlPanel_ActionButton.Width) / 2 + Picture_Main_PlayOrPause.Width;
        }

        private void Timer_Main_Show_ControlPanel_Tick(object sender, EventArgs e)
        {
            if (ControlPanel.Visible)
                Invoke((EventHandler)delegate
                {
                    ControlPanel.Visible = false;
                });
            Timer_Main_Show_ControlPanel.Enabled = false;
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel_Config_Save_Command.Visible = TabControlMain.SelectedTab == TabPageConfig ? true : false;
            if (Toggle_Config_UI_AutoPause.Checked && TabControlMain.SelectedTab != TabPageMain && player.GetState() == Player.EnumPlayState.IsPlay)
                player.Pause();
        }

        #region Main控制面板按钮

        #region Main控制面板按钮图片

        Image _Image_Main_Play_Normal;
        Image _Image_Main_Pause_Normal;
        Image _Image_Main_Last_Normal;
        Image _Image_Main_Next_Normal;
        Image _Image_Main_Stop_Normal;
        Image _Image_Main_Volume_Normal;
        Image _Image_Main_Silent_Normal;
        Image _Image_Main_Subtilte_Normal;
        Image _Image_Main_Play_List_Normal;

        Image _Image_Main_Play_MouseOn;
        Image _Image_Main_Pause_MouseOn;
        Image _Image_Main_Last_MouseOn;
        Image _Image_Main_Next_MouseOn;
        Image _Image_Main_Stop_MouseOn;
        Image _Image_Main_Volume_MouseOn;
        Image _Image_Main_Silent_MouseOn;
        Image _Image_Main_Subtilte_MouseOn;
        Image _Image_Main_Play_List_MouseOn;

        Image _Image_Main_Play_MouseDown;
        Image _Image_Main_Pause_MouseDown;
        Image _Image_Main_Last_MouseDown;
        Image _Image_Main_Next_MouseDown;
        Image _Image_Main_Stop_MouseDown;
        Image _Image_Main_Volume_MouseDown;
        Image _Image_Main_Silent_MouseDown;
        Image _Image_Main_Subtilte_MouseDown;
        Image _Image_Main_Play_List_MouseDown;

        #endregion

        private void MakeButtonImage(Color NormalColor, Color MouseOnColor, Color MouseDownColor)
        {
            _Image_Main_Play_Normal = BWPic(Resources.ic_play_arrow_black_48dp, Color.Black, NormalColor);
            _Image_Main_Pause_Normal = BWPic(Resources.ic_pause_black_48dp, Color.Black, NormalColor);
            _Image_Main_Last_Normal = BWPic(Resources.ic_skip_previous_black_48dp, Color.Black, NormalColor);
            _Image_Main_Next_Normal = BWPic(Resources.ic_skip_next_black_48dp, Color.Black, NormalColor);
            _Image_Main_Stop_Normal = BWPic(Resources.ic_stop_black_48dp, Color.Black, NormalColor);
            _Image_Main_Volume_Normal = BWPic(Resources.ic_volume_down_black_24dp, Color.Black, NormalColor);
            _Image_Main_Silent_Normal = BWPic(Resources.ic_volume_off_black_24dp, Color.Black, NormalColor);
            _Image_Main_Subtilte_Normal = BWPic(new Bitmap(ReduceImage(Resources.ic_translate_black_48dp, 30, 30)), Color.Black, NormalColor);
            _Image_Main_Play_List_Normal = BWPic(new Bitmap(ReduceImage(Resources.ic_my_library_books_black_48dp, 30, 30)), Color.Black, NormalColor);

            _Image_Main_Play_MouseOn = BWPic(Resources.ic_play_arrow_black_48dp, Color.Black, MouseOnColor);
            _Image_Main_Pause_MouseOn = BWPic(Resources.ic_pause_black_48dp, Color.Black, MouseOnColor);
            _Image_Main_Last_MouseOn = BWPic(Resources.ic_skip_previous_black_48dp, Color.Black, MouseOnColor);
            _Image_Main_Next_MouseOn = BWPic(Resources.ic_skip_next_black_48dp, Color.Black, MouseOnColor);
            _Image_Main_Stop_MouseOn = BWPic(Resources.ic_stop_black_48dp, Color.Black, MouseOnColor);
            _Image_Main_Volume_MouseOn = BWPic(Resources.ic_volume_down_black_24dp, Color.Black, MouseOnColor);
            _Image_Main_Silent_MouseOn = BWPic(Resources.ic_volume_off_black_24dp, Color.Black, MouseOnColor);
            _Image_Main_Subtilte_MouseOn = BWPic(new Bitmap(ReduceImage(Resources.ic_translate_black_48dp, 30, 30)), Color.Black, MouseOnColor);
            _Image_Main_Play_List_MouseOn = BWPic(new Bitmap(ReduceImage(Resources.ic_my_library_books_black_48dp, 30, 30)), Color.Black, MouseOnColor);

            _Image_Main_Play_MouseDown = BWPic(Resources.ic_play_arrow_black_48dp, Color.Black, MouseDownColor);
            _Image_Main_Pause_MouseDown = BWPic(Resources.ic_pause_black_48dp, Color.Black, MouseDownColor);
            _Image_Main_Last_MouseDown = BWPic(Resources.ic_skip_previous_black_48dp, Color.Black, MouseDownColor);
            _Image_Main_Next_MouseDown = BWPic(Resources.ic_skip_next_black_48dp, Color.Black, MouseDownColor);
            _Image_Main_Stop_MouseDown = BWPic(Resources.ic_stop_black_48dp, Color.Black, MouseDownColor);
            _Image_Main_Volume_MouseDown = BWPic(Resources.ic_volume_down_black_24dp, Color.Black, MouseDownColor);
            _Image_Main_Silent_MouseDown = BWPic(Resources.ic_volume_off_black_24dp, Color.Black, MouseDownColor);
            _Image_Main_Subtilte_MouseDown = BWPic(new Bitmap(ReduceImage(Resources.ic_translate_black_48dp, 30, 30)), Color.Black, MouseDownColor);
            _Image_Main_Play_List_MouseDown = BWPic(new Bitmap(ReduceImage(Resources.ic_my_library_books_black_48dp, 30, 30)), Color.Black, MouseDownColor);

            Picture_Main_PlayOrPause.Image = _Image_Main_Play_Normal;
            Picture_Main_Last.Image = _Image_Main_Last_Normal;
            Picture_Main_Next.Image = _Image_Main_Next_Normal;
            Picture_Main_Stop.Image = _Image_Main_Stop_Normal;
            Picture_Main_Volume.Image = _Image_Main_Volume_Normal;
            Picture_Main_Subtilte.Image = _Image_Main_Subtilte_Normal;
            Picture_Main_Play_List.Image = _Image_Main_Play_List_Normal;
        }

        #region 播放按钮

        private void Picture_Main_PlayOrPause_MouseEnter(object sender, EventArgs e)
        {
            if (Picture_Main_PlayOrPause_IsPlay)
                Picture_Main_PlayOrPause.Image = _Image_Main_Pause_MouseOn;
            else
                Picture_Main_PlayOrPause.Image = _Image_Main_Play_MouseOn;
        }

        private void Picture_Main_PlayOrPause_MouseDown(object sender, MouseEventArgs e)
        {
            if (Picture_Main_PlayOrPause_IsPlay)
                Picture_Main_PlayOrPause.Image = _Image_Main_Pause_MouseDown;
            else
                Picture_Main_PlayOrPause.Image = _Image_Main_Play_MouseDown;
        }

        private void Picture_Main_PlayOrPause_MouseLeave(object sender, EventArgs e)
        {
            if (Picture_Main_PlayOrPause_IsPlay)
                Picture_Main_PlayOrPause.Image = _Image_Main_Pause_Normal;
            else
                Picture_Main_PlayOrPause.Image = _Image_Main_Play_Normal;
        }

        private void Picture_Main_PlayOrPause_MouseUp(object sender, MouseEventArgs e)
        {
            if (Picture_Main_PlayOrPause_IsPlay)
                Picture_Main_PlayOrPause.Image = _Image_Main_Pause_MouseOn;
            else
                Picture_Main_PlayOrPause.Image = _Image_Main_Play_MouseOn;
        }

        #endregion

        #region 上一个按钮

        private void Picture_Main_Last_MouseDown(object sender, MouseEventArgs e)
        {
            Picture_Main_Last.Image = _Image_Main_Last_MouseDown;
        }

        private void Picture_Main_Last_MouseEnter(object sender, EventArgs e)
        {
            Picture_Main_Last.Image = _Image_Main_Last_MouseOn;
        }

        private void Picture_Main_Last_MouseLeave(object sender, EventArgs e)
        {
            Picture_Main_Last.Image = _Image_Main_Last_Normal;
        }

        private void Picture_Main_Last_MouseUp(object sender, MouseEventArgs e)
        {
            Picture_Main_Last.Image = _Image_Main_Last_MouseOn;
        }

        #endregion

        #region 下一个按钮

        private void Picture_Main_Next_MouseDown(object sender, MouseEventArgs e)
        {
            Picture_Main_Next.Image = _Image_Main_Next_MouseDown;
        }

        private void Picture_Main_Next_MouseEnter(object sender, EventArgs e)
        {
            Picture_Main_Next.Image = _Image_Main_Next_MouseOn;
        }

        private void Picture_Main_Next_MouseLeave(object sender, EventArgs e)
        {
            Picture_Main_Next.Image = _Image_Main_Next_Normal;
        }

        private void Picture_Main_Next_MouseUp(object sender, MouseEventArgs e)
        {
            Picture_Main_Next.Image = _Image_Main_Next_MouseOn;
        }

        #endregion

        #region 停止按钮

        private void Picture_Main_Stop_MouseDown(object sender, MouseEventArgs e)
        {
            Picture_Main_Stop.Image = _Image_Main_Stop_MouseDown;
        }

        private void Picture_Main_Stop_MouseEnter(object sender, EventArgs e)
        {
            Picture_Main_Stop.Image = _Image_Main_Stop_MouseOn;
        }

        private void Picture_Main_Stop_MouseLeave(object sender, EventArgs e)
        {
            Picture_Main_Stop.Image = _Image_Main_Stop_Normal;
        }

        private void Picture_Main_Stop_MouseUp(object sender, MouseEventArgs e)
        {
            Picture_Main_Stop.Image = _Image_Main_Stop_MouseOn;
        }

        #endregion

        #region 音量按钮

        private void Picture_Main_Volume_MouseDown(object sender, MouseEventArgs e)
        {
            if (Picture_Main_Volume_IsSilent)
                Picture_Main_Volume.Image = _Image_Main_Silent_MouseDown;
            else
                Picture_Main_Volume.Image = _Image_Main_Volume_MouseDown;
        }

        private void Picture_Main_Volume_MouseEnter(object sender, EventArgs e)
        {
            if (Picture_Main_Volume_IsSilent)
                Picture_Main_Volume.Image = _Image_Main_Silent_MouseOn;
            else
                Picture_Main_Volume.Image = _Image_Main_Volume_MouseOn;
        }

        private void Picture_Main_Volume_MouseLeave(object sender, EventArgs e)
        {
            if (Picture_Main_Volume_IsSilent)
                Picture_Main_Volume.Image = _Image_Main_Silent_Normal;
            else
                Picture_Main_Volume.Image = _Image_Main_Volume_Normal;
        }

        private void Picture_Main_Volume_MouseUp(object sender, MouseEventArgs e)
        {
            if (Picture_Main_Volume_IsSilent)
                Picture_Main_Volume.Image = _Image_Main_Silent_MouseOn;
            else
                Picture_Main_Volume.Image = _Image_Main_Volume_MouseOn;
        }

        #endregion

        #region 字幕按钮

        private void Picture_Main_Subtilte_MouseDown(object sender, MouseEventArgs e)
        {
            Picture_Main_Subtilte.Image = _Image_Main_Subtilte_MouseDown;
        }

        private void Picture_Main_Subtilte_MouseEnter(object sender, EventArgs e)
        {
            Picture_Main_Subtilte.Image = _Image_Main_Subtilte_MouseOn;
        }

        private void Picture_Main_Subtilte_MouseLeave(object sender, EventArgs e)
        {
            Picture_Main_Subtilte.Image = _Image_Main_Subtilte_Normal;
        }

        private void Picture_Main_Subtilte_MouseUp(object sender, MouseEventArgs e)
        {
            Picture_Main_Subtilte.Image = _Image_Main_Subtilte_MouseOn;
        }

        #endregion

        #region 播放列表按钮

        private void Picture_Main_Play_List_MouseLeave(object sender, EventArgs e)
        {
            Picture_Main_Play_List.Image = _Image_Main_Play_List_Normal;
        }

        private void Picture_Main_Play_List_MouseUp(object sender, MouseEventArgs e)
        {
            Picture_Main_Play_List.Image = _Image_Main_Play_List_MouseOn;
        }

        private void Picture_Main_Play_List_MouseDown(object sender, MouseEventArgs e)
        {
            Picture_Main_Play_List.Image = _Image_Main_Play_List_MouseDown;
        }

        private void Picture_Main_Play_List_MouseEnter(object sender, EventArgs e)
        {
            Picture_Main_Play_List.Image = _Image_Main_Play_List_MouseOn;
        }

        #endregion

        #endregion

        #region 全屏控制

        private bool IsFullScreen = false;

        private void Action_FullScreen()
        {
            if (IsFullScreen)   //退出全屏状态
            {
                Timer_Main_Show_ControlPanel.Enabled = false;
                WindowState = FormWindowState.Normal;
                TabControlMain.Location = new Point(4, 5);
                TabControlMain.Size = new Size(Width - 8, Height - 9);
                ControlPanel.Left = 0;
                ControlPanel.Top = TabControlMain.Height - ControlPanel.Height - 40;
                Label_Main_TrackBar_Tip.Visible = false;
                Label_Main_TrackBar_Tip.Top = ControlPanel.Top - Label_Main_TrackBar_Tip.Height + 5;
                ControlPanel.Visible = true;
                IsFullScreen = false;
                ShowPictureText("退出全屏");
            }
            else                //进入全屏状态 
            {
                WindowState = FormWindowState.Maximized;
                TabControlMain.Location = new Point(-10, -46);
                TabControlMain.Size = new Size(Width + 14, Height + 126);
                ControlPanel.Left = 6;
                ControlPanel.Top = TabControlMain.Height - ControlPanel.Height - 115;
                Label_Main_TrackBar_Tip.Visible = false;
                Label_Main_TrackBar_Tip.Top = ControlPanel.Top - Label_Main_TrackBar_Tip.Height + 5;
                ControlPanel.Visible = true;
                IsFullScreen = true;
                Timer_Main_Show_ControlPanel.Enabled = true;
                ShowPictureText("进入全屏");
            }
        }

        #endregion

        #endregion

        #region 字幕

        private List<string> SubtitleSupptorList = new List<string>();

        private void LoadSupptorList(string config)
        {
            if (!config.EndsWith(";"))
                config += ";";
            Regex regResult = new Regex("(.+?);");
            MatchCollection mcResult = regResult.Matches(config);
            foreach (Match mc in mcResult)
                SubtitleSupptorList.Add(mc.Groups[1].Value);
        }

        #endregion

        #region 字幕设置子窗体

        private void LoadSubtitleForm()
        {
            fs = new FormSubtilte();
            fs.Show();
            fs.MetroStyleManager.Theme = MetroStyleManager.Theme == MetroThemeStyle.Dark ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
            fs.MetroStyleManager.Style = MetroStyleManager.Style;
            fs._C_ChangeSubtitleEnable += Fs__C_ChangeSubtitleEnable;
            fs._C_ChangeSubtitleSize += Fs__C_ChangeSubtitleSize;
            fs._C_ChangeSubtitleTop += Fs__C_ChangeSubtitleTop;
            fs._C_SearchSubtitle += Fs__C_SearchSubtitle;
            fs.Visible = false;
        }

        private void Fs__C_SearchSubtitle()
        {
            SearchSubtitle();
        }

        private void Fs__C_ChangeSubtitleTop(int Top, int Left)
        {
            player.SetSubtitlePlacement(true, Left, Top);
            ShowPictureText("字幕位置：  垂直 " + Top + "%  水平 " + Left + "%");
        }

        private void Fs__C_ChangeSubtitleSize(int Size)
        {
            player.SetSubtitleFont("", Size, Color.White, 0);
            ShowPictureText("字幕字号：" + Size.ToString());
        }

        private void Fs__C_ChangeSubtitleEnable(bool IsEnable)
        {
            player.SetSubtitleShow(IsEnable);
            ShowPictureText("字幕" + (IsEnable ? "开启" : "关闭"));
        }

        #endregion

        #region 自定义方法

        private Image ReduceImage(Image originalImage, int toWidth, int toHeight)
        {
            if (toWidth <= 0 && toHeight <= 0)
            {
                return originalImage;
            }
            else if (toWidth > 0 && toHeight > 0)
            {
                if (originalImage.Width < toWidth && originalImage.Height < toHeight)
                    return originalImage;
            }
            else if (toWidth <= 0 && toHeight > 0)
            {
                if (originalImage.Height < toHeight)
                    return originalImage;
                toWidth = originalImage.Width * toHeight / originalImage.Height;
            }
            else if (toHeight <= 0 && toWidth > 0)
            {
                if (originalImage.Width < toWidth)
                    return originalImage;
                toHeight = originalImage.Height * toWidth / originalImage.Width;
            }
            Image toBitmap = new Bitmap(toWidth, toHeight);
            using (Graphics g = Graphics.FromImage(toBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(originalImage,
                            new Rectangle(0, 0, toWidth, toHeight),
                            new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                            GraphicsUnit.Pixel);
                originalImage.Dispose();
                return toBitmap;
            }
        }

        /// <summary>
        /// 将mybm图像中的KeyColor颜色转换为ToColor颜色，阈值threshold
        /// </summary>
        /// <param name="mybm">目标图像</param>
        /// <param name="KeyColor">需转换的颜色</param>
        /// <param name="ToColor">转换后的颜色</param>
        /// <param name="threshold">阈值</param>
        /// <returns></returns>
        private Bitmap BWPic(Bitmap mybm, Color KeyColor, Color ToColor, int threshold = 200)
        {
            Bitmap bm = new Bitmap(mybm.Width, mybm.Height);
            try
            {
                int x, y;
                Color pixelColor, result;
                for (x = 0; x < bm.Width; x++)
                {
                    for (y = 0; y < bm.Height; y++)
                    {
                        pixelColor = mybm.GetPixel(x, y);
                        result = pixelColor;
                        if (result.A < KeyColor.A + threshold && result.A > KeyColor.A - threshold)
                            if (result.R < KeyColor.R + threshold && result.R > KeyColor.R - threshold)
                                if (result.G < KeyColor.G + threshold && result.G > KeyColor.G - threshold)
                                    if (result.B < KeyColor.B + threshold && result.B > KeyColor.B - threshold)
                                        result = Color.FromArgb(result.A, ToColor.R, ToColor.G, ToColor.B);
                        bm.SetPixel(x, y, result);
                    }
                }
            }
            catch { }
            return bm;
        }

        long GetCurrentSpeedByKB_Old_Size = 0;
        long GetCurrentSpeedByKB_Current_Size = 0;
        long GetCurrentSpeedByKB_CurrentSpeedByKB = 0;

        /// <summary>
        /// 获取当前下载速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerCurrentSpeed_Tick(object sender, EventArgs e)
        {
            GetCurrentSpeedByKB_Current_Size = player.GetReadSize();
            if (GetCurrentSpeedByKB_Current_Size - GetCurrentSpeedByKB_Old_Size == 0)
                GetCurrentSpeedByKB_Old_Size = 0;
            else
                GetCurrentSpeedByKB_CurrentSpeedByKB = (long)((GetCurrentSpeedByKB_Current_Size - GetCurrentSpeedByKB_Old_Size) / 1024 / ((double)TimerCurrentSpeed.Interval / 1000));
            if (GetCurrentSpeedByKB_CurrentSpeedByKB < 0)
                GetCurrentSpeedByKB_CurrentSpeedByKB = 0;
            GetCurrentSpeedByKB_Old_Size = GetCurrentSpeedByKB_Current_Size;
            if (Label_Main_Download_CurrentSpeed.Visible)
                Label_Main_Download_CurrentSpeed.Text = "预加载中 - " + GetCurrentSpeedByKB_CurrentSpeedByKB.ToString() + " KB/s";
        }

        private double GetDownloadPercent(string str)
        {
            long length = str.Length;
            MatchCollection matches = Regex.Matches(str, @"1");
            long completed = matches.Count;
            return (double)completed / (double)length;
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        private void MessageBoxFX_Show(string content, string title = "")
        {
            MessageBoxFX mb = new MessageBoxFX(content, title, MetroColorStyle.Red,
                                               MetroStyleManager.Theme == MetroThemeStyle.Dark ? MetroThemeStyle.Light : MetroThemeStyle.Dark);
            mb.ShowDialog();
            mb.Dispose();
        }

        private void PlayerOpen(string url, string file_name = "", bool ShowLoadScreen = true, bool Auto_Change_Tab = true, bool IsLocal = true, string cookie = "", string user_agent = "")
        {
            if (Auto_Change_Tab && TabControlMain.SelectedTab != TabPageMain)
                TabControlMain.SelectedTab = TabPageMain;
            if (ShowLoadScreen)
            {
                if (IsLocal)
                    Label_Main_Download_CurrentSpeed.Text = @"打开中";
                else
                    Label_Main_Download_CurrentSpeed.Text = @"预加载中 - 0 KB/s";
                Panel_Main_Download.Visible = true;
            }
            if (IsLocal && url.EndsWith(".ori"))
            {
                cookie = url.Substring(url.LastIndexOf("^") + 1, url.LastIndexOf(".ori") - url.LastIndexOf("^") - 1);
                player.SetConfig(1108, "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
                player.SetConfig(1105, "FTN5K=" + cookie);
            }
            if (string.IsNullOrEmpty(file_name))
            {
                if (IsLocal)
                    file_name = url.Substring(url.LastIndexOf("\\") + 1);
                else
                    file_name = url.Substring(url.LastIndexOf("/") + 1);
            }
            if (IsLocal && string.IsNullOrEmpty(user_agent))
                user_agent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            if (!string.IsNullOrEmpty(cookie))
                player.SetConfig(1105, cookie);
            if (!string.IsNullOrEmpty(user_agent))
                player.SetConfig(1108, user_agent);
            string CacheFileName = string.Empty;
            if (Cache_Enabled)
            {
                CacheFileName = Text_Config_Download_Cache_Path.Text + "//" + file_name + "^" + cookie + ".ori";
                player.SetConfig(2201, CacheFileName);
            }
            if (string.IsNullOrEmpty(file_name))
                file_name = PlayList_CurrentMediaName;
            player.Open(url);
            PlayList_Add(url, file_name, IsLocal ? 1 : 0, CacheFileName, cookie);
        }

        #endregion

        #region 搜索界面

        private void Search_DEBUG()
        {
            string hash = Text_Search_SearchBox.Text;
            WebClient web = new WebClient();
            string postString = "torrent_para={\"uin\":\"`\",\"hash\":\"" + "info_hash" + "\",\"taskname\":\"M\",\"data\":[{\"index\":\"" + "item.index" + "\",\"filesize\":\"1\",\"filename\":\"M.mkv\"}]}";
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            string url = "http://fenxiang.qq.com/upload/index.php/upload_c/checkExist";//地址
            web.Headers.Add("Referer", "http://fenxiang.qq.com/upload/index.php/upload_c/checkExist");
            web.Headers.Add("Accept", "*/*");
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
            MessageBox.Show(this, srcString);
        }

        private void Button_Search_Search_Click(object sender, EventArgs e)
        {
            if (RadioButton_Search_DEBUG.Checked)
            {
                Search_DEBUG();
                return;
            }
            if (string.IsNullOrEmpty(Text_Search_SearchBox.Text))
            {
                MessageBoxFX_Show("搜索内容不能为空!", "提示");
                return;
            }
            if (Text_Search_SearchBox.Text.EndsWith("mkv") || Text_Search_SearchBox.Text.EndsWith("avi") || Text_Search_SearchBox.Text.EndsWith("mp4"))
            {
                PlayerOpen(Text_Search_SearchBox.Text, "", true, true, false, "", "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
                return;
            }
            /*
            if (Text_Search_SearchBox.Text.StartsWith("http://"))
            {
                player.Open(Text_Search_SearchBox.Text);
                return;
            }
            */
            //如果已启动停止线程
            if (Thread_Action_Search != null)
            {
                ProgressSpinner_Search.Visible = false;
                Thread_Action_Search.Abort();
                Thread_Action_Search = null;
                return;
            }
            Search_Result_PageIndex = 1;
            Button_Search_Result_Last_Page.Enabled = false;
            Button_Search_Result_Next_Page.Enabled = true;
            Search_KeyWord = Text_Search_SearchBox.Text;
            ProgressSpinner_Search.Visible = true;
            Load_Thread_Action_Search();
        }

        private void Text_Search_SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Button_Search_Search_Click(null, null);
        }

        private void RadioButton_Search_BT_SOSO_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Search_BT_SOSO.Checked)
            {
                SearchMode = EnumSearchMode.BT_SOSO_Search;
                Text_Search_SearchBox.WaterMarkText = " 输入关键词进行种子搜索 [ 搜索内容源为 www.xhub.cn，请勿用于非法用途 ] ";
                Text_Search_SearchBox.BaseTextBoxLostFocus(null, null);
            }
        }

        private void RadioButton_Search_US_Video_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Search_US_Video.Checked)
                SearchMode = EnumSearchMode.US_Video_Search;
        }

        private void RadioButton_Search_BT_TianTang_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Search_BT_TianTang.Checked)
                SearchMode = EnumSearchMode.BT_TianTang;
        }

        private void RadioButton_Search_CI_LI_BA_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Search_CI_LI_BA.Checked)
            {
                SearchMode = EnumSearchMode.CI_LI_BA_Search;
                Text_Search_SearchBox.WaterMarkText = " 输入关键词进行种子搜索 [ 搜索内容源为 www.cili8.org，请勿用于非法用途 ] ";
                Text_Search_SearchBox.BaseTextBoxLostFocus(null, null);
            }
        }

        private void RadioButton_Search_juhe_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Search_juhe.Checked)
            {
                SearchMode = EnumSearchMode.JuHe;
                Text_Search_SearchBox.WaterMarkText = " 输入关键词进行视频搜索 [ 搜索内容源为优酷等国内主流视频站，请勿用于非法用途 ] ";
                Text_Search_SearchBox.BaseTextBoxLostFocus(null, null);
            }
        }

        private void Button_Search_Result_Last_Page_Click(object sender, EventArgs e)
        {
            if (Search_Result_PageIndex <= 1)
                return;
            else if (Search_Result_PageIndex <= 2)
                Button_Search_Result_Last_Page.Enabled = false;
            Search_Result_PageIndex--;
            Button_Search_Result_Next_Page.Enabled = true;
            Load_Thread_Action_Search();
        }

        private void Button_Search_Result_Next_Page_Click(object sender, EventArgs e)
        {
            Search_Result_PageIndex++;
            Button_Search_Result_Last_Page.Enabled = true;
            Load_Thread_Action_Search();
        }

        private void Button_Search_Result_Back_Click(object sender, EventArgs e)
        {
            if (DataGridView_Search_Result_Info.Visible != true)
            {
                if (Thread_Action_Search != null)
                {
                    ProgressSpinner_Search.Visible = false;
                    Thread_Action_Search.Abort();
                    Thread_Action_Search = null;
                }
                ProgressSpinner_Search.Visible = false;
                ProgressSpinner_Search_Result.Visible = false;
                Panel_Search_Result.Visible = false;
                List_BT_Search.Clear();
                DataGridView_Search_Result.DataSource = new List<Struct_BT_Search>();
                Panel_Search_Panel.Visible = true;
                GC.Collect();
            }
            else
            {
                if (Thread_Action_SourceInfo != null)
                {
                    ProgressSpinner_Search.Visible = false;
                    Thread_Action_SourceInfo.Abort();
                    Thread_Action_SourceInfo = null;
                }
                LoadSearchResult();
                GC.Collect();
            }
        }

        private void DataGridView_Search_Result_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DataGridView_Search_Result.SelectedRows.Count == 0)
            {
                MessageBoxFX_Show("请选择一项后操作！");
                return;
            }
            PlayList_CurrentMediaName = (DataGridView_Search_Result.SelectedRows[0].DataBoundItem as Struct_BT_Search).Name;
            Load_Thread_Action_SourceInfo((DataGridView_Search_Result.SelectedRows[0].DataBoundItem as Struct_BT_Search).ToUrl);
        }

        private void LoadSearchResult()
        {
            MetroLabel_Search_Result_Page.Visible = true;
            Button_Search_Result_Last_Page.Visible = true;
            Button_Search_Result_Next_Page.Visible = true;
            ProgressSpinner_Search_Result.Location = new Point(363, Button_Search_Result_Next_Page.Top);

            DataGridView_Search_Result_Info.Visible = false;
        }

        private void LoadSourceInfo()
        {
            MetroLabel_Search_Result_Page.Visible = false;
            Button_Search_Result_Last_Page.Visible = false;
            Button_Search_Result_Next_Page.Visible = false;
            ProgressSpinner_Search_Result.Location = new Point(3, Button_Search_Result_Next_Page.Top);

            DataGridView_Search_Result_Info.Visible = true;
        }

        private void DataGridView_Search_Result_Info_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DataGridView_Search_Result_Info.SelectedRows.Count == 0)
            {
                MessageBoxFX_Show("请选择一项后操作！");
                return;
            }
            Load_Thread_Action_SourceInfo_GetHTTP(e.RowIndex);
        }

        #endregion

        #region 搜索方法

        /// <summary>
        /// 搜索方式枚举
        /// </summary>
        public enum EnumSearchMode
        {
            BT_SOSO_Search = 1,     //BT-SOSO
            CI_LI_BA_Search = 2,    //磁力吧
            US_Video_Search = 3,    //美剧搜索
            BT_TianTang = 4,        //BT天堂
            JuHe = 5                //聚合搜索
        }

        /// <summary>
        /// 搜索方式
        /// </summary>
        EnumSearchMode SearchMode = EnumSearchMode.BT_SOSO_Search;

        private List<Struct_BT_Search> List_BT_Search = new List<Struct_BT_Search>();

        /// <summary>
        /// 搜索线程
        /// </summary>
        private Thread Thread_Action_Search;

        private string Search_KeyWord;
        private int Search_Result_PageIndex;

        private void Load_Thread_Action_Search()
        {
            //如果已启动停止线程
            if (Thread_Action_Search != null)
            {
                ProgressSpinner_Search.Visible = false;
                Thread_Action_Search.Abort();
                Thread_Action_Search = null;
            }
            if (Thread_Action_Search != null)
                return;
            //启新线程
            Thread_Action_Search = new Thread(new ThreadStart(Action_Search));
            Thread_Action_Search.SetApartmentState(System.Threading.ApartmentState.STA);
            Thread_Action_Search.Start();
        }

        private void Action_Search()
        {
            try
            {
                Invoke((EventHandler)delegate
                {
                    List_BT_Search.Clear();
                    DataGridView_Search_Result.DataSource = new List<Struct_BT_Search>();
                    MetroLabel_Search_Result_Page.Text = "当前页数：" + Search_Result_PageIndex.ToString();
                    if (Search_Result_PageIndex <= 1)
                        ProgressSpinner_Search.Visible = true;
                    else
                        ProgressSpinner_Search_Result.Visible = true;
                    if (Column_Search_Result_Address.Visible)
                        Column_Search_Result_Address.Visible = false;
                    if (Column_Search_Result_Source.Visible)
                        Column_Search_Result_Source.Visible = false;
                });
                if (SearchMode == EnumSearchMode.BT_SOSO_Search)
                {
                    Search_Engine_BT_SOSO search = new Search_Engine_BT_SOSO();
                    List_BT_Search = search.Get_Search_BT_SOSO(Search_KeyWord, Search_Result_PageIndex);
                }
                else if (SearchMode == EnumSearchMode.CI_LI_BA_Search)
                {
                    Search_Engine_CI_LI_BA search = new Search_Engine_CI_LI_BA();
                    List_BT_Search = search.Get_Search_CI_LI_BA(Search_KeyWord, Search_Result_PageIndex);
                }
                else if (SearchMode == EnumSearchMode.US_Video_Search)
                {

                }
                else if (SearchMode == EnumSearchMode.BT_TianTang)
                {
                    Search_Engine_BT_TianTang search = new Search_Engine_BT_TianTang();
                    List_BT_Search = search.Get_Search_BT_TianTang(Search_KeyWord, Search_Result_PageIndex);
                }
                else if (SearchMode == EnumSearchMode.JuHe)
                {
                    Search_Engine_360 search = new Search_Engine_360();
                    List_BT_Search = search.Get_Search_360(Search_KeyWord, Search_Result_PageIndex);
                }
                Invoke(new MethodInvoker(delegate
                {
                    if (Search_Result_PageIndex <= 1)       //第一次搜索
                    {
                        DataGridView_Search_Result.DataSource = List_BT_Search;
                        Panel_Search_Panel.Visible = false;
                        Panel_Search_Result.Visible = true;
                    }
                    else                                    //翻页操作
                    {
                        DataGridView_Search_Result.DataSource = List_BT_Search;
                    }
                }));

            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Invoke((EventHandler)delegate
                {
                    if (e.Message == "No Data")
                    {
                        if (Search_Result_PageIndex <= 1)       //第一次搜索
                            MessageBoxFX_Show("没有搜索到相关数据！", "提示");
                        else                                    //翻页操作
                        {
                            Button_Search_Result_Next_Page.Enabled = false;
                            MessageBoxFX_Show("没有搜索到更多数据！\n\n请返回上一页！", "提示");
                        }
                    }
                    else
                        MessageBoxFX_Show("搜索失败\n\n" + e.Message, "错误");
                });
            }
            finally
            {
                Invoke((EventHandler)delegate
                {
                    if (Search_Result_PageIndex <= 1)
                        ProgressSpinner_Search.Visible = false;
                    else
                        ProgressSpinner_Search_Result.Visible = false;
                    Thread_Action_Search = null;
                });
            }
        }

        #endregion

        #region 详细信息检索方法

        private Thread Thread_Action_SourceInfo;

        private void Load_Thread_Action_SourceInfo(string hash)
        {
            //如果已启动停止线程
            if (Thread_Action_SourceInfo != null)
            {
                ProgressSpinner_Search.Visible = false;
                Thread_Action_SourceInfo.Abort();
                Thread_Action_SourceInfo = null;
            }
            if (Thread_Action_SourceInfo != null)
                return;
            //启新线程
            if (SearchMode == EnumSearchMode.JuHe)
                Thread_Action_SourceInfo = new Thread(new ParameterizedThreadStart(Action_SourceInfoBy_flvcd));
            else
                Thread_Action_SourceInfo = new Thread(new ParameterizedThreadStart(Action_SourceInfo));
            Thread_Action_SourceInfo.Start(hash);
        }

        private class StructSourceInfo
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
            /// 资源大小
            /// </summary>
            public string VideoLength { get; set; }
        }

        ThunderHelper.ThunderJSONStuct SourceInfo_JSONStuct = new ThunderHelper.ThunderJSONStuct();

        private void Action_SourceInfo(object obj)
        {
            try
            {
                string hash = (string)obj;
                if (string.IsNullOrEmpty(hash))
                {
                    Invoke((EventHandler)delegate
                    {
                        MessageBoxFX_Show("播放地址无效，请重试！", "错误");
                        return;
                    });
                }
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Search_Result.Visible = true;
                    LoadSourceInfo();
                    DataGridView_Search_Result_Info.DataSource = new List<StructSourceInfo>();
                });
                ThunderHelper th = new ThunderHelper();
                SourceInfo_JSONStuct = th.GetList(hash);
                if (SourceInfo_JSONStuct.resp.record_num <= 0)
                {
                    MessageBoxFX_Show("未找到有效资源，请重试！", "错误");
                    return;
                }
                List<StructSourceInfo> temp = new List<StructSourceInfo>();
                foreach (ThunderHelper.Subfile_List sl in SourceInfo_JSONStuct.resp.subfile_list)
                {
                    StructSourceInfo item = new StructSourceInfo()
                    {
                        ToUrl = sl.url_hash,
                        Name = sl.name.Replace("【失效链接】", "").Replace("【无效链接】", "").Replace("【处理中,请等待】", ""),
                        VideoLength = (sl.file_size / 1024 / 1024).ToString() + "MB"
                    };
                    temp.Add(item);
                }
                Invoke((EventHandler)delegate
                {
                    DataGridView_Search_Result_Info.DataSource = temp;
                });
            }
            catch (NullReferenceException)
            {
                Invoke((EventHandler)delegate
                {
                    MessageBoxFX_Show("未找到有效资源，请重试！", "错误");
                    return;
                });
            }
            catch (Exception e)
            {
                Invoke((EventHandler)delegate
                {
                    MessageBoxFX_Show("程序发生异常，请重试！\n\n" + e.Message, "错误");
                    return;
                });
            }
            finally
            {
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Search_Result.Visible = false;
                });
            }
        }

        private void Action_SourceInfoBy_flvcd(object obj)
        {
            try
            {
                string url = (string)obj;
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Search_Result.Visible = true;
                    if (string.IsNullOrEmpty(url))
                    {
                        MessageBoxFX_Show("播放地址无效，请重试！", "错误");
                        return;
                    }
                });
                Search_Engine_flvcd info = new Search_Engine_flvcd();
                List<string> res = info.GetSrcURL(url);
                if (res.Count <= 0)
                    Invoke((EventHandler)delegate
                    {
                        MessageBoxFX_Show("未找到有效资源，请重试！", "错误");
                        return;
                    });
                Invoke((EventHandler)delegate
                {
                    PlayerOpen(res[0], "", true, true, false);
                    TabControlMain.SelectedTab = TabPageMain;
                    foreach (string i in res)
                    {
                        PlayList_Add(i, PlayList_CurrentMediaName, 0);
                    }
                });
            }
            catch (Exception e)
            {
                Invoke((EventHandler)delegate
                {
                    if (e.Message == "No Data")
                        MessageBoxFX_Show("没有搜索到相关数据！", "提示");
                    else
                        MessageBoxFX_Show("程序发生异常，请重试！\n\n" + e.Message, "错误");
                    return;
                });
            }
            finally
            {
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Search_Result.Visible = false;
                });
            }
        }

        #endregion

        #region 根据HASH获取HTTP播放

        private Thread Thread_Action_SourceInfo_GetHTTP;

        private void Load_Thread_Action_SourceInfo_GetHTTP(int index)
        {
            //如果已启动停止线程
            if (Thread_Action_SourceInfo_GetHTTP != null)
            {
                ProgressSpinner_Search.Visible = false;
                Thread_Action_SourceInfo_GetHTTP.Abort();
                Thread_Action_SourceInfo_GetHTTP = null;
            }
            if (Thread_Action_SourceInfo_GetHTTP != null)
                return;
            //启新线程
            Thread_Action_SourceInfo_GetHTTP = new Thread(new ParameterizedThreadStart(Action_SourceInfo_GetHTTP));
            Thread_Action_SourceInfo_GetHTTP.Start(index);
        }

        private void Action_SourceInfo_GetHTTP(object obj)
        {
            try
            {
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Search_Result.Visible = true;
                });
                int index = (int)obj;
                ThunderHelper.URLData url = th.GetHTTPURL(SourceInfo_JSONStuct.resp.subfile_list[index], SourceInfo_JSONStuct.resp.info_hash);
                /*
                for (int i = 0; i < 10; ++i)
                {
                    if (!string.IsNullOrEmpty(url.url))
                        break;
                    url = th.GetHTTPURL(SourceInfo_JSONStuct.resp.subfile_list[index], SourceInfo_JSONStuct.resp.info_hash);
                }
                */
                if (string.IsNullOrEmpty(url.url))
                {
                    Invoke((EventHandler)delegate
                      {
                          MessageBoxFX_Show("未获取到有效播放地址，请重试", "提示");
                      });
                    ProgressSpinner_Search_Result.Visible = false;
                    return;
                }
                Invoke((EventHandler)delegate
                {
                    PlayerOpen(url.url, SourceInfo_JSONStuct.resp.subfile_list[index].name, true, true, false, "FTN5K=" + url.cookie,
                        "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)");
                });
            }
            catch
            {
            }
            finally
            {
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Search_Result.Visible = false;
                });
            }
        }

        #endregion

        #region 文件拖曳

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in filePath)
            {
                bool IsSubtitle = false;
                foreach (string str in SubtitleSupptorList)
                {
                    if (file.EndsWith(str))
                    {
                        IsSubtitle = true;
                        break;
                    }
                }
                if (IsSubtitle)
                    LoadSubtitle(file);
                else
                    PlayerOpen(file);
            }
            if (TabControlMain.SelectedTab != TabPageMain)
                TabControlMain.SelectedTab = TabPageMain;
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        #endregion

        #region 提示文字

        private void Timer_PictureText_Close_Tick(object sender, EventArgs e)
        {
            Invoke((EventHandler)delegate
            {
                player.SetPictureEnable(false);
                Timer_PictureText_Close.Enabled = false;
            });
        }

        /// <summary>
        /// 显示提示文字
        /// </summary>
        /// <param name="text">需显示的文字</param>
        /// <param name="time">显示持续时间</param>
        public void ShowPictureText(string text, int time = 3500)
        {
            if (!player.GetPictureEnable())
                player.SetPictureEnable(true);
            player.SetPictureLeft(20);
            player.SetPictureTop(20);
            player.SetPictureText(text);
            Timer_PictureText_Close.Enabled = false;
            Timer_PictureText_Close.Interval = time;
            Timer_PictureText_Close.Enabled = true;
        }

        #region 音量调节模块

        /// <summary>
        /// 音量加
        /// </summary>
        /// <param name="step">步进，默认为1</param>
        /// <param name="AutoTip">是否自动显示提示文字，默认显示</param>
        /// <param name="TipHead">提示文字文字头，仅当AutoTip为真时生效</param>
        /// <param name="time">提示文字持续时间，仅当AutoTip为真时生效</param>
        /// <returns>执行操作后的音量值</returns>
        public int VolumeUp(int step = 1, bool AutoTip = true, string TipHead = "音量：", int time = 2000)
        {
            int CurrentVolumeUp = player.GetVolume();
            player.SetVolume(CurrentVolumeUp += step);
            if (AutoTip)
                ShowPictureText(TipHead + CurrentVolumeUp.ToString(), time);
            return CurrentVolumeUp;
        }

        /// <summary>
        /// 音量减
        /// </summary>
        /// <param name="step">步进，默认为1</param>
        /// <param name="AutoTip">是否自动显示提示文字，默认显示</param>
        /// <param name="TipHead">提示文字文字头，仅当AutoTip为真时生效</param>
        /// <param name="time">提示文字持续时间，仅当AutoTip为真时生效</param>
        /// <returns>执行操作后的音量值</returns>
        public int VolumeDown(int step = 1, bool AutoTip = true, string TipHead = "音量：", int time = 2000)
        {
            int CurrentVolumeUp = player.GetVolume();
            player.SetVolume(CurrentVolumeUp -= step);
            if (AutoTip)
                ShowPictureText(TipHead + CurrentVolumeUp.ToString(), time);
            return CurrentVolumeUp;
        }

        #endregion

        #endregion

        #region 设置界面

        #region 主题及颜色

        private void RadioButton_Config_UI_Theme_Dark_CheckedChanged(object sender, EventArgs e)
        {
            MetroStyleManager.Theme = RadioButton_Config_UI_Theme_Dark.Checked ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
        }

        private void RadioButton_Config_UI_Style_Green_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Lime_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Teal_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Orange_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Brown_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Pink_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Magenta_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Purple_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Red_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Yellow_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Black_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_White_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Silver_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void RadioButton_Config_UI_Style_Blue_CheckedChanged(object sender, EventArgs e)
        {
            Config_UI_Style_CheckedChanged();
        }

        private void Config_UI_Style_CheckedChanged()
        {
            MetroStyleManager.Style = RadioButton_Config_UI_Style_Black.Checked ? MetroColorStyle.Black : RadioButton_Config_UI_Style_Blue.Checked ? MetroColorStyle.Blue :
                                      RadioButton_Config_UI_Style_Brown.Checked ? MetroColorStyle.Brown : RadioButton_Config_UI_Style_Green.Checked ? MetroColorStyle.Green :
                                      RadioButton_Config_UI_Style_Lime.Checked ? MetroColorStyle.Lime : RadioButton_Config_UI_Style_Magenta.Checked ? MetroColorStyle.Magenta :
                                      RadioButton_Config_UI_Style_Orange.Checked ? MetroColorStyle.Orange : RadioButton_Config_UI_Style_Pink.Checked ? MetroColorStyle.Pink :
                                      RadioButton_Config_UI_Style_Purple.Checked ? MetroColorStyle.Purple : RadioButton_Config_UI_Style_Red.Checked ? MetroColorStyle.Red :
                                      RadioButton_Config_UI_Style_Silver.Checked ? MetroColorStyle.Silver : RadioButton_Config_UI_Style_Teal.Checked ? MetroColorStyle.Teal :
                                      RadioButton_Config_UI_Style_White.Checked ? MetroColorStyle.White : MetroColorStyle.Yellow;
        }

        #endregion

        private void Button_Config_Save_Click(object sender, EventArgs e)
        {
            PanelConfig.Enabled = false;
            ProgressSpinner_Config_Save.Visible = true;

            StructConfig_root config;

            config.UI.MetroColorStyle = (int)MetroStyleManager.Style;
            config.UI.MetroThemeStyle = (int)MetroStyleManager.Theme;
            config.UI.AutoPause = Toggle_Config_UI_AutoPause.Checked ? 1 : 0;

            config.player.CacheEnable = Toggle_Config_Download_Cache_Enabled.Checked ? 1 : 0;
            config.player.CachePath = Text_Config_Download_Cache_Path.Text;
            config.player.DecodingPath = player.GetDecodingPath();
            config.player.ImageNormalizeEnable = Toggle_Config_Video_Image_Nomallize.Checked ? 1 : 0;
            config.player.RenderMode = (int)player.GetRenderModeConfig();
            config.player.SoundSilent = (int)player.GetSoundSilent();
            config.player.SpeedupEnable = player.GetSpeedupEnable() ? 1 : 0;
            config.player.SubtitleFontColor = _ParseRGB(Color.FromArgb(255, 255, 255));
            config.player.SubtitleFontName = "黑体";
            config.player.SubtitleFontShadow = 1;
            config.player.SubtitleFontSize = 20;
            config.player.SubtitleLeftPercent = 50;
            config.player.SubtitleShow = player.GetSubtitleShow() ? 1 : 0;
            config.player.SubtitleTopPercent = 80;
            config.player.Volume = player.GetVolume();

            config.CheckCode = GetCheckCode();

            SaveConfigFile(MakeConfigFile(config));
            ProgressSpinner_Config_Save.Visible = false;
            PanelConfig.Enabled = true;
            MessageBoxFX_Show("设置已成功保存！");
        }

        private void Button_Config_Download_Cache_Path_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (Directory.Exists(Text_Config_Download_Cache_Path.Text.Trim()))//判断是否存在
                fbd.SelectedPath = Text_Config_Download_Cache_Path.Text.Trim();
            else
            {
                string default_path = Application.StartupPath + "\\download";
                if (!Directory.Exists(default_path))//判断是否存在
                    Directory.CreateDirectory(default_path);
                fbd.SelectedPath = default_path;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Text_Config_Download_Cache_Path.Text = fbd.SelectedPath;
            }
        }

        private void Toggle_Config_Video_SpeedUp_CheckedChanged(object sender, EventArgs e)
        {
            if (Toggle_Config_Video_SpeedUp.Checked)
                player.SetSpeedupEnable(true);
            else
                player.SetSpeedupEnable(false);
        }

        private void RadioButton_Config_Video_Render_Mode_Overlay_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Config_Video_Render_Mode_Overlay.Checked)
                player.SetRenderModeConfig(Player.EnumRenderMode.Overlay);
        }

        private void RadioButton_Config_Video_Render_Mode_EVR_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Config_Video_Render_Mode_EVR.Checked)
                player.SetRenderModeConfig(Player.EnumRenderMode.EVR);
        }

        private void RadioButton_Config_Video_Render_Mode_Renderless_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Config_Video_Render_Mode_Renderless.Checked)
                player.SetRenderModeConfig(Player.EnumRenderMode.Renderless);
        }

        private void RadioButton_Config_Video_Render_Mode_EVRCP_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_Config_Video_Render_Mode_EVRCP.Checked)
                player.SetRenderModeConfig(Player.EnumRenderMode.EVRCP);
        }

        private void Toggle_Config_Video_Image_Nomallize_CheckedChanged(object sender, EventArgs e)
        {
            if (Toggle_Config_Video_Image_Nomallize.Checked)
                player.SetConfig(305, "1");
            else
                player.SetConfig(305, "0");
        }

        private bool Cache_Enabled;

        private void Toggle_Config_Download_Cache_Enabled_CheckedChanged(object sender, EventArgs e)
        {
            Cache_Enabled = Toggle_Config_Download_Cache_Enabled.Checked;
            player.SetConfig(2201, Cache_Enabled ? Text_Config_Download_Cache_Path.Text + "\\temp.ori" : "");
        }

        private void Text_Config_Download_Cache_Path_TextChanged(object sender, EventArgs e)
        {
            ToolTip.SetToolTip(Text_Config_Download_Cache_Path, Text_Config_Download_Cache_Path.Text);
        }

        private void Button_Config_About_About_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
            about.Dispose();
        }

        private void Button_Config_About_CheckUpdate_Click(object sender, EventArgs e)
        {
            //如果已启动停止线程
            if (Thread_CheckUpdate != null)
            {
                ProgressSpinner_Config_CheckUpdate.Visible = false;
                Thread_CheckUpdate.Abort();
                Thread_CheckUpdate = null;
            }
            if (Thread_CheckUpdate != null)
                return;
            //启新线程
            Thread_CheckUpdate = new Thread(new ThreadStart(Action_CheckUpdate));
            Thread_CheckUpdate.SetApartmentState(System.Threading.ApartmentState.STA);
            Thread_CheckUpdate.Start();
        }

        private Thread Thread_CheckUpdate;

        private void Action_CheckUpdate()
        {
            try
            {
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Config_CheckUpdate.Visible = true;
                });
                Thread.Sleep(2000);
                Invoke((EventHandler)delegate
                {
                    MessageBoxFX_Show("服务器版本： " + version + "\n客户端版本： " + version + "\n\n当前为最新版本，不需要更新。");
                });
            }
            catch
            {

            }
            finally
            {
                Invoke((EventHandler)delegate
                {
                    ProgressSpinner_Config_CheckUpdate.Visible = false;
                });
            }
        }

        #endregion

        #region 字幕模块

        private bool LoadSubtitle(string path)
        {
            player.SetSubtitleFilename(path);
            ShowPictureText("加载字幕 - " + path);
            return true;
        }

        private void SearchSubtitle()
        {
            LoadSearchSubtitleThread();
        }

        private Thread ThreadSearchSubtitle;

        private void LoadSearchSubtitleThread()
        {
            //如果已启动停止线程
            if (ThreadSearchSubtitle != null)
            {
                fs.Subtitle_Main_IsProcess.Visible = false;
                ThreadSearchSubtitle.Abort();
                ThreadSearchSubtitle = null;
            }
            if (ThreadSearchSubtitle != null)
                return;
            //启新线程
            ThreadSearchSubtitle = new Thread(new ThreadStart(SearchSubtitleThread));
            ThreadSearchSubtitle.SetApartmentState(System.Threading.ApartmentState.STA);
            ThreadSearchSubtitle.Start();
        }

        private void SearchSubtitleThread()
        {
            try
            {
                string FileName = string.Empty;
                string Length = string.Empty;
                Invoke((EventHandler)delegate
                {
                    ShowPictureText("正在自动匹配字幕");
                    fs.Subtitle_Main_IsProcess.Visible = true;
                    if (player.GetCurrentURL().StartsWith("http://"))   //网络文件
                    {
                        if (string.IsNullOrEmpty(FileName))
                            FileName = PlayList_CurrentMediaName;
                    }
                    else                                                //本地文件
                    {
                        if (string.IsNullOrEmpty(FileName))
                            FileName = new Regex("[^/\\\\]+$").Match(player.GetCurrentURL()).ToString();
                    }
                    Length = player.GetDuration().ToString();
                });
                ThunderSubtitles TS = new ThunderSubtitles();
                var res = TS.GetSubtitlesStruct(FileName, Length);
                if (res.sublist.Count <= 0)
                {
                    Invoke((EventHandler)delegate
                    {
                        ShowPictureText("没有发现匹配字幕");
                        fs.Subtitle_Main_IsProcess.Visible = false;
                    });
                    return;
                }
                Invoke((EventHandler)delegate
                {
                    fs.ComboBox_Select_Subtitles.DataSource = new List<Struct_ThunderSubtitles>();
                    fs.ComboBox_Select_Subtitles.DataSource = res.sublist;
                    fs.ComboBox_Select_Subtitles.DisplayMember = "sname";
                    fs.ComboBox_Select_Subtitles.ValueMember = "sname";
                });
            }
            catch(Exception e)
            {
                Invoke((EventHandler)delegate
                {
                    ShowPictureText("自动匹配字幕失败 - " + e.Message);
                });
            }
            finally
            {
                Invoke((EventHandler)delegate
                {
                    fs.Subtitle_Main_IsProcess.Visible = false;
                });
            }
        }

        #endregion

        #region 播放列表

        private string PlayList_FilePath = Application.StartupPath + @"\play_list.json";

        private PlayList pl = new PlayList();

        private void PlayList_Load()
        {

            pl.Total = 0;

            if (!File.Exists(PlayList_FilePath))
                PlayList_Save();

            StreamReader sr = new StreamReader(PlayList_FilePath);
            string json = "", temp;
            while ((temp = sr.ReadLine()) != null)
                json += temp;
            sr.Close();

            pl = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<PlayList>(json);
            DataGridView_Main_Play_List.DataSource = pl.Info;

        }

        private void PlayList_Save()
        {
            FileStream fs = new FileStream(PlayList_FilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pl));
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void PlayList_Add(string URL, string Name, int IsLocal = 1, string Path = "", string Cookie = "")
        {
            PlayList.info info = new PlayList.info();
            info.URL = URL;
            info.Name = Name;
            info.Path = Path;
            info.IsLocal = IsLocal;
            info.Cookie = Cookie;
            if (PlaiList_IsRepeat(info.URL))
                return;
            DataGridView_Main_Play_List.DataSource = new List<PlayList.info>();
            pl.Info.Add(info);
            pl.Total = pl.Info.Count;
            DataGridView_Main_Play_List.DataSource = pl.Info;
        }

        private string PlayList_CurrentMediaName = "";

        private void PlayList_AddCurrentMedia()
        {
            PlayList.info info = new PlayList.info();
            info.URL = player.GetCurrentURL();
            if (PlaiList_IsRepeat(info.URL))
                return;
            DataGridView_Main_Play_List.DataSource = new List<PlayList.info>();
            if (info.URL.StartsWith(@"http://"))
            {
                info.Name = PlayList_CurrentMediaName;
                info.IsLocal = 0;
                string cache_path = player.GetConfig(2201);
                string cookie = player.GetConfig(1105);
                if (!string.IsNullOrEmpty(cache_path))
                    info.Path = cache_path;
                if (!string.IsNullOrEmpty(cookie))
                    info.Cookie = cookie;
            }
            else
            {
                info.Name = new Regex("[^/\\\\]+$").Match(info.URL).ToString();
            }
            pl.Info.Add(info);
            pl.Total = pl.Info.Count;
            DataGridView_Main_Play_List.DataSource = pl.Info;
        }

        private bool PlaiList_IsRepeat(string File_Path)
        {
            foreach (PlayList.info Info in pl.Info)
            {
                if (Info.URL == File_Path)
                    return true;
            }
            return false;
        }

        private void PlayList_Open(PlayList.info info)
        {
            if (info.IsLocal == 1)  //本地文件
            {
                PlayerOpen(info.URL, info.Name);
                player.SetPosition(info.LastTime * 1000);
            }
            else                    //网络文件
                if (!string.IsNullOrEmpty(info.Path) && File.Exists(info.Path))         //存在缓存文件
                    PlayerOpen(info.Path, info.Name, true, true, false, info.Cookie, "");
                else                                                                    //不存在缓存文件
                    PlayerOpen(info.URL, info.Name, true, true, false, info.Cookie, "");
        }

        private void PlayList_Delete()
        {

        }

        private void PlayList_Clear()
        {

        }

        private void DataGridView_Main_Play_List_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DataGridView_Main_Play_List.SelectedRows.Count == 0)
            {
                MessageBoxFX_Show("请选择一项后操作！");
                return;
            }
            PlayList_Open(DataGridView_Main_Play_List.SelectedRows[0].DataBoundItem as PlayList.info);
        }

        private void DataGridView_Main_Play_List_DataSourceChanged(object sender, EventArgs e)
        {
            if (DataGridView_Main_Play_List.RowCount > 0)
                PlayList_Save();
        }

        #endregion

    }
}
