using System;
using System.Timers;
using System.Windows.Input;
using MVVMLib;
using System.Diagnostics;
using DD_DVR.Video;
using System.Windows.Media;
using System.Windows;

namespace DD_DVR.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        
        PerformanceCounter cpucounter;
        PerformanceCounter memcounter;

        public MainViewModel()
        {
            
            //cpucounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //memcounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");

            //timer = new Timer(500);
            //timer.Elapsed += Callback;
            //timer.Start();

        }

        private void Callback(object sender, ElapsedEventArgs e)
        {
            UsedMemory = GC.GetTotalMemory(true) / 1024;
            UsedCpu = cpucounter.NextValue();
        }

        private long _usedMemory;
        public long UsedMemory
        {
            get { return _usedMemory; }
            set
            {
                _usedMemory = value;
                OnPropertyChanged();
            }
        }

        private float _usedCpu;
        public float UsedCpu
        {
            get { return _usedCpu; }
            set
            {
                _usedCpu = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _test;
        public ICommand Test
        {
            get
            {
                return _test ?? (_test = new RelayCommand(param =>
                {
                    var videoVM = VideoViewModel.GetInstance();
                    if (videoVM == null && param == null) return;
                    if ("W" == param.ToString())
                    {
                        videoVM.DvrPlayCommand.Execute(null);
                    }
                    else if("S" == param.ToString())
                    {
                        videoVM.DvrPauseCommand.Execute(null);
                    }
                    else if ("A" == param.ToString())
                    {
                        videoVM.DvrSpeedDownCommand.Execute(null);
                    }
                    else if ("D" == param.ToString())
                    {
                        videoVM.DvrSpeedUpCommand.Execute(null);
                    }
                    else if ("E" == param.ToString())
                    {
                        videoVM.DvrRightStepCommand.Execute(null);
                    }
                    else if ("Q" == param.ToString())
                    {
                        videoVM.DvrLeftStepCommand.Execute(null);
                    }
                    else if ("Space" == param.ToString())
                    {
                        MessageBox.Show("Space");
                    }

                }));
            }
        }

       

    }
}
