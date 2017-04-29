using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace DD_DVR.Model
{
    /// <summary>
    /// Структура описывающая данные для хранения настроек
    /// </summary>
    public struct Settings
    {
        public string ip;
        public string port;
        public int GoodsCount;
    }
    class SettingsProvider
    {
        private SettingsProvider() { }

        public static void SaveSettings(object s)
        {
            XmlSerializer myXmlSer = new XmlSerializer(s.GetType());
            StreamWriter myWriter = new StreamWriter(Environment.CurrentDirectory + @"\settings.cfg");
            myXmlSer.Serialize(myWriter, s);
            myWriter.Close();
        }

        public static void GetSettings(ref Settings s)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "settings.cfg"))
            {
                XmlSerializer myXmlSer = new XmlSerializer(typeof(Settings));
                FileStream mySet = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "settings.cfg", FileMode.Open);
                s = (Settings)myXmlSer.Deserialize(mySet);
                mySet.Close();
            }
            else
            {
                MessageBox.Show("Файл: " + AppDomain.CurrentDomain.BaseDirectory + "settings.cfg" + " Необнаружен!");
            }
        }
    }
}
