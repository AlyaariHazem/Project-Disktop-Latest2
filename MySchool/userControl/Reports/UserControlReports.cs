using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySchool.TOOLS_HELPER;

namespace MySchool.userControl
{
    public partial class UserControlReports : UserControl
    {
        private readonly SchoolDBEntities db = new SchoolDBEntities();
        private tools tool = new tools();

        public UserControlReports()
        {
            InitializeComponent();
           // InitializeReportGrid();
           // LoadReportData();
        }

        private void InitializeReportGrid()
        {
            tool.StyleDataGridView(guna2DataGridView1);
            tool.StyleDataGridView(guna2DataGridView2);

            // Set the border style for cells
            guna2DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Set column header height
            guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        //private void LoadReportData()
        //{
        //    // Fetch report data
        //    var reportData = db.Grades
        //                    .Select(grade => new
        //                    {
        //                        SubjectName = grade.Subjects.SubjectName,
        //                        GradeType = grade.grade_type,
        //                        Grade = grade.value,
        //                        Date = db.Grades.Sum(g => g.value)
        //})
        //                    .ToList();

        //    // Load data into DataGridView using the tool's LoadDataIntoDataGridView method
        //    tool.LoadDataIntoDataGridView(guna2DataGridView1, reportData, _report => new object[]
        //    {
                
        //        _report.SubjectName,
        //        _report.GradeType,
        //        _report.Grade,
        //        _report.Date
        //    });

        //}

        private void guna2ButtonPrint_Click(object sender, EventArgs e)
        {
            // Implement print functionality here
        }

        private void UserControlReports_Load(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }
    }
}
