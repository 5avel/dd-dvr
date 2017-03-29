using DD_DVR.Video;
using MVVMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DD_DVR.ViewModel
{
    class VideoViewModel : ViewModelBase
    {

        private DVRPlayer dvr = new DVRPlayer(@"D:\TEST\128");
        public DrawingBrush VideoBrushCam1 { get; set; }
        public DrawingBrush VideoBrushCam2 { get; set; }
        public DrawingBrush VideoBrushCam3 { get; set; }
        public DrawingBrush VideoBrushCam4 { get; set; }

        public VideoViewModel()
        {
            VideoBrushCam1 = dvr.Streams[0].VideoBrush;
            VideoBrushCam2 = dvr.Streams[1].VideoBrush;
            VideoBrushCam3 = dvr.Streams[2].VideoBrush;
            VideoBrushCam4 = dvr.Streams[3].VideoBrush;
        }

        private RelayCommand _dvrPlayCommand;
        public ICommand DvrPlayCommand
        {
            get
            {
                return _dvrPlayCommand ?? (_dvrPlayCommand = new RelayCommand(param => dvr.Play()));
            }
        }

        private RelayCommand _dvrStopCommand;
        public ICommand DvrStopCommand
        {
            get
            {
                return _dvrStopCommand ?? (_dvrStopCommand = new RelayCommand(param => dvr.Stop()));
            }
        }

        private RelayCommand _dvrPauseCommand;
        public ICommand DvrPauseCommand
        {
            get
            {
                return _dvrPauseCommand ?? (_dvrPauseCommand = new RelayCommand(param => dvr.Pause()));
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
