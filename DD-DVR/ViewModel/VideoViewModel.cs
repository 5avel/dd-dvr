using DD_DVR.Video;
using MVVMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace DD_DVR.ViewModel
{
    class VideoViewModel : ViewModelBase
    {

        private DVRPlayer dvr = new DVRPlayer(@"D:\TEST\128");
        public DrawingBrush VideoBrushCam1 { get; set; }
        public DrawingBrush VideoBrushCam2 { get; set; }
        public DrawingBrush VideoBrushCam3 { get; set; }
        public DrawingBrush VideoBrushCam4 { get; set; }

        private List<DrawingBrush> ListVideoBrushs;
   

        private System.Timers.Timer timer;

        private int _position;
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                //if (Math.Abs(_position - value) <= 20000) return;
                _position = value;
                foreach (Stream s in dvr.Streams) s.player.Position = new TimeSpan(0, 0, 0, 0, _position); //  _position;
                OnPropertyChanged();
            }
        } // позиия воспроизведения видео, Tiks от начала файла

        private int _naturalDuration;
        public int NaturalDuration
        {
            get { return _naturalDuration; }
            set
            {
                if (_naturalDuration == value) return;
                _naturalDuration = value;
                OnPropertyChanged();
            }
        } // позиия воспроизведения видео, милисекунды от начала файла


        public VideoViewModel()
        {
            VideoBrushCam1 = dvr.Streams[0]?.VideoBrush;
            VideoBrushCam2 = dvr.Streams[1]?.VideoBrush;
            VideoBrushCam3 = dvr.Streams[2]?.VideoBrush;
            VideoBrushCam4 = dvr.Streams[3]?.VideoBrush;


            timer = new System.Timers.Timer(100);
            timer.Elapsed += Callback;

        }

        private void Callback(object sender, ElapsedEventArgs e)
        {
            dvr.Streams[0].player.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Position = (int)dvr.Streams[0].player.Position.TotalMilliseconds;
                if (dvr.Streams[0].player.NaturalDuration.HasTimeSpan)
                {
                    NaturalDuration = (int)dvr.Streams[0].player.NaturalDuration.TimeSpan.TotalMilliseconds;
                }
            }));
        }

        private RelayCommand _dvrPlayCommand;
        public ICommand DvrPlayCommand
        {
            get
            {
                return _dvrPlayCommand ?? (_dvrPlayCommand = new RelayCommand(
                    param =>
                    {
                        dvr.Play();
                        timer.Start();
                    }));
            }
        }

        private RelayCommand _dvrStopCommand;
        public ICommand DvrStopCommand
        {
            get
            {
                return _dvrStopCommand ?? (_dvrStopCommand = new RelayCommand(
                    param => 
                    {
                        dvr.Stop();
                        timer.Stop();
                    }));
            }
        }

        private RelayCommand _dvrPauseCommand;
        public ICommand DvrPauseCommand
        {
            get
            {
                return _dvrPauseCommand ?? (_dvrPauseCommand = new RelayCommand(
                    param =>
                    {
                        dvr.Pause();
                        timer.Stop();
                    }));
            }
        }

        private RelayCommand _dvrSpeedUpCommand;
        public ICommand DvrSpeedUpCommand
        {
            get
            {
                return _dvrSpeedUpCommand ?? (_dvrSpeedUpCommand = new RelayCommand(param => dvr.SpeedUp()));
            }
        }

        private RelayCommand _dvrSpeedDownCommand;
        public ICommand DvrSpeedDownCommand
        {
            get
            {
                return _dvrSpeedDownCommand ?? (_dvrSpeedDownCommand = new RelayCommand(param => dvr.SpeedDown()));
            }
        }
    }
}
