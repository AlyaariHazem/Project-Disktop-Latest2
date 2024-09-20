using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.AnimatorNS;
using Guna.UI2.WinForms;
using Myschool.Helpers;
using MySchool.TOOLS_HELPER;
namespace MySchool.userControl
{
    public partial class UserControlgrade : UserControl
    {
        private readonly SchoolDBEntities db1 = new SchoolDBEntities();
        private tools tool = new tools();
        private int is_update = 0;
        private int student_id = 0;
    
        public UserControlgrade()
        {
            InitializeComponent();

            var ClassNames = db1.Classes.Select(c => c.ClassName).ToArray();
            var MONTHSNames = db1.MONTHS.Where(year => year.Year.Active == true).Select(c => c.value).ToArray();


            tool.FillComboBox(compo_class, ClassNames);
            tool.FillComboBox(compo_months, MONTHSNames);
            loaddata();
            tool.ApplyNumberValidation(txthomwork, 0, 20);
            tool.ApplyNumberValidation(txt_share, 0, 10);
            tool.ApplyNumberValidation(txt_shfahi, 0, 10);
            tool.ApplyNumberValidation(txt_attendance, 0, 10);
            tool.ApplyNumberValidation(txt_testing, 0, 40);

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void birth_date_student_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            List<Control> optionalControls = new List<Control> { txt_attendance, txtsubject, txtName, txthomwork
            ,txt_ratio,txt_share,txt_shfahi,txt_sum,txt_testing};

            if (!tool.ValidateInputs(this, optionalControls))
            {

                MessageBox.Show("هناك حقول مطلوبة ");
            }

            else
            {



                is_update = 1;

                update_grade();
                loaddata();
            }
        }

        private void compo_class_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);



            var divisionnames = db1.Divisions
    .Where(div => div.ClassID == selectedClass.ClassID)
    .Select(div => div.DivisionName)
    .ToArray();
            var subjectsnames = db1.Subjects
    .Where(div => div.ClassID == selectedClass.ClassID)
    .Select(div => div.SubjectName)
    .ToArray();

            tool.FillComboBox(compo_subjects, subjectsnames);
            tool.FillComboBox(compo_division, divisionnames);
            cheak_is_grade_student_add();
            loaddata();
        }




        private void cheak_is_grade_student_add()
        {



            if (compo_class.SelectedIndex != -1 && compo_division.SelectedIndex != -1 && compo_subjects.SelectedIndex != -1 && compo_months.SelectedIndex != -1)
            {

                bool hasDivisioncGrade = db1.Grades.Any(g => g.Students.Divisions.DivisionName == compo_division.Text && g.Students.Divisions.Classes.ClassName == compo_class.Text
                && g.Subject.SubjectName == compo_subjects.Text && g.MONTH.Year.Active == true && g.MONTH.value == compo_months.Text);

                if (!hasDivisioncGrade)
                {
                    var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);
                    var selectedDivision = db1.Divisions.FirstOrDefault(c => c.DivisionName == compo_division.Text);
                    var selectedsubject = db1.Subjects.FirstOrDefault(c => c.SubjectName == compo_subjects.Text);
                    var selectedmonth = db1.MONTHS.FirstOrDefault(c => c.value == compo_months.Text);
                    var students = db1.Students.Where(s => s.Divisions.Classes.ClassName == compo_class.Text && s.Divisions.DivisionName == compo_division.Text).ToList();
                    foreach (var student in students)
                    {



                        using (var db = new DatabaseHelper())
                        {




                            var values = new Dictionary<string, object>
                {
                    { "studentID", student.StudentID },
                    { "subject_id", selectedsubject.SubjectID },
                    { "month_id", selectedmonth.id },
                    { "testing_value", 0 },
                    { "shafahi_value", 0 },
                    { "atendance_value", 0 },
                    { "share_value", 0 },
                    { "homework_value", 0 },
                    { "user_id", Forms.Login.userid },


                };

                            try
                            {
                                db.Insert("Grades", values);


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error occurred while adding : " + ex.Message);
                            }





                        }
                    }
                }

                else
                {
                    Console.WriteLine("Grades already exist for this student.");
                }
            }



        }





        private void update_grade()
        {

            if (is_update == 1 && student_id != 0)
            {

                if (compo_class.SelectedIndex != -1 && compo_division.SelectedIndex != -1 && compo_subjects.SelectedIndex != -1 && compo_months.SelectedIndex != -1)
                {



                    var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);
                    var selectedDivision = db1.Divisions.FirstOrDefault(c => c.DivisionName == compo_division.Text);
                    var selectedsubject = db1.Subjects.FirstOrDefault(c => c.SubjectName == compo_subjects.Text);
                    var selectedmonth = db1.MONTHS.FirstOrDefault(c => c.value == compo_months.Text);





                    using (var db = new DatabaseHelper())
                    {




                        var values = new Dictionary<string, object>
                {
                    { "studentID",student_id },
                    { "subject_id", selectedsubject.SubjectID },
                    { "month_id", selectedmonth.id },
                    { "testing_value", float.Parse(txt_testing.Text) },
                    { "shafahi_value", float.Parse(txt_shfahi.Text) },
                    { "atendance_value", float.Parse(txt_attendance.Text) },
                    { "share_value", float.Parse(txt_share.Text) },
                    { "homework_value",  float.Parse(txthomwork.Text) },
                    { "user_id", Forms.Login.userid },


                };

                        try
                        {

                            db.Update("Grades", values, $"studentID={student_id} and subject_id={selectedsubject.SubjectID} " +
                                $" and month_id ={selectedmonth.id} ");

                            MessageBox.Show("ّ!تم تعديل  الدرجات بنجاح");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while adding : " + ex.Message);
                        }


                    }






                }


            }


        }



        private void InitializeDataGridViewColumns()
        {

            tool.InitializeDataGridView(guna2DataGridView2, new Dictionary<string, string>
    {
        {"StudentID", "#"},
        {"StudentName", "اسم الطالب "},
        {"subject", "المادة"},
        {"homwork", "واجبات"},
        {"attendance", "مواظبة"},
        {"share", " مشاركة"},
        {"shfahi", "شفهي"},
        {"testing", "تحريري"},
        {"sum", "المجموع"},
        {"ratio", "النسبة"}


    });



        }


        private void loaddata()
        {

            var grades = db1.Grades
                .Where(g =>
               g.Students.Divisions.Classes.ClassName.ToLower().Equals(compo_class.Text) &&
               g.Students.Divisions.DivisionName.ToLower().Equals(compo_division.Text) &&
               (g.MONTH.value.ToLower().Equals(compo_months.Text) && g.MONTH.Year.Active == true) &&
               g.Subject.SubjectName.ToLower().Equals(compo_subjects.Text))
                .Select(g => new
                {
                    studentid = g.Students.StudentID,
                    first_name = g.Students.FullName_FirstName,
                    socend_name = g.Students.FullName_SecondName,
                    third_name = g.Students.FullName_ThirdName,
                    last_name = g.Students.FullName_LastName,
                    subject_name = g.Subject.SubjectName,
                    howmwork = g.homework_value,
                    attendance = g.atendance_value,
                    share = g.share_value,
                    shfahi = g.shafahi_value,
                    testing = g.testing_value

                })
            .ToList();
            
            tool.LoadDataIntoDataGridView(guna2DataGridView2, grades, _grade => new object[]
            {
        _grade.studentid,
        _grade.first_name + " " + _grade.socend_name + " " + _grade.third_name + " " + _grade.last_name,
        _grade.subject_name,
        _grade.howmwork,
        _grade.attendance,
        _grade.share,
        _grade.shfahi,
        _grade.testing,
        _grade.howmwork+  _grade.attendance+ _grade.share+_grade.shfahi+  _grade.testing,
        (_grade.howmwork+  _grade.attendance+ _grade.share+_grade.shfahi+  _grade.testing)%100
            });
        }

        private void compo_division_SelectedIndexChanged(object sender, EventArgs e)
        {
            cheak_is_grade_student_add();
            loaddata();
        }

        private void compo_subjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            cheak_is_grade_student_add();
            loaddata();
        }

        private void compo_months_SelectedIndexChanged(object sender, EventArgs e)
        {
            cheak_is_grade_student_add();
            loaddata();
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {




                student_id = int.Parse(guna2DataGridView2.Rows[e.RowIndex].Cells["StudentID"].Value.ToString());
                txtName.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["StudentName"].Value.ToString();
                txtsubject.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["subject"].Value.ToString();
                txthomwork.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["homwork"].Value.ToString();
                txt_attendance.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["attendance"].Value.ToString();
                txt_share.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["share"].Value.ToString();
                txt_shfahi.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["shfahi"].Value.ToString();
                txt_testing.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["testing"].Value.ToString();
                txt_sum.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["sum"].Value.ToString();
                txt_ratio.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["ratio"].Value.ToString();



            }

        }

        private void guna2DataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
