using System;
using System.Windows.Forms;

namespace datetime
{
    public class Program
    {
        public static Timer timer = new Timer(new TimeSpan(0, 0, 0, 1, 0));

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());            
        }
    }
}
