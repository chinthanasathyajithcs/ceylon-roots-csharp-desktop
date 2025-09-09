namespace WindowsFormsApp1
{
    partial class Sdash
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ordersBtn = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.orders1 = new WindowsFormsApp1.orders();
            this.sellercategories1 = new WindowsFormsApp1.sellercategories();
            this.inventoryForm1 = new WindowsFormsApp1.InventoryForm();
            this.userControl12 = new WindowsFormsApp1.UserControl1();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.panel1.Controls.Add(this.ordersBtn);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 749);
            this.panel1.TabIndex = 0;
            // 
            // ordersBtn
            // 
            this.ordersBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(217)))), ((int)(((byte)(196)))));
            this.ordersBtn.FlatAppearance.BorderSize = 0;
            this.ordersBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ordersBtn.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.ordersBtn.Location = new System.Drawing.Point(55, 503);
            this.ordersBtn.Name = "ordersBtn";
            this.ordersBtn.Size = new System.Drawing.Size(265, 93);
            this.ordersBtn.TabIndex = 6;
            this.ordersBtn.Text = "Orders";
            this.ordersBtn.UseVisualStyleBackColor = false;
            this.ordersBtn.Click += new System.EventHandler(this.ordersBtn_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(217)))), ((int)(((byte)(196)))));
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.button4.Location = new System.Drawing.Point(55, 390);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(265, 93);
            this.button4.TabIndex = 5;
            this.button4.Text = "Categories";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Coral;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.button3.Location = new System.Drawing.Point(0, 686);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(375, 63);
            this.button3.TabIndex = 4;
            this.button3.Text = "Log out";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(217)))), ((int)(((byte)(196)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.button2.Location = new System.Drawing.Point(55, 277);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(265, 93);
            this.button2.TabIndex = 3;
            this.button2.Text = "Inventory";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(111, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome, Admin !";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WindowsFormsApp1.Properties.Resources.User_41;
            this.pictureBox1.Location = new System.Drawing.Point(137, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 103);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.orders1);
            this.panel2.Controls.Add(this.sellercategories1);
            this.panel2.Controls.Add(this.inventoryForm1);
            this.panel2.Controls.Add(this.userControl12);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(373, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(997, 749);
            this.panel2.TabIndex = 2;
            // 
            // orders1
            // 
            this.orders1.Location = new System.Drawing.Point(0, 0);
            this.orders1.Name = "orders1";
            this.orders1.Size = new System.Drawing.Size(997, 749);
            this.orders1.TabIndex = 7;
            this.orders1.Load += new System.EventHandler(this.orders1_Load);
            // 
            // sellercategories1
            // 
            this.sellercategories1.Location = new System.Drawing.Point(0, 0);
            this.sellercategories1.Name = "sellercategories1";
            this.sellercategories1.Size = new System.Drawing.Size(997, 749);
            this.sellercategories1.TabIndex = 3;
            this.sellercategories1.Load += new System.EventHandler(this.sellercategories1_Load);
            // 
            // inventoryForm1
            // 
            this.inventoryForm1.Location = new System.Drawing.Point(0, 0);
            this.inventoryForm1.Name = "inventoryForm1";
            this.inventoryForm1.Size = new System.Drawing.Size(1000, 749);
            this.inventoryForm1.TabIndex = 2;
            // 
            // userControl12
            // 
            this.userControl12.Location = new System.Drawing.Point(3, 0);
            this.userControl12.Name = "userControl12";
            this.userControl12.Size = new System.Drawing.Size(997, 749);
            this.userControl12.TabIndex = 1;
            // 
            // Sdash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(217)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1386, 788);
            this.MinimumSize = new System.Drawing.Size(1386, 788);
            this.Name = "Sdash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form3";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form3_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel2;
        private UserControl1 userControl12;
        private System.Windows.Forms.Button button4;
        private sellercategories sellercategories1;
        private InventoryForm inventoryForm1;
        private System.Windows.Forms.Button ordersBtn;
        private orders orders1;
    }
}