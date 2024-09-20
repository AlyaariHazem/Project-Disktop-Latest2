using Guna.UI2.WinForms;
using MySchool.TOOLS_HELPER;
using System;
using System.Collections.Generic;
using System.Data.Entity; // For Entity Framework 6
using System.Linq;
using System.Windows.Forms;

namespace MySchool.userControl
{
    public partial class UserControlSubjects : UserControl
    {
        private readonly SchoolDBEntities db = new SchoolDBEntities();
        private tools tool = new tools();
        private int? currentEditingSubjectID;

        public UserControlSubjects()
        {
            InitializeComponent();

            // Style the DataGridView
            tool.StyleDataGridView(guna2DataGridView2);

            // Fill the ComboBox with class data
            var classData = db.Classes
                .Select(C => new { C.ClassID, C.ClassName })
                .ToList();
            guna2ComboBox1.DataSource = classData;
            guna2ComboBox1.DisplayMember = "ClassName";
            guna2ComboBox1.ValueMember = "ClassID";

            // Initialize DataGridView columns
            tool.InitializeDataGridView(guna2DataGridView2, new Dictionary<string, string>
            {
                {"SubjectID", "#"},
                {"SubjectName", "اسم المادة"},
                {"ClassName", "الصـف"}
            }, new Dictionary<string, (string, string)>
            {
                {"Delete", ("حذف", "حذف")},
                {"Edit", ("تعديل", "تعديل")}
            });

            // Load subjects data
            LoadSubjects();

            // Subscribe to the CellContentClick event
            guna2DataGridView2.CellContentClick += guna2DataGridView2_CellContentClick;
        }

        private void UserControlSubjects_Load(object sender, EventArgs e)
        {
            LoadSubjects(); // Load subjects data on control load
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2Button1.Text == "تعديل")
            {
                UpdateSubject();
            }
            else
            {
                AddSubject();
            }
        }

        private void AddSubject()
        {
            if (ValidateInputs())
            {
                try
                {
                    var selectedSubject = db.Classes.FirstOrDefault(s => s.ClassName == guna2ComboBox1.Text);
                    if (selectedSubject != null)
                    {
                        var newSubject = new Subjects
                        {
                            SubjectName = guna2TextBox11.Text,
                            ClassID = selectedSubject.ClassID
                        };

                        db.Subjects.Add(newSubject);
                        db.SaveChanges();

                        MessageBox.Show("!تمت إضافة المادة بنجاح");
                    }

                    LoadSubjects();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message);
                }
            }
        }

        private void UpdateSubject()
        {
            if (currentEditingSubjectID.HasValue && ValidateInputs())
            {
                try
                {
                    var subject = db.Subjects.Find(currentEditingSubjectID.Value);

                    if (subject != null)
                    {
                        subject.SubjectName = guna2TextBox11.Text;
                        subject.ClassID = (int)guna2ComboBox1.SelectedValue;

                        db.SaveChanges();

                        MessageBox.Show("!تم تعديل المادة بنجاح");

                        LoadSubjects();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(".المادة غير موجودة");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message);
                }
            }
        }

        private void LoadSubjects()
        {
            var subjects = db.Subjects
                .Select(s => new
                {
                    s.SubjectID,
                    s.SubjectName,
                    ClassName = s.Classes.ClassName // Assuming Class is a navigation property of Subject
                }).ToList();

            tool.LoadDataIntoDataGridView(guna2DataGridView2, subjects, _subject => new object[]
            {
                _subject.SubjectID,
                _subject.SubjectName,
                _subject.ClassName
            });
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Check if the clicked cell is the Delete button column
                if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "Delete")
                {
                    int subjectID = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["SubjectID"].Value);
                    DeleteSubject(subjectID);
                }
                // Check if the clicked cell is the Edit button column
                else if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "Edit")
                {
                    int subjectID = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["SubjectID"].Value);
                    LoadSubjectForEditing(subjectID);
                }
            }
        }

        private void DeleteSubject(int subjectID)
        {
            try
            {
                var subject = db.Subjects.Find(subjectID);
                if (subject != null)
                {
                    db.Subjects.Remove(subject);
                    db.SaveChanges();

                    MessageBox.Show("تم حذف المادة بنجاح!");

                    LoadSubjects(); // Refresh the data grid after deletion
                }
                else
                {
                    MessageBox.Show(".المادة غير موجودة");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء الحذف: " + ex.Message);
            }
        }

        private void LoadSubjectForEditing(int subjectID)
        {
            var subject = db.Subjects.Find(subjectID);
            if (subject != null)
            {
                guna2TextBox11.Text = subject.SubjectName;
                guna2ComboBox1.SelectedValue = subject.ClassID; // Correctly set the class name

                guna2Button1.Text = "تعديل"; // Change the button text to "تعديل" when editing
                currentEditingSubjectID = subjectID; // Store the ID of the subject being edited
            }
            else
            {
                MessageBox.Show(".المادة غير موجودة");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox11.Text))
            {
                MessageBox.Show(".الرجاء إدخال اسم المادة");
                return false;
            }
            if (guna2ComboBox1.SelectedIndex < 0)
            {
                MessageBox.Show(".الرجاء اختيار صف");
                return false;
            }
            return true;
        }

        private void ResetForm()
        {
            guna2TextBox11.Clear();
            guna2ComboBox1.SelectedIndex = -1;
            guna2Button1.Text = "إضافة"; // Reset the button text to "إضافة" after editing
            currentEditingSubjectID = null;
        }
    }
}