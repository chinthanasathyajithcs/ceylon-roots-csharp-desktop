using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Historythree : Form
    {
        public Historythree()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string videoPath = Path.Combine(Application.StartupPath, "videos", "Fabric_video.mp4");
            if (!File.Exists(videoPath))
            {
                // Fallback: check project directory
                string projectVideoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "videos", "Fabric_video.mp4");
                if (File.Exists(projectVideoPath))
                {
                    videoPath = projectVideoPath;
                }
            }

            if (File.Exists(videoPath))
            {
                axWindowsMediaPlayer1.URL = videoPath;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Video Files|*.mp4;*.wmv;*.avi;*.mkv|All Files|*.*";
                    ofd.Title = "Select Fabric Video File";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        axWindowsMediaPlayer1.URL = ofd.FileName;
                        axWindowsMediaPlayer1.Ctlcontrols.play();

                        try
                        {
                            string targetDir = Path.Combine(Application.StartupPath, "videos");
                            Directory.CreateDirectory(targetDir);
                            string targetPath = Path.Combine(targetDir, "Fabric_video.mp4");
                            if (!File.Exists(targetPath))
                            {
                                File.Copy(ofd.FileName, targetPath, true);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            historytwo HistoryFormtwo = new historytwo(); 
            HistoryFormtwo.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new customerinterface().Show();
            this.Hide();
        }
    }
}
