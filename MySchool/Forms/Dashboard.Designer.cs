namespace MySchool.Forms
{
    partial class Dashboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("معلومات المدرسة");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("عرض المواد");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("المراحل والصفوف");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("الإعدادات", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode24,
            treeNode25});
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("جميع المدرسين");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("المدرسن", new System.Windows.Forms.TreeNode[] {
            treeNode27});
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("جميع الطلاب");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("إضافة طالب");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("عن الطلاب");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("الطلاب", new System.Windows.Forms.TreeNode[] {
            treeNode29,
            treeNode30,
            treeNode31});
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("عرض المواد");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("إضافة مادة");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("المواد الدراسية", new System.Windows.Forms.TreeNode[] {
            treeNode33,
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("أولياء الأمور");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("اضافة الدرجات");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("شهادة شهرية");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("شهادة سنوية");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("الشهادات", new System.Windows.Forms.TreeNode[] {
            treeNode38,
            treeNode39});
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("النسخ الأحتياطي");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("الحسابات");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("عننا");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("الإدارة");
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PictureBox5 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox4 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox3 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icon-1.png");
            this.imageList1.Images.SetKeyName(1, "icon-2.png");
            this.imageList1.Images.SetKeyName(2, "icon-3.png");
            this.imageList1.Images.SetKeyName(3, "icon-4.png");
            this.imageList1.Images.SetKeyName(4, "icon-5.png");
            this.imageList1.Images.SetKeyName(5, "icon-6.png");
            this.imageList1.Images.SetKeyName(6, "icon-7.png");
            this.imageList1.Images.SetKeyName(7, "icon-8.png");
            this.imageList1.Images.SetKeyName(8, "icon-9.png");
            this.imageList1.Images.SetKeyName(9, "icon-10.png");
            this.imageList1.Images.SetKeyName(10, "icon-11.png");
            this.imageList1.Images.SetKeyName(11, "icon-12.png");
            this.imageList1.Images.SetKeyName(12, "icon-13.png");
            this.imageList1.Images.SetKeyName(13, "icon-14.png");
            this.imageList1.Images.SetKeyName(14, "icon-15.png");
            this.imageList1.Images.SetKeyName(15, "icon-17.png");
            this.imageList1.Images.SetKeyName(16, "icon-18.png");
            this.imageList1.Images.SetKeyName(17, "icon-19.png");
            this.imageList1.Images.SetKeyName(18, "icon-20.png");
            this.imageList1.Images.SetKeyName(19, "icon-21.png");
            this.imageList1.Images.SetKeyName(20, "icon-22.png");
            this.imageList1.Images.SetKeyName(21, "icon-23.png");
            this.imageList1.Images.SetKeyName(22, "icon-26.png");
            this.imageList1.Images.SetKeyName(23, "icon-27.png");
            this.imageList1.Images.SetKeyName(24, "icon-28.png");
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.guna2Panel1.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel1.BorderRadius = 5;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.guna2PictureBox5);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox4);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox3);
            this.guna2Panel1.Location = new System.Drawing.Point(255, 4);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1069, 64);
            this.guna2Panel1.TabIndex = 1;
            // 
            // guna2PictureBox5
            // 
            this.guna2PictureBox5.BorderRadius = 8;
            this.guna2PictureBox5.Image = global::MySchool.Properties.Resources.icon_23;
            this.guna2PictureBox5.ImageRotate = 0F;
            this.guna2PictureBox5.Location = new System.Drawing.Point(163, 7);
            this.guna2PictureBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PictureBox5.Name = "guna2PictureBox5";
            this.guna2PictureBox5.Size = new System.Drawing.Size(65, 48);
            this.guna2PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox5.TabIndex = 5;
            this.guna2PictureBox5.TabStop = false;
            // 
            // guna2PictureBox4
            // 
            this.guna2PictureBox4.BorderRadius = 8;
            this.guna2PictureBox4.Image = global::MySchool.Properties.Resources.icon_22;
            this.guna2PictureBox4.ImageRotate = 0F;
            this.guna2PictureBox4.Location = new System.Drawing.Point(99, 10);
            this.guna2PictureBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PictureBox4.Name = "guna2PictureBox4";
            this.guna2PictureBox4.Size = new System.Drawing.Size(51, 39);
            this.guna2PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox4.TabIndex = 5;
            this.guna2PictureBox4.TabStop = false;
            this.guna2PictureBox4.Click += new System.EventHandler(this.guna2PictureBox4_Click);
            // 
            // guna2PictureBox3
            // 
            this.guna2PictureBox3.BorderRadius = 15;
            this.guna2PictureBox3.Image = global::MySchool.Properties.Resources.user_061;
            this.guna2PictureBox3.ImageRotate = 0F;
            this.guna2PictureBox3.Location = new System.Drawing.Point(21, 9);
            this.guna2PictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PictureBox3.Name = "guna2PictureBox3";
            this.guna2PictureBox3.Size = new System.Drawing.Size(64, 48);
            this.guna2PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox3.TabIndex = 5;
            this.guna2PictureBox3.TabStop = false;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.guna2Panel2.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel2.BorderRadius = 2;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.label1);
            this.guna2Panel2.Controls.Add(this.guna2PictureBox1);
            this.guna2Panel2.Location = new System.Drawing.Point(3, 4);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(251, 64);
            this.guna2Panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 31);
            this.label1.TabIndex = 6;
            this.label1.Text = "مدرستي";
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = global::MySchool.Properties.Resources.logo1;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(173, 5);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(75, 57);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 5;
            this.guna2PictureBox1.TabStop = false;
            // 
            // treeView1
            // 
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 130);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeView1.Name = "treeView1";
            treeNode23.Name = "Node10";
            treeNode23.Text = "معلومات المدرسة";
            treeNode24.Name = "Node11";
            treeNode24.Text = "عرض المواد";
            treeNode25.Name = "Node12";
            treeNode25.Text = "المراحل والصفوف";
            treeNode26.ImageKey = "icon-14.png";
            treeNode26.Name = "Node0";
            treeNode26.Text = "الإعدادات";
            treeNode27.Name = "Node14";
            treeNode27.Text = "جميع المدرسين";
            treeNode28.ImageKey = "icon-3.png";
            treeNode28.Name = "Node1";
            treeNode28.Text = "المدرسن";
            treeNode29.Name = "Node15";
            treeNode29.Text = "جميع الطلاب";
            treeNode30.Name = "Node16";
            treeNode30.Text = "إضافة طالب";
            treeNode31.Name = "Node17";
            treeNode31.Text = "عن الطلاب";
            treeNode32.ImageKey = "icon-10.png";
            treeNode32.Name = "Node2";
            treeNode32.SelectedImageIndex = 9;
            treeNode32.Text = "الطلاب";
            treeNode33.Name = "Node1";
            treeNode33.Text = "عرض المواد";
            treeNode34.Name = "Node3";
            treeNode34.Text = "إضافة مادة";
            treeNode35.Name = "Node0";
            treeNode35.Text = "المواد الدراسية";
            treeNode36.ImageKey = "icon-4.png";
            treeNode36.Name = "Node3";
            treeNode36.SelectedImageIndex = 3;
            treeNode36.Text = "أولياء الأمور";
            treeNode37.ImageKey = "icon-7.png";
            treeNode37.Name = "Node5";
            treeNode37.Text = "اضافة الدرجات";
            treeNode38.Name = "Node0";
            treeNode38.Text = "شهادة شهرية";
            treeNode39.Name = "Node1";
            treeNode39.Text = "شهادة سنوية";
            treeNode40.ImageKey = "icon-18.png";
            treeNode40.Name = "Node4";
            treeNode40.SelectedImageIndex = 16;
            treeNode40.Text = "الشهادات";
            treeNode41.ImageKey = "icon-8.png";
            treeNode41.Name = "Node6";
            treeNode41.Text = "النسخ الأحتياطي";
            treeNode42.ImageKey = "icon-10.png";
            treeNode42.Name = "Node7";
            treeNode42.Text = "الحسابات";
            treeNode43.ImageKey = "icon-17.png";
            treeNode43.Name = "Node8";
            treeNode43.Text = "عننا";
            treeNode44.ImageKey = "icon-20.png";
            treeNode44.Name = "Node9";
            treeNode44.Text = "الإدارة";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode28,
            treeNode32,
            treeNode35,
            treeNode36,
            treeNode37,
            treeNode40,
            treeNode41,
            treeNode42,
            treeNode43,
            treeNode44});
            this.treeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.treeView1.RightToLeftLayout = true;
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(249, 682);
            this.treeView1.TabIndex = 3;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.SystemColors.HighlightText;
            this.guna2Panel3.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel3.BorderRadius = 2;
            this.guna2Panel3.BorderThickness = 1;
            this.guna2Panel3.Controls.Add(this.label3);
            this.guna2Panel3.Controls.Add(this.guna2PictureBox2);
            this.guna2Panel3.Location = new System.Drawing.Point(3, 66);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(251, 64);
            this.guna2Panel3.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 31);
            this.label3.TabIndex = 6;
            this.label3.Text = "الصفحة الرئيسية";
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = global::MySchool.Properties.Resources.icon_1;
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(189, 9);
            this.guna2PictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(49, 43);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox2.TabIndex = 5;
            this.guna2PictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(255, 71);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1067, 727);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1329, 801);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Dashboard";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "Dashboard";
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.TreeView treeView1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox3;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox5;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox4;
        private System.Windows.Forms.Panel panel1;
    }
}