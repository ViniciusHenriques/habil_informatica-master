using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WindowsService2
{
    public class Config
    {
        #region Default Data

        private const string CONNECTSTRING = @"Data Source=SERVIDOR\SQLEXPRESS2012;Initial Catalog=Habil;user=habil;password=habil";

        #endregion

        // name of the .xml file
        public static string CONFIG_FNAME = @"c:\\DIRETORIO_CONFIG\\config.xml";

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
            public string ConnectString;

            public ConfigData()
            {
                ConnectString = CONNECTSTRING;
            }
        }
    }
}
