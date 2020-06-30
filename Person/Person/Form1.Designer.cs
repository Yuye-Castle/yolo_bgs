namespace Person
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cam_left = new System.Windows.Forms.PictureBox();
            this.cam_right = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.cam_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cam_right)).BeginInit();
            this.SuspendLayout();
            // 
            // cam_left
            // 
            resources.ApplyResources(this.cam_left, "cam_left");
            this.cam_left.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cam_left.Name = "cam_left";
            this.cam_left.TabStop = false;
            // 
            // cam_right
            // 
            resources.ApplyResources(this.cam_right, "cam_right");
            this.cam_right.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cam_right.Name = "cam_right";
            this.cam_right.TabStop = false;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cam_right);
            this.Controls.Add(this.cam_left);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cam_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cam_right)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox cam_left;
        private System.Windows.Forms.PictureBox cam_right;
    }
}

