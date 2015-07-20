using System;
using System.Xml;

public class AppConfigHelper
    {
        private XmlDocument xmlDoc;
        private string _appConfigName;

        public AppConfigHelper(string appConfigName)
        {
            _appConfigName = appConfigName;

            if (xmlDoc == null)
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(Application.StartupPath + "\\" + _appConfigName);
            }
        }

        public void AddKey(string strKey, string strValue)
        {
            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
            try
            {
                if (KeyExists(strKey))
                    throw new ArgumentException("Key name: <" + strKey + "> already exists in the configuration.");

                XmlNode newChild = appSettingsNode.FirstChild.Clone();
                newChild.Attributes["key"].Value = strKey;
                newChild.Attributes["value"].Value = strValue;
                appSettingsNode.AppendChild(newChild);

                xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\" + _appConfigName);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateKey(string strKey, string newValue)
        {
            if (!KeyExists(strKey))
                throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");
            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    childNode.Attributes["value"].Value = newValue;
            }
            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\" + _appConfigName);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        public void DeleteKey(string strKey)
        {
            if (!KeyExists(strKey))
                throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");
            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    appSettingsNode.RemoveChild(childNode);
            }
            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\" + _appConfigName);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        private bool KeyExists(string strKey)
        {
            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    return true;
            }
            return false;
        }
    }
