namespace MySchool.userControl
{
    partial class Backup
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_backup = new Guna.UI2.WinForms.Guna2Button();
            this.btn_restor = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // btn_backup
            // 
            this.btn_backup.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn_backup.BorderRadius = 8;
            this.btn_backup.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_backup.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_backup.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_backup.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_backup.FillColor = System.Drawing.Color.LimeGreen;
            this.btn_backup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_backup.ForeColor = System.Drawing.Color.White;
            this.btn_backup.Location = new System.Drawing.Point(497, 58);
            this.btn_backup.Name = "btn_backup";
            this.btn_backup.Size = new System.Drawing.Size(533, 52);
            this.btn_backup.TabIndex = 61;
            this.btn_backup.Text = "انشاء نسخة احتياطية";
            this.btn_backup.Click += new System.EventHandler(this.btn_backup_Click);
            // 
            // btn_restor
            // 
            this.btn_restor.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btn_restor.BorderRadius = 8;
            this.btn_restor.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_restor.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_restor.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_restor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_restor.FillColor = System.Drawing.Color.LimeGreen;
            this.btn_restor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_restor.ForeColor = System.Drawing.Color.White;
            this.btn_restor.Location = new System.Drawing.Point(18, 58);
            this.btn_restor.Name = "btn_restor";
            this.btn_restor.Size = new System.Drawing.Size(444, 52);
            this.btn_restor.TabIndex = 62;
            this.btn_restor.Text = "استعادة نسخة احتياطية";
            this.btn_restor.Click += new System.EventHandler(this.btn_restor_Click);
            // 
            // Backup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_restor);
            this.Controls.Add(this.btn_backup);
            this.Name = "Backup";
            this.Size = new System.Drawing.Size(1051, 157);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btn_backup;
        private Guna.UI2.WinForms.Guna2Button btn_restor;
    }
}
