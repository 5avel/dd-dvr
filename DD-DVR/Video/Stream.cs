using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DD_DVR.Video
{
    class Stream
    {
        private MediaPlayer player = null;
        private string CurentSource = null; // текущий файл воспроизведения
        private int CurentPosition = 0;     // позиция воспроизведения

        public DateTime Position { get; set; } // позиия воспроизведения видео

        List<string> FileList = new List<string>(); // список файлов

        public double SpeedRatio { get; set; } // привязать напрямую к свойству плеера 
        public DrawingBrush VideoBrush { set; get; }


        public Stream(List<string> fileList)
        {
            FileList = fileList;
            SpeedRatio = 1;
           
        }

        public void Play()
        {
            if(Position == null)
            {
                CurentSource = FileList[0];
                CurentPosition = 0;
                player = new MediaPlayer() { SpeedRatio = SpeedRatio };
                player.Open(new Uri(CurentSource, UriKind.RelativeOrAbsolute));
                VideoBrush = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player });
            }
            player.Play();
        }

      

    }
}
