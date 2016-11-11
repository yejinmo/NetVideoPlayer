using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxAPlayer3Lib;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Security;
using System.IO;
/*
ConfigID = 
return (int.Parse(base.GetConfig()) == 1 ? true : false);
return ()int.Parse(base.GetConfig());
return int.Parse(base.GetConfig());
return base.GetConfig();
base.SetConfig( , value);
base.SetConfig( , value.ToString());
base.SetConfig( , ((int)value).ToString());
base.SetConfig( , value ? "1" : "0");
*/
namespace NetVideoPlayer
{
    public class Player : AxPlayer
    {
        #region 全局

        #endregion

        #region 播放器自有方法

        /// <summary>
        /// 播放器初始化
        /// </summary>
        public Player()
        {

        }

        /// <summary>
        /// 鼠标事件    
        /// </summary>
        public enum WinMsg
        {
            WM_LBUTTONDOWN = 0x201, //Left mousebutton down
            WM_LBUTTONUP = 0x202,  //Left mousebutton up
            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
            WM_RBUTTONDOWN = 0x204, //Right mousebutton down
            WM_RBUTTONUP = 0x205,   //Right mousebutton up
            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
            WM_KEYDOWN = 0x100,  //Key down
            WM_KEYUP = 0x101   //Key up
        }

        /// <summary>
        /// 打开文件
        /// 用来打开需要播放的媒体文件
        /// </summary>
        /// <param name="url">一个表征媒体文件地址的 Unicode 字符串，该字符串可以是本地、局域网共享或网络文件</param>
        /// <returns></returns>
        new public void Open(string url)
        {
            base.Open(url);
        }

        /// <summary>
        /// 关闭播放
        /// 用来关闭 APlayer 引擎正在播放的媒体文件
        /// </summary>
        /// <returns></returns>
        new public bool Close()
        {
            base.Close();
            return true;
        }

        /// <summary>
        /// 开始播放
        /// 使 APlayer 引擎由暂停状态进入播放状态
        /// </summary>
        /// <returns></returns>
        new public bool Play()
        {
            base.Play();
            return true;
        }

        /// <summary>
        /// 暂停播放
        /// 使 APlayer 引擎由播放状态进入暂停状态
        /// </summary>
        /// <returns></returns>
        new public bool Pause()
        {
            base.Pause();
            return true;
        }

        /// <summary>
        /// 获取 APlayer 引擎的版本号
        /// </summary>
        /// <returns></returns>
        new public string GetVersion()
        {
            return base.GetVersion();
        }

        /// <summary>
        /// 设置 APlayer 引擎视频区域在未播放视频时显示的图片
        /// </summary>
        /// <param name="Logo"></param>
        new public void SetCustomLogo(int Logo)
        {
            base.SetCustomLogo(Logo);
        }

        /// <summary>
        /// 设置 APlayer 引擎视频区域在未播放视频时显示的图片
        /// </summary>
        /// <param name="Logo"></param>
        public void SetCustomLogo(IntPtr Logo)
        {
            base.SetCustomLogo(Logo.ToInt32());
        }

        /// <summary>
        /// 播放器状态枚举
        /// </summary>
        public enum EnumPlayState
        {
            IsError = -1,       //播放器错误
            IsReady = 0,        //播放器准备就绪
            IsOpening = 1,      //播放器正在打开
            IsPauseing = 2,     //播放器正在暂停
            IsPause = 3,        //播放器暂停状态
            IsPlaying = 4,      //播放器正在开始播放
            IsPlay = 5,         //播放器正在播放
            IsStoping = 6       //播放器正在停止
        }

        /// <summary>
        /// 获取播放器状态
        /// </summary>
        /// <returns>播放器状态</returns>
        new public EnumPlayState GetState()
        {
            try
            {
                return (EnumPlayState)(base.GetState());
            }
            catch
            {
                return EnumPlayState.IsError;
            }
        }

        /// <summary>
        /// 获取当前播放媒体文件的持续时长
        /// </summary>
        /// <returns></returns>
        new public int GetDuration()
        {
            return base.GetDuration();
        }

        /// <summary>
        /// 获取当前播放媒体文件的播放进度
        /// </summary>
        /// <returns></returns>
        new public int GetPosition()
        {
            return base.GetPosition();
        }

        /// <summary>
        /// 设置当前播放媒体文件的播放位置
        /// </summary>
        /// <param name="Position">需要设置的播放位置值，单位毫秒</param>
        /// <returns></returns>
        new public bool SetPosition(int Position)
        {
            return base.SetPosition(Position) == 1 ? true : false;
        }

        /// <summary>
        /// 获取当前播放媒体文件的视频宽度
        /// 单位像素
        /// </summary>
        /// <returns></returns>
        new public int GetVideoWidth()
        {
            return base.GetVideoWidth();
        }

        /// <summary>
        /// 获取当前播放媒体文件的视频高度
        /// 单位像素
        /// </summary>
        /// <returns></returns>
        new public int GetVideoHeight()
        {
            return base.GetVideoHeight();
        }

        /// <summary>
        /// 获取当前播放音量
        /// </summary>
        /// <returns>音量</returns>
        new public int GetVolume()
        {
            return base.GetVolume();
        }

        /// <summary>
        /// 设置当前播放音量
        /// 100 时就是正常的满音量
        /// 取值范围为 0 - 1000
        /// </summary>
        /// <param name="volume"></param>
        /// <returns>音量</returns>
        new public bool SetVolume(int volume)
        {
            base.SetVolume(volume);
            return true;
        }

        /// <summary>
        /// 获取 APlayer 引擎当前是否处于设置播放进度(Seek)过程中
        /// </summary>
        /// <returns></returns>
        new public bool IsSeeking()
        {
            return (base.IsSeeking() == 1 ? true : false);
        }

        /// <summary>
        /// 获取 APlayer 引擎播放网络文件时的数据缓冲进度
        /// -1      不在缓冲过程中
        /// 0-99    缓冲进度
        /// </summary>
        /// <returns></returns>
        new public int GetBufferProgress()
        {
            return base.GetBufferProgress();
        }

        #endregion

        #region 播放器扩展方法

        #region PART 1 | 1 - 38

        /// <summary>
        /// 获取一个 APlayer 引擎的组成文件的信息串。
        /// ConfigId = 1
        /// </summary>
        /// <returns></returns>
        public string GetAPlayerInfo()
        {
            return base.GetConfig(1);
        }

        /// <summary>
        /// 获取解码器路径
        /// 解码器路径，允许用户自定义解码器的存放路径，默认为 APlayer.DLL 所在的路径的 codecs 目录
        /// ConfigId = 2
        /// </summary>
        /// <returns></returns>
        public string GetDecodingPath()
        {
            return base.GetConfig(2);
        }

        /// <summary>
        /// 设置解码器路径
        /// 解码器路径，允许用户自定义解码器的存放路径，默认为 APlayer.DLL 所在的路径的 codecs 目录
        /// ConfigId = 2
        /// </summary>
        /// <param name="path">解码器路径</param>
        /// <returns></returns>
        public bool SetDecodingPath(string path = "")
        {
            SetConfig(2, path);
            return true;
        }

        /// <summary>
        /// 获取屏蔽的解码器 CLSID 列表
        /// ConfigID = 3
        /// </summary>
        /// <returns></returns>
        public string GetDecodingDisableList()
        {
            return base.GetConfig(3);
        }

        /// <summary>
        /// 设置屏蔽的解码器 CLSID 列表
        /// ConfigID = 3
        /// <param name="DisableList">屏蔽的解码器 CLSID 列表</param>
        /// <returns></returns>
        public bool SetDecodingDisableList(string DisableList)
        {
            base.SetConfig(3, DisableList);
            return true;
        }

        /// <summary>
        /// 获取当前播放的媒体文件的 URL
        /// ConfigID = 4
        /// </summary>
        /// <returns></returns>
        public string GetCurrentURL()
        {
            return base.GetConfig(4);
        }

        /// <summary>
        /// 获取当前播放的媒体文件的文件大小
        /// ConfigID = 5
        /// </summary>
        /// <returns></returns>
        public long GetFileSize()
        {
            return long.Parse(base.GetConfig(5));
        }

        /// <summary>
        /// 播放模式枚举
        /// </summary>
        public enum EnumPlayMode
        {
            Play = 1,           //播放
            Conversion = 2,    //格式转换
            Transcoding = 3     //转码
        }

        /// <summary>
        /// 获取播放模式设置
        /// 1-播放, 2-格式转换, 3-转码, 默认值为1
        /// ConfigID = 6
        /// </summary>
        /// <returns></returns>
        public EnumPlayMode GetPlayMode()
        {
            return (EnumPlayMode)int.Parse((base.GetConfig(6)));
        }

        /// <summary>
        /// 设置播放模式设置
        /// 1-播放, 2-格式转换, 3-转码, 默认值为1
        /// ConfigID = 6
        /// </summary>
        /// <param name="playmode"></param>
        /// <returns></returns>
        public bool SetPlayMode(EnumPlayMode playmode)
        {
            base.SetConfig(6, ((int)playmode).ToString());
            return true;
        }

        /// <summary>
        /// 获取播放结果
        /// 0-播放完成, 1-主动关闭，其他-播放失败错误代码
        /// ConfigID = 7
        /// </summary>
        /// <returns></returns>
        public int GetPlayResult()
        {
            return int.Parse(base.GetConfig(7));
        }

        /// <summary>
        /// 是否自动播放枚举
        /// </summary>
        public enum EnumAutoPlay
        {
            NoAutoPlay = 0, //不自动播放
            AutoPlay = 1    //自动播放
        }

        /// <summary>
        /// 获取是否打开成功后自动播放
        /// 0-不自动播放，1-自动播放，默认为1
        /// ConfigID = 8
        /// </summary>
        /// <returns></returns>
        public EnumAutoPlay GetAutoPlay()
        {
            return (EnumAutoPlay)int.Parse(base.GetConfig(8));
        }

        /// <summary>
        /// 设置是否打开成功后自动播放
        /// 0-不自动播放，1-自动播放，默认为1
        /// ConfigID = 8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetAutoPlay(EnumAutoPlay value)
        {
            base.SetConfig(8, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 是否尝试使用系统解码器枚举
        /// </summary>
        public enum EnumUseOpenChain
        {
            Disabled = 0,  //不尝试
            Enabled = 1     //尝试
        }

        /// <summary>
        /// 获取当 APlayer 内部解码器播放失败后是否尝试使用系统解码器
        /// 0-不尝试，1-尝试，默认为0，尝试使用系统解码器可能会造成播放不稳定。
        /// ConfigID = 9
        /// </summary>
        /// <returns></returns>
        public EnumUseOpenChain GetUseOpenChain()
        {
            return (EnumUseOpenChain)int.Parse(base.GetConfig(9));
        }

        /// <summary>
        /// 设置当 APlayer 内部解码器播放失败后是否尝试使用系统解码器
        /// 0-不尝试，1-尝试，默认为0，尝试使用系统解码器可能会造成播放不稳定。
        /// ConfigID = 9
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetUseOpenChain(EnumUseOpenChain value)
        {
            base.SetConfig(9, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 获取系统声音设备列表
        /// 用";"分割
        /// ConfigID = 10
        /// </summary>
        /// <returns></returns>
        public string GetSoundDeviceList()
        {
            return base.GetConfig(10);
        }

        /// <summary>
        /// 获取当前使用的声音设备
        /// ConfigID = 11
        /// </summary>
        /// <returns></returns>
        public string GetSoundDevicCurrent()
        {
            return base.GetConfig(11);
        }

        /// <summary>
        /// 设置当前使用的声音设备
        /// ConfigID = 11
        /// </summary>
        /// <param name="SoundDevicCurrent">当前使用的声音设备</param>
        /// <returns></returns>
        public bool SetSoundDevicCurrent(string SoundDevicCurrent)
        {
            base.SetConfig(11, SoundDevicCurrent);
            return true;
        }

        /// <summary>
        /// 静音枚举
        /// </summary>
        public enum EnumSilent
        {
            Disabled = 0,  //不静音
            Enabled = 1    //静音
        }

        /// <summary>
        /// 获取是否静音
        /// ConfigID = 12
        /// </summary>
        /// <returns></returns>
        public EnumSilent GetSoundSilent()
        {
            return (EnumSilent)int.Parse(base.GetConfig(12));
        }

        /// <summary>
        /// 设置是否静音
        /// ConfigID = 12
        /// </summary>
        /// <param name="value">是否静音</param>
        /// <returns></returns>
        public bool SetSoundSilent(EnumSilent value)
        {
            base.SetConfig(12, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 获取左右声道音量平衡
        /// 范围0-100，50代表左右声道音量相等
        /// ConfigID = 13
        /// </summary>
        /// <returns>范围0-100，50代表左右声道音量相等</returns>
        public int GetSoundBalance()
        {
            return int.Parse(base.GetConfig(13));
        }

        /// <summary>
        /// 设置左右声道音量平衡
        /// 范围0-100，50代表左右声道音量相等
        /// ConfigID = 13
        /// </summary>
        /// <param name="SoundBalance">范围0-100，50代表左右声道音量相等</param>
        /// <returns></returns>
        public bool SetSoundBalance(int SoundBalance)
        {
            base.SetConfig(13, SoundBalance.ToString());
            return true;
        }

        /// <summary>
        /// 获取是否不渲染视频
        /// 即把视频当音频播放
        /// ConfigID = 14
        /// </summary>
        /// <returns>是否不渲染视频</returns>
        public bool GetDisableVideo()
        {
            return (int.Parse(base.GetConfig(14)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否不渲染视频
        /// 即把视频当音频播放
        /// ConfigID = 14
        /// </summary>
        /// <param name="value">是否不渲染视频</param>
        /// <returns></returns>
        public bool SetDisableVideo(bool value)
        {
            base.SetConfig(14, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取是否不渲染音频
        /// 即把视频无声播放
        /// ConfigID = 15
        /// </summary>
        /// <returns>是否不渲染音频</returns>
        public bool GetDisableAudio()
        {
            return (int.Parse(base.GetConfig(15)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否不渲染音频
        /// 即把视频无声播放
        /// ConfigID = 15
        /// </summary>
        /// <param name="value">是否不渲染音频</param>
        /// <returns></returns>
        public bool SetDisableAudio(bool value)
        {
            base.SetConfig(15, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取是否激活 VsFilter
        /// 默认为1，即激活
        /// ConfigID = 16
        /// </summary>
        /// <returns>是否激活 VsFilter</returns>
        public bool GetEnableVsFilter()
        {
            return (int.Parse(base.GetConfig(16)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否激活 VsFilter
        /// 默认为1，即激活
        /// ConfigID = 16
        /// </summary>
        /// <param name="value">是否激活 VsFilter</param>
        /// <returns></returns>
        public bool SetEnableVsFilter(bool value)
        {
            base.SetConfig(16, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取是否激活 AudioSwitcher
        /// 默认为1，即激活
        /// ConfigID = 17
        /// </summary>
        /// <returns>是否激活 AudioSwitcher</returns>
        public bool GetEnableAudioSwitcher()
        {
            return (int.Parse(base.GetConfig(17)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否激活 AudioSwitcher
        /// 默认为1，即激活
        /// ConfigID = 17
        /// </summary>
        /// <param name="value">是否激活 AudioSwitcher</param>
        /// <returns></returns>
        public bool SetEnableAudioSwitcher(bool value)
        {
            base.SetConfig(17, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 下载解码器的模式枚举
        /// </summary>
        public enum EnumDownloadCodecsSync
        {
            Asynchronization = 0,  //异步
            Synchronization = 1     //同步
        }

        /// <summary>
        /// 获取下载解码器的模式
        /// 0-异步，1-同步，默认0
        /// ConfigID = 18
        /// </summary>
        /// <returns>下载解码器的模式</returns>
        public EnumDownloadCodecsSync GetDownloadCodecsSync()
        {
            return (EnumDownloadCodecsSync)int.Parse(base.GetConfig(18));
        }

        /// <summary>
        /// 设置下载解码器的模式
        /// 0-异步，1-同步，默认0
        /// ConfigID = 18
        /// </summary>
        /// <param name="value">下载解码器的模式</param>
        /// <returns></returns>
        public bool SetDownloadCodecsSync(EnumDownloadCodecsSync value)
        {
            base.SetConfig(18, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 设置用于在异步模式下，通知 APlayer，下载解码器已完成。
        /// ConfigID = 19
        /// </summary>
        /// <returns></returns>
        public bool SetDownloadCodecsCompleted()
        {
            base.SetConfig(19, null);
            return true;
        }

        /// <summary>
        /// 获取APlayer 视频窗口句柄
        /// ConfigID = 20
        /// </summary>
        /// <returns></returns>
        public int GetWindowHandle()
        {
            return int.Parse(base.GetConfig(20));
        }

        /// <summary>
        /// 设置通知 APlayer 顶层窗口位置已改变
        /// 更新 Overlay 渲染模式时的覆盖表面，使播放暂停时视频画面能跟着窗口移动。
        /// ConfigID = 21
        /// </summary>
        /// <returns></returns>
        public bool SetWindowMoved()
        {
            base.SetConfig(21, null);
            return true;
        }

        /// <summary>
        /// 获取所支持的文本设置列表解释说明
        /// ConfigID = 22
        /// 例如：""video_bitrate;kbps;range(100,20000) \r\n 64_profile;none;list(main,base,high)"
        /// </summary>
        /// <returns>所支持的文本设置列表解释说明</returns>
        public string GetSettingExplain()
        {
            return base.GetConfig(22);
        }

        /// <summary>
        /// 获取文本设置
        /// 例如："video_bitrate=600;audio_bitrate=32;" 代表转码时把视频设置为600kbps，音频码率设置为32kbps
        /// ConfigID = 23
        /// </summary>
        /// <returns>文本设置</returns>
        public string GetSettingValues()
        {
            return base.GetConfig(23);
        }

        /// <summary>
        /// 设置文本设置
        /// 例如："video_bitrate=600;audio_bitrate=32;" 代表转码时把视频设置为600kbps，音频码率设置为32kbps
        /// ConfigID = 23
        /// </summary>
        /// <param name="SettingValues">文本设置</param>
        /// <returns></returns>
        public bool SetSettingValues(string SettingValues)
        {
            base.SetConfig(23, SettingValues);
            return true;
        }

        /// <summary>
        /// 设置装载 APlayer 插件
        /// 如果调用者需要获取或者修改 APlayer 解码后的图像和视频，可以使用 APlayer 插件来实现
        /// ConfigID = 24
        /// </summary>
        /// <param name="DLLPath">插件 DLL 的全路径名</param>
        /// <returns></returns>
        public bool SetLoadPlugin(string DLLPath)
        {
            base.SetConfig(24, DLLPath);
            return true;
        }

        /// <summary>
        /// 设置卸载 APlayer 插件
        /// ConfigID = 25
        /// </summary>
        /// <param name="DLLPath">插件 DLL 的全路径名</param>
        /// <returns></returns>
        public bool SetFreePlugin(string DLLPath)
        {
            base.SetConfig(25, DLLPath);
            return true;
        }

        /// <summary>
        /// 获取是否开启播放 Flash 时的用户交互
        /// 1-开启，0-禁止，默认1
        /// ConfigID = 26
        /// </summary>
        /// <returns>是否开启播放 Flash 时的用户交互</returns>
        public bool GetFlashInteraction()
        {
            return (int.Parse(base.GetConfig(26)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否开启播放 Flash 时的用户交互
        /// 1-开启，0-禁止，默认1
        /// ConfigID = 26
        /// </summary>
        /// <param name="value">是否开启播放 Flash 时的用户交互</param>
        /// <returns></returns>
        public bool SetFlashInteraction(bool value)
        {
            base.SetConfig(26, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取是否优先使用 Intel Media SDK 解码
        /// 1-优先，0-不优先，默认0
        /// ConfigID = 27
        /// </summary>
        /// <returns>是否优先使用 Intel Media SDK 解码</returns>
        public bool GetIntelMediaSDKDecoder()
        {
            return (int.Parse(base.GetConfig(27)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否优先使用 Intel Media SDK 解码
        /// 1-优先，0-不优先，默认0
        /// ConfigID = 27
        /// </summary>
        /// <param name="value">是否优先使用 Intel Media SDK 解码</param>
        /// <returns></returns>
        public bool SetIntelMediaSDKDecoder(bool value)
        {
            base.SetConfig(27, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取当前播放文件总共读取字节数
        /// ConfigID = 29
        /// </summary>
        /// <returns>当前播放文件总共读取字节数</returns>
        public long GetReadSize()
        {
            try
            {
                return long.Parse(base.GetConfig(29));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取当前读取文件偏移字节数
        /// ConfigID = 30
        /// </summary>
        /// <returns>当前读取文件偏移字节数</returns>
        public long GetReadOffset()
        {
            return long.Parse(base.GetConfig(30));
        }

        /// <summary>
        /// 获取当前读取时间点
        /// 单位毫秒
        /// ConfigID = 31
        /// </summary>
        /// <returns>当前读取时间点</returns>
        public long GetReadPosition()
        {
            return long.Parse(base.GetConfig(31));
        }

        /// <summary>
        /// 获取当前写入文件偏移
        /// 转码/转格式时候用到
        /// ConfigID = 32
        /// </summary>
        /// <returns>当前写入文件偏移</returns>
        public long GetWriteOffset()
        {
            return long.Parse(base.GetConfig(32));
        }

        /// <summary>
        /// 获取是否允许用户打开播放日志
        /// 1-允许，0-不允许，默认1
        /// ConfigID = 33
        /// </summary>
        /// <returns>是否允许用户打开播放日志</returns>
        public bool GetAllowLog()
        {
            return (int.Parse(base.GetConfig(33)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否允许用户打开播放日志
        /// 1-允许，0-不允许，默认1
        /// ConfigID = 33
        /// </summary>
        /// <param name="value">是否允许用户打开播放日志</param>
        /// <returns></returns>
        public bool SetAllowLog(bool value)
        {
            base.SetConfig(33, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// APlayer 插件的导出函数调用方式枚举
        /// </summary>
        public enum EnumPluginStdcall
        {
            Cdecl = 0,
            Stdcall = 1
        }

        /// <summary>
        /// 获取 APlayer 插件的导出函数调用方式
        /// 1-stdcall, 0-cdecl，默认0
        /// 设置这个参数的目的是支持不方便编译出 cdecl 调用的动态链接库的语言。
        /// ConfigID = 35
        /// </summary>
        /// <returns>APlayer 插件的导出函数调用方式</returns>
        public EnumPluginStdcall GetPluginStdcall()
        {
            return (EnumPluginStdcall)int.Parse(base.GetConfig(35));
        }

        /// <summary>
        /// 设置 APlayer 插件的导出函数调用方式
        /// 1-stdcall, 0-cdecl，默认0
        /// 设置这个参数的目的是支持不方便编译出 cdecl 调用的动态链接库的语言。
        /// ConfigID = 35
        /// </summary>
        /// <param name="value">APlayer 插件的导出函数调用方式</param>
        /// <returns></returns>
        public bool SetPluginStdcall(EnumPluginStdcall value)
        {
            base.SetConfig(35, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 获取 Logo 风格设置字符串
        /// 格式："backcolor;xpercent;ypercent"
        /// 如设置白色背景(16777215为0xffffff的十进制串)、水平偏左(30%)、垂直居中(50%)的 Logo 位置："16777215;30;50"
        /// ConfigID = 36
        /// </summary>
        /// <returns>Logo 风格设置字符串</returns>
        public string GetLogoSettings()
        {
            return base.GetConfig(36);
        }

        /// <summary>
        /// 设置 Logo 风格设置字符串
        /// 格式："backcolor;xpercent;ypercent"
        /// 如设置白色背景(16777215为0xffffff的十进制串)、水平偏左(30%)、垂直居中(50%)的 Logo 位置："16777215;30;50"
        /// ConfigID = 36
        /// </summary>
        /// <param name="backcolor">背景颜色</param>
        /// <param name="xpercent">水平位置（0-100%）</param>
        /// <param name="ypercent">垂直位置（0-100%）</param>
        /// <returns></returns>
        public bool SetLogoSettings(Color backcolor, int xpercent, int ypercent)
        {
            if (xpercent < 0 || xpercent > 100)
                return false;
            if (ypercent < 0 || ypercent > 100)
                return false;
            base.SetConfig(36, _ParseRGB(backcolor).ToString() + ';' + xpercent.ToString() + ';' + ypercent.ToString());
            return true;
        }

        /// <summary>
        /// 设置本地或者 http 网络文件作为 Logo 图片
        /// 目前支持两种格式 BMP 和 JPG
        /// 如果本地文件不存在或未拉取到网络图片，则不显示任何 Logo（相当于隐藏 Logo）。
        /// ConfigID = 37
        /// </summary>
        /// <param name="url">图片的全文件名或 URL</param>
        /// <returns></returns>
        public bool SetLogoFile(string path)
        {
            base.SetConfig(37, path);
            return true;
        }

        /// <summary>
        /// 获取在 APlayer 视频窗口上显示的鼠标指针
        /// 设置为 0 时恢复默认鼠标指针
        /// ConfigID = 38
        /// </summary>
        /// <returns>光标句柄</returns>
        public IntPtr GetCustomCursor()
        {
            return (IntPtr)int.Parse(base.GetConfig(38));
        }

        /// <summary>
        /// 设置在 APlayer 视频窗口上显示的鼠标指针
        /// 设置为 0 时恢复默认鼠标指针
        /// ConfigID = 38
        /// </summary>
        /// <param name="value">光标句柄</param>
        /// <returns></returns>
        public bool SetCustomCursor(IntPtr value)
        {
            base.SetConfig(38, ((int)value).ToString());
            return true;
        }

        #endregion

        #region PART 2 | 101 - 120

        /// <summary>
        /// 获取标志 GetPosition/SetPosition/GetDuration 函数所使用的单位是帧，还是毫秒
        /// 为1表示帧，0表示毫秒。
        /// ConfigID = 101
        /// </summary>
        /// <returns>标志 GetPosition/SetPosition/GetDuration 函数所使用的单位是帧，还是毫秒</returns>
        public bool GetTimeIsFrame()
        {
            return (int.Parse(base.GetConfig(101)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取文件打开后跳到哪里开始播放
        /// 单位毫秒
        /// ConfigID = 102
        /// </summary>
        /// <returns>文件打开后跳到哪里开始播放</returns>
        public int GetStartPosition()
        {
            return int.Parse(base.GetConfig(102));
        }

        /// <summary>
        /// 设置文件打开后跳到哪里开始播放
        /// 单位毫秒
        /// ConfigID = 102
        /// </summary>
        /// <param name="value">文件打开后跳到哪里开始播放</param>
        /// <returns></returns>
        public bool SetStartPosition(int value)
        {
            base.SetConfig(102, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取播放到哪里自动停止播放
        /// 单位毫秒
        /// ConfigID = 103
        /// </summary>
        /// <returns>播放到哪里自动停止播放</returns>
        public int GetStopPosition()
        {
            return int.Parse(base.GetConfig(103));
        }

        /// <summary>
        /// 设置播放到哪里自动停止播放
        /// 单位毫秒
        /// ConfigID = 103
        /// </summary>
        /// <param name="value">播放到哪里自动停止播放</param>
        /// <returns></returns>
        public bool SetStopPosition(int value)
        {
            base.SetConfig(103, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取播放速度
        /// 100-为正常速度，大于100为快速播放，小于100 为慢速播放
        /// ConfigID = 104
        /// </summary>
        /// <returns>播放速度</returns>
        public int GetPlaySpeed()
        {
            return int.Parse(base.GetConfig(104));
        }

        /// <summary>
        /// 设置播放速度
        /// 100-为正常速度，大于100为快速播放，小于100 为慢速播放
        /// ConfigID = 104
        /// </summary>
        /// <param name="value">播放速度</param>
        /// <returns></returns>
        public bool SetPlaySpeed(int value)
        {
            base.SetConfig(104, value.ToString());
            return true;
        }

        /// <summary>
        /// Seek 模式枚举
        /// </summary>
        public enum EnumKeyframeSeek
        {
            Normal = 0,     //Seek较慢但精确
            Keyframe = 1    //Seek较快但不精确
        }

        /// <summary>
        /// 获取 Seek 模式
        /// 1-Keyframe(Seek较快但不精确), 0-normal(Seek较慢但精确), 默认1
        /// ConfigID = 105
        /// </summary>
        /// <returns> Seek 模式</returns>
        public EnumKeyframeSeek GetKeyframeSeek()
        {
            return (EnumKeyframeSeek)int.Parse(base.GetConfig(105));
        }

        /// <summary>
        /// 设置 Seek 模式
        /// 1-Keyframe(Seek较快但不精确), 0-normal(Seek较慢但精确), 默认1
        /// ConfigID = 105
        /// </summary>
        /// <param name="value"> Seek 模式</param>
        /// <returns></returns>
        public bool SetKeyframeSeek(EnumKeyframeSeek value)
        {
            base.SetConfig(105, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 获取当前播放视频的关键帧个数
        /// ConfigID = 106
        /// </summary>
        /// <returns>当前播放视频的关键帧个数</returns>
        public int GetKeyframeCount()
        {
            return int.Parse(base.GetConfig(106));
        }

        /// <summary>
        /// 获取当前播放视频的关键帧列表
        /// 单位毫秒，即这些时间点为关键帧，例如："0;12000;36000;52000;98000"
        /// ConfigID = 107
        /// </summary>
        /// <returns></returns>
        public string GetKeyframeList()
        {
            return base.GetConfig(107);
        }

        /// <summary>
        /// 获取当前播放的关键帧索引
        /// ConfigID = 108
        /// </summary>
        /// <returns>当前播放的关键帧索引</returns>
        public int GetKeyframeCurrent()
        {
            return int.Parse(base.GetConfig(108));
        }

        /// <summary>
        /// 设置当前播放的关键帧索引
        /// ConfigID = 108
        /// </summary>
        /// <param name="value">当前播放的关键帧索引</param>
        /// <returns></returns>
        public bool SetKeyframeCurrent(int value)
        {
            base.SetConfig(108, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取当前视频是否支持单帧步进
        /// ConfigID = 109
        /// </summary>
        /// <returns>当前视频是否支持单帧步进</returns>
        public bool GetCanFramestepForwardOne()
        {
            return (int.Parse(base.GetConfig(109)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取当前视频是否支持多帧步进
        /// ConfigID = 110
        /// </summary>
        /// <returns></returns>
        public bool GetCanFramestepForwardMulti()
        {
            return (int.Parse(base.GetConfig(110)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取当前视频是否支持单帧步退
        /// ConfigID = 111
        /// </summary>
        /// <returns>当前视频是否支持单帧步退</returns>
        public bool GetCanFramestepBackwardOne()
        {
            return (int.Parse(base.GetConfig(111)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取当前视频是否支持多帧步退
        /// ConfigID = 112
        /// </summary>
        /// <returns>当前视频是否支持多帧步退</returns>
        public bool GetCanFramestepBackwardMulti()
        {
            return (int.Parse(base.GetConfig(112)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置帧步进或步退
        /// 例如，1-单帧步进，-1-单帧步退，2-步进两帧，依此类推
        /// ConfigID = 113
        /// </summary>
        /// <param name="value">帧步进或步退值</param>
        /// <returns></returns>
        public bool SetFramestep(int value)
        {
            base.SetConfig(113, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取是否当前正在帧步进步退过程中
        /// ConfigID = 114
        /// </summary>
        /// <returns>是否当前正在帧步进步退过程中</returns>
        public bool GetIsFrameStepping()
        {
            return (int.Parse(base.GetConfig(114)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取打开文件是否读取 RM/RMVB 文件索引信息
        /// 对于 RM/RMVB 文件为了打开速度快，默认不读取索引信息，所以不会有关键列表信息，设置这个参数为1后会打开时读取索引信息
        /// ConfigID = 115
        /// </summary>
        /// <returns>打开文件是否读取 RM/RMVB 文件索引信息</returns>
        public bool GetReadIndexWhenOpen()
        {
            return (int.Parse(base.GetConfig(115)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置打开文件是否读取 RM/RMVB 文件索引信息
        /// 对于 RM/RMVB 文件为了打开速度快，默认不读取索引信息，所以不会有关键列表信息，设置这个参数为1后会打开时读取索引信息
        /// ConfigID = 115
        /// </summary>
        /// <param name="value">打开文件是否读取 RM/RMVB 文件索引信息</param>
        /// <returns></returns>
        public bool SetReadIndexWhenOpen(bool value)
        {
            base.SetConfig(115, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取文件偏移列表信息
        /// 显示网络缓冲数据段状态时能用到该信息
        /// ConfigID = 116
        /// </summary>
        /// <returns>文件偏移列表信息</returns>
        public string GetTimePositionList()
        {
            return base.GetConfig(116);
        }

        /// <summary>
        /// 获取平均视频帧间隔
        /// 单位毫秒
        /// ConfigID = 117
        /// </summary>
        /// <returns></returns>
        public int GetFrameInterval()
        {
            return int.Parse(base.GetConfig(117));
        }

        /// <summary>
        /// 获取视频渲染器本次当前已经绘制的帧数
        /// ConfigID = 118
        /// </summary>
        /// <returns>视频渲染器本次当前已经绘制的帧数</returns>
        public int GetFramesDrawn()
        {
            return int.Parse(base.GetConfig(118));
        }

        /// <summary>
        /// 循环播放枚举模式
        /// </summary>
        public enum EnumLoopPlay
        {
            Auto = 0,   //自动
            Loop = 1,   //循环
            NoLoop = 2  //不循环
        }

        /// <summary>
        /// 获取循环播放模式
        /// 0-自动, 1-循环, 2-不循环, 默认0 (自动模式中, GIF 会自动循环, 其他格式默认不循环)
        /// ConfigID = 119
        /// </summary>
        /// <returns>循环播放模式</returns>
        public EnumLoopPlay GetLoopPlay()
        {
            return (EnumLoopPlay)int.Parse(base.GetConfig(119));
        }

        /// <summary>
        /// 设置循环播放模式
        /// 0-自动, 1-循环, 2-不循环, 默认0 (自动模式中, GIF 会自动循环, 其他格式默认不循环)
        /// ConfigID = 119
        /// </summary>
        /// <param name="value">循环播放模式</param>
        /// <returns></returns>
        public bool SetLoopPlay(EnumAutoPlay value)
        {
            base.SetConfig(119, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 获取是否播放完成不自动 Close 
        /// (自动 Close 会返回 PS_READY 状态)，0-自动 Close，1-不自动 Close，默认 0，
        /// 设置为1时，播放结束不自动 Close，调用者还可以 SetPositon 继续播放，但还是会发送 OnEvent(PLAYCOMPLETE) 事件
        /// ConfigID = 120
        /// </summary>
        /// <returns>是否播放完成不自动 Close </returns>
        public bool GetNoCloseWhenComplete()
        {
            return (int.Parse(base.GetConfig(120)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否播放完成不自动 Close 
        /// (自动 Close 会返回 PS_READY 状态)，0-自动 Close，1-不自动 Close，默认 0，
        /// 设置为1时，播放结束不自动 Close，调用者还可以 SetPositon 继续播放，但还是会发送 OnEvent(PLAYCOMPLETE) 事件
        /// ConfigID = 120
        /// </summary>
        /// <param name="value">是否播放完成不自动 Close </param>
        /// <returns></returns>
        public bool SetNoCloseWhenComplete(bool value)
        {
            base.SetConfig(120, value ? "1" : "0");
            return true;
        }

        #endregion

        #region PART 3 | 201 - 217

        /// <summary>
        /// 渲染模式枚举
        /// </summary>
        public enum EnumRenderMode
        {
            Overlay = 1,
            Renderless = 2,
            EVR = 3,
            EVRCP = 4,
            AVR = 5
        }

        /// <summary>
        /// 获取渲染模式
        /// 1-Overlay, 2-Renderless, 3-EVR, 4-EVRCP, 5-AVR
        /// ConfigID = 201
        /// </summary>
        /// <returns>渲染模式</returns>
        public EnumRenderMode GetRenderModeConfig()
        {
            return (EnumRenderMode)int.Parse(base.GetConfig(201));
        }

        /// <summary>
        /// 设置渲染模式
        /// 1-Overlay, 2-Renderless, 3-EVR, 4-EVRCP, 5-AVR
        /// ConfigID = 201
        /// </summary>
        /// <param name="value">渲染模式</param>
        /// <returns></returns>
        public bool SetRenderModeConfig(EnumRenderMode value)
        {
            base.SetConfig(201, ((int)value).ToString());
            return true;
        }

        /// <summary>
        /// 获取当前使用的渲染模式
        /// 1-Overlay, 2-Renderless, 3-EVR, 4-EVRCP, 5-AVR
        /// ConfigID = 202
        /// </summary>
        /// <returns>当前使用的渲染模式</returns>
        public EnumRenderMode GetRenderModeCurrent()
        {
            return (EnumRenderMode)int.Parse(base.GetConfig(202));
        }

        /// <summary>
        /// 获取视频的自然纵横比
        /// 格式："4;3"
        /// ConfigID = 203
        /// </summary>
        /// <returns>视频的自然纵横比</returns>
        public string GetAspectRatioNative()
        {
            return base.GetConfig(203);
        }

        /// <summary>
        /// 获取视频的自定义纵横比
        /// 格式："4;3"
        /// ConfigID = 204
        /// </summary>
        /// <returns></returns>
        public string GetAspectRatioCustom()
        {
            return base.GetConfig(204);
        }

        /// <summary>
        /// 设置视频的自定义纵横比
        /// 格式："4;3"
        /// ConfigID = 204
        /// </summary>
        /// <param name="x">纵</param>
        /// <param name="y">横</param>
        /// <returns></returns>
        public bool SetAspectRatioCustom(int x, int y)
        {
            if (x <= 0 || y <= 0)
                return false;
            base.SetConfig(204, x.ToString() + ';' + y.ToString());
            return true;
        }

        /// <summary>
        /// 获取视频的源矩形
        /// 格式："left;top;right;bottom"
        /// 例如只显示左上角400x300区域："0;0;400;300"
        /// ConfigID = 205
        /// </summary>
        /// <returns>视频的源矩形</returns>
        public string GetVideoSourcePosition()
        {
            return base.GetConfig(205);
        }

        /// <summary>
        /// 设置视频的源矩形
        /// 格式："left;top;right;bottom"
        /// 例如只显示左上角400x300区域："0;0;400;300"
        /// ConfigID = 205
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public bool SetVideoSourcePosition(int left, int top, int right, int bottom)
        {
            base.SetConfig(205, left.ToString() + ';' + top.ToString() + ';' + right.ToString() + ';' + bottom.ToString());
            return true;
        }

        /// <summary>
        /// 获取视频的目标矩形
        /// 即视频画面在视频区域的中的位置
        /// 格式："left;top;right;bottom"
        /// ConfigID = 206
        /// </summary>
        /// <returns>视频的目标矩形</returns>
        public string GetVideoTargetPsoition()
        {
            return base.GetConfig(206);
        }

        /// <summary>
        /// 设置视频的目标矩形
        /// 即视频画面在视频区域的中的位置
        /// 格式："left;top;right;bottom"
        /// ConfigID = 206
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public bool SetVideoTargetPsoition(int left, int top, int right, int bottom)
        {
            base.SetConfig(206, left.ToString() + ';' + top.ToString() + ';' + right.ToString() + ';' + bottom.ToString());
            return true;
        }

        /// <summary>
        /// 获取播放时是否智能去除当前视频黑边
        /// 只是渲染时去除，不改变视频内容
        /// ConfigID = 207
        /// </summary>
        /// <returns>播放时是否智能去除当前视频黑边</returns>
        public bool GetClipBlackbandEnable()
        {
            return (int.Parse(base.GetConfig(207)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置播放时是否智能去除当前视频黑边
        /// 只是渲染时去除，不改变视频内容
        /// ConfigID = 207
        /// </summary>
        /// <param name="value">播放时是否智能去除当前视频黑边</param>
        /// <returns></returns>
        public bool SetClipBlackbandEnable(bool value)
        {
            base.SetConfig(207, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取智能去黑边的阈值
        /// 低于这个亮度就算做黑边
        /// ConfigID = 208
        /// </summary>
        /// <returns>智能去黑边的阈值</returns>
        public int GetClipBlackbandStill()
        {
            return int.Parse(base.GetConfig(208));
        }

        /// <summary>
        /// 设置智能去黑边的阈值
        /// 低于这个亮度就算做黑边
        /// ConfigID = 208
        /// </summary>
        /// <param name="value">智能去黑边的阈值</param>
        /// <returns></returns>
        public bool SetClipBlackbandStill(int value)
        {
            base.SetConfig(208, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取是否开启硬件加速
        /// ConfigID = 209
        /// </summary>
        /// <returns>是否开启硬件加速</returns>
        public bool GetSpeedupEnable()
        {
            return (int.Parse(base.GetConfig(209)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否开启硬件加速
        /// ConfigID = 209
        /// </summary>
        /// <param name="value">是否开启硬件加速</param>
        /// <returns></returns>
        public bool SetSpeedupEnable(bool value)
        {
            base.SetConfig(209, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取硬件加速是否优先使用 CUDA
        /// 而不是 DXVA/DXVA2。
        /// ConfigID = 210
        /// </summary>
        /// <returns>硬件加速是否优先使用 CUDA</returns>
        public bool GetSpeedupCUDAFirst()
        {
            return (int.Parse(base.GetConfig(210)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置硬件加速是否优先使用 CUDA
        /// 而不是 DXVA/DXVA2。
        /// ConfigID = 210
        /// </summary>
        /// <param name="value">硬件加速是否优先使用</param>
        /// <returns></returns>
        public bool SetSpeedupCUDAFirst(bool value)
        {
            base.SetConfig(210, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 硬件加速状态枚举
        /// </summary>
        public enum EnumSpeedupStatus
        {
            Disabled = 0,           //未开启
            Enabled = 1,            //开启成功
            UnknowError = 2,        //未知错误
            DeviceUnsupport = 3,    //设备不支持
            FormatUnsupport = 4,    //格式不支持
            SystemUnsupport = 5,    //操作系统不支持
            CodecdUnsupport = 6     //解码器不支持
        }

        /// <summary>
        /// 获取硬件加速的开启状态
        /// 0 - 未开启, 1 - 开启成功, 2 - 未知错误, 3 - 设备不支持, 4 - 格式不支持, 5 - 操作系统不支持, 6 - 解码器不支持
        /// ConfigID = 211
        /// </summary>
        /// <returns>硬件加速的开启状态</returns>
        public EnumSpeedupStatus GetSpeedupStatus()
        {
            return (EnumSpeedupStatus)int.Parse(base.GetConfig(211));
        }

        /// <summary>
        /// 获取开启了何种硬件加速
        /// ConfigID = 212
        /// </summary>
        /// <returns>开启了何种硬件加速</returns>
        public string GetSpeedupQuery()
        {
            return base.GetConfig(212);
        }

        /// <summary>
        /// 获取色彩调节功能是否可用
        /// ConfigID = 213
        /// </summary>
        /// <returns>色彩调节功能是否可用</returns>
        public bool GetVideoAdjustUsable()
        {
            return (int.Parse(base.GetConfig(213)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取亮度
        /// 范围 0-100，默认50
        /// ConfigID = 214
        /// </summary>
        /// <returns>亮度</returns>
        public int GetBrightness()
        {
            return int.Parse(base.GetConfig(214));
        }

        /// <summary>
        /// 设置亮度
        /// 范围 0-100，默认50
        /// ConfigID = 214
        /// </summary>
        /// <param name="value">亮度</param>
        /// <returns></returns>
        public bool SetBrightness(int value)
        {
            base.SetConfig(214, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取对比度
        /// 范围 0-100，默认50
        /// ConfigID = 215
        /// </summary>
        /// <returns>对比度</returns>
        public int GetContrast()
        {
            return int.Parse(base.GetConfig(215));
        }

        /// <summary>
        /// 设置对比度
        /// 范围 0-100，默认50
        /// ConfigID = 215
        /// </summary>
        /// <param name="value">对比度</param>
        /// <returns></returns>
        public bool SetContrast(int value)
        {
            base.SetConfig(215, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取饱和度
        /// 范围 0-100，默认50
        /// ConfigID = 216
        /// </summary>
        /// <returns>饱和度</returns>
        public int GetSaturation()
        {
            return int.Parse(base.GetConfig(216));
        }

        /// <summary>
        /// 设置饱和度
        /// 范围 0-100，默认50
        /// ConfigID = 216
        /// </summary>
        /// <param name="value">饱和度</param>
        /// <returns></returns>
        public bool SetSaturation(int value)
        {
            base.SetConfig(216, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取色相
        /// 范围 0-100，默认50
        /// ConfigID = 217
        /// </summary>
        /// <returns>色相</returns>
        public int GetHue()
        {
            return int.Parse(base.GetConfig(217));
        }

        /// <summary>
        /// 设置色相
        /// 范围 0-100，默认50
        /// ConfigID = 217
        /// </summary>
        /// <param name="value">色相</param>
        /// <returns></returns>
        public bool SetHue(int value)
        {
            base.SetConfig(217, value.ToString());
            return true;
        }

        #endregion

        #region PART 6 | 501 - 511

        /// <summary>
        /// 获取字幕加载功能是否可用
        /// ConfigID = 501
        /// </summary>
        /// <returns>字幕加载功能是否可用</returns>
        public bool GetSubtitleUsable()
        {
            return (int.Parse(base.GetConfig(501)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取支持的字幕格式列表
        /// 格式："srt;ssa;ass;idx"
        /// ConfigID = 502
        /// </summary>
        /// <returns>支持的字幕格式列表</returns>
        public string GetSubtitleExtnameList()
        {
            return base.GetConfig(502);
        }

        /// <summary>
        /// 获取外挂字幕的文件名
        /// 例如："c:\subtitle.srt"
        /// ConfigID = 503
        /// </summary>
        /// <returns>外挂字幕的文件名</returns>
        public string GetSubtitleFilename()
        {
            return base.GetConfig(503);
        }

        /// <summary>
        /// 设置外挂字幕的文件名
        /// 例如："c:\subtitle.srt"
        /// ConfigID = 503
        /// </summary>
        /// <param name="value">外挂字幕的文件名</param>
        /// <returns></returns>
        public bool SetSubtitleFilename(string value)
        {
            base.SetConfig(503, value);
            return true;
        }

        /// <summary>
        /// 获取字幕是否显示
        /// ConfigID = 504
        /// </summary>
        /// <returns>字幕是否显示</returns>
        public bool GetSubtitleShow()
        {
            return (int.Parse(base.GetConfig(504)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置字幕是否显示
        /// ConfigID = 504
        /// </summary>
        /// <param name="value">字幕是否显示</param>
        /// <returns></returns>
        public bool SetSubtitleShow(bool value)
        {
            base.SetConfig(504, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取当前加载的字幕的可用语言列表
        /// 用";"分割，例如："chinese;english"
        /// ConfigID = 505
        /// </summary>
        /// <returns>当前加载的字幕的可用语言列表</returns>
        public string GetSubtitleLanguageList()
        {
            return base.GetConfig(505);
        }

        /// <summary>
        /// 获取当前选择的字幕语言索引
        /// ConfigID = 506
        /// </summary>
        /// <returns>当前选择的字幕语言索引</returns>
        public int GetSubtitlelanguageCurrent()
        {
            return int.Parse(base.GetConfig(506));
        }

        /// <summary>
        /// 设置当前选择的字幕语言索引
        /// ConfigID = 506
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetSubtitlelanguageCurrent(int value)
        {
            base.SetConfig(506, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取字幕位置
        /// 例如："1;50;90"，其中 1表示设置生效，50表示设置在水平位置 50%，90垂直位置 90%
        /// ConfigID = 507
        /// </summary>
        /// <returns>字幕位置</returns>
        public string GetSubtitlePlacement()
        {
            return base.GetConfig(507);
        }

        /// <summary>
        /// 设置字幕位置
        /// 例如："1;50;90"，其中 1表示设置生效，50表示设置在水平位置 50%，90垂直位置 90%
        /// ConfigID = 507
        /// </summary>
        /// <param name="IsEnabled">设置是否生效</param>
        /// <param name="XPercent">水平位置</param>
        /// <param name="YPercent">垂直位置</param>
        /// <returns></returns>
        public bool SetSubtitlePlacement(bool IsEnabled, int XPercent, int YPercent)
        {
            base.SetConfig(507, (IsEnabled ? "1" : "0") + ';' + XPercent.ToString() + ';' + YPercent.ToString());
            return true;
        }

        /// <summary>
        /// 获取字幕默认字体
        /// 格式："fontname;fontsize;fontcolor;shadow"，例如："宋体;18;16777215;1"
        /// ConfigID = 508
        /// </summary>
        /// <returns></returns>
        public string GetSubtitleFont()
        {
            return base.GetConfig(508);
        }

        /// <summary>
        /// 设置字幕默认字体
        /// 格式："fontname;fontsize;fontcolor;shadow"，例如："宋体;18;16777215;1"
        /// ConfigID = 508
        /// </summary>
        /// <param name="FontName">字体</param>
        /// <param name="FontSize">字号</param>
        /// <param name="FontColor">字色</param>
        /// <param name="Shadow">阴影</param>
        /// <returns></returns>
        public bool SetSubtitleFont(string FontName, int FontSize, Color FontColor, int Shadow)
        {
            base.SetConfig(508, FontName + ';' + FontSize.ToString() + ';' + _ParseRGB(FontColor).ToString() + ';' + Shadow.ToString());
            return true;
        }

        /// <summary>
        /// 获取字幕延时
        /// 格式，"delay;speedmul;speeddiv"，例如："5000;1000;1000"，表示字幕延时 5000 毫秒
        /// ConfigID = 509
        /// </summary>
        /// <returns>字幕延时</returns>
        public string GetSubtitleTiming()
        {
            return base.GetConfig(509);
        }

        /// <summary>
        /// 设置字幕延时
        /// 格式，"delay;speedmul;speeddiv"，例如："5000;1000;1000"，表示字幕延时 5000 毫秒
        /// ConfigID = 509
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="speedmul"></param>
        /// <param name="speeddiv"></param>
        /// <returns></returns>
        public bool SetSubtitleTiming(int delay, int speedmul, int speeddiv)
        {
            base.SetConfig(509, delay.ToString() + ';' + speedmul.ToString() + ';' + speeddiv.ToString());
            return true;
        }

        /// <summary>
        /// 设置字符串形式的内存字幕
        /// Unicode 格式
        /// ConfigID = 510
        /// </summary>
        /// <param name="value">字符串形式的内存字幕</param>
        /// <returns></returns>
        public bool SetSubtitleContent(string value)
        {
            base.SetConfig(510, value);
            return true;
        }

        #endregion

        #region PART 7 | 601 - 624

        /// <summary>
        /// 获取视频叠图加功能是否可用
        /// ConfigID = 601
        /// </summary>
        /// <returns>视频叠图加功能是否可用</returns>
        public bool GetPictureUsable()
        {
            return (int.Parse(base.GetConfig(601)) == 1 ? true : false);
        }

        /// <summary>
        /// 获取视频叠图加功能是否激活
        /// ConfigID = 602
        /// </summary>
        /// <returns>视频叠图加功能是否激活</returns>
        public bool GetPictureEnable()
        {
            return (int.Parse(base.GetConfig(602)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置视频叠图加功能是否激活
        /// ConfigID = 602
        /// </summary>
        /// <param name="value">视频叠图加功能是否激活</param>
        /// <returns></returns>
        public bool SetPictureEnable(bool value)
        {
            base.SetConfig(602, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取图像可叠加区域
        /// 坐标基于 APlayer 视频窗口
        /// 格式："left;top;right;bottom"
        /// ConfigID = 603
        /// </summary>
        /// <returns>图像可叠加区域</returns>
        public string GetPictureBound()
        {
            return base.GetConfig(603);
        }

        /// <summary>
        /// 获取图像宽度
        /// 单位像素
        /// ConfigID = 604
        /// </summary>
        /// <returns>图像宽度</returns>
        public int GetPictureWidth()
        {
            return int.Parse(base.GetConfig(604));
        }

        /// <summary>
        /// 获取图像高度
        /// 单位像素
        /// ConfigID = 605
        /// </summary>
        /// <returns>图像高度</returns>
        public int GetPictureHeight()
        {
            return int.Parse(base.GetConfig(605));
        }

        /// <summary>
        /// 获取叠加图像水平位置
        /// 单位像素
        /// ConfigID = 606
        /// </summary>
        /// <returns>叠加图像水平位置</returns>
        public int GetPictureLeft()
        {
            return int.Parse(base.GetConfig(606));
        }

        /// <summary>
        /// 设置叠加图像水平位置
        /// 单位像素
        /// ConfigID = 606
        /// </summary>
        /// <param name="value">叠加图像水平位置</param>
        /// <returns></returns>
        public bool SetPictureLeft(int value)
        {
            base.SetConfig(606, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取叠加图像垂直位置
        /// 单位像素
        /// ConfigID = 607
        /// </summary>
        /// <returns>叠加图像垂直位置</returns>
        public int GetPictureTop()
        {
            return int.Parse(base.GetConfig(607));
        }

        /// <summary>
        /// 设置叠加图像垂直位置
        /// 单位像素
        /// ConfigID = 607
        /// </summary>
        /// <param name="value">叠加图像垂直位置</param>
        /// <returns></returns>
        public bool SetPictureTop(int value)
        {
            base.SetConfig(607, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取叠加图像的 alpha 值
        /// 范围 0-255，0为完全透明，255为完全不透明。
        /// ConfigID = 608
        /// </summary>
        /// <returns>叠加图像的 alpha 值</returns>
        public int GetPictureAlpha()
        {
            return int.Parse(base.GetConfig(608));
        }

        /// <summary>
        /// 设置叠加图像的 alpha 值
        /// 范围 0-255，0为完全透明，255为完全不透明。
        /// ConfigID = 608
        /// </summary>
        /// <param name="value">叠加图像的 alpha 值</param>
        /// <returns></returns>
        public bool SetPictureAlpha(int value)
        {
            if (value > 255 || value < 0)
                return false;
            base.SetConfig(608, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取图像颜色键
        /// 图像中颜色等于颜色键的区域自动完全透明，如果该值为-1，则使用图像自身的 Alpha 通道。
        /// ConfigID = 609
        /// </summary>
        /// <returns></returns>
        public Color GetPictureColorkey()
        {
            return _ParseColor(int.Parse(base.GetConfig(609)));
        }

        /// <summary>
        /// 设置图像颜色键
        /// 图像中颜色等于颜色键的区域自动完全透明，如果该值为-1，则使用图像自身的 Alpha 通道。
        /// ConfigID = 609
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetPictureColorkey(Color value)
        {
            base.SetConfig(609, _ParseRGB(value).ToString());
            return true;
        }

        /// <summary>
        /// 获取一个位置是否命中了所叠加的图像
        /// 格式："x;y"
        /// 返回值，0-未命中，1-命中图像的矩形区域，2-命中图像的可见区域。
        /// ConfigID = 610
        /// </summary>
        /// <param name="value">是否命中了所叠加的图像</param>
        /// <returns></returns>
        public int GetPictureHitTest(int x, int y)
        {
            return base.SetConfig(610, x.ToString() + ';' + y.ToString());
        }

        /// <summary>
        /// 获取命中测试的 alpha 下限
        /// 如果图像中某像素的 alpha 值小于该下限则不会被视为可命中的可见区域
        /// ConfigID = 611
        /// </summary>
        /// <returns>命中测试的 alpha 下限</returns>
        public int GetPictureHitAlpha()
        {
            return int.Parse(base.GetConfig(611));
        }

        /// <summary>
        /// 设置命中测试的 alpha 下限
        /// 如果图像中某像素的 alpha 值小于该下限则不会被视为可命中的可见区域
        /// ConfigID = 611
        /// </summary>
        /// <param name="value">命中测试的 alpha 下限</param>
        /// <returns></returns>
        public bool SetPictureHitAlpha(int value)
        {
            base.SetConfig(611, value.ToString());
            return true;
        }

        /// <summary>
        /// 设置一段文本作为叠加图像
        /// 支持回车换行符主动换行和自动换行（文本宽度参数 623 限制下的自动换行）
        /// ConfigID = 612
        /// </summary>
        /// <param name="value">作为叠加图像的文本</param>
        /// <returns></returns>
        public bool SetPictureText(string value)
        {
            base.SetConfig(612, value);
            return true;
        }

        /// <summary>
        /// 获取叠加文本的字体
        /// 格式："fontname;fontsize;fontcolor;edge"
        /// ConfigID = 613
        /// </summary>
        /// <returns></returns>
        public string GetPictureFont()
        {
            return base.GetConfig(613);
        }

        /// <summary>
        /// 设置叠加文本的字体
        /// 格式："fontname;fontsize;fontcolor;edge"
        /// ConfigID = 613
        /// </summary>
        /// <param name="fontname">字体</param>
        /// <param name="fontsize">字号</param>
        /// <param name="fontcolor">字色</param>
        /// <param name="edge">描边大小</param>
        /// <returns></returns>
        public bool SetPictureFont(string fontname, int fontsize, Color fontcolor, int edge)
        {
            base.SetConfig(613, fontname + ';' + fontsize.ToString() + ';' + _ParseRGB(fontcolor) + ';' + edge.ToString());
            return true;
        }

        /// <summary>
        /// 设置叠加一个 RGBA 内存区
        /// 格式："address;width;height"
        /// ConfigID = 614
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public bool SetPictureRGBABuffer(string address, int width, int height)
        {
            base.SetConfig(614, address + ';' + width.ToString() + ';' + height.ToString());
            return true;
        }

        /// <summary>
        /// 设置叠加一个 BMP 位图
        /// ConfigID = 615
        /// </summary>
        /// <param name="value">位图句柄</param>
        /// <returns></returns>
        public bool SetPictureBMPHandle(IntPtr value)
        {
            base.SetConfig(615, value.ToString());
            return true;
        }

        /// <summary>
        /// 设置叠加一个 BMP 位图文件
        /// ConfigID = 616
        /// </summary>
        /// <param name="value">文件的路径</param>
        /// <returns></returns>
        public bool SetPictureBMPFilename(string value)
        {
            base.SetConfig(616, value);
            return true;
        }

        /// <summary>
        /// 设置叠加一个 PNG 图像文件
        /// ConfigID = 617
        /// </summary>
        /// <param name="value">文件的路径</param>
        /// <returns></returns>
        public bool SetPicturePNGFilename(string value)
        {
            base.SetConfig(617, value);
            return true;
        }

        /// <summary>
        /// 设置叠加一个 SWF 动画文件
        /// ConfigID = 618
        /// </summary>
        /// <param name="value">文件的路径</param>
        /// <returns></returns>
        public bool SetPictureSWFFilename(string value)
        {
            base.SetConfig(618, value);
            return true;
        }

        /// <summary>
        /// 获取叠加 SWF 图像大小
        /// 格式："width;height"
        /// 默认值："150;150"
        /// ConfigID = 619
        /// </summary>
        /// <returns>叠加 SWF 图像大小</returns>
        public string GetPictureSWFSize()
        {
            return base.GetConfig(619);
        }

        /// <summary>
        /// 设置叠加 SWF 图像大小
        /// 格式："width;height"
        /// 默认值："150;150"
        /// ConfigID = 619
        /// </summary>
        /// <param name="width">叠加 SWF 图像宽度</param>
        /// <param name="height">叠加 SWF 图像高度</param>
        /// <returns></returns>
        public bool SetPictureSWFSize(int width, int height)
        {
            base.SetConfig(619, width.ToString() + ';' + height.ToString());
            return true;
        }

        /// <summary>
        /// 获取所叠加的 SWF 文件的 OLE 容器控件的指针
        /// ShockwaveFlashObjects::IShockwaveFlash* 类型
        /// ConfigID = 620
        /// </summary>
        /// <returns>所叠加的 SWF 文件的 OLE 容器控件的指针</returns>
        public int GetPictureSWFControl()
        {
            return int.Parse(base.GetConfig(620));
        }

        /// <summary>
        /// 获取 EVRCP 是否使用线形插值叠图
        /// 0-不是用，1-使用，默认0
        /// ConfigID = 621
        /// </summary>
        /// <returns> EVRCP 是否使用线形插值叠图</returns>
        public bool GetPictureEVRCPLinear()
        {
            return (int.Parse(base.GetConfig(621)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置 EVRCP 是否使用线形插值叠图
        /// 0-不是用，1-使用，默认0
        /// ConfigID = 621
        /// </summary>
        /// <param name="value">EVRCP 是否使用线形插值叠图</param>
        /// <returns></returns>
        public bool SetPictureEVRCPLinear(bool value)
        {
            base.SetConfig(621, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取是否attach to VR
        /// 设置为 0 时图片在窗口上，设置为 1 时，图片附加到视频上，而不是在当前视口，感觉就像场景中的物体，这时的可叠加范围为视频尺寸
        /// ConfigID = 622
        /// </summary>
        /// <returns>是否attach to VR</returns>
        public bool GetPictureAttachToVR()
        {
            return (int.Parse(base.GetConfig(622)) == 1 ? true : false);
        }

        /// <summary>
        /// 设置是否attach to VR
        /// 设置为 0 时图片在窗口上，设置为 1 时，图片附加到视频上，而不是在当前视口，感觉就像场景中的物体，这时的可叠加范围为视频尺寸
        /// ConfigID = 622
        /// </summary>
        /// <param name="value">是否attach to VR</param>
        /// <returns></returns>
        public bool SetPictureAttachToVR(bool value)
        {
            base.SetConfig(622, value ? "1" : "0");
            return true;
        }

        /// <summary>
        /// 获取多行文本模式时每行文本的宽度
        /// 默认2000像素
        /// ConfigID = 623
        /// </summary>
        /// <returns>多行文本模式时每行文本的宽度</returns>
        public int GetPictureLineWidth()
        {
            return int.Parse(base.GetConfig(623));
        }

        /// <summary>
        /// 设置多行文本模式时每行文本的宽度
        /// 默认2000像素
        /// ConfigID = 623
        /// </summary>
        /// <param name="value">多行文本模式时每行文本的宽度</param>
        /// <returns></returns>
        public bool SetPictureLineWidth(int value)
        {
            base.SetConfig(623, value.ToString());
            return true;
        }

        /// <summary>
        /// 获取多行文本模式时的行距
        /// 默认5像素
        /// ConfigID = 624
        /// </summary>
        /// <returns>多行文本模式时的行距</returns>
        public int GetPictureLineSpace()
        {
            return int.Parse(base.GetConfig(624));
        }

        /// <summary>
        /// 设置多行文本模式时的行距
        /// 默认5像素
        /// ConfigID = 624
        /// </summary>
        /// <param name="value">多行文本模式时的行距</param>
        /// <returns></returns>
        public bool SetPictureLineSpace(int value)
        {
            base.SetConfig(624, value.ToString());
            return true;
        }

        #endregion

        #region PART 21 | 2201 - 2207



        #endregion

        #endregion

        #region 播放器事件

        #endregion

        #region 自定义方法

        /// <summary>
        /// 播放时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string _ConvertTime(int time)
        {
            bool ShowMsec = false; //是否显示毫秒
            int hour = time / (1000 * 60 * 60);
            int min = (time - hour * (1000 * 60 * 60)) / (1000 * 60);
            int sec = (time - hour * (1000 * 60 * 60) - min * (1000 * 60)) / 1000;
            int msec = time - hour * (1000 * 60 * 60) - min * (1000 * 60) - sec * 1000;
            string res = string.Format("{0:00}:{1:00}", min, sec);
            if (hour > 0)
                res = string.Format("{0:00}:", hour) + res;
            if (ShowMsec)
                res = res + string.Format(":{0:00}", msec / 10);
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
        /// 设置Logo
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public bool _SetLogo(Bitmap bt)
        {
            Bitmap logo = new Bitmap(bt.Width, bt.Height);
            Graphics g = Graphics.FromImage(logo);
            g.Clear(Color.Black);
            g.DrawImage(bt, 0, 0, bt.Width, bt.Height);
            SetCustomLogo(logo.GetHbitmap());
            return true;
        }

        /// <summary>
        /// 获取播放进度信息
        /// 格式：00:00:00/00:00:00
        /// </summary>
        /// <returns>播放进度信息</returns>
        public string _GetPositionString()
        {
            return _ConvertTime(GetPosition()) + "/" + _ConvertTime(GetDuration());
        }

        /// <summary>
        /// 获取视频大小
        /// 格式：1920x1080（宽x高）
        /// </summary>
        /// <returns></returns>
        public string _GetVideoSize()
        {
            return base.GetVideoWidth().ToString() + 'x' + base.GetVideoHeight().ToString();
        }

        #endregion
    }
}
