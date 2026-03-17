using System;
using System.Windows.Forms;
using EVASION.WinForms.Forms;

namespace EVASION.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new LoginForm());
    }
}