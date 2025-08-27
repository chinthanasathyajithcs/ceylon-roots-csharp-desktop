namespace WindowsFormsApp1
{
    partial class Welcome
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
            this.label1 = new System.Windows.Forms.Label();
            this.Countinue_button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 266);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Monotype Corsiva", 48F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(288, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 97);
            this.label1.TabIndex = 0;
            this.label1.Text = "WELCOME";
            // 
            // Countinue_button
            // 
            this.Countinue_button.BackColor = System.Drawing.Color.Navy;
            this.Countinue_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Countinue_button.FlatAppearance.BorderSize = 0;
            this.Countinue_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Countinue_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Countinue_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.Countinue_button.Location = new System.Drawing.Point(408, 350);
            this.Countinue_button.Name = "Countinue_button";
            this.Countinue_button.Size = new System.Drawing.Size(173, 56);
            this.Countinue_button.TabIndex = 1;
            this.Countinue_button.Text = "Continue";
            this.Countinue_button.UseVisualStyleBackColor = false;
            this.Countinue_button.Click += new System.EventHandler(this.Countinue_button_Click);
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.Countinue_button);
            this.Controls.Add(this.panel1);
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ceylon Roots";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Countinue_button;
    }
}

