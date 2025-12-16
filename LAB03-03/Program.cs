using System;
using System.Windows.Forms;

namespace LAB03_03
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Đảm bảo chạy StudentInfoForm
            Application.Run(new StudentInfoForm());
        }
    }
}