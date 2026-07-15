using System;
using System.Windows.Forms;
using AP_Final_Project.UI;

namespace AP_Final_Project
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainMenuForm());
        }
    }
}
