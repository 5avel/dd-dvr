using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.BL
{
    public class VideoFolderResolver
    {
        private ILogerService loger; // для логирования 
        private IConfigurationService configuration; // для доступа к настройкам прогаммы

        public VideoFolderResolver(ILogerService loger, IConfigurationService configuration)
        {
            this.loger = loger;
            this.configuration = configuration;
        }
        public VideoFolderResolver()
        {
        }

        public bool ResolveRawVideoFolder(string path, out string saveVideoFolder, out int streamCount, out int videoFilesCount)
        {
            // получить количество файлов с расширением *.*264
            // получить количество потоков
            // получить название родительской папки
            // получить дату из названия родительской папки
            // получить путь к корню для сохранения конвертированного вигео
            // сформировать полный путь для сохранения конвертированного вигео: корень/номер_автобуса/дата
            // проверить свободное место на диске для сохранения конвертированного видео


            saveVideoFolder = @"C:\video\4567\2017-02-20";
            System.IO.Directory.CreateDirectory(saveVideoFolder);
            streamCount = 3;
            videoFilesCount = 69;
            return true;
        }


    }
}
