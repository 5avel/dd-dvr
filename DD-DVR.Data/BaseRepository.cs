using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                return (T)formatter.Deserialize(fs);
            }
        }

        public void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
            }
        }

    }
}
