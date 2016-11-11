namespace NetVideoPlayer
{
    partial class About
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
            this.SuspendLayout();
            // 
            // MetroStyleManager
            // 
            this.MetroStyleManager.OwnerForm = this;
            this.MetroStyleManager.Style = MetroFramework.MetroColorStyle.Red;
            this.MetroStyleManager.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 420);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "About";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.StyleManager = this.MetroStyleManager;
            this.Text = "关于本软件";
            this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Components.MetroStyleManager MetroStyleManager;
    }
}