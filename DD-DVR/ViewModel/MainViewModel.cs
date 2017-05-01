using System;
using System.Timers;
using System.Windows.Input;
using MVVMLib;
using System.Diagnostics;
using DD_DVR.Video;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;
using DD_DVR.Model;
using DD_DVR.View;

namespace DD_DVR.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        
        public MainViewModel()
        {
            Routs.Add(new Rout() { Title = "Орловщина" });
        }


        ObservableCollection<Rout> _routs = new ObservableCollection<Rout>();
        public ObservableCollection<Rout> Routs { get => _routs; set => _routs = value; }

        private RelayCommand _videoKeyBinding;
        public ICommand VideoKeyBinding
        {
            get
            {
                return _videoKeyBinding ?? (_videoKeyBinding = new RelayCommand(param =>
                {
                    var videoVM = VideoViewModel.GetInstance();
                    if (videoVM == null && param == null) return;
                    if ("W" == param.ToString())
                    {
                        videoVM.DvrPlayPauseCommand.Execute(null);
                    }
                    else if ("E" == param.ToString())
                    {
                        videoVM.DvrSpeedUpRightStepCommand.Execute(null);
                    }
                    else if ("Q" == param.ToString())
                    {
                        videoVM.DvrSpeedDownLeftStepCommand.Execute(null);
                    }
                    else if ("Space" == param.ToString())
                    {
                        MessageBox.Show("Space");
                    }

                }));
            }
        }

        private RelayCommand _settingsWindowOpenCommand;
        public ICommand SettingsWindowOpenCommand
        {
            get
            {
                return _settingsWindowOpenCommand ?? (_settingsWindowOpenCommand = new RelayCommand(param =>
                {
                    SettingsView sv = new SettingsView();
                    sv.ShowDialog();
                }));
            }
        }

        private RelayCommand _loadRawVideoCommand;
        public ICommand LoadRawVideoCommand
        {
            get
            {
                return _loadRawVideoCommand ?? (_loadRawVideoCommand = new RelayCommand(param =>
                {
                    // VM -> BL -> Converter
                }));
            }
        }


    }
}
