using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LampaEventService
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionInformer;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new LampaEventService()
            };
            ServiceBase.Run(ServicesToRun);
        }

        static void UnhandledExceptionInformer(object sender, UnhandledExceptionEventArgs e)
        {
            List<string> admEmails = new List<string>(new string[] { "pcissosimple@gmail.com", "lenka1.2@ya.ru" });


            
        }
    }
}
