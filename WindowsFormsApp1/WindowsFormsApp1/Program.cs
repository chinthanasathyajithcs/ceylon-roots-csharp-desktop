using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show($"Error: {e.Exception.Message}\n\n{e.Exception.StackTrace}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                MessageBox.Show($"Error: {ex?.Message}\n\n{ex?.StackTrace}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            DbHelper.InitializeDatabase();

            Application.Run(new LauncherForm());
        }
    }
}
