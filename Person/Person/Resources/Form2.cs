using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Person.Resources; 
namespace Person.Resources
{
    public partial class Form2 : Form
    {

        private static Form1 m_mainForm = null; 
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Form1 parentForm)
        {
            InitializeComponent();
            m_mainForm = parentForm; 
        }

        private void OnClickedSetting(object sender, EventArgs e)
        {
            m_mainForm.GetControlRes(Cnt_res.e_Setting);
            this.Close(); 
        }

        private void OnClickedExit(object sender, EventArgs e)
        {
            m_mainForm.GetControlRes(Cnt_res.e_Exit);
            this.Close();
        }

        private void OnClickedCancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
