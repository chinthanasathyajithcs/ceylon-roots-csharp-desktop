using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class paint_interface : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetSystemMetrics(int nIndex);

        private Process paintProcess;
        private string imageFolder;
        private DriveService driveService;
        private string currentImagePath;


        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public paint_interface()
        {
            InitializeComponent();
            LoadQrImage();

            imageFolder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            InitializeDriveService();
        }
        private void InitializeDriveService()
        {
            try
            {
                string serviceAccountFile = Path.Combine(Application.StartupPath, "service_account.json");
                if (!File.Exists(serviceAccountFile))
                {
                    string projPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "service_account.json");
                    if (File.Exists(projPath))
                    {
                        serviceAccountFile = projPath;
                    }
                }

                if (File.Exists(serviceAccountFile))
                {
                    string[] scopes = { DriveService.Scope.DriveReadonly, DriveService.Scope.DriveFile };
                    GoogleCredential credential;
                    using (var stream = new FileStream(serviceAccountFile, FileMode.Open, FileAccess.Read))
                    {
                        credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
                    }

                    driveService = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "PaintUploader"
                    });
                }
            }
            catch { }
        }
        private string UploadFileToDrive(string filePath)
        {
            if (driveService == null || string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return filePath;

            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { "1KskNi1_4yz8HfKykeSosoW0dvPF5OvZb" } 
                };

                FilesResource.CreateMediaUpload request;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    request = driveService.Files.Create(fileMetadata, stream, "image/png");
                    request.Fields = "id, webViewLink, webContentLink";
                    request.Upload();
                }

                var file = request.ResponseBody;
                if (file != null && !string.IsNullOrEmpty(file.Id))
                {
                    string driveLink = $"https://drive.google.com/file/d/{file.Id}/view";
                    return driveLink;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to upload to Drive: " + ex.Message);
            }
            return filePath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                string localPath = null;

                // 1. Check for latest previous design in local Images folder
                var directoryInfo = new DirectoryInfo(imageFolder);
                var latestLocalFile = directoryInfo.GetFiles("*.*")
                    .Where(f => f.Extension.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
                                f.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                f.Extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(f => f.LastWriteTime)
                    .FirstOrDefault();

                if (latestLocalFile != null)
                {
                    localPath = latestLocalFile.FullName;
                }

                // 2. Check for newly uploaded photo from Google Drive
                if (driveService != null)
                {
                    try
                    {
                        string folderId = "1KskNi1_4yz8HfKykeSosoW0dvPF5OvZb";
                        var listReq = driveService.Files.List();
                        listReq.Q = $"'{folderId}' in parents and mimeType contains 'image/' and trashed = false";
                        listReq.OrderBy = "createdTime desc, modifiedTime desc";
                        listReq.PageSize = 1;
                        listReq.Fields = "files(id, name, createdTime, modifiedTime)";
                        listReq.SupportsAllDrives = true;
                        listReq.IncludeItemsFromAllDrives = true;

                        var result = listReq.Execute();
                        var latestDriveFile = result.Files != null ? result.Files.FirstOrDefault() : null;

                        if (latestDriveFile != null)
                        {
                            string ext = Path.GetExtension(latestDriveFile.Name);
                            string drivePath = Path.Combine(imageFolder, $"DriveImage_{DateTime.Now:yyyyMMdd_HHmmss}{ext}");

                            var getReq = driveService.Files.Get(latestDriveFile.Id);
                            getReq.SupportsAllDrives = true;
                            using (var fs = new FileStream(drivePath, FileMode.Create, FileAccess.Write))
                            {
                                getReq.Download(fs);
                            }
                            localPath = drivePath;
                        }
                    }
                    catch { }
                }

                // 3. Fallback: Open file dialog if no design exists yet
                if (string.IsNullOrEmpty(localPath) || !File.Exists(localPath))
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Title = "Select a Design or Photo to Edit";
                        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string ext = Path.GetExtension(openFileDialog.FileName);
                            localPath = Path.Combine(imageFolder, $"Photo_{DateTime.Now:yyyyMMdd_HHmmss}{ext}");
                            File.Copy(openFileDialog.FileName, localPath, true);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                if (!File.Exists(localPath))
                {
                    MessageBox.Show("No previous design or photo found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                currentImagePath = localPath;

                paintProcess = Process.Start("mspaint.exe", $"\"{localPath}\"");
                paintProcess.WaitForInputIdle();

                IntPtr paintHandle = IntPtr.Zero;
                for (int i = 0; i < 10 && paintHandle == IntPtr.Zero; i++)
                {
                    paintHandle = paintProcess.MainWindowHandle;
                    Thread.Sleep(300);
                }

                if (paintHandle != IntPtr.Zero)
                {
                    ShowWindow(paintHandle, SW_RESTORE);

                    int screenWidth = GetSystemMetrics(0);
                    int screenHeight = GetSystemMetrics(1);

                    int formWidth = screenWidth / 3;
                    int paintWidth = screenWidth - formWidth;

                    this.WindowState = FormWindowState.Normal;
                    this.SetBounds(0, 0, formWidth, screenHeight);

                    MoveWindow(paintHandle, formWidth, 0, paintWidth, screenHeight, true);
                }
                else
                {
                    MessageBox.Show("Could not arrange Paint window.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open previous design: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Launch Paint
            paintProcess = Process.Start("mspaint.exe");

            // Wait until Paint is ready
            paintProcess.WaitForInputIdle();

            // Retry getting the window handle until it’s available
            IntPtr paintHandle = IntPtr.Zero;
            for (int i = 0; i < 10 && paintHandle == IntPtr.Zero; i++)
            {
                paintHandle = paintProcess.MainWindowHandle;
                Thread.Sleep(300);
            }

            if (paintHandle != IntPtr.Zero)
            {
                // Restore window in case Paint is maximized
                ShowWindow(paintHandle, SW_RESTORE);

                // Get screen dimensions
                int screenWidth = GetSystemMetrics(0); // SM_CXSCREEN
                int screenHeight = GetSystemMetrics(1); // SM_CYSCREEN

                // === Split calculation ===
                int formWidth = screenWidth / 3;      // 1/3 for Form
                int paintWidth = screenWidth - formWidth; // remaining 2/3 for Paint

                // Resize Form1 to left 1/3
                this.WindowState = FormWindowState.Normal; // ensure not maximized
                this.SetBounds(0, 0, formWidth, screenHeight);

                // Move Paint to right 2/3
                int x = formWidth; // start right after Form
                int y = 0;
                MoveWindow(paintHandle, x, y, paintWidth, screenHeight, true);
            }
            else
            {
                MessageBox.Show("Could not find Paint window.");
            }
        }

        private Bitmap CropArtworkCanvas(Bitmap fullWindowBmp)
        {
            try
            {
                int topRibbonHeight = 240;
                int statusbarHeight = 35;
                int sideMargin = 10;

                int canvasX = Math.Min(sideMargin, fullWindowBmp.Width - 1);
                int canvasY = Math.Min(topRibbonHeight, fullWindowBmp.Height - 1);
                int canvasW = Math.Max(10, fullWindowBmp.Width - (sideMargin * 2));
                int canvasH = Math.Max(10, fullWindowBmp.Height - topRibbonHeight - statusbarHeight);

                Rectangle baseRect = new Rectangle(canvasX, canvasY, canvasW, canvasH);
                Bitmap baseBmp = fullWindowBmp.Clone(baseRect, fullWindowBmp.PixelFormat);

                int whiteMinX = baseBmp.Width, whiteMinY = baseBmp.Height;
                int whiteMaxX = 0, whiteMaxY = 0;
                bool foundWhiteCanvas = false;

                for (int y = 0; y < baseBmp.Height; y += 4)
                {
                    for (int x = 0; x < baseBmp.Width; x += 4)
                    {
                        Color pixel = baseBmp.GetPixel(x, y);
                        if (pixel.R > 250 && pixel.G > 250 && pixel.B > 250)
                        {
                            foundWhiteCanvas = true;
                            if (x < whiteMinX) whiteMinX = x;
                            if (x > whiteMaxX) whiteMaxX = x;
                            if (y < whiteMinY) whiteMinY = y;
                            if (y > whiteMaxY) whiteMaxY = y;
                        }
                    }
                }

                int startX = foundWhiteCanvas ? whiteMinX : 0;
                int startY = foundWhiteCanvas ? whiteMinY : 0;
                int endX = foundWhiteCanvas ? whiteMaxX : baseBmp.Width - 1;
                int endY = foundWhiteCanvas ? whiteMaxY : baseBmp.Height - 1;

                int strokeMinX = endX, strokeMinY = endY;
                int strokeMaxX = startX, strokeMaxY = startY;
                bool foundStrokes = false;

                for (int y = startY; y <= endY; y += 2)
                {
                    for (int x = startX; x <= endX; x += 2)
                    {
                        Color pixel = baseBmp.GetPixel(x, y);
                        if (pixel.R < 240 || pixel.G < 240 || pixel.B < 240)
                        {
                            foundStrokes = true;
                            if (x < strokeMinX) strokeMinX = x;
                            if (x > strokeMaxX) strokeMaxX = x;
                            if (y < strokeMinY) strokeMinY = y;
                            if (y > strokeMaxY) strokeMaxY = y;
                        }
                    }
                }

                if (foundStrokes && strokeMaxX > strokeMinX && strokeMaxY > strokeMinY)
                {
                    int margin = 30;
                    int finalX = Math.Max(0, strokeMinX - margin);
                    int finalY = Math.Max(0, strokeMinY - margin);
                    int finalW = Math.Min(baseBmp.Width - finalX, (strokeMaxX - strokeMinX) + (margin * 2));
                    int finalH = Math.Min(baseBmp.Height - finalY, (strokeMaxY - strokeMinY) + (margin * 2));

                    Rectangle finalRect = new Rectangle(finalX, finalY, finalW, finalH);
                    Bitmap finalBmp = baseBmp.Clone(finalRect, baseBmp.PixelFormat);
                    baseBmp.Dispose();
                    return finalBmp;
                }
                else if (foundWhiteCanvas && whiteMaxX > whiteMinX && whiteMaxY > whiteMinY)
                {
                    Rectangle canvasOnlyRect = new Rectangle(whiteMinX, whiteMinY, (whiteMaxX - whiteMinX) + 1, (whiteMaxY - whiteMinY) + 1);
                    Bitmap canvasOnlyBmp = baseBmp.Clone(canvasOnlyRect, baseBmp.PixelFormat);
                    baseBmp.Dispose();
                    return canvasOnlyBmp;
                }

                return baseBmp;
            }
            catch
            {
                return fullWindowBmp;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (paintProcess == null || paintProcess.HasExited)
            {
                MessageBox.Show("Paint is not open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(currentImagePath))
            {
                currentImagePath = Path.Combine(imageFolder, $"Drawing_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            }

            try
            {
                IntPtr paintHandle = paintProcess.MainWindowHandle;
                if (paintHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(paintHandle);
                    ShowWindow(paintHandle, SW_RESTORE);
                    Thread.Sleep(500);

                    if (GetWindowRect(paintHandle, out RECT rect))
                    {
                        int width = rect.Right - rect.Left;
                        int height = rect.Bottom - rect.Top;

                        if (width > 0 && height > 0)
                        {
                            using (Bitmap fullBmp = new Bitmap(width, height))
                            {
                                using (Graphics g = Graphics.FromImage(fullBmp))
                                {
                                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height));
                                }

                                using (Bitmap artworkBmp = CropArtworkCanvas(fullBmp))
                                {
                                    artworkBmp.Save(currentImagePath, System.Drawing.Imaging.ImageFormat.Png);
                                }
                            }
                        }
                    }
                }

                if (!paintProcess.HasExited)
                {
                    paintProcess.Kill();
                }

                if (File.Exists(currentImagePath))
                {
                    MessageBox.Show($"Saved drawing successfully: {Path.GetFileName(currentImagePath)}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (driveService != null)
                    {
                        UploadFileToDrive(currentImagePath);
                    }

                    Form3 nextPage = new Form3(currentImagePath);
                    nextPage.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Failed to save drawing file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Add this DllImport to bring Paint to foreground
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        private void LoadQrImage()
        {
            try
            {
                string[] searchPaths = new string[]
                {
                    Path.Combine(Application.StartupPath, "Resources", "QR.jpg"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "QR.jpg"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "QR.jpg"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "QR.jpg"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Resources", "QR.jpg")
                };

                foreach (string qrPath in searchPaths)
                {
                    if (File.Exists(qrPath))
                    {
                        using (var stream = new FileStream(qrPath, FileMode.Open, FileAccess.Read))
                        {
                            pictureBox1.Image = Image.FromStream(stream);
                        }
                        return;
                    }
                }

                pictureBox1.Image = Properties.Resources.QR;
            }
            catch { }
        }

        private void paint_interface_Load(object sender, EventArgs e)
        {
            LoadQrImage();
        }
    }
}
