using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Person.Resources
{
    public partial class FormSetting : Form
    {
        private static Form1 m_mainForm = null; 

        public FormSetting()
        {
            InitializeComponent();
        }
        
        public FormSetting(Form1 parent)
        {
            m_mainForm = parent;
            InitializeComponent();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {

        }

        private void slideR1_Scroll(object sender, EventArgs e)
        {

        }
        private void slide_x1_Scroll(object sender, EventArgs e)
        {

        }

        private void slide_y1_Scroll(object sender, EventArgs e)
        {

        }
        private void slidebar_r2(object sender, EventArgs e)
        {

        }
        private void slide_x2_Scroll(object sender, EventArgs e)
        {

        }

        private void slide_y2_Scroll(object sender, EventArgs e)
        {

        }
    }
}
