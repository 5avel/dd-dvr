using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DD_DVR.Video
{
    class Stream
    {
        public MediaPlayer player = new MediaPlayer();
        private string CurentSource = null; // текущий файл воспроизведения
        private int CurentPosition = 0;     // позиция воспроизведения

        public DateTime Position { get; set; } // позиия воспроизведения видео

        List<string> FileList = new List<string>(); // список файлов

        public double SpeedRatio
        {
            get { return player.SpeedRatio; }
            set { player.SpeedRatio = value; }
        } // привязать напрямую к свойству плеера 
        public DrawingBrush VideoBrush { set; get; }


        public Stream(List<string> fileList)
        {
            if(fileList != null && fileList.Count > 0)
            {
                FileList = fileList;
                player.MediaEnded += Player_MediaEnded;
                

                player.Open(new Uri(FileList[CurentPosition], UriKind.RelativeOrAbsolute));
                VideoBrush = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player });

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
            player.Play();
        }

        internal void Stop()
        {
            player.Stop();
        }

        internal void Pause()
        {
            player.Pause();
        }

        internal void Step(bool isRightStep = true)
        {
            if(isRightStep)
                player.Position = new TimeSpan(0, 0, 0, 0, (int)player.Position.TotalMilliseconds+200);
            else
                player.Position = new TimeSpan(0, 0, 0, 0, (int)player.Position.TotalMilliseconds - 200);

            Play();
            Pause();
        }
    }
}
