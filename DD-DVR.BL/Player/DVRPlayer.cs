 using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace DD_DVR.BL.Player
{
    public enum SourceType {Null, Old, New }
    public class DVRPlayer
    {
        public SourceType _sourceType;
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

            //channel3_161103_100401_103401_20110300.264

            FileInfo[] stream1FileInfo = null;
            FileInfo[] stream2FileInfo = null;
            FileInfo[] stream3FileInfo = null;
            FileInfo[] stream4FileInfo = null;

            if (dir.GetFiles(@"*-*-*-*-*.mkv").Length > 0 & dir.GetFiles(@"*_*_*_*_*.mkv").Length == 0)
            {
                 stream1FileInfo = dir.GetFiles(@"*-01-*-*-*.mkv");
                 stream2FileInfo = dir.GetFiles(@"*-02-*-*-*.mkv");
                 stream3FileInfo = dir.GetFiles(@"*-03-*-*-*.mkv");
                 stream4FileInfo = dir.GetFiles(@"*-04-*-*-*.mkv");
                _sourceType = SourceType.New;
            }
            else if (dir.GetFiles(@"*_*_*_*_*.mkv").Length > 0 & dir.GetFiles(@"*-*-*-*-*.mkv").Length == 0)
            {
                 stream1FileInfo = dir.GetFiles(@"channel1_*_*_*_*.mkv");
                 stream2FileInfo = dir.GetFiles(@"channel2_*_*_*_*.mkv");
                 stream3FileInfo = dir.GetFiles(@"channel3_*_*_*_*.mkv");
                 stream4FileInfo = dir.GetFiles(@"channel4_*_*_*_*.mkv");
                _sourceType = SourceType.Old;
            }

            int maxCount = 0;
            try
            {
                maxCount = Math.Max(Math.Max(stream1FileInfo.Length, stream2FileInfo.Length), Math.Max(stream3FileInfo.Length, stream4FileInfo.Length));
            }
            catch { }

            for (int i = 0; i < maxCount; i++)
            {
                MediaSourceCollection.Add(new MediaSource
                {
                    Stream1 = i < stream1FileInfo.Length ? stream1FileInfo[i].FullName : null,
                    Stream2 = i < stream2FileInfo.Length ? stream2FileInfo[i].FullName : null,
                    Stream3 = i < stream3FileInfo.Length ? stream3FileInfo[i].FullName : null,
                    Stream4 = i < stream4FileInfo.Length ? stream4FileInfo[i].FullName : null
                    //TODO: перед добавлением в сримы сверять время файлов, отличия 2 секунды.
                    //BUG: Если один файл не переконвертировался, то после загрузки медиа стрима на этом плеере остается старый видео файл
                    //FIX: Очищаем колекцию перед заполнением
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
                    SetStartAndFinishDT(value);
                }
            }
        }

        public string Stream2 { set; get; }
        public string Stream3 { set; get; }
        public string Stream4 { set; get; }

        public DateTime StartDT { private set; get; }
        public DateTime FinishDT { private set; get; }

        private void SetStartAndFinishDT(string value)
        {  
            FileInfo fi = new FileInfo(value);
            string fName = fi.Name;            // "201-01-194924-200424-00p000.h264.mkv"
            string pName = fi.Directory.Name;  // "2017-04-26"

            if (DVRPlayer.Instance._sourceType == SourceType.New)
            {
                string temp = pName + "_" + fName.Substring(7, 6);
                StartDT = DateTime.ParseExact(temp, "yyyy-MM-dd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);

                string temp2 = pName + "_" + fName.Substring(14, 6);
                FinishDT = DateTime.ParseExact(temp2, "yyyy-MM-dd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (DVRPlayer.Instance._sourceType == SourceType.Old)
            {
                string temp = pName + "_" + fName.Substring(16, 6);
                StartDT = DateTime.ParseExact(temp, "yyyy-MM-dd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);

                string temp2 = pName + "_" + fName.Substring(23, 6);
                FinishDT = DateTime.ParseExact(temp2, "yyyy-MM-dd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
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
