using System.IO;
using System.Xml.Serialization;

namespace WSHabilMail
{
    public class Config
    {
        #region Default Data

        private const bool CORPOEHHTML = true;
        private const string PATHPADRAO = @"C:\TEMP";
        private const string CONNECTSTRING = @"Data Source=HABILSERVER\SQLEXPRESS2014;Initial Catalog=HABIL_INFO_TESTE;user=habil;password=habil";
        private const string SERVIDORPOP = "smtp.gmail.com";
        private const int PORTA = 587;
        private const string NOMEUSUARIO = "habilINFO";
        private const string USUARIO = "disparador.emails.habil@gmail.com";
        private const string SENHA = "@disparadorHabil@";
        private const bool USASSL = false;
        private static readonly string[] ARQUIVOS = new string[] {
        @"pdf|c:\temp",
        @"txt|c:\temp",
    };
        #endregion

        // name of the .xml file


        public static string CONFIG_FNAME = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config.xml";

        public static ConfigData GetConfigData()
        {
            if (!File.Exists(CONFIG_FNAME)) // create config file with default values
            {
                using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Create))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
                    ConfigData sxml = new ConfigData();
                    xs.Serialize(fs, sxml);
                    return sxml;
                }
            }
            else // read configuration from file
            {
                using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
                    ConfigData sc = (ConfigData)xs.Deserialize(fs);
                    return sc;
                }
            }
        }

        public static bool SaveConfigData(ConfigData config)
        {
            if (!File.Exists(CONFIG_FNAME)) return false; // don't do anything if file doesn't exist

            using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
                xs.Serialize(fs, config);
                return true;
            }
        }

        // this class holds configuration data
        public class ConfigData
        {
            public string PathPadrao;
            public string ServidorPop;
            public string ConnectString;
            public int Porta;
            public string Usuario;
            public string NomeUsuario;
            public string Senha;
            public string[] Arquivos;
            public bool CorpoEhHTML;
            public bool UsaSSL;

            public ConfigData()
            {
                PathPadrao = PATHPADRAO;
                ConnectString = CONNECTSTRING;
                ServidorPop = SERVIDORPOP;
                Porta = PORTA;
                Usuario = USUARIO;
                NomeUsuario = NOMEUSUARIO;
                Senha = SENHA;
                Arquivos = ARQUIVOS;
                CorpoEhHTML = CORPOEHHTML;
                UsaSSL= USASSL;
            }
        }
    }
}
