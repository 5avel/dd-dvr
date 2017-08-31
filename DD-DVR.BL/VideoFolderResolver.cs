using DD_DVR.Data;
using System.IO;
using System.Security.Cryptography;
using System.Management;
using System.Text;

namespace DD_DVR.BL
{
    public class VideoFolderResolver
    {
        public VideoFolderResolver()
        {
            
        }

        public bool ResolveRawVideoFolder(string path, string busTitle, out string saveVideoFolder, out int streamCount, out int videoFilesCount)
        {
            // получить количество файлов с расширением *.*264
            // получить количество потоков
            // получить название родительской папки
            // получить дату из названия родительской папки
            // получить путь к корню для сохранения конвертированного вигео
            // сформировать полный путь для сохранения конвертированного вигео: корень/номер_автобуса/дата
            // проверить свободное место на диске для сохранения конвертированного видео

            string outputVodeoDir = ConfigurationRepository.LoadObjFromFile().OutputVodeoDir;
            
           DirectoryInfo di = new DirectoryInfo(path);
            var separator = Path.DirectorySeparatorChar;

            saveVideoFolder = outputVodeoDir + busTitle + separator +di.Name;
            System.IO.Directory.CreateDirectory(saveVideoFolder);
            streamCount = 3;
            videoFilesCount = 69;
            return true;
        }

        #region Методы для проверки лицензий
        public static bool Test(string key)
        {
            if (GetHashString2(GetHashString()) == key) return true;
            else return false;
        }

        private static string GetHashString()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Product, SerialNumber, Manufacturer FROM Win32_BaseBoard");

            ManagementObjectCollection information = searcher.Get();

            string s = "";
            foreach (ManagementObject obj in information)
            {
                foreach (PropertyData data in obj.Properties)
                {
                    //Console.WriteLine(string.Format("{0} = {1}", data.Name, data.Value));
                    s += data.Value;
                }

            }
            s += "omyr;@#$";
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        private static string GetHashString2(string s)
        {
            s += "ubrwk~!@DD-DVR";
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        #endregion


    }
}
