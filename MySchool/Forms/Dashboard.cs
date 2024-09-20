using MySchool.userControl;
using MySchool.userControl.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySchool.Forms
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();

            panel1.Controls.Clear();
            UserControlDashboard userControlDashboard = new UserControlDashboard();
            userControlDashboard.Dock = DockStyle.Fill;
            panel1.Controls.Add(userControlDashboard);

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Get the selected node
            TreeNode selectedNode = e.Node;

            // Build a message to display information about the selected node
            string message = selectedNode.Text;

            if (message.ToLower() == "المراحل والصفوف")
            {
                LoadUserControlStage();


            }
            else if(message.ToLower() == "جميع الطلاب")
            {
                LoadUserControlStudents();
            }  
            else if(message.ToLower() == "جميع المدرسين")
            {
                LoadUserControlTetchers();
            }
            else if (message.ToLower() == "عرض المواد")
            {
                LoadUserControlSubject();
            }
            else if (message.ToLower() == "التقويم")
            {
                LoadUserControlReports();
            }
            else if (message.ToLower() == "شهادة شهرية")
            {
                LoadUserControlcertificate();
            }
            else if (message.ToLower() == "شهادة سنوية")
            {
                LoadUserControlcertificateForYear();
            }   else if (message.ToLower() == "اضافة الدرجات")
            {
                LoadUserControlgrades();
            }

        }

        private void LoadUserControlcertificateForYear()
        {
            panel1.Controls.Clear();

            UserControlReports userControlReports = new UserControlReports();

            userControlReports.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlReports);
        }

        private void LoadUserControlcertificate()
        {
            panel1.Controls.Clear();

            UserControlCertifcate userControlCertifcate = new UserControlCertifcate();

            userControlCertifcate.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlCertifcate);
        }

        private void LoadUserControlReports()
        {
            panel1.Controls.Clear();

            UserControlReports userControlReports = new UserControlReports();

            userControlReports.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlReports);
        }

        private void LoadUserControlStage()
        {
            // Clear the panel before adding a new control
            panel1.Controls.Clear();

            userControlStages userControlStages = new userControlStages();

            userControlStages.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlStages);
        }
        private void LoadUserControlStudents()
        {
            panel1.Controls.Clear();

            UserControlStudents userControlStudents = new UserControlStudents();

            userControlStudents.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlStudents);
        }  
        
        
        private void LoadUserControlTetchers()
        {
            panel1.Controls.Clear();

            UserControlTeachers userControlTetcher= new UserControlTeachers();

            userControlTetcher.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlTetcher);
        }

        private void LoadUserControlSubject()
        {
            panel1.Controls.Clear();

            UserControlSubjects userControlSubjects = new UserControlSubjects();

            userControlSubjects.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlSubjects);
        }


        private void LoadUserControlgrades()
        {
            panel1.Controls.Clear();

            UserControlgrade userControlgrades = new UserControlgrade();

            userControlgrades.Dock = DockStyle.Fill;

            panel1.Controls.Add(userControlgrades);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
