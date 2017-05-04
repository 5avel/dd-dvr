using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DD_DVR.BL.Player
{
    public class DVRPlayer
    {
        public MediaPlayer p1 = new MediaPlayer() { ScrubbingEnabled = true };
        private MediaPlayer p2 = new MediaPlayer() { ScrubbingEnabled = true };
        private MediaPlayer p3 = new MediaPlayer() { ScrubbingEnabled = true };
        private MediaPlayer p4 = new MediaPlayer() { ScrubbingEnabled = true };

        private ObservableCollection<MediaSource> mediaSourceCollection = new ObservableCollection<MediaSource>();
        private MediaSource curentMediaSource;
        private DVRPlayer()
        {
            Cam1 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p1 });
            Cam2 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p2 });
            Cam3 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p3 });
            Cam4 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p4 });
            MediaSourceCollection = new ObservableCollection<MediaSource>();
        }

        public DrawingBrush Cam1 { set; get; }
        public DrawingBrush Cam2 { set; get; }
        public DrawingBrush Cam3 { set; get; }
        public DrawingBrush Cam4 { set; get; }

        public ObservableCollection<MediaSource> MediaSourceCollection { set; get; }
        public MediaSource CurentMediaSource
        {
            set
            {
                curentMediaSource = value;
                SetMediaSource(curentMediaSource);
            }
            get { return curentMediaSource; }
        }

        public void LoadMedia(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            var stream1FileInfo = dir.GetFiles(@"*-01-*-*-*.mkv");
            var stream2FileInfo = dir.GetFiles(@"*-02-*-*-*.mkv");
            var stream3FileInfo = dir.GetFiles(@"*-03-*-*-*.mkv");
            var stream4FileInfo = dir.GetFiles(@"*-04-*-*-*.mkv");

            for (int i = 0; i < stream1FileInfo.Length; i++)
            {
                MediaSourceCollection.Add(new MediaSource
                {
                    Stream1 = stream1FileInfo[i].FullName,
                    Stream2 = stream2FileInfo[i].FullName,
                    Stream3 = stream3FileInfo[i].FullName,
                    Stream4 = stream4FileInfo[i].FullName
                });
            }

            CurentMediaSource = MediaSourceCollection[0];
        }

        private void SetMediaSource(MediaSource ms)
        {
            p1.Open(new Uri(ms.Stream1, UriKind.RelativeOrAbsolute));
            p2.Open(new Uri(ms.Stream2, UriKind.RelativeOrAbsolute));
            p3.Open(new Uri(ms.Stream3, UriKind.RelativeOrAbsolute));
            p4.Open(new Uri(ms.Stream4, UriKind.RelativeOrAbsolute));
        }


        public void Play()
        {
            p1.Play();
            p2.Play();
            p3.Play();
            p4.Play();
        }

        public void Pause()
        {
            p4.Pause();
            p3.Pause();
            p2.Pause();
            p1.Pause();
        }

        public void SetSpeedRatio(double curspeedRatio)
        {
            p1.SpeedRatio = curspeedRatio;
            p2.SpeedRatio = curspeedRatio;
            p3.SpeedRatio = curspeedRatio;
            p4.SpeedRatio = curspeedRatio;
        }



        #region Implementing Multithreaded Singleton
        private static volatile DVRPlayer instance;
        private static object syncRoot = new Object();
        public static DVRPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DVRPlayer();
                    }
                }
                return instance;
            }
        }
        #endregion Implementing Multithreaded Singleton
            
    }

    public class MediaSource
    {
        public string Stream1 { set; get; }
        public string Stream2 { set; get; }
        public string Stream3 { set; get; }
        public string Stream4 { set; get; }

        public DateTime StartDT { set; get; }
        public DateTime FinishDT { set; get; }

    }


}
