using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySchool.userControl
{
    public partial class UserControlDashboard : UserControl
    {
        private readonly SchoolDBEntities db = new SchoolDBEntities();

        public UserControlDashboard()
        {
            InitializeComponent();
            load();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void load()
        {
            int totalStudents = db.Students.Count();
            label5.Text = totalStudents.ToString();

            string totalTeacher=db.Teachers.Count().ToString();
            label6.Text = totalTeacher;

            string totalGardian=db.Guardians.Count().ToString();
            label7.Text = totalGardian;
        }
    }
}
