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
            GerandoArquivoLog("Começou");
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
            GerandoArquivoLog("iniciando servico");
            try
            {
                ThreadStart start = new ThreadStart(verificarDocumentosEletronicos);
                Thread thread = new Thread(start);

                thread.Start();
            }catch(Exception ex)
            {
                GerandoArquivoLog(ex.Message);
            }
            
        }

        protected override void OnStop()
        {
            NFSeFuncoes NFSe = new NFSeFuncoes();

            NFSe.GerandoArquivoLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                        "           >>>>>>>>>>>>>>>>>>>>>>>>>>>> Finalizando HabilServiceNFse <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                        "           >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n",1);



        }

        public void verificarDocumentosEletronicos()
        {
            //GerandoArquivoLog("Entrou na rotina");
            while (true)
            {
                //GerandoArquivoLog("Vai rodar 5 seg");
                Thread.Sleep(5000);
                //GerandoArquivoLog("rodou");
                try
                {
                    NFeFuncoes NFe = new NFeFuncoes();
                    NFe.EnviarNFe();

                    //GerandoArquivoLog("entrou try");
                    //clsEmail c2 = new clsEmail();
                    //c2.ProcessaEnvio();
                    //GerandoArquivoLog("instanciou class NFSe");
                    NFSeFuncoes NFSe = new NFSeFuncoes();

                    //GerandoArquivoLog("Log de inicio de serviço");
                    NFSe.GerandoArquivoLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                                "           >>>>>>>>>>>>>>>>>>>>>>>>>>>> Inicializando HabilServiceNFse <<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                                "           >>>>>>>>>>>> Produto de Habil Informatica - Versao Sistema de Faturamento <<<<<<<<<<<<<<\n" +
                                "           >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n", 1);
                    //NFSe.ExecutarFuncoes();
                    //GerandoArquivoLog("Enviou NFSe's");
                    Thread.Sleep(5000);

                    //GerandoArquivoLog("Vai rodar class CTE");
                    CTEFuncoes CTe = new CTEFuncoes();
                    CTe.EnviarDesacordo();

                    //GerandoArquivoLog("enviou desacordos");


                    
                    
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
