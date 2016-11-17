namespace NetVideoPlayer
{
    partial class MessageBoxFX
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
            this.Label_Title = new MetroFramework.Controls.MetroLabel();
            this.Label_Content = new MetroFramework.Controls.MetroLabel();
            this.Button_Confirm = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // MetroStyleManager
            // 
            this.MetroStyleManager.OwnerForm = this;
            this.MetroStyleManager.Style = MetroFramework.MetroColorStyle.Red;
            this.MetroStyleManager.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // Label_Title
            // 
            this.Label_Title.AutoSize = true;
            this.Label_Title.CustomBackground = false;
            this.Label_Title.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.Label_Title.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.Label_Title.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.Label_Title.Location = new System.Drawing.Point(3, 6);
            this.Label_Title.Name = "Label_Title";
            this.Label_Title.Size = new System.Drawing.Size(37, 19);
            this.Label_Title.Style = MetroFramework.MetroColorStyle.Red;
            this.Label_Title.StyleManager = this.MetroStyleManager;
            this.Label_Title.TabIndex = 0;
            this.Label_Title.Text = "标题";
            this.Label_Title.Theme = MetroFramework.MetroThemeStyle.Light;
            this.Label_Title.UseStyleColors = false;
            // 
            // Label_Content
            // 
            this.Label_Content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_Content.CustomBackground = false;
            this.Label_Content.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.Label_Content.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.Label_Content.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.Label_Content.Location = new System.Drawing.Point(23, 68);
            this.Label_Content.Name = "Label_Content";
            this.Label_Content.Size = new System.Drawing.Size(304, 103);
            this.Label_Content.Style = MetroFramework.MetroColorStyle.Red;
            this.Label_Content.StyleManager = this.MetroStyleManager;
            this.Label_Content.TabIndex = 1;
            this.Label_Content.Text = "内容";
            this.Label_Content.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Content.Theme = MetroFramework.MetroThemeStyle.Light;
            this.Label_Content.UseStyleColors = false;
            // 
            // Button_Confirm
            // 
            this.Button_Confirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Button_Confirm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Confirm.Highlight = false;
            this.Button_Confirm.Location = new System.Drawing.Point(126, 193);
            this.Button_Confirm.Name = "Button_Confirm";
            this.Button_Confirm.Size = new System.Drawing.Size(97, 32);
            this.Button_Confirm.Style = MetroFramework.MetroColorStyle.Red;
            this.Button_Confirm.StyleManager = this.MetroStyleManager;
            this.Button_Confirm.TabIndex = 11;
            this.Button_Confirm.Text = "确定";
            this.Button_Confirm.Theme = MetroFramework.MetroThemeStyle.Light;
            this.Button_Confirm.Click += new System.EventHandler(this.Button_Confirm_Click);
            // 
            // MessageBoxFX
            // 
            this.AcceptButton = this.Button_Confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Button_Confirm;
            this.ClientSize = new System.Drawing.Size(350, 248);
            this.Controls.Add(this.Button_Confirm);
            this.Controls.Add(this.Label_Content);
            this.Controls.Add(this.Label_Title);
            this.DisplayHeader = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxFX";
            this.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.StyleManager = this.MetroStyleManager;
            this.Text = "MessageBoxFX";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public MetroFramework.Components.MetroStyleManager MetroStyleManager;
        private MetroFramework.Controls.MetroLabel Label_Title;
        private MetroFramework.Controls.MetroLabel Label_Content;
        private MetroFramework.Controls.MetroButton Button_Confirm;
    }
}