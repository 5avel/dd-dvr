using System.IO;
using System.Xml.Serialization;

namespace DD_DVR.Data
{
    public class Repository
    {
        private readonly string defoultePath = "Configs\\";
        private string path;

        /// <summary>
        /// загружает объект из файла XML
        /// </summary>
        /// <typeparam name="T">закрывается тибом объекта который нужно получить</typeparam>
        /// <param name="savePath">принимает путь к объекту, без имени.(имя - это имя типа +".xml"),
        /// если путь не задан то используется Configs\. </param>
        /// <returns>возвращает экземплар обекта</returns>
        public T LoadObjFromFile<T>(string loadPath = "") where T : new()
        {
            if (!string.IsNullOrEmpty(loadPath)) path = loadPath;
            else path = defoultePath;

            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path + typeof(T).Name+".xml", FileMode.Open))
            {
                return (T)formatter.Deserialize(fs);
            }
        }

        public void SaveObjToFile<T>(string savePath = "") where T : new()
        {
            if (!string.IsNullOrEmpty(savePath)) path = savePath;
            else path = defoultePath;

            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path + typeof(T).Name + ".xml", FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

    }
}
