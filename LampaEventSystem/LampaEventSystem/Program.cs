using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace LampaEventSystem
{
   public static class Program
    {
        private static void NewStatusText(string message)
        {
            EasySQL logger = new EasySQL("Insert into EventSystemLog(message) values (SYSDATETIME() +\""
                + " "+ message + "\")");
            logger.Update();
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new formMain());
        }
    }
}
