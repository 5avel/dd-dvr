using DD_DVR.BL.Player;
using MVVMLib;
using System;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;

namespace DD_DVR.ViewModel
{
    class VideoViewModel : ViewModelBase
    {
        DVRPlayer dvr = DVRPlayer.Instance;
        public VideoViewModel()
        {
            instance = this;
            // событие видео закружено, можно привязывать кисти.
           
            VideoBrushCam1 = dvr.Cam1;
            VideoBrushCam2 = dvr.Cam2;
            VideoBrushCam3 = dvr.Cam3;
            VideoBrushCam4 = dvr.Cam4;

            timer = new System.Timers.Timer(300);
            timer.Elapsed += Callback;
            
            dvr.p1.MediaOpened += Player_MediaOpened;
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            var ndts = ((MediaPlayer)sender).NaturalDuration.TimeSpan;
            NaturalDurationS = Convert.ToDateTime(ndts.ToString()).ToLongTimeString();
            NaturalDuration = (int)ndts.TotalMilliseconds;
        }
       
        public DrawingBrush VideoBrushCam1 { get; set; }
        public DrawingBrush VideoBrushCam2 { get; set; }
        public DrawingBrush VideoBrushCam3 { get; set; }
        public DrawingBrush VideoBrushCam4 { get; set; }

        private bool isPoused = true;
        public bool IsPoused
        {
            set { isPoused = value; }
            get { return isPoused; }
        }

        private System.Timers.Timer timer;

        private int _position = 0;
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                _position = value;
                    
                    //BUG: падает при клацание кнопками, без открытого видео! (как вариант делать не активными пока не загружено видео)
                var newPosition = new TimeSpan(0, 0, 0, 0, _position);
                PositionS = Convert.ToDateTime(newPosition.ToString()).ToLongTimeString();
                dvr.Position = newPosition;
                //foreach (StreamOld s in dvr.Streams) s.player.Position = newPosition;

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
            dvr.p1.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Position = (int)dvr.p1.Position.TotalMilliseconds;
               
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
                        dvr.LeftStep();
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
                            if (dvr.p1.NaturalDuration.HasTimeSpan)
                            {
                                var ndts = dvr.p1.NaturalDuration.TimeSpan;
                               // NaturalDurationS = Convert.ToDateTime(ndts.ToString()).ToLongTimeString();
                               // NaturalDuration = (int)ndts.TotalMilliseconds;
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
                        dvr.RightStep();
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
                if (value >= 0.25 && value <= 128)
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
