using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO; 
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
using Emgu.CV.CvEnum;
using Person.Resources;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Person
{
    public enum Cnt_res{
        e_Setting, 
        e_Exit
    }

    public enum BORDER_TYPE
    {
        e_border_circle, 
        e_border_poly
    }

    [StructLayout(LayoutKind.Sequential, Pack =4)]
    public unsafe struct polygon
    {
        public int nNodes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public float[] x_nodes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public float[] y_nodes;

    }
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _SETTING_INFO{
        public BORDER_TYPE type; 
        public PointF  pt1;    // center point of boundary1 of left camera 
        public PointF pt2;    // center point of boundary2 of left camera 
        public float rad1;         // radius of boundary1 of left camera 
        public float rad2;         // radius of boundary2 of left camera 
        public PointF pt3;    // center point of circle1 of right camera
        public PointF pt4;    // center point of circle2 of right camera
        public float rad3;         //radius of circle 1 of right caemra
        public float rad4;         // radius of circle 2 of right caemra. 
        public polygon border1_1;
        public polygon border1_2;
        public polygon border2_1;
        public polygon border2_2; 
    }

    public partial class Form1 : Form
    {
        private String m_szExeDir;      // exe directory
        private string m_szSettingFile; // setting file path
        private string m_szLeftVideo;
        private string m_szRightVideo; 
        private _SETTING_INFO  m_setting;   // setting
        private Mat  m_imgLeft, m_imgRight;
        private Size m_szLeftImg, m_szRightImg;
        private Size m_rtLeft, m_rtRight;
        private float m_LRatio, m_RRatio;  
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Init_controls(); 
            Register_handler();
            if (!LoadVideo())
            {
                Console.WriteLine("cannot load default video\n"); 
            }

            if(!LoadSetting())
            {
                Console.WriteLine("Failed to load setting\n"); 
            }
            DrawLeftImage(m_imgLeft);
            DrawRightImage(m_imgRight); 
        }

        private bool LoadVideo()
        {
            String szExe = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            m_szExeDir = new Uri(System.IO.Path.GetDirectoryName(szExe)).LocalPath;

            m_szLeftVideo = Path.Combine(new string[] { m_szExeDir, "data", "left.mp4" }); 
            m_szRightVideo = Path.Combine(new string[] { m_szExeDir, "data", "right.mp4" });
            if (!File.Exists(m_szLeftVideo) || !File.Exists(m_szRightVideo))
                return false;

            Emgu.CV.VideoCapture lCap = new Emgu.CV.VideoCapture(m_szLeftVideo);
            Emgu.CV.VideoCapture rCap = new Emgu.CV.VideoCapture(m_szRightVideo);
            if (!lCap.IsOpened || !rCap.IsOpened) return false;

            m_imgLeft = lCap.QueryFrame();
            m_imgRight = rCap.QueryFrame();

            m_szLeftImg = new Size(m_imgLeft.Cols, m_imgLeft.Rows) ;
            m_szRightImg = new Size(m_imgRight.Cols, m_imgRight.Rows);
            lCap.Stop();
            rCap.Stop();
            //DrawLeftImage(ref m_imgLeft); 
            return true; 
        }

        
        void DrawLeftImage(Mat _image)
        {
            m_rtLeft = cam_left.Size;
            int W = m_rtLeft.Width;
            int H = m_rtLeft.Height;
            int w = _image.Cols;
            int h = _image.Rows;

            m_LRatio = Math.Min((float)W / w, (float)H / h);
            Size szValid = new Size((int)(w * m_LRatio), (int)(h * m_LRatio));
            Mat image2 = new Mat(H, W, DepthType.Cv8U, 3); 
            CvInvoke.Resize(_image, image2, szValid);
            int l = (W - szValid.Width);
            int t = (H - szValid.Height) / 2;
            Rectangle roi = new Rectangle(l, t, szValid.Width, szValid.Height);
            Mat back = new Mat( m_rtLeft.Height, m_rtLeft.Width, DepthType.Cv8U, 3);
            back.SetTo(new Bgr(50, 50, 50).MCvScalar);
            image2.CopyTo(new Mat(back, roi)); 

            //CvInvoke.CvtColor(_image, image, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            Bitmap bmp= back.ToImage<Bgr, byte>().ToBitmap();
            cam_left.Image = bmp;
            
        }


        void DrawRightImage(Mat _image)
        {
            m_rtRight = cam_right.Size;
            int W = m_rtRight.Width;
            int H = m_rtRight.Height;
            int w = _image.Cols;
            int h = _image.Rows;

            m_RRatio = Math.Min((float)W / w, (float)H / h);
            Size szValid = new Size((int)(w * m_LRatio), (int)(h * m_LRatio));
            Mat image2 = new Mat(H, W, DepthType.Cv8U, 3);
            CvInvoke.Resize(_image, image2, szValid);
            int l = 0 ;
            int t = (H - szValid.Height) / 2;
            Rectangle roi = new Rectangle(l, t, szValid.Width, szValid.Height);
            Mat back = new Mat(m_rtRight.Height, m_rtRight.Width, DepthType.Cv8U, 3);
            back.SetTo(new Bgr(50, 50, 50).MCvScalar);
            image2.CopyTo(new Mat(back, roi));

            //CvInvoke.CvtColor(_image, image, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            Bitmap bmp = back.ToImage<Bgr, byte>().ToBitmap();
            cam_right.Image = bmp;

        }

        private bool  LoadSetting()
        {

            m_szSettingFile = m_szExeDir + "\\" + "setting.ini";
            string[] lines = System.IO.File.ReadAllLines(m_szSettingFile);
            if (lines.Length != 17) return false; 
            try
            {
                m_setting.type = (BORDER_TYPE)Utils.getFirstFloat(lines[0]);
                m_setting.pt1.X = Utils.getFirstFloat(lines[1]);
                m_setting.pt1.Y = Utils.getFirstFloat(lines[2]);
                m_setting.rad1 = Utils.getFirstFloat(lines[3]);
                m_setting.pt2.X = Utils.getFirstFloat(lines[4]);
                m_setting.pt2.Y = Utils.getFirstFloat(lines[5]);
                m_setting.rad2 = Utils.getFirstFloat(lines[6]);
                m_setting.pt3.X = Utils.getFirstFloat(lines[7]);
                m_setting.pt3.Y = Utils.getFirstFloat(lines[8]);
                m_setting.rad3= Utils.getFirstFloat(lines[9]);
                m_setting.pt4.X = Utils.getFirstFloat(lines[10]);
                m_setting.pt4.Y = Utils.getFirstFloat(lines[11]);
                m_setting.rad4= Utils.getFirstFloat(lines[12]);
                Utils.getFloadArray(lines[13], m_szLeftImg.Width, m_szLeftImg.Height, ref m_setting.border1_1);
                Utils.getFloadArray(lines[14], m_szLeftImg.Width, m_szLeftImg.Height, ref m_setting.border1_2);
                Utils.getFloadArray(lines[15], m_szRightImg.Width, m_szRightImg.Height, ref m_setting.border2_1);
                Utils.getFloadArray(lines[16], m_szRightImg.Width, m_szRightImg.Height, ref m_setting.border2_2);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message); 
            }
            return true; 
        }

        private void ConvertSettingDown()
        {
            float W = m_szLeftImg.Width;
            float H = m_szLeftImg.Height;
            float W1 = m_szRightImg.Width;
            float H1 = m_szRightImg.Height;

            m_setting.pt1.X = ((float)m_setting.pt1.X) / W;
            m_setting.pt1.Y = ((float)m_setting.pt1.Y) / H;
            m_setting.pt2.X = ((float)m_setting.pt2.X) / W;
            m_setting.pt2.Y = ((float)m_setting.pt2.Y) / H;
            m_setting.rad1 /= W;
            m_setting.rad2 /= W;
            m_setting.pt3.X = ((float)m_setting.pt3.X) / W1;
            m_setting.pt3.Y = ((float)m_setting.pt3.Y) / H1;
            m_setting.pt4.X = ((float)m_setting.pt4.X) / W1;
            m_setting.pt4.Y = ((float)m_setting.pt4.Y) / H1;
            m_setting.rad3 /= W;
            m_setting.rad4 /= W;


            int n = m_setting.border1_1.nNodes; 
            for(int i = 0; i < n; i++)
            {
                m_setting.border1_1.x_nodes[i] /= W;
                m_setting.border1_1.y_nodes[i] /= H;
            }

            n = m_setting.border1_2.nNodes;
            for (int i = 0; i < n; i++)
            {
                m_setting.border1_2.x_nodes[i] /= W;
                m_setting.border1_2.y_nodes[i] /= H;
            }

            n = m_setting.border2_1.nNodes;
            for (int i = 0; i < n; i++)
            {
                m_setting.border2_1.x_nodes[i] /= W1;
                m_setting.border2_1.y_nodes[i] /= H1;
            }

            n = m_setting.border2_2.nNodes;
            for (int i = 0; i < n; i++)
            {
                m_setting.border2_2.x_nodes[i] /= W1;
                m_setting.border2_2.y_nodes[i] /= H1;
            }
        }

        private void Init_controls()
        {
            cam_left.Image = Properties.Resources.test;
            cam_right.Image = Properties.Resources.test;
        }
        private void Register_handler()
        {
            this.FormClosing += Form1_FormClosing; 
            this.Resize += Handler_resize;
            cam_left.MouseClick += MouseEventHandler;
            cam_right.MouseClick += MouseEventHandler; 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConvertSettingDown(); 
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
                FormSetting fmSetting = new FormSetting(this);
                fmSetting.Show(); 
            }
            else if(res == Cnt_res.e_Exit) // exit window
            {
                this.Close(); 
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
            int h = (height - 2 * margin); 
            cam_left.Location = new Point(margin, margin);
            cam_left.Size = new Size(w, h);

            cam_right.Location = new Point(2 * margin + w, margin);
            cam_right.Size = new Size(w, h);

            cam_left.SizeMode = PictureBoxSizeMode.StretchImage;
            cam_right.SizeMode = PictureBoxSizeMode.StretchImage;

            DrawLeftImage(m_imgLeft);
            DrawRightImage(m_imgRight); 
        }
    }
}
