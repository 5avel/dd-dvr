using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DD_DVR.Data
{
    public class Repository<T> where T : new()
    {
        private static readonly string defoultePath = "Configs\\";
        private static string path;
        private static object syncRoot = new object();

        /// <summary>
        /// загружает объект из файла XML
        /// </summary>
        /// <typeparam name="T">закрывается тибом объекта который нужно получить</typeparam>
        /// <param name="savePath">принимает путь к объекту, без имени.(имя - это имя типа +".xml"),
        /// если путь не задан то используется Configs\. </param>
        /// <returns>возвращает экземплар обекта</returns>
        public static T LoadObjFromFile(string loadPath = "") 
        {
            if (!string.IsNullOrEmpty(loadPath)) path = loadPath;
            else path = defoultePath;

            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path + typeof(T).Name+".xml", FileMode.Open))
            {
                return (T)formatter.Deserialize(fs);
            }
        }

        public static void SaveObjToFile(T obj, string savePath = "")
        {
            if (!string.IsNullOrEmpty(savePath)) path = savePath;
            else path = defoultePath;

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            XmlSerializer formatter = new XmlSerializer(typeof(T));
            lock(syncRoot)
            {
                using (FileStream fs = new FileStream(path + typeof(T).Name + ".xml", FileMode.Create))
                {
                    formatter.Serialize(fs, obj);
                }
            }
        }

    }
}
