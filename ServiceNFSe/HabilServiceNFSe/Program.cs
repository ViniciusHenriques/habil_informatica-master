
using System;
using System.IO;
using System.ServiceProcess;


namespace HabilServiceNFSe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static void Main()
        {
            
            try
            {
#if DEBUG
                Service1 service = new Service1();
                service.OnDebug();

#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1()
                };
                ServiceBase.Run(ServicesToRun);
#endif
            }
            catch (Exception ex)
            {
                GerandoArquivoLog(ex.Message);
            }

        }
        static void GerandoArquivoLog(string strDescrição)
        {
            try
            {

                DateTime data = DateTime.Now;

                string CaminhoArquivoLog = "";

                CaminhoArquivoLog = "C:\\HabilInfoWeb\\Log.txt";

                if (!System.IO.File.Exists(CaminhoArquivoLog))
                {
                    FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(file);
                    bw.Close();
                }

                string nomeArquivo = CaminhoArquivoLog;
                System.IO.TextWriter arquivo = System.IO.File.AppendText(nomeArquivo);

                // Agora é só sair escrevendo
                arquivo.WriteLine(data.ToString("HH:mm:ss") + " - " + strDescrição);

                arquivo.Close();
            }
            catch
            {

            }
        }
    }
}
