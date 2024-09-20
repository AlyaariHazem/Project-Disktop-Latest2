
using Guna.UI2.AnimatorNS;
using Guna.UI2.WinForms;
using Myschool.Helpers;
using MySchool.TOOLS_HELPER;
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
    public partial class UserControlTeachers : UserControl
    {
        private readonly SchoolDBEntities db1 = new SchoolDBEntities();
        private tools tool = new tools();
        private int flag = 0;
        private int tetcherid = 0;
        public UserControlTeachers()
        {
            InitializeComponent();

         

            // Initialize DataGridView columns
            tool.InitializeDataGridView(guna2DataGridView2, new Dictionary<string, string>
            {
                {"TeacherID", "#"},
                {"TeacherName", "اسم الإستاذ"},
                {"Subject", "البريد الإلكتروني"},
                {"Phone", "الجوال"},
                {"Type", "النوع"}
            }, new Dictionary<string, (string, string)>
            {
                {"Edit", ("تعديل", "تعديل")},
                {"Delete", ("حذف", "حذف")}
            });



            loaddata();

        }



        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "Delete")
                {
                    int teacherID = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["TeacherID"].Value);
                    DeleteTeacher(teacherID);
                }
                
                else if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "Edit")
                {
                    tetcherid= Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["TeacherID"].Value);
                    flag = 1;
                    tool.ResetForms(this);
                    filldata_edit();

                }
            }
        }



        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            loaddata();
        }


       

        private void btnsave_Click(object sender, EventArgs e)
        {
            List<Control> optionalControls = new List<Control> { birthofdate, txtemail,txtsearch,txtfull_name};

            if (!tool.ValidateInputs(this, optionalControls))
            {

                MessageBox.Show("هناك حقول مطلوبة ");
            }

            else
            {



                using (var db = new DatabaseHelper())
                {
                    var values = new Dictionary<string, object>
                {
                    { "FullName_FirstName", txtfirst_name.Text },
                    { "FullName_SecondName", txtsocend_name.Text },
                    { "FullName_ThirdName", txtthird_name.Text },
                    { "FullName_LastName", txtlast_name.Text },
                    { "PhoneNum", txtphon.Text },
                    { "Email", txtemail.Text },
                    { "Gender", combo_gender.Text },
                    { "Age", CalculateAge(birthofdate.Value) },
                    { "HireDate", hirdate.Value },
                    { "ManagerID", 1 },
                    { "UserID", Forms.Login.userid }
                  
                };

                    try
                    {
                        if (flag == 0)
                        {
                            db.Insert("Teachers", values);
                            MessageBox.Show("ّ!تم اضافة الإستاذ بنجاح");
                           
                        }

                        else if (flag == 1)
                        {

                            db.Update("Teachers", values, $"TeacherID={tetcherid}");
                            MessageBox.Show("ّ!تم تعديل الإستاذ بنجاح");
                          
                        }
                        tool.ResetForms(this);
                        loaddata();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while adding : " + ex.Message);
                    }

                }

            }

        }


      
        private void txtfirst_name_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();
        }

        private void txtsocend_name_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();

        }

        private void txtthird_name_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();

        }

        private void txtlast_name_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();

        }

       
        /// /////////////////////
        
        private void loaddata()
        {

            var tetchers = db1.Teachers
                .Where(s => string.IsNullOrEmpty(txtsearch.Text) ||
                            s.FullName_FirstName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.FullName_SecondName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.FullName_ThirdName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.FullName_LastName.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.Gender.ToLower().Contains(txtsearch.Text.ToLower()) ||
                            s.PhoneNum.ToLower().Contains(txtsearch.Text.ToLower()))
                .Select(s => new
                {
                    techtid = s.TeacherID,
                    first_name = s.FullName_FirstName,
                    socend_name = s.FullName_SecondName,
                    third_name = s.FullName_ThirdName,
                    last_name = s.FullName_LastName,
                    email = s.Email,
                    phone = s.PhoneNum,
                    gender = s.Gender

                })
                .ToList();

            tool.LoadDataIntoDataGridView(guna2DataGridView2, tetchers, _teatcher => new object[]
            {
        _teatcher.techtid,
        _teatcher.first_name + " " + _teatcher.socend_name + " " + _teatcher.third_name + " " + _teatcher.last_name,
        _teatcher.email,
        _teatcher.phone,
        _teatcher.gender,

            });
        }


        private void filldata_edit()
        {
            if (flag == 1)
            {
                var teaher = db1.Teachers.FirstOrDefault(c => c.TeacherID == tetcherid);


                txtfirst_name.Text = teaher.FullName_FirstName;
                txtsocend_name.Text = teaher.FullName_SecondName;
                txtthird_name.Text = teaher.FullName_ThirdName;
                txtlast_name.Text = teaher.FullName_LastName;
                txtemail.Text = teaher.Email;
                txtphon.Text = teaher.PhoneNum;
                combo_gender.Text = teaher.Gender;
                hirdate.Value = teaher.HireDate;

            }
        }





        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            // If the birthday hasn't occurred yet this year, subtract 1 from the age
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }


        private void DeleteTeacher(int teacherID)
        {
            var result = MessageBox.Show("هل أنت متأكد من أنك تريد حذف الإستاذ ؟", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var teachr = db1.Teachers.Find(teacherID);
                if (teachr != null)
                {
                    db1.Teachers.Remove(teachr);
                    db1.SaveChanges();
                    MessageBox.Show("ّ!تم حذف الإستاذ بنجاح");

                    loaddata();
                }
                else
                {
                    MessageBox.Show(".الإستاذ غير موجود");
                }
            }
        }



        private void UpdateFullName()
        {
            string firstName = txtfirst_name.Text.Trim();
            string secondName = txtsocend_name.Text.Trim();
            string thirddName = txtthird_name.Text.Trim();
            string lastName = txtlast_name.Text.Trim();


            string fullName = $"{firstName} {secondName} {thirddName}  {lastName}";

            txtfull_name.Text = fullName;
        }






    }
}
