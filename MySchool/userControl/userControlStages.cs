using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Data.Entity; // For Entity Framework 6
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml.Linq;
using MySchool.TOOLS_HELPER;
using System.Collections.Generic;
namespace MySchool.userControl
{
    public partial class userControlStages : UserControl
    {
        private readonly SchoolDBEntities db = new SchoolDBEntities();
        private int? currentEditingStageID = null;
        private int? currentEditingClassID = null;
        private int? currentEditingDivisionID = null;
        private tools tool=new tools();
        public userControlStages()
        {
            InitializeComponent();
            InitializeDataGridViewColumns();
            LoadData();



            tool.StyleDataGridView(guna2DataGridView1, guna2TabControl1);
            tool.StyleDataGridView(guna2DataGridView2, guna2TabControl1);
            tool.StyleDataGridView(guna2DataGridView3, guna2TabControl1);

         
            guna2DataGridView2.Columns["StudentCount"].Width = 150;

            // Attach event handlers for each DataGridView
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
            guna2DataGridView2.CellContentClick += guna2DataGridView2_CellContentClick;
            guna2DataGridView3.CellContentClick += guna2DataGridView3_CellContentClick;
            
            // fill comboBox in the Class
            var stageNames = db.Stages
                .Select(stage => stage.StageName)
                .ToArray();

            tool.FillComboBox(guna2ComboBox2, stageNames);

            // Retrieve all ClassNames from the Classes table and store them in a string array
            var ClassNames = db.Classes
                .Select(c => c.ClassName)
                .ToArray();

            tool.FillComboBox(guna2ComboBox1, ClassNames);

        }



        private void InitializeDataGridViewColumns()
        {
            
            tool.InitializeDataGridView(guna2DataGridView2, new Dictionary<string, string>
    {
        {"StageID", "#"},
        {"StageName", "اسم المرحلة"},
        {"Classes", "الصفوف"},
        {"StudentCount", "إجمالي الطلاب"},
        {"Note", "الملاحظة"}
    }, new Dictionary<string, (string, string)>
    {
        {"Delete", ("حذف", "حذف")},
        {"Edit", ("تعديل", "تعديل")}
    });


            // Initialize Guna2DataGridView3
    tool.InitializeDataGridView(guna2DataGridView3, new Dictionary<string, string>
    {
        {"ClassID", "#"},
        {"ClassName", "الصف"},
        {"StageName", "المرحلة"},
        {"DivisionName", "الشعب"},
        {"Active", "الحالة"}
    }, new Dictionary<string, (string, string)>
    {
        {"Delete", ("حذف", "حذف")},
        {"Edit", ("تعديل", "تعديل")}
    });

            // Initialize Guna2DataGridView1
            tool.InitializeDataGridView(guna2DataGridView1, new Dictionary<string, string>
    {
        {"DivisionID", "#"},
        {"DivisionName", "الشعبة"},
        {"className", "الصف"},
        {"SUMStudent", "إجمالي الطلاب"}
    }, new Dictionary<string, (string, string)>
    {
        {"Delete", ("حذف", "حذف")},
        {"Edit", ("تعديل", "تعديل")}
    });


        }

      

        private void LoadData()
        {
            // Load data for Guna2DataGridView3 (Classes)
            var classData = db.Classes
                .Select(s => new
                {
                    s.ClassID,
                    s.ClassName,
                    StageName = s.Stages.StageName,
                    DivisionCount = s.Divisions.Count(),
                    StudentCount = s.Divisions.SelectMany(c => c.Students).Count(),
                    classActive = s.IsActive
                })
                .ToList();

            tool.LoadDataIntoDataGridView(guna2DataGridView3, classData, _class => new object[]
            {
        _class.ClassID,
        _class.ClassName,
        _class.StageName,
        _class.DivisionCount,
        _class.classActive
            });

            // Load data for Guna2DataGridView1 (Divisions)
            var divisionData = db.Divisions
                .Select(d => new
                {
                    d.DivisionID,
                    d.DivisionName,
                    ClassName = d.Classes.ClassName,
                    StudentCount = d.Students.Count()
                })
                .ToList();

            tool.LoadDataIntoDataGridView(guna2DataGridView1, divisionData, division => new object[]
            {
        division.DivisionID,
        division.DivisionName,
        division.ClassName,
        division.StudentCount            });

            // Load data for Guna2DataGridView2 (Stages)
            var stagesData = db.Stages
                .Select(s => new
                {
                    s.StageID,
                    s.StageName,
                    Classes = s.Classes.Count(),
                    StudentCount = s.Classes.SelectMany(c => c.Students).Count(),
                    s.Note
                })
                .ToList();

            tool.LoadDataIntoDataGridView(guna2DataGridView2, stagesData, stage => new object[]
            {
        stage.StageID,
        stage.StageName,
        stage.Classes,
        stage.StudentCount,
        stage.Note
            });


        }

        private void AddButtonColumns()
        {
           
                tool.AddButtonColumn(guna2DataGridView2,"Edit", "تعديل", "تعديل");
                tool.AddButtonColumn(guna2DataGridView2,"Delete", "حـذف", "حـذف");

        }

       
        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var selectedStageID = guna2DataGridView2.Rows[e.RowIndex].Cells["StageID"].Value?.ToString();

                if (!string.IsNullOrEmpty(selectedStageID))
                {
                    int stageID = int.Parse(selectedStageID);

                    if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        EditStage(stageID);
                    }
                    else if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        DeleteStage(stageID);
                    }
                }
            }
        }

        private void EditStage(int stageID)
        {
            var stage = db.Stages.Find(stageID);
            if (stage != null)
            {
                guna2TextBox4.Text = stage.StageName;
                guna2TextBox3.Text = stage.Note;

                currentEditingStageID = stageID;
                guna2TileButton2.Text = "تعديل"; // Change the button text to "تعديل" (Edit)
            }
            else
            {
                MessageBox.Show("المرحلة غير موجودة");
            }
        }

        private void AddStage()
        {
            if (ValidateInputs())
            {
                try
                {
                    var stage = new Stages
                    {
                        StageName = guna2TextBox4.Text,
                        Note = guna2TextBox3.Text,
                        Active = true,
                        HireDate = DateTime.Now,
                        YearID = 1
                    };

                    db.Stages.Add(stage);
                    db.SaveChanges();
                    MessageBox.Show("!تم إضافة المرحلة بنجاح");

                    LoadData();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message);
                }
            }
        }

        private void UpdateStage()
        {
            if (currentEditingStageID.HasValue && ValidateInputs())
            {
                try
                {
                    var stage = db.Stages.Find(currentEditingStageID.Value);

                    if (stage != null)
                    {
                        stage.StageName = guna2TextBox4.Text;
                        stage.Note = guna2TextBox3.Text;

                        db.SaveChanges();

                        MessageBox.Show("!تم تعديل المرحلة بنجاح");

                        LoadData();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(".المرحلة غير موجودة");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message);
                }
            }
        }

        private void DeleteStage(int stageID)
        {
            var result = MessageBox.Show("هل أنت متأكد من أنك تريد حذف المرحلة ؟", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var stage = db.Stages.Find(stageID);
                if (stage != null)
                {
                    db.Stages.Remove(stage);
                    db.SaveChanges();
                    MessageBox.Show("ّ!تم حذف المرحلة بنجاح");

                    LoadData();
                }
                else
                {
                    MessageBox.Show(".المرحلة غير موجودة");
                }
            }
        }


        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(guna2TextBox4.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم المرحلة.");
                return false;
            }

            if (string.IsNullOrEmpty(guna2TextBox3.Text))
            {
                MessageBox.Show("الرجاء إدخال ملاحظة.");
                return false;
            }

            return true;
        }

        private void ResetForm()
        {
            tool.ResetForms(this);
            currentEditingStageID = null;
            currentEditingClassID = null;
            
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            if (guna2TileButton2.Text == "تعديل")
            {
                UpdateStage();
            }
            else
            {
                AddStage();
            }
        }
        
        // Event handler for guna2DataGridView3 (Classes)*******************************************************************************
        private void guna2DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var selectedClassID = guna2DataGridView3.Rows[e.RowIndex].Cells["ClassID"].Value?.ToString();

                if (!string.IsNullOrEmpty(selectedClassID))
                {
                    int classID = int.Parse(selectedClassID);

                    if (guna2DataGridView3.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        EditClass(classID);  // Class edit logic
                    }
                    else if (guna2DataGridView3.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        DeleteClass(classID);  // Class delete logic
                    }
                }
            }
        }

        // Example of Edit/Delete methods for Classes (similar to Stages)
        private void EditClass(int classID)
        {
            // Fetch the Class record based on classID
            var class1 = db.Classes.Include(c => c.Stages).FirstOrDefault(c => c.ClassID == classID);


            if (class1 != null)
            {
                // Retrieve all StageNames from the Stages table and store them in a string array
                var stageNames = db.Stages
                    .Select(stage => stage.StageName)
                    .ToArray();

                // Optionally, you can assign the array to a ComboBox or process it as needed
                guna2ComboBox2.Items.Clear(); // Clear previous items
                guna2ComboBox2.Items.AddRange(stageNames); // Add the stage names to the ComboBox

                // Set the class information to the appropriate UI elements
                guna2ComboBox2.Text = class1.Stages?.StageName;  // Access the StageName through the navigation property
                guna2TextBox2.Text = class1.ClassName;

                // Set the current editing ID to track the class being edited
                currentEditingClassID = classID;
                guna2TileButton3.Text = "تعديل"; // Change the button text to "تعديل" (edit)
            }
            else
            {
                MessageBox.Show("المرحلة غير موجودة");
            }
        }
        private void AddClass()
        {
            if (ValidateInputsForClass())
            {
                try
                {
                    // Find the selected stage based on the stage name in guna2ComboBox2
                    var selectedStage = db.Stages.FirstOrDefault(s => s.StageName == guna2ComboBox2.Text);

                    if (selectedStage != null)
                    {
                        // Create new class object
                        var class1 = new Classes
                        {
                            ClassName = guna2TextBox2.Text,
                            StageID = selectedStage.StageID, // Assign the StageID
                            IsActive = 1,
                            ClassYear = DateTime.Now.Year.ToString(),
                        };

                        db.Classes.Add(class1); // Add class to the database
                        db.SaveChanges(); // Save changes
                        MessageBox.Show("!تم إضافة الصـف بنجاح");

                        LoadData(); // Reload data to refresh the DataGridView
                        ResetForm(); // Reset the form fields after successful addition
                    }
                    else
                    {
                        MessageBox.Show("!الرجاء اختيار المرحلة المناسبة.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message);
                }
            }
        }

        private bool ValidateInputsForClass()
        {
            if (string.IsNullOrEmpty(guna2ComboBox2.Text))
            {
                MessageBox.Show("الرجاء إختيار اسم المرحلة.");
                return false;
            }

            if (string.IsNullOrEmpty(guna2TextBox2.Text))
            {
                MessageBox.Show("الرجاء إدخال الصـف.","error");
                return false;
            }

            return true;
        }
        private bool ValidateInputsForDivision()
        {
            if (string.IsNullOrEmpty(guna2ComboBox1.Text))
            {
                MessageBox.Show("الرجاء إختيار اسم الصـف.");
                return false;
            }

            if (string.IsNullOrEmpty(guna2TextBox1.Text))
            {
                MessageBox.Show("الرجاء إدخال الشعبة.", "error");
                return false;
            }

            return true;
        }
        private void UpdateClass()
        {
            if (currentEditingClassID.HasValue && ValidateInputsForClass())
            {
                try
                {
                    // Fetch the Class record to be updated
                    var class1 = db.Classes.Include(s => s.Stages).FirstOrDefault(c => c.ClassID == currentEditingClassID);

                    if (class1 != null)
                    {
                        // Update the class name from the TextBox
                        class1.ClassName = guna2TextBox2.Text;

                        // Find and update the related Stage using the selected stage name from the ComboBox
                        var selectedStage = db.Stages.FirstOrDefault(s => s.StageName == guna2ComboBox2.Text);
                        if (selectedStage != null)
                        {
                            class1.StageID = selectedStage.StageID; // Update the StageID to the selected Stage's ID
                        }

                        // Save changes to the database
                        db.SaveChanges();

                        MessageBox.Show("!تم تعديل المرحلة بنجاح"); // Show success message

                        // Reload data and reset form fields
                        LoadData();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(".المرحلة غير موجودة"); // Show error message if class not found
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message); // Show error message in case of an exception
                }
            }
        }

        private void DeleteClass(int classID)
        {
            var result = MessageBox.Show("هل أنت متأكد من أنك تريد حذف الصـف ؟", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var Class1 = db.Classes.Find(classID);
                if (Class1 != null)
                {
                    db.Classes.Remove(Class1);
                    db.SaveChanges();
                    MessageBox.Show("ّ!تم حذف الصـف بنجاح");

                    LoadData();
                }
                else
                {
                    MessageBox.Show(".الصـف غير موجودة");
                }
            }
        }
        
        private void guna2TileButton3_Click_1(object sender, EventArgs e)
        {
            if (guna2TileButton3.Text == "تعديل")
            {
                UpdateClass();
            }
            else
            {
                AddClass();
            }
        }

        // Event handler for guna2DataGridView1***************************************************************************************
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var selectedDivisionID = guna2DataGridView1.Rows[e.RowIndex].Cells["DivisionID"].Value?.ToString();

                if (!string.IsNullOrEmpty(selectedDivisionID))
                {
                    int DivisionID = int.Parse(selectedDivisionID);

                    if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        EditDivision(DivisionID);  // Implement class editing logic
                    }
                    else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        DeleteDivision(DivisionID);  // Implement class deletion logic
                    }
                }
            }
        }

        private void EditDivision(int DivisionID)
        {
            // Fetch the Class record based on classID
            var Division = db.Divisions.Include(c => c.Classes).FirstOrDefault(c => c.DivisionID == DivisionID);


            if (Division != null)
            {
                // Retrieve all ClassNames from the Classes table and store them in a string array
                var ClassNames = db.Classes
                    .Select(c => c.ClassName)
                    .ToArray();

                guna2ComboBox1.Items.Clear(); // Clear previous items
                guna2ComboBox1.Items.AddRange(ClassNames); // Add the stage names to the ComboBox

                guna2ComboBox1.Text = Division.Classes?.ClassName; 
                guna2TextBox1.Text = Division.DivisionName;

                currentEditingDivisionID = DivisionID;
                guna2TileButton1.Text = "تعديل"; // Change the button text to "تعديل" (edit)
            }
            else
            {
                MessageBox.Show("الشعبة غير موجودة");
            }
        }

        private void AddDivision()
        {
            if (ValidateInputsForDivision())
            {
                try
                {
                    // Find the selected stage based on the stage name in guna2ComboBox2
                    var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName== guna2ComboBox1.Text);

                    if (selectedClass != null)
                    {
                        // Create new class object
                        var division = new Divisions
                        {
                            DivisionName = guna2TextBox1.Text,
                            ClassID = selectedClass.ClassID,
                        };

                        db.Divisions.Add(division);
                        db.SaveChanges();
                        MessageBox.Show("!تم إضافة الشعبة بنجاح");

                        LoadData(); // Reload data to refresh the DataGridView
                        ResetForm(); // Reset the form fields after successful addition
                    }
                    else
                    {
                        MessageBox.Show("!الرجاء اختيار الصـف المناسبة.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message);
                }
            }
        }


        private void DeleteDivision(int DivisionID)
        {
            var result = MessageBox.Show("هل أنت متأكد من أنك تريد حذف الشعبة ؟", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var Division = db.Divisions.Find(DivisionID);
                if (Division != null)
                {
                    db.Divisions.Remove(Division);
                    db.SaveChanges();
                    MessageBox.Show("ّ!تم حذف الشعبة بنجاح");

                    LoadData();
                }
                else
                {
                    MessageBox.Show(".الصـف غير موجودة");
                }
            }

        }

        private void UpdateDivision()
        {
            if (currentEditingDivisionID.HasValue && ValidateInputsForDivision())
            {
                try
                {
                    // Fetch the Class record to be updated
                    var Division = db.Divisions.Include(C => C.Classes).FirstOrDefault(c => c.DivisionID == currentEditingDivisionID);

                    if (Division != null)
                    {
                        Division.DivisionName = guna2TextBox1.Text;

                        var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == guna2ComboBox1.Text);
                        if (selectedClass != null)
                        {
                            Division.ClassID = selectedClass.ClassID;
                        }

                        db.SaveChanges();

                        MessageBox.Show("!تم تعديل الشبعة بنجاح"); // Show success message

                        LoadData();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(".الشعبة غير موجودة"); // Show error message if class not found
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ: " + ex.Message); // Show error message in case of an exception
                }
            }
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            if (guna2TileButton1.Text == "تعديل")
            {
                UpdateDivision();
            }
            else
            {
                AddDivision();
            } 
        }



    }


}
