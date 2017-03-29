using System;
using System.Timers;
using System.Windows.Input;
using MVVMLib;
using System.Diagnostics;
using DD_DVR.Video;
using System.Windows.Media;

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


       


    }
}
