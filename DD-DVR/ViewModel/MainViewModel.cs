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
using DD_DVR.Converter;
using DD_DVR.BL;

namespace DD_DVR.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        
        public MainViewModel()
        {
            Routs.Add(new Rout() { Title = "Орловщина" });
        }

        private int _convrtationItemCount = 5;
        public int ConvrtationItemCount
        {
            get { return _convrtationItemCount; }
            set
            {
                if (_convrtationItemCount == value) return;
                _convrtationItemCount = value;
                OnPropertyChanged();
            }
        }

        private int _convrtationItemNum = 0;
        public int ConvrtationItemNum
        {
            get { return _convrtationItemNum; }
            set
            {
                if (_convrtationItemNum == value) return;
                _convrtationItemNum = value;
                OnPropertyChanged();
            }
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
                    // открываем папку с rawVideo
                    using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                       
                        
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
                        {
                            var vfr = new VideoFolderResolver();
                            string saveVideoFolder;
                            int streamCount;
                            int videoFilesCount;
                            if (vfr.ResolveRawVideoFolder(dlg.SelectedPath, out saveVideoFolder, out streamCount, out videoFilesCount))
                            {
                                // если все ок, открываем флайаут с информацией о ковертации и предлагаем начать конвертацию
                                VideoConverter vc = new VideoConverter(dlg.SelectedPath, saveVideoFolder);
                                vc.ConvertingStarted += (s, e) => ConvrtationItemCount = e.VideoFileCount;
                                vc.OneFileConvertingComplete += (s, e) => ConvrtationItemNum = e.VideoFileNum;
                                vc.OneFileConvertingFiled += (s, e) => MessageBox.Show(DateTime.Now + " OneFileConvertingFiled fileNum:" + e.VideoFileNum + " fileName:" + e.VideoFileName);
                                vc.ConvertingComplete += (s, e) => ConvrtationItemCount = 0;
                                vc.StartConvertAsync();
                            }
                        }
                        
                    }
                  
                }));
            }
        }

       
    }
}
