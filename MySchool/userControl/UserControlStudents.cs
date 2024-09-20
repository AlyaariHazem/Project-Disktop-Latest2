using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySchool.TOOLS_HELPER;
using static Guna.UI2.Native.WinApi;
using System.IO;
using System.Deployment.Internal;

namespace MySchool.userControl
{
    public partial class UserControlStudents : UserControl
    {
        private readonly SchoolDBEntities db = new SchoolDBEntities();

        private tools tool=new tools();
        public UserControlStudents()
        {
            InitializeComponent();
            tool.StyleDataGridView(guna2DataGridView1); 
            InitializeDataGridViewColumns();
            loaddata();
        }

      
       

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            open_addstudent_control();

        }


        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure it's not a header row
            {
                DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex]; if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Edit")
                {
                    

                    int student_id = (int)guna2DataGridView1.Rows[e.RowIndex].Cells["StudentID"].Value;
                    
                    open_addstudent_control(student_id,1);

                }
                else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Delete")
                {

              

                    int studentId = (int)guna2DataGridView1.Rows[e.RowIndex].Cells["StudentID"].Value;
                    var result = MessageBox.Show("هل أنت متأكد من أنك تريد حذف الطالب ؟", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        var student = db.Students.Find(studentId);
                        if (student != null)
                        {
                            db.Students.Remove(student);
                            db.SaveChanges();
                            MessageBox.Show("ّ!تم حذف الطالب بنجاح");

                            loaddata();
                        }
                        else
                        {
                            MessageBox.Show(".الطالب غير موجود");
                        }
                    }

                }
            }
        }



    

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            loaddata();
        }



        private void InitializeDataGridViewColumns()
        {

            tool.InitializeDataGridView(guna2DataGridView1, new Dictionary<string, string>
    {
        {"StudentID", "#"},
        {"StudentName", "اسم الطالب "},
        {"Stage", "المرحلة"},
        {"Class1", "الصـف"},
        {"Division", "الشعبة"},
        {"Age", "تاريخ المبلاد"},
        {"Type", "النوع"},

    }, new Dictionary<string, (string, string)>
    {
        {"Delete", ("حذف", "حذف")},
        {"Edit", ("تعديل", "تعديل")}
    },
     new Dictionary<string, string>
    {
        { "ProfilePicture", "الصورة" }
    });



        }


        private void loaddata()
        {

            var students = db.Students
                .Where(s => string.IsNullOrEmpty(txtsearch.Text) ||
                            s.FullName_FirstName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.FullName_SecondName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.FullName_ThirdName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.FullName_LastName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.Divisions.DivisionName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.Divisions.Classes.ClassName.ToLower().Contains(txtsearch.Text.ToLower()))
                .Select(s => new
                {
                    studentid = s.StudentID,
                    first_name = s.FullName_FirstName,
                    socend_name = s.FullName_SecondName,
                    third_name = s.FullName_ThirdName,
                    last_name = s.FullName_LastName,
                    stge_name = s.Divisions.Classes.Stages.StageName,
                    class_name = s.Divisions.Classes.ClassName,
                    div_name = s.Divisions.DivisionName,
                    birth_ofdate = s.DateOfBirth,
                    gurdian_name = s.Guardians.FullName_FirstName,
                    gender = s.Gender,
                    imag_path = s.URLImage
                })
                .ToList();

            tool.LoadDataIntoDataGridView(guna2DataGridView1, students, _student => new object[]
            {
        _student.studentid,
        _student.first_name + " " + _student.socend_name + " " + _student.third_name + " " + _student.last_name,
        _student.stge_name,
        _student.class_name,
        _student.div_name,
        _student.birth_ofdate,
        _student.gender,
        tool.LoadImage(_student.imag_path)
            });
        }



        private void open_addstudent_control(int id = 0, int flag = 0)
        {
            this.Controls.Clear();

            UserControlAddStudent userControlAddStudent = new UserControlAddStudent(id, flag);

            userControlAddStudent.Dock = DockStyle.Fill;

            this.Controls.Add(userControlAddStudent);
        }





    }
}
