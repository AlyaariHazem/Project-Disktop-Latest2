using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Myschool.Helpers;

namespace MySchool.userControl
{
    public partial class Backup : UserControl
    {
        DatabaseHelper db = new DatabaseHelper();

        public Backup()
        {
            InitializeComponent();
        }

        private void btn_backup_Click(object sender, EventArgs e)
        {
            string databaseName = "SchoolDB";
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            string defaultFileName = $"{databaseName}_{todayDate}.bak";

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Backup Files (*.bak)|*.bak";
            saveFileDialog1.Title = "Save Database Backup";
            saveFileDialog1.FileName = defaultFileName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string destinationPath = saveFileDialog1.FileName;
                db.BackupDatabase(databaseName, destinationPath);
            }
        }

        private async void btn_restor_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Backup Files (*.bak)|*.bak";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string backupFilePath = openFileDialog1.FileName;

                // Disable UI during restore operation
                btn_backup.Enabled = false;
                btn_restor.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Run restore in a separate thread
                await Task.Run(() => db.RestoreDatabase("SchoolDB", backupFilePath));

                // Re-enable UI once restore is complete
                btn_backup.Enabled = true;
                btn_restor.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
