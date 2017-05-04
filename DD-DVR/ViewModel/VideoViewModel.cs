using DD_DVR.BL.Playr;
using MVVMLib;
using System;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;

namespace DD_DVR.ViewModel
{
    class VideoViewModel : ViewModelBase
    {
        public VideoViewModel()
        {
            instance = this;
            // событие видео закружено, можно привязывать кисти.
            //VideoBrushCam1 = dvr.Streams[0]?.VideoBrush;
            //VideoBrushCam2 = dvr.Streams[1]?.VideoBrush;
            //VideoBrushCam3 = dvr.Streams[2]?.VideoBrush;
            //VideoBrushCam4 = dvr.Streams[3]?.VideoBrush;

            timer = new System.Timers.Timer(300);
            timer.Elapsed += Callback;

            //dvr.Streams[0].player.MediaOpened += Player_MediaOpened;
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            var ndts = ((MediaPlayer)sender).NaturalDuration.TimeSpan;
            NaturalDurationS = Convert.ToDateTime(ndts.ToString()).ToLongTimeString();
            NaturalDuration = (int)ndts.TotalMilliseconds;
        }

        public void LoadVideo(string pathToVideo)
        {

        }

        private DVRPlayer dvr = new DVRPlayer();
        public DrawingBrush VideoBrushCam1 { get; set; }
        public DrawingBrush VideoBrushCam2 { get; set; }
        public DrawingBrush VideoBrushCam3 { get; set; }
        public DrawingBrush VideoBrushCam4 { get; set; }

        private bool isPoused = true;

        private System.Timers.Timer timer;

        private int _position = 0;
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                _position = value;
                    
                var newPosition = new TimeSpan(0, 0, 0, 0, _position);
                PositionS = Convert.ToDateTime(newPosition.ToString()).ToLongTimeString();
                foreach (Stream s in dvr.Streams) s.player.Position = newPosition;
                
                OnPropertyChanged();
            }
        } // позиия воспроизведения видео, Tiks от начала файла

        private int _naturalDuration = 100;
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

        private string _positionS;
        public string PositionS
        {
            get { return _positionS; }
            set
            {
                if (_positionS == value) return;
                _positionS = value;
                OnPropertyChanged();
            }
        }

        private string _naturalDurationS;
        public string NaturalDurationS
        {
            get { return _naturalDurationS; }
            set
            {
                if (_naturalDurationS == value) return;
                _naturalDurationS = value;
                OnPropertyChanged();
            }
        }

        private void Callback(object sender, ElapsedEventArgs e)
        {
            dvr.Streams[0].player.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Position = (int)dvr.Streams[0].player.Position.TotalMilliseconds;
               
            }));
        }

        private RelayCommand _dvrSpeedDownLeftStepCommand;
        public ICommand DvrSpeedDownLeftStepCommand
        {
            get
            {
                return _dvrSpeedDownLeftStepCommand ?? (_dvrSpeedDownLeftStepCommand = new RelayCommand(param =>
                {
                    if (isPoused)
                    {
                        dvr.Step(false);
                    }
                    else
                    {
                        CurspeedRatio /= 2;
                    }
                }));
            }
        }



        private RelayCommand _dvrPlayPauseCommand;
        public ICommand DvrPlayPauseCommand
        {
            get
            {
                return _dvrPlayPauseCommand ?? (_dvrPlayPauseCommand = new RelayCommand(
                    param =>
                    {
                        if (isPoused)
                        {
                            if (dvr.Streams[0].player.NaturalDuration.HasTimeSpan)
                            {
                                var ndts = dvr.Streams[0].player.NaturalDuration.TimeSpan;
                                NaturalDurationS = Convert.ToDateTime(ndts.ToString()).ToLongTimeString();
                                NaturalDuration = (int)ndts.TotalMilliseconds;
                            }
                            dvr.Play();
                            timer.Start();
                            isPoused = false;
                        }
                        else
                        {
                            dvr.Pause();
                            timer.Stop();
                            CurspeedRatio = 1;
                            isPoused = true;
                        }
                    }));
            }
        }

        private RelayCommand _dvrSpeedUpRightStepCommand;
        public ICommand DvrSpeedUpRightStepCommand
        {
            get
            {
                return _dvrSpeedUpRightStepCommand ?? (_dvrSpeedUpRightStepCommand = new RelayCommand(param =>
                {
                    if (isPoused)
                    {
                        dvr.Step();
                    }
                    else
                    {
                        CurspeedRatio *= 2;
                    }
                }));
            }
        }



        #region SpeedRatio

        private double _curspeedRatio = 1;
        public double CurspeedRatio
        {
            get => _curspeedRatio;
            set
            {
                if (value >= 0.25 && value <= 64)
                {
                    _curspeedRatio = value;
                    dvr.SetSpeedRatio(_curspeedRatio);
                    OnPropertyChanged();
                }
            }
        }

     
        #endregion SpeedRatio
        #region implementation GetInstance
        private static VideoViewModel instance;

        public static VideoViewModel GetInstance()
        {
            return instance;
        }

        #endregion implementation GetInstance
    }
}
