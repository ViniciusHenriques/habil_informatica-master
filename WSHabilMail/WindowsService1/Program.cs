using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace WSHabilMail
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

#if DEBUG
            clsEmail c3 = new clsEmail();
            c3.ProcessaEnvioMails();

#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1()
                };
                ServiceBase.Run(ServicesToRun);
#endif

//            clsEmail c3 = new clsEmail();
//            c3.ProcessaEnvioMails();
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
