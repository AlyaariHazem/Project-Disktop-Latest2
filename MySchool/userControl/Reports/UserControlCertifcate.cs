using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Bouncycastle;
using iText.Layout.Element;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using MySchool.TOOLS_HELPER;
using static iText.Svg.SvgConstants;
using System.Xml.Linq;

namespace MySchool.userControl.Reports
{
    public partial class UserControlCertifcate : UserControl
    {
        string pdfPath;
        private readonly SchoolDBEntities db1 = new SchoolDBEntities();
        private tools tool = new tools();
        public UserControlCertifcate()
        {
            InitializeComponent();
            var ClassNames = db1.Classes.Select(c => c.ClassName).ToArray();
            var MONTHSNames = db1.MONTHS.Where(year => year.Year.Active == true).Select(c => c.value).ToArray();


            tool.FillComboBox(compo_class, ClassNames);
            tool.FillComboBox(compo_months, MONTHSNames);
        }

        private void UserControlCertifcate_Load(object sender, EventArgs e)
        {

        }




       

        private void compo_class_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);



            var divisionnames = db1.Divisions
        .Where(div => div.ClassID == selectedClass.ClassID)
        .Select(div => div.DivisionName)
        .ToArray();

            tool.FillComboBox(compo_division, divisionnames);
        }


        private void compo_division_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);
            var selecteddivistion = db1.Divisions.FirstOrDefault(c => c.DivisionName == compo_division.Text && c.ClassID == selectedClass.ClassID);

            var students = db1.Students
            .Where(div => div.DivisionID == selecteddivistion.DivisionID)
            .Select(div => new
            {
                StudentID = div.StudentID, // Return value
                FullName = div.FullName_FirstName + " " + div.FullName_SecondName + " " + div.FullName_ThirdName + " " + div.FullName_LastName // Display value
            })
            .ToList();

            compo_students.DataSource = students;

            compo_students.DisplayMember = "FullName";
            compo_students.ValueMember = "StudentID";
        }

        private void compo_months_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            refrech();

        }

        private void btn_save_Click_1(object sender, EventArgs e)
        {
            if (!tool.ValidateInputs(this))
            {

                MessageBox.Show("هناك حقول مطلوبة ");
            }

            else
            {

                refrech_cert();
                PrintPanelToPdf(panelToPrint);

            }
        }
      



   
        private void PrintPanelToPdf(Panel panel)
        {
            // Create a bitmap from the panel
            Bitmap bmp = new Bitmap(panel.Width, panel.Height);
            panel.DrawToBitmap(bmp, new Rectangle(0, 0, panel.Width, panel.Height));

            // Save bitmap to a memory stream
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                // Specify the path to save the PDF document
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "التقارير");
                Directory.CreateDirectory(folderPath); // Create the directory if it doesn't exist
                pdfPath = Path.Combine(folderPath, classname.Text+"_"+divitionname.Text+"_" +studentName.Text+".pdf");

                // Create PDF document
                using (PdfWriter writer = new PdfWriter(pdfPath))
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    // Add image to PDF
                    iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(ms.ToArray()));
                    document.Add(pdfImage);

                    document.Close();
                }

                // Open the PDF in Edge or the default browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true // Important for opening files with their associated applications
                });
            }

            MessageBox.Show($"PDF created successfully at {pdfPath}!");
        }

        private void refrech_cert()
        {
            int StudentID = (int)compo_students.SelectedValue;
            float total = get_subject_grade(label27.Text, StudentID) + get_subject_grade(label26.Text, StudentID)
                + get_subject_grade(label25.Text, StudentID) + get_subject_grade(label24.Text, StudentID)
                + get_subject_grade(label23.Text, StudentID);
            studentName.Text = compo_students.Text;
            studentnum.Text = StudentID.ToString();
            classname.Text = compo_class.Text;
            divitionname.Text = compo_division.Text;
            monthname.Text = compo_months.Text;

            garde1.Text = get_subject_grade(label23.Text, StudentID).ToString();
            garde2.Text = get_subject_grade(label24.Text, StudentID).ToString();
            garde3.Text = get_subject_grade(label25.Text, StudentID).ToString();
            garde4.Text = get_subject_grade(label26.Text, StudentID).ToString();
            garde5.Text = get_subject_grade(label27.Text, StudentID).ToString();
            gardesum.Text = total.ToString();
            garderatio.Text = (total / 5).ToString();
            if (total < 250) result.Text = "راسب";
            else result.Text = "ناجح";


        }


        private float get_subject_grade(string subject_name, int student_id)
        {

            var grade = db1.Grades
                  .FirstOrDefault(g =>
                 g.Students.Divisions.Classes.ClassName.ToLower().Equals(compo_class.Text) &&
                 g.Students.Divisions.DivisionName.ToLower().Equals(compo_division.Text) &&
                 (g.MONTH.value.ToLower().Equals(compo_months.Text) && g.MONTH.Year.Active == true) &&
                 g.Subject.SubjectName.ToLower().Equals(subject_name)
                 && g.studentID == student_id
                 );



            if (grade == null) return 0;

            return (float)grade.homework_value + (float)grade.atendance_value + (float)grade.share_value + (float)grade.shafahi_value + (float)grade.testing_value;


        }


        private void refrech()
        {
            if (!tool.ValidateInputs(this))
            {

                MessageBox.Show("هناك حقول مطلوبة ");
            }

            else
            {

                refrech_cert();
            }

        }

        private void compo_students_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

     
    }
}
