using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace WSHabilMail
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ThreadStart start = new ThreadStart(verificarprocesso);
            Thread thread = new Thread(start);

            thread.Start();
            
        }

        protected override void OnStop()
        {
        }

        public void verificarprocesso()
        {

            while (true)
            {
                Thread.Sleep(10000);

                clsEmail c2 = new clsEmail();
                c2.ProcessaEnvioMails();
            }

        
        
        }


    
    }
}
