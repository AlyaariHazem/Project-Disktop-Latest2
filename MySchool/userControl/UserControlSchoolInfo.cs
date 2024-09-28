using System;
using System.Linq;
using System.Windows.Forms;

namespace MySchool.userControl
{
    public partial class UserControlSchoolInfo : UserControl
    {
        // Assuming db is your database context, which you may need to pass or initialize.
        private readonly SchoolDBEntities db = new SchoolDBEntities();

        public UserControlSchoolInfo()
        {
            InitializeComponent();
        }

        private void UserControlSchoolInfo_Load(object sender, EventArgs e)
        {
            LoadSchoolData();
        }

        private void LoadSchoolData()
        {
            // Assuming there's only one school or you're fetching a specific one by ID.
            var schoolData = db.Schools.FirstOrDefault(s=>s.SchoolID==1);

            if (schoolData != null)
            {
                lblSchoolName.Text = "School Name: " + schoolData.SchoolName;
                lblSchoolCreationDate.Text = "Creation Date: " + schoolData.School_Crea_Date.ToString("d");
                lblSchoolVision.Text = "Vision: " + schoolData.SchoolVison;
                lblSchoolMission.Text = "Mission: " + schoolData.SchoolMission;
                lblSchoolGoal.Text = "Goal: " + schoolData.SchoolGoal;
                lblSchoolPhone.Text = "Phone: " + schoolData.SchoolPhone;
                lblSchoolEmail.Text = "Email: " + schoolData.Email;
                lblSchoolFax.Text = "Fax: " + (schoolData.fax.HasValue ? schoolData.fax.ToString() : "N/A");
                lblSchoolAddress.Text = $"Address: {schoolData.Street}, {schoolData.City}, {schoolData.Country}, Zone: {schoolData.zone}";
            }
            else
            {
                MessageBox.Show("School data not found.");
            }
        }
    }
}
