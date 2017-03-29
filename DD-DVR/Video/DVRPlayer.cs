using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Video
{
    class DVRPlayer : INotifyPropertyChanged
    {

        // Может исползовать массив стримов, удобней перебирать цыклом и делать однотипные операции!
        public List<Stream> Streams { set; get; }

        public string VideoSourcePath { get; set; } // путь к папке с файлами *.mkv

       
        public DVRPlayer(string videoFolder)
        {
            // проверка папки на сушествование
            Streams = new List<Stream>();
            DirectoryInfo dir = new DirectoryInfo(videoFolder);

            List<string> videoPath1 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-01-*-*-*.mkv")) videoPath1.Add(item.FullName);
            if (videoPath1.Count > 0) Streams.Add(new Stream(videoPath1));

            List<string> videoPath2 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-02-*-*-*.mkv")) videoPath2.Add(item.FullName);
            if (videoPath2.Count > 0) Streams.Add(new Stream(videoPath2));

            List<string> videoPath3 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-03-*-*-*.mkv")) videoPath3.Add(item.FullName);
            if (videoPath3.Count > 0) Streams.Add(new Stream(videoPath3));

            List<string> videoPath4 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-04-*-*-*.mkv")) videoPath4.Add(item.FullName);
            if (videoPath4.Count > 0) Streams.Add(new Stream(videoPath4));
        }



        public void Play()
        {
            foreach (Stream s in Streams) { s.Play(); } 
        }

        public void Stop()
        {
            foreach (Stream s in Streams) s.Stop();
        }

        public void Pause()
        {
            foreach (Stream s in Streams) s.Pause();
        }

        public void SpeedUp()
        {
            foreach (Stream s in Streams) s.player.SpeedRatio*=2;
        }

        public void SpeedDown()
        {
            foreach (Stream s in Streams) s.player.SpeedRatio /= 2;
        }

        #region implementation INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Новый OnPropertyChanged которому вообще не нужно передавать ни свойство ни название свойства)))
        /// </summary>
        /// <param name="propertyName"> Заполняется автоматически названием вызываюшего члена!</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion implementation INotifyPropertyChanged


    }
}
