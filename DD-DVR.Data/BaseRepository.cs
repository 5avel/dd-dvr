using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace DD_DVR.Data
{
    public abstract class BaseRepository<T> where T : class
    {
        protected BaseRepository() { }

        public static string savePath = (string)(typeof(T).GetField("savePath", BindingFlags.Public | BindingFlags.Static).GetValue(null));
        private static object syncRoot = new Object();
        private static T instance;
        public static T Instanse
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = Load();
                    }
                }
                return instance;
            }
        }
        protected static T Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(savePath, FileMode.Open))
            {
                return (T)formatter.Deserialize(fs);
            }
        }

        public void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

    }
}
