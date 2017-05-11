using System;
using System.Collections.ObjectModel;
using System.IO;
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

        public event EventHandler<MediaSourceEventArgs> CurentMediaSourceUpdated = delegate { };

        private ObservableCollection<MediaSource> mediaSourceCollection = new ObservableCollection<MediaSource>();
        private MediaSource curentMediaSource;
        private DVRPlayer()
        {
            Cam1 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p1 });
            Cam2 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p2 });
            Cam3 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p3 });
            Cam4 = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = p4 });
            MediaSourceCollection = new ObservableCollection<MediaSource>();

            // подписка на событие окончания видео файла p1
            p1.MediaEnded += P1_MediaEnded;

        }

        private void P1_MediaEnded(object sender, EventArgs e)
        {
            var curentMediaSourceNum = MediaSourceCollection.IndexOf(CurentMediaSource);
            if (MediaSourceCollection.Count - 1 <= curentMediaSourceNum) return;

            CurentMediaSource = (MediaSourceCollection[curentMediaSourceNum + 1]);
            Play();
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
                if(curentMediaSource != null) SetMediaSource(curentMediaSource);
                CurentMediaSourceUpdated(this, new MediaSourceEventArgs(curentMediaSource));
            }
            get { return curentMediaSource; }
        }

        private TimeSpan position;
        public TimeSpan Position
        {
            get { return position; }
            set
            {
                position = value;
                p1.Position = position;
                p2.Position = position;
                p3.Position = position;
                p4.Position = position;
            }
        }

        public bool LoadMedia(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            Stop();
            MediaSourceCollection.Clear(); // clear MediaSourceCollection
            //Close(); // clear MediaPlayer

            var stream1FileInfo = dir.GetFiles(@"*-01-*-*-*.mkv");
            var stream2FileInfo = dir.GetFiles(@"*-02-*-*-*.mkv");
            var stream3FileInfo = dir.GetFiles(@"*-03-*-*-*.mkv");
            var stream4FileInfo = dir.GetFiles(@"*-04-*-*-*.mkv");

            for (int i = 0; i < stream1FileInfo.Length; i++)
            {
                MediaSourceCollection.Add(new MediaSource
                {
                    Stream1 = i < stream1FileInfo.Length ? stream1FileInfo[i].FullName : null,
                    Stream2 = i < stream2FileInfo.Length ? stream2FileInfo[i].FullName : null,
                    Stream3 = i < stream3FileInfo.Length ? stream3FileInfo[i].FullName : null,
                    Stream4 = i < stream4FileInfo.Length ? stream4FileInfo[i].FullName : null
                });
            }

            if(MediaSourceCollection.Count == 0) return false;

            CurentMediaSource = MediaSourceCollection[0];
            return true;
        }

        private void SetMediaSource(MediaSource ms)
        {
            if (ms.Stream1 != null)
            {
                p1.ScrubbingEnabled = true;
                p1.Open(new Uri(ms.Stream1, UriKind.RelativeOrAbsolute));
            }
            if (ms.Stream2 != null)
            {
                p2.ScrubbingEnabled = true;
                p2.Open(new Uri(ms.Stream2, UriKind.RelativeOrAbsolute));
            }
            if (ms.Stream3 != null)
            {
                p3.ScrubbingEnabled = true;
                p3.Open(new Uri(ms.Stream3, UriKind.RelativeOrAbsolute));
            }
            if (ms.Stream4 != null)
            {
                p4.ScrubbingEnabled = true;
                p4.Open(new Uri(ms.Stream4, UriKind.RelativeOrAbsolute));
            }
            
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

        public void Stop()
        {
            p4.Stop();
            p3.Stop();
            p2.Stop();
            p1.Stop();
        }

        private void Close()
        {
            p4.Close();
            p3.Close();
            p2.Close();
            p1.Close();
        }

        public void SetSpeedRatio(double curspeedRatio)
        {
            p1.SpeedRatio = curspeedRatio;
            p2.SpeedRatio = curspeedRatio;
            p3.SpeedRatio = curspeedRatio;
            p4.SpeedRatio = curspeedRatio;
        }

        public void LeftStep()
        {
            var step = new TimeSpan(0, 0, 0, 0, (int)p1.Position.TotalMilliseconds - 84);
            p4.Position = step;
            p3.Position = step;
            p2.Position = step;
            p1.Position = step;
        }

        public void RightStep()
        {
            var step = new TimeSpan(0, 0, 0, 0, (int)p1.Position.TotalMilliseconds + 84);
            p1.Position = step;
            p2.Position = step;
            p3.Position = step;
            p4.Position = step;
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
        private string _stream1;
        public string Stream1
        {
            get { return _stream1; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _stream1 = value;
                    SetStartOrFinishDT(value);
                }
            }
        }

        public string Stream2 { set; get; }
        public string Stream3 { set; get; }
        public string Stream4 { set; get; }

        public DateTime StartDT { set; get; }
        public DateTime FinishDT { set; get; }

        private void SetStartOrFinishDT(string value)
        {  
            FileInfo fi = new FileInfo(value);
            string fName = fi.Name;            // "201-01-194924-200424-00p000.h264.mkv"
            string pName = fi.Directory.Name;  // "2017-04-26"

            string temp = pName + "_" + fName.Substring(7,6);
            StartDT = DateTime.ParseExact(temp, "yyyy-MM-dd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);

            string temp2 = pName + "_" + fName.Substring(14, 6);
            FinishDT = DateTime.ParseExact(temp, "yyyy-MM-dd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);  
        }

        public string Text
        {
            get
            {
                return StartDT.ToString("HH:mm:ss") + "-" + FinishDT.ToString("HH:mm:ss");
            }
            set { }
        }

    }

    public class MediaSourceEventArgs: EventArgs
    {
        public MediaSource mediaSource;
        public MediaSourceEventArgs(MediaSource ms)
        {
            mediaSource = ms;
        }
    }
}
