using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3(string imagePath)
        {
            InitializeComponent();
            this.Text = "Custom Artwork Preview";
            this.BackColor = Color.FromArgb(78, 52, 46);

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(60, 40, 35)
            };

            Label titleLabel = new Label
            {
                Text = " Custom Artwork Preview ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(238, 217, 196),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            topPanel.Controls.Add(titleLabel);
            this.Controls.Add(topPanel);

            Panel cardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(60, 40, 60, 100),
                BackColor = Color.FromArgb(78, 52, 46)
            };

            PictureBox preview = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    preview.Image = Image.FromStream(stream);
                }
            }

            cardPanel.Controls.Add(preview);
            this.Controls.Add(cardPanel);

            // Notice Label
            Label lblNote = new Label
            {
                Text = "ℹ️ Note: You must download your artwork first to unlock 'Proceed to Order'.",
                Font = new Font("Segoe UI", 11, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.FromArgb(245, 224, 195),
                AutoSize = true,
                Location = new Point(40, this.ClientSize.Height - 120),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.Controls.Add(lblNote);
            lblNote.BringToFront();

            // Proceed to Order button (Initially Disabled)
            button1.Text = "PROCEED TO ORDER";
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Enabled = false;
            button1.BackColor = Color.FromArgb(120, 100, 95);
            button1.ForeColor = Color.FromArgb(170, 160, 155);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Cursor = Cursors.Hand;
            button1.Size = new Size(220, 50);
            button1.Location = new Point(this.ClientSize.Width - 260, this.ClientSize.Height - 75);
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.BringToFront();

            // Download Artwork Button
            Button btnDownload = new Button
            {
                Text = "⬇ DOWNLOAD ARTWORK",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(238, 217, 196),
                ForeColor = Color.FromArgb(78, 52, 46),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(240, 50),
                Location = new Point(this.ClientSize.Width - 520, this.ClientSize.Height - 75),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.Click += (s, ev) =>
            {
                using (SaveFileDialog saveDlg = new SaveFileDialog())
                {
                    saveDlg.Title = "Download Custom Artwork";
                    saveDlg.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                    saveDlg.FileName = "MyCustomToteBagDesign.png";
                    if (saveDlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (preview.Image != null)
                            {
                                preview.Image.Save(saveDlg.FileName);
                            }
                            else if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                            {
                                File.Copy(imagePath, saveDlg.FileName, true);
                            }

                            MessageBox.Show("Artwork downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Unlock Proceed button
                            button1.Enabled = true;
                            button1.BackColor = Color.FromArgb(238, 217, 196);
                            button1.ForeColor = Color.FromArgb(78, 52, 46);

                            lblNote.Text = "✅ Artwork downloaded! You can now proceed to order.";
                            lblNote.ForeColor = Color.FromArgb(180, 230, 180);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to download artwork: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };
            this.Controls.Add(btnDownload);
            btnDownload.BringToFront();

            // Back to Design Button
            Button btnBack = new Button
            {
                Text = "⬅ BACK TO DESIGN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(238, 217, 196),
                ForeColor = Color.FromArgb(78, 52, 46),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(210, 50),
                Location = new Point(40, this.ClientSize.Height - 75),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, ev) =>
            {
                paint_interface paintForm = new paint_interface();
                paintForm.Show();
                this.Hide();
            };
            this.Controls.Add(btnBack);
            btnBack.BringToFront();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new customer().Show();
            this.Hide();
        }
    }
}
