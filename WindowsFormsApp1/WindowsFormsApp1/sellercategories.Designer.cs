namespace WindowsFormsApp1
{
    partial class sellercategories
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.categories_status = new System.Windows.Forms.ComboBox();
            this.categories_deleteBtn = new System.Windows.Forms.Button();
            this.categories_clearBtn = new System.Windows.Forms.Button();
            this.categories_updateBtn = new System.Windows.Forms.Button();
            this.categories_addBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.categories_category = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.categories_status);
            this.panel1.Controls.Add(this.categories_deleteBtn);
            this.panel1.Controls.Add(this.categories_clearBtn);
            this.panel1.Controls.Add(this.categories_updateBtn);
            this.panel1.Controls.Add(this.categories_addBtn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.categories_category);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 723);
            this.panel1.TabIndex = 0;
            // 
            // categories_status
            // 
            this.categories_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categories_status.FormattingEnabled = true;
            this.categories_status.Items.AddRange(new object[] {
            "Available",
            "Unavailable"});
            this.categories_status.Location = new System.Drawing.Point(20, 120);
            this.categories_status.Name = "categories_status";
            this.categories_status.Size = new System.Drawing.Size(262, 26);
            this.categories_status.TabIndex = 11;
            this.categories_status.SelectedIndexChanged += new System.EventHandler(this.inventory_status_SelectedIndexChanged);
            // 
            // categories_deleteBtn
            // 
            this.categories_deleteBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.categories_deleteBtn.FlatAppearance.BorderSize = 0;
            this.categories_deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categories_deleteBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categories_deleteBtn.ForeColor = System.Drawing.Color.White;
            this.categories_deleteBtn.Location = new System.Drawing.Point(170, 265);
            this.categories_deleteBtn.Name = "categories_deleteBtn";
            this.categories_deleteBtn.Size = new System.Drawing.Size(112, 41);
            this.categories_deleteBtn.TabIndex = 9;
            this.categories_deleteBtn.Text = "Delete";
            this.categories_deleteBtn.UseVisualStyleBackColor = false;
            this.categories_deleteBtn.Click += new System.EventHandler(this.categories_deleteBtn_Click);
            // 
            // categories_clearBtn
            // 
            this.categories_clearBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.categories_clearBtn.FlatAppearance.BorderSize = 0;
            this.categories_clearBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categories_clearBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categories_clearBtn.ForeColor = System.Drawing.Color.White;
            this.categories_clearBtn.Location = new System.Drawing.Point(20, 265);
            this.categories_clearBtn.Name = "categories_clearBtn";
            this.categories_clearBtn.Size = new System.Drawing.Size(112, 41);
            this.categories_clearBtn.TabIndex = 8;
            this.categories_clearBtn.Text = "Clear";
            this.categories_clearBtn.UseVisualStyleBackColor = false;
            this.categories_clearBtn.Click += new System.EventHandler(this.categories_clearBtn_Click);
            // 
            // categories_updateBtn
            // 
            this.categories_updateBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.categories_updateBtn.FlatAppearance.BorderSize = 0;
            this.categories_updateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categories_updateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categories_updateBtn.ForeColor = System.Drawing.Color.White;
            this.categories_updateBtn.Location = new System.Drawing.Point(170, 193);
            this.categories_updateBtn.Name = "categories_updateBtn";
            this.categories_updateBtn.Size = new System.Drawing.Size(112, 41);
            this.categories_updateBtn.TabIndex = 7;
            this.categories_updateBtn.Text = "Update";
            this.categories_updateBtn.UseVisualStyleBackColor = false;
            this.categories_updateBtn.Click += new System.EventHandler(this.categories_updateBtn_Click);
            // 
            // categories_addBtn
            // 
            this.categories_addBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.categories_addBtn.FlatAppearance.BorderSize = 0;
            this.categories_addBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categories_addBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categories_addBtn.ForeColor = System.Drawing.Color.White;
            this.categories_addBtn.Location = new System.Drawing.Point(20, 193);
            this.categories_addBtn.Name = "categories_addBtn";
            this.categories_addBtn.Size = new System.Drawing.Size(112, 41);
            this.categories_addBtn.TabIndex = 6;
            this.categories_addBtn.Text = "Add";
            this.categories_addBtn.UseVisualStyleBackColor = false;
            this.categories_addBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.label3.Location = new System.Drawing.Point(17, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Status";
            // 
            // categories_category
            // 
            this.categories_category.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categories_category.Location = new System.Drawing.Point(20, 53);
            this.categories_category.Name = "categories_category";
            this.categories_category.Size = new System.Drawing.Size(262, 24);
            this.categories_category.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.label2.Location = new System.Drawing.Point(17, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Category";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(334, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(648, 723);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(616, 672);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(52)))), ((int)(((byte)(46)))));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "All Categories";
            // 
            // sellercategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "sellercategories";
            this.Size = new System.Drawing.Size(997, 749);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox categories_category;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button categories_addBtn;
        private System.Windows.Forms.Button categories_deleteBtn;
        private System.Windows.Forms.Button categories_clearBtn;
        private System.Windows.Forms.Button categories_updateBtn;
        private System.Windows.Forms.ComboBox categories_status;
    }
}
