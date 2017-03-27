using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Video
{
    class DVRPlayer
    {

        // Может исползовать массив стримов, удобней перебирать цыклом и делать однотипные операции!
        List<Stream> streams = new List<Stream>();

        public string VideoSourcePath { get; set; } // путь к папке с файлами *.mkv

        public DateTime Position { get; set; } // позиия воспроизведения видео


    }
}
