using System;
using System.Collections.Generic;
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
            if(fileList != null && fileList.Count > 0)
            {
                FileList = fileList;
                player = new MediaPlayer();
                player.MediaEnded += Player_MediaEnded;
                player.SpeedRatio = 32;
            }
            
            

           
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            CurentPosition++;
            player.Open(new Uri(FileList[CurentPosition], UriKind.RelativeOrAbsolute));
            player.Play();
        }

        public void Play()
        {
            
            player.Open(new Uri(FileList[CurentPosition], UriKind.RelativeOrAbsolute));
            VideoBrush = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player });
            
            player.Play();
        }

      

    }
}
