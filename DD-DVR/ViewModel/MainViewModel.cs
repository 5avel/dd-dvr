using System;
using System.Windows.Input;
using MVVMLib;
using System.Windows;
using System.Collections.ObjectModel;
using DD_DVR.View;
using DD_DVR.Converter;
using DD_DVR.BL;
using NLog;
using DD_DVR.Data;
using DD_DVR.BL.Player;

namespace DD_DVR.ViewModel
{
    enum AppState { Waiting, Playing, Converting }
    class MainViewModel : ViewModelBase
    {
        public AppState AppState { set; get; }
        
        public MainViewModel()
        {
            AppState = AppState.Waiting;

            var routRepository = new RoutRepository();
            Drivers = new ObservableCollection<Driver>(routRepository.GetAllDrivers());
            Routes = new ObservableCollection<Rout>(routRepository.GetAllRoutes());
            Buses = new ObservableCollection<Bus>(routRepository.GetAllBuses());

            DVRPlayer.Instance.CurentMediaSourceUpdated += (s, e) => OnPropertyChanged("SelectedMediaSource");
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

        private bool _convrtationFlyoutIsOpen = false;
        public bool ConvrtationFlyoutIsOpen
        {
            get { return _convrtationFlyoutIsOpen; }
            set
            {

                _convrtationFlyoutIsOpen = value;
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


        ObservableCollection<Driver> _drivers = new ObservableCollection<Driver>();
        public ObservableCollection<Driver> Drivers { get => _drivers; set => _drivers = value; }

        private Driver _selectedDriver;
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Rout> _routes = new ObservableCollection<Rout>();
        public ObservableCollection<Rout> Routes { get => _routes; set => _routes = value; }

        private Rout _selectedRout;
        public Rout SelectedRout
        {
            get => _selectedRout;
            set
            {
                _selectedRout = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Bus> _bus = new ObservableCollection<Bus>();
        public ObservableCollection<Bus> Buses { get => _bus; set => _bus = value; }

        private Bus _selectedBus;
        public Bus SelectedBus
        {
            get => _selectedBus;
            set
            {
                _selectedBus = value;
                OnPropertyChanged();
            }
        }


        ObservableCollection<MediaSource> _mediaSource = DVRPlayer.Instance.MediaSourceCollection;
        public ObservableCollection<MediaSource> MediaSourceCollection { get => DVRPlayer.Instance.MediaSourceCollection; set => DVRPlayer.Instance.MediaSourceCollection = value; }

        private MediaSource _selectedMediaSource;
        public MediaSource SelectedMediaSource
        {
            get => DVRPlayer.Instance.CurentMediaSource;
            set
            {
                DVRPlayer.Instance.CurentMediaSource = value;
                OnPropertyChanged();
            }
        }

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
                            if (vfr.ResolveRawVideoFolder(dlg.SelectedPath, SelectedBus.Title, out saveVideoFolder, out streamCount, out videoFilesCount))
                            {
                                // если все ок, открываем флайаут с информацией о ковертации и предлагаем начать конвертацию
                                VideoConverter vc = new VideoConverter(dlg.SelectedPath, saveVideoFolder);
                                vc.ConvertingStarted += (s, e) =>
                                {
                                    ConvrtationItemCount = e.VideoFileCount;
                                    AppState = AppState.Converting;
                                };
                                vc.OneFileConvertingComplete += (s, e) => ConvrtationItemNum = e.VideoFileNum;
                                vc.OneFileConvertingFiled += (s, e) => MessageBox.Show(DateTime.Now + " OneFileConvertingFiled fileNum:" + e.VideoFileNum + " fileName:" + e.VideoFileName);
                                vc.ConvertingComplete += (s, e) =>
                                {
                                    ConvrtationItemNum = 0;
                                    // меняем состояние на плей
                                    AppState = AppState.Playing;
                                    // Подгатавливаем обекты для воспроизведения видео
                                    VideoViewModel.GetInstance().IsPoused = true;
                                    DVRPlayer.Instance.p1.Dispatcher.BeginInvoke(new Action(delegate ()
                                    {
                                        
                                        if (!DVRPlayer.Instance.LoadMedia(saveVideoFolder))
                                            MessageBox.Show("В папке '" + saveVideoFolder + "' не найдены файлы *.mkv!");

                                    }));
                                   

                                };
                                vc.StartConvertAsync();
                            }
                            else // не удалось получить данные 
                            {

                            }
                        }                    
                    }
                  
                },
                param => 
                {
                    if ( SelectedRout == null || SelectedBus == null || SelectedDriver == null
                    ) return false;
                    return true;
                }));
            }
        }

        private RelayCommand _loadVideoCommand;
        public ICommand LoadVideoCommand
        {
            get
            {
                return _loadVideoCommand ?? (_loadVideoCommand = new RelayCommand(param =>
                {
                    // меняем состояние на плей
                    // Подгатавливаем обекты для воспроизведения видео
                    using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        ConfigurationRepository cr = new ConfigurationRepository();
                        dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                        dlg.SelectedPath = cr.GetOutputVodeoDir();
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
                        {
                            VideoViewModel.GetInstance().IsPoused = true;
                            if(!DVRPlayer.Instance.LoadMedia(dlg.SelectedPath))
                                MessageBox.Show("В папке '"+dlg.SelectedPath+"' не найдены файлы *.mkv!");
                        }
                    }

                }));
            }
        }

        private RelayCommand _showConvrtationFlyoutCommand;
        public ICommand ShowConvrtationFlyoutCommand
        {
            get
            {
                return _showConvrtationFlyoutCommand ?? (_showConvrtationFlyoutCommand = new RelayCommand(param =>
                {
                    ConvrtationFlyoutIsOpen = !ConvrtationFlyoutIsOpen;

                }));
            }
        }

        //ConvrtationFlyoutIsOpen

    }
}
