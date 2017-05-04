using DD_DVR.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ConfigurationRepository cr = new ConfigurationRepository();
            string outputVodeoDir = cr.GetOutputVodeoDir();
            DirectoryInfo di = new DirectoryInfo(path);
            var separator = Path.DirectorySeparatorChar;

            saveVideoFolder = outputVodeoDir + separator + busTitle + separator +di.Name;
            System.IO.Directory.CreateDirectory(saveVideoFolder);
            streamCount = 3;
            videoFilesCount = 69;
            return true;
        }


    }
}
