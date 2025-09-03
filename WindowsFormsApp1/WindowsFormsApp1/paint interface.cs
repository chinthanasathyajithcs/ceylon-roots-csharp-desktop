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
            imageFolder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            // Initialize Google Drive API
            InitializeDriveService();
        }
        private void InitializeDriveService()
        {
            try
            {
                string[] scopes = { DriveService.Scope.DriveReadonly };

                var clientSecrets = new ClientSecrets
                {
                    ClientId = "991942011635-v5a807q0v0g94u5mottsl0f4jergdhg3.apps.googleusercontent.com",
                    ClientSecret = "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"
                };

                // Use a fixed token path in AppData
                var tokenPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Drive.Auth.Store");
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    scopes,
                    "my_app_user", // fixed user key
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true) // 'true' to keep tokens
                ).Result;

                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "PaintUploader"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize Google Drive API: " + ex.Message);
            }
        }
        private void UploadFileToDrive(string filePath)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { "1KZFjLkd9fXEUaNJYJ_auX0UCxP1cjWvS" } // your folder id
                };

                FilesResource.CreateMediaUpload request;
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    request = driveService.Files.Create(fileMetadata, stream, "image/png");
                    request.Fields = "id";
                    request.Upload();
                }

                var file = request.ResponseBody;
                MessageBox.Show("Uploaded to Drive. File ID: " + file.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to upload to Drive: " + ex.Message);
            }

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

                // Google Drive "images" folder ID
                string folderId = "1KZFjLkd9fXEUaNJYJ_auX0UCxP1cjWvS";

                // Query for the latest image
                var listReq = driveService.Files.List();
                listReq.Q = $"'{folderId}' in parents and mimeType contains 'image/' and trashed = false";
                listReq.OrderBy = "createdTime desc, modifiedTime desc";
                listReq.PageSize = 1;
                listReq.Fields = "files(id, name, createdTime, modifiedTime)";
                listReq.SupportsAllDrives = true;
                listReq.IncludeItemsFromAllDrives = true;

                var result = listReq.Execute();
                var latest = result.Files.FirstOrDefault();

                if (latest == null)
                {
                    MessageBox.Show("No images found in the Drive folder.");
                    return;
                }

                // Save with unique name to avoid overwriting
                string ext = Path.GetExtension(latest.Name);
                string localPath = Path.Combine(imageFolder, $"DriveImage_{DateTime.Now:yyyyMMdd_HHmmss}{ext}");

                var getReq = driveService.Files.Get(latest.Id);
                getReq.SupportsAllDrives = true;
                using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                {
                    getReq.Download(fs);
                }

                MessageBox.Show($"Downloaded latest image: {localPath}");

                // Set current image path for saving later
                currentImagePath = localPath;

                // === Open Paint side-by-side with the form ===
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
                MessageBox.Show("Failed to open latest image: " + ex.Message);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (paintProcess == null || paintProcess.HasExited)
            {
                MessageBox.Show("Paint is not open.");
                return;
            }

            // If no image path set, create a new one
            if (string.IsNullOrEmpty(currentImagePath))
            {
                currentImagePath = Path.Combine(imageFolder, $"Drawing_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            }

            try
            {
                Clipboard.SetText(currentImagePath);

                IntPtr paintHandle = paintProcess.MainWindowHandle;
                if (paintHandle != IntPtr.Zero)
                    SetForegroundWindow(paintHandle);

                Thread.Sleep(1500);

                // Save file
                SendKeys.SendWait("^s");
                Thread.Sleep(1500);

                SendKeys.SendWait("^v");
                Thread.Sleep(500);

                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(2000);

                // Close Paint
                if (!paintProcess.HasExited)
                {
                    paintProcess.CloseMainWindow();
                    paintProcess.WaitForExit(3000);
                    if (!paintProcess.HasExited)
                        paintProcess.Kill();
                }

                MessageBox.Show($"Saved as {currentImagePath}");

                // Open Form2 with the saved image
                Form3 nextPage = new Form3(currentImagePath);
                nextPage.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save image: " + ex.Message);
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

    }
}
