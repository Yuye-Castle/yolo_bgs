using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.XImgproc;
using Person.Resources;

namespace Person
{
    public enum Cnt_res{
        e_Setting, 
        e_Exit
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Init_controls(); 
            Register_handler(); 
        }

        private void Init_controls()
        {
            cam_left.Image = Properties.Resources.test;
            cam_right.Image = Properties.Resources.test;
        }
        private void Register_handler()
        {
            this.Resize += Handler_resize;
            cam_left.MouseClick += MouseEventHandler;
            cam_right.MouseClick += MouseEventHandler; 
        }

        private void MouseEventHandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point position = e.Location;

            String ss = "";
            if (e.Button != MouseButtons.Right)
                return; 
            Control ctrl = (Control)sender;
            position = ctrl.PointToScreen(position); 

            String sz = String.Format("{0}: x = {1}: y = {2}\n", ss, position.X, position.Y); 
            Console.WriteLine(sz);

            // create control window
            Form2 cnt_panel = new Form2(this);
            cnt_panel.Show();
            cnt_panel.Location = position; 
        }
        
        public void GetControlRes(Cnt_res res)
        {
            Console.WriteLine("Recieve feedback\n"); 
            if(res == Cnt_res.e_Setting) // open setting window
            {

            }
            else if(res == Cnt_res.e_Exit) // exit window
            {

            }
        }
        

        private void Handler_resize(Object sender, EventArgs e)
        {
            // Control ctrl = (Control)sender;
            Size sz = this.ClientSize; 
            int width = sz.Width;
            int height = sz.Height;
            const int margin = 2;
            int w = (width - 3 * margin - 5) / 2;
            int h = (height - 2 * margin) / 2; 
            cam_left.Location = new Point(margin, margin);
            cam_left.Size = new Size(w, h);

            cam_right.Location = new Point(2 * margin + w, margin);
            cam_right.Size = new Size(w, h); 
        }
    }
}
