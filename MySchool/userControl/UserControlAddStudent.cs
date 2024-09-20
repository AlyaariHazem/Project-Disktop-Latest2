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
using MySchool;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.IO;
using Myschool.Helpers;

namespace MySchool.userControl
{
    public partial class UserControlAddStudent : UserControl
    {

        private readonly SchoolDBEntities db1 = new SchoolDBEntities();
        private tools tool = new tools();
        private string image_path="";
        private  int flag = 0;
        private  int student_id = 0;
        private  int gurdian_id = 0;

        public UserControlAddStudent(int s_id=0,int student_flag=0)
        {
            InitializeComponent();
            var ClassNames = db1.Classes
               .Select(c => c.ClassName)
            .ToArray();

            tool.FillComboBox(compo_class, ClassNames);
            this.flag = student_flag;
            this.student_id = s_id;
            filldata_edit();

        }

    
       
        private void btn_save_Click(object sender, EventArgs e)
        {
           
            
            List<Control> optionalControls = new List<Control> { txtphon, txtbirthplace, txtbirthplace,txtFulltName ,txtfatheraddrees
            ,birth_date_father,txtaddress};

            if (!tool.ValidateInputs(this, optionalControls) ) 
            {

                MessageBox.Show("هناك حقول مطلوبة ");
            }

            else
            {



                using (var db = new DatabaseHelper())
                {
                    var pvalues = new Dictionary<string, object>
                {
                    { "FullName_FirstName", txtfatherName.Text },
                    { "FullName_SecondName", txtfatherName.Text },
                    { "FullName_ThirdName", txtfatherName.Text },
                    { "FullName_LastName", txtfatherName.Text },
                    { "Phone", txtfatherphon.Text },
                    { "Email", txtfatheremail.Text },
                    { "TypeGuardian", txtcall_father.Text },
                    { "UserID", Forms.Login.userid }
                };

                    try
                    {
                        if (flag == 0)
                        {
                            db.Insert("Guardians", pvalues);
                        }

                        else if (flag == 1)
                        {
                            db.Update("Guardians", pvalues, $"GuardianID={gurdian_id}");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while adding : " + ex.Message);
                    }


                    var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);
                    var selecteddiv = db1.Divisions.FirstOrDefault(c => c.DivisionName == compo_stage.Text && c.ClassID==selectedClass.ClassID);
                    var gurdian = db1.Guardians.FirstOrDefault(c => c.FullName_FirstName == txtfatherName.Text);


                    

                    var svalues = new Dictionary<string, object>
                {
                    { "FullName_FirstName", txtFirstName.Text },
                    { "FullName_SecondName", txtSecondName.Text },
                    { "FullName_ThirdName",  txtThirdName.Text },
                    { "FullName_LastName", txtLastName.Text },
                    { "DateOfBirth", birth_date_student.Value },
                    { "Gender", compo_gender.Text },
                    { "Phone", txtphon.Text },
                    { "UserID", Forms.Login.userid },
                    { "GuardianID",gurdian.GuardianID },
                    { "DivisionID", selecteddiv.DivisionID },
                    { "URLImage", image_path },
                    { "PayMent", int.Parse(pay.Text) }
                };

                    try
                    {
                        if (flag == 0)
                        {
                            db.Insert("Students", svalues);
                            MessageBox.Show("تم اضاقة الطالب بنجاح");
                            this.Controls.Clear();

                            UserControlAddStudent Student_control = new UserControlAddStudent();

                            Student_control.Dock = DockStyle.Fill;

                            this.Controls.Add(Student_control);
                           
                        }
                        
                        else if (flag == 1)
                        {

                            db.Update("Students", svalues,$"StudentID={student_id}");
                            MessageBox.Show("تم تعديل الطالب بنجاح");
                            open_redirect_addstudent_control();


                        }

                       
                      
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while adding : " + ex.Message);
                    }



                }



               

            }

        }

        private void btn_choos_image_Click(object sender, EventArgs e)
        {




            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select an Image File",
               
                CheckFileExists = true,
                CheckPathExists = true
            };

           
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
             
                string sourceFilePath = openFileDialog.FileName;

              
                string targetDirectory = Path.Combine(Application.StartupPath, "Uploads");

             
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

           
                string fileName = Path.GetFileName(sourceFilePath);
                string targetFilePath = Path.Combine(targetDirectory, fileName);

                // If the file already exists, append a number to the filename to make it unique
                int counter = 1;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);

                while (File.Exists(targetFilePath))
                {
                    string newFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                    targetFilePath = Path.Combine(targetDirectory, newFileName);
                    counter++;
                }

          
                try
                {
                    File.Copy(sourceFilePath, targetFilePath);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"An error occurred while copying the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

              
                string relativePath = Path.Combine("Uploads", Path.GetFileName(targetFilePath));

               
                picther_userimage.Image = new Bitmap(relativePath);
             
              image_path = relativePath;

               
            }


        }



    

        private void compo_class_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedClass = db1.Classes.FirstOrDefault(c => c.ClassName == compo_class.Text);

           

            var divisionnames = db1.Divisions
    .Where(div => div.ClassID == selectedClass.ClassID)
    .Select(div => div.DivisionName)
    .ToArray();

     tool.FillComboBox(compo_stage, divisionnames);

        }




        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();
        }

        private void txtSecondName_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();

        }

        private void txtThirdName_TextChanged(object sender, EventArgs e)
        {
            UpdateFullName();
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
UpdateFullName();
        }




        private void btncancel_Click(object sender, EventArgs e)
        {
            open_redirect_addstudent_control();
        }







        private void UpdateFullName()
        {
            string firstName = txtFirstName.Text.Trim();
            string secondName = txtSecondName.Text.Trim();
            string thirddName = txtThirdName.Text.Trim();
            string lastName = txtLastName.Text.Trim();


            string fullName = $"{firstName} {secondName} {thirddName}  {lastName}";

            // Display the full name in the full name TextBox.
            txtFulltName.Text = fullName;
        }



        private void filldata_edit()
        {
            if (flag == 1)
            {
                var student = db1.Students.FirstOrDefault(c => c.StudentID == student_id);
                var gurdian = db1.Guardians.FirstOrDefault(c => c.GuardianID == student.GuardianID);

                txtFirstName.Text = student.FullName_FirstName;
                txtSecondName.Text = student.FullName_SecondName;
                txtThirdName.Text = student.FullName_ThirdName;
                txtLastName.Text = student.FullName_LastName;
                birth_date_student.Value = student.DateOfBirth;
                compo_gender.Text = student.Gender;
                txtphon.Text = student.Phone;
                compo_class.Text = student.Divisions.Classes.ClassName;
                compo_stage.Text = student.Divisions.DivisionName;
               
              if(student.URLImage != null)
                {
                    picther_userimage.Image = tool.LoadImage(student.URLImage);
                    image_path = student.URLImage;

                }
              else
                {
                    picther_userimage.Image= Properties.Resources.Hazem;
                }
                pay.Text = student.PayMent.ToString();
              
                txtcall_father.Text = gurdian.TypeGuardian;
                txtfatherName.Text = gurdian.FullName_FirstName;
                txtfatherphon.Text = gurdian.Phone;
                txtfatheremail.Text = gurdian.Email;
                btncancel.Visible=true;
                gurdian_id=student.GuardianID;
            }
        }


        private void open_redirect_addstudent_control()
        {
            this.Controls.Clear();

            UserControlStudents Student_control = new UserControlStudents();

            Student_control.Dock = DockStyle.Fill;

            this.Controls.Add(Student_control);
        }



    }
}
