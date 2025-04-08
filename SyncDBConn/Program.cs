using System;
using System.Windows.Forms;

namespace SyncDBConn
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm()); // ← 시작할 폼 이름
        }
    }
}
