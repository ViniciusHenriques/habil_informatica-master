using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using DAL.Persistence;
namespace HabilServiceNFSe
{
    public partial class Service1: ServiceBase
    {
        public Service1()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                GerandoArquivoLog(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadStart start = new ThreadStart(VerificarDocumentosEletronicos);
                Thread thread = new Thread(start);

                thread.Start();

            }catch(Exception ex)
            {
                GerandoArquivoLog(ex.Message);
            }
            
        }

        protected override void OnStop()
        {
            GerandoArquivoLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                        "           >>>>>>>>>>>>>>>>>>>>>>>>>>>> Finalizando HabilService  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                        "           >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n");
        }

        public void VerificarDocumentosEletronicos()
        {
            while (true)
            {
                Thread.Sleep(5000);
                try
                {

                    GerandoArquivoLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                                "           >>>>>>>>>>>>>>>>>>>>>>>>>>>> Inicializando HabilService <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                                "           >>>>>>>>>>>> Produto de Habil Informatica - Versao Sistema de Faturamento <<<<<<<<<<<<<<\n" +
                                "           >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n");

                    NFeFuncoes NFe = new NFeFuncoes();
                    NFe.EnviarEvento();

                    //NFSeFuncoes NFSe = new NFSeFuncoes();
                    //NFSe.ExecutarFuncoes();

                    //CTEFuncoes CTe = new CTEFuncoes();
                    //CTe.EnviarDesacordo();

                }
                catch (Exception ex)
                {
                    GerandoArquivoLog(ex.Message);
                }
            }
        }
        public void GerandoArquivoLog(string strDescrição)
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
        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
