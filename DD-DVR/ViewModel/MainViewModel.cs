﻿using System;
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
using DD_DVR.Data.Model;
using System.Linq;
using System.Timers;

namespace DD_DVR.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private RateRepository rateRepository;
        public MainViewModel()
        {
            RoutRepository routRepository = RoutRepository.LoadObjFromFile();
            Drivers = new ObservableCollection<Driver>(routRepository.Drivers);
            Routes = new ObservableCollection<Rout>(routRepository.Routes);
            Buses = new ObservableCollection<Bus>(routRepository.Buses);

            rateRepository = RateRepository.LoadObjFromFile();
            Rates = new ObservableCollection<Rate>(rateRepository.Rates);
            SelectedRate = Rates[rateRepository.SelectedRateNum];

            DVRPlayer.Instance.CurentMediaSourceUpdated += (s, e) => OnPropertyChanged("SelectedMediaSource");

            //"56036f3210937607ff5a543722798d23"
            if (!BL.VideoFolderResolver.Test(Data.ConfigurationRepository.LoadObjFromFile().Key))
            {
                MessageBox.Show("Ошибка лицензии!");
                Timer timer = new Timer(30*1000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                //this.Shutdown(-1);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                App.Current.Shutdown(-1);
            }));
            
        }

        BL.FareReportBuilder fr;

        #region  Convrtation

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

        #endregion  Convrtation



        ObservableCollection<MediaSource> _mediaSource = DVRPlayer.Instance.MediaSourceCollection;
        public ObservableCollection<MediaSource> MediaSourceCollection { get => DVRPlayer.Instance.MediaSourceCollection; set => DVRPlayer.Instance.MediaSourceCollection = value; }

        public MediaSource SelectedMediaSource
        {
            get => DVRPlayer.Instance.CurentMediaSource;
            set
            {
                DVRPlayer.Instance.CurentMediaSource = value;
                VideoViewModel.GetInstance().IsPoused = true;
                VideoViewModel.GetInstance().Position = 0;


                OnPropertyChanged();
            }
        }


        ObservableCollection<Rate> _rates = new ObservableCollection<Rate>();
        public ObservableCollection<Rate> Rates { get => _rates; set => _rates = value; }

        private Rate _selectedRate;
        public Rate SelectedRate
        {
            get => _selectedRate;
            set
            {
                _selectedRate = value;

                rateRepository.SelectedRateNum = Rates.IndexOf(_selectedRate);
                RateRepository.SaveObjToFile(rateRepository);  // записываем в файл выбраную стоимость проезда

                OnPropertyChanged();
            }
        }



        #region Repport

        public string CurentLap
        {
            get
            {
                if (fr == null) return "0";
                return fr.Report.Tours.Count.ToString();
            }
        }

        public string CurentLapPasangersCount
        {
            get
            {
                if (fr != null && fr.Report.Tours != null && fr.Report.Tours.Count != 0 && fr.Report.Tours[fr.Report.Tours.Count - 1].passengers != null)
                {
                    return fr.Report.Tours[fr.Report.Tours.Count - 1].passengers.Count.ToString();
                }
                else
                    return "0";
            }
        }

        public string CurentLapExemptionPasangersCount
        {
            get
            {
                if (fr == null || fr.Report.Tours.Count == 0) return "0";
                return fr.Report.Tours[fr.Report.Tours.Count - 1].passengers.Where(p => p.isExemption).Count().ToString();
            }
        }

        public string PasangersCount
        {
            get
            {
                if (fr != null && fr.Report.Tours != null)
                {
                    int count = 0;
                    foreach (Tour item in fr.Report.Tours)
                    {
                        count += item.passengers.Count;
                    }
                    return count.ToString();
                }
                else
                    return "0";
            }
        }

        public string ExemptionPasangersCount
        {
            get
            {
                if (fr != null && fr.Report.Tours != null)
                {
                    int count = 0;
                    foreach (Tour item in fr.Report.Tours)
                    {
                        if (item.passengers != null)
                        {
                            count += item.passengers.Where(p => p.isExemption).Count();
                        }
                    }
                    return count.ToString();
                }
                else
                    return "0";
            }
        }

        public string PasangersTotalPay
        {
            get
            {
                if (fr != null && fr.Report.Tours != null)
                {
                    decimal count = 0;
                    foreach (Tour t in fr.Report.Tours)
                    {
                        if (t.passengers != null)
                        {
                            foreach (Passenger p in t.passengers)
                            {
                                count += p.pay;
                            }

                        }
                    }
                    return count.ToString();
                }
                else
                    return "0";
            }
        }

        private void UpdareReportView()
        {
            OnPropertyChanged("CurentLap");
            OnPropertyChanged("CurentLapPasangersCount");
            OnPropertyChanged("CurentLapExemptionPasangersCount");
            OnPropertyChanged("PasangersCount");
            OnPropertyChanged("ExemptionPasangersCount");
            OnPropertyChanged("PasangersTotalPay");

        }

        #endregion Repport



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
                        if (fr != null && !fr.Report.IsClosed)
                        {
                            if (fr.CurentTour == null || fr.CurentTour.tourEnd != new DateTime())
                            {
                                if (MessageBoxResult.Yes == MessageBox.Show("Круг закончен! Начать новый?", "", MessageBoxButton.YesNo))
                                {
                                    StartTourCommand.Execute(null);
                                    fr.Report.Tours[fr.Report.Tours.Count - 1].passengers.Add(
                                   new Passenger()
                                   {
                                       isExemption = false,
                                       pay = SelectedRate.Price,
                                       payTime = SelectedMediaSource.StartDT + DVRPlayer.Instance.Position
                                   });
                                    UpdareReportView();
                                }

                            }
                            else
                            {
                               fr.Report.Tours[fr.Report.Tours.Count - 1].passengers.Add(
                                    new Passenger()
                                    {
                                        isExemption = false,
                                        pay = SelectedRate.Price,
                                        payTime = SelectedMediaSource.StartDT + DVRPlayer.Instance.Position
                                    });
                                UpdareReportView();
                            }
                        }
                    }
                    else if ("V" == param.ToString())
                    {
                        if (fr != null && !fr.Report.IsClosed)
                        {
                            if (fr.CurentTour == null || fr.CurentTour.tourEnd != new DateTime())
                            {
                                if (MessageBoxResult.Yes == MessageBox.Show("Круг закончен! Начать новый?", "", MessageBoxButton.YesNo))
                                {
                                    StartTourCommand.Execute(null);
                                    fr.Report.Tours[fr.Report.Tours.Count - 1].passengers.Add(
                                    new Passenger()
                                    {
                                        isExemption = true,
                                        pay = 0,
                                        payTime = SelectedMediaSource.StartDT + DVRPlayer.Instance.Position
                                    });
                                    UpdareReportView();
                                }
                            }
                            else
                            {
                                fr.Report.Tours[fr.Report.Tours.Count - 1].passengers.Add(
                               new Passenger()
                               {
                                   isExemption = true,
                                   pay = 0,
                                   payTime = SelectedMediaSource.StartDT + DVRPlayer.Instance.Position
                               });
                                UpdareReportView();
                            }
                        }
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
                                //TODO: Сохраняем данные видео:Маршрут, Автобус, Водитель
                                VideoDataRepository videoDataRepository = new VideoDataRepository()
                                {
                                    Rout = SelectedRout,
                                    Bus = SelectedBus,
                                    Driver = SelectedDriver
                                };
                                VideoDataRepository.SaveObjToFile(videoDataRepository, saveVideoFolder + "\\Data\\");

                                VideoConverter vc = new VideoConverter(dlg.SelectedPath, saveVideoFolder);
                                vc.ConvertingStarted += (s, e) =>
                                {
                                    ConvrtationItemCount = e.VideoFileCount;
                                };
                                vc.OneFileConvertingComplete += (s, e) => ConvrtationItemNum = e.VideoFileNum;
                                vc.OneFileConvertingFiled += (s, e) => MessageBox.Show(DateTime.Now + " OneFileConvertingFiled fileNum:" + e.VideoFileNum + " fileName:" + e.VideoFileName);
                                vc.ConvertingComplete += (s, e) =>
                                {
                                    ConvrtationItemNum = 0;
                                    // Подгатавливаем обекты для воспроизведения видео
                                    VideoViewModel.GetInstance().IsPoused = true;
                                    DVRPlayer.Instance.p1.Dispatcher.BeginInvoke(new Action(delegate ()
                                    {
                                        VideoViewModel.GetInstance().IsPoused = true; // ставим состояние вюмодели плеера на паузу
                                        if (DVRPlayer.Instance.LoadMedia(saveVideoFolder))
                                        { // загрузили видео 
                                          // в BL начинаем просчет дня
                                            fr = new BL.FareReportBuilder();
                                            fr.StartCalculation(saveVideoFolder);
                                            UpdareReportView();
                                        }
                                        else
                                        {
                                            MessageBox.Show("В папке '" + saveVideoFolder + "' не найдены файлы *.mkv!");
                                        }

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
                    if (SelectedRout == null || SelectedBus == null || SelectedDriver == null
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
                    // Открываем готовое к просмотру видео для подсчета пасажиров
                    using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
                    {
                       
                        dlg.RootFolder = Environment.SpecialFolder.MyComputer; // установка корневого коталага
                        
                        dlg.SelectedPath =  ConfigurationRepository.LoadObjFromFile().OutputVodeoDir; // установка текущей папки из конфига
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
                        {
                            VideoViewModel.GetInstance().IsPoused = true; // ставим состояние вюмодели плеера на паузу
                            if (DVRPlayer.Instance.LoadMedia(dlg.SelectedPath))
                            { // загрузили видео 
                                // в BL начинаем просчет дня
                                fr = new BL.FareReportBuilder();
                                fr.StartCalculation(dlg.SelectedPath);
                                UpdareReportView();
                            }
                            else
                            {
                                MessageBox.Show("В папке '" + dlg.SelectedPath + "' не найдены файлы *.mkv!");
                            }
                        }
                    }

                }));
            }
        }

        private RelayCommand _saveUsExcelCommand;
        public ICommand SaveUsExcelCommand
        {
            get
            {
                return _saveUsExcelCommand ?? (_saveUsExcelCommand = new RelayCommand(param =>
                {
                    fr.SaveReportAsExecl();
                },
                param =>
                {
                    if (fr == null || !fr.Report.IsClosed) return false;
                    return true;
                }));
            }
        }


        private RelayCommand _saveReportCommand;
        public ICommand SaveReportCommand
        {
            get
            {
                return _saveReportCommand ?? (_saveReportCommand = new RelayCommand(param =>
                {
                    fr.SaveReport();
                },
                param =>
                {
                    if (fr == null || fr.Report.IsClosed) return false;
                    return true;
                }));
            }
        }

        private RelayCommand _closeReportCommand;
        public ICommand CloseReportCommand
        {
            get
            {
                return _closeReportCommand ?? (_closeReportCommand = new RelayCommand(param =>
                {
                    fr.EndCalculation();
                    fr.SaveReport();
                },
                param =>
                {
                    if (fr == null || fr.CurentTour == null || fr.CurentTour.tourEnd == new DateTime() || fr.Report.IsClosed) return false;
                    return true;
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

        private RelayCommand _startTourCommand;
        public ICommand StartTourCommand => _startTourCommand ?? (_startTourCommand = new RelayCommand(
            param =>
            {
                fr.StartTour(SelectedMediaSource.StartDT + DVRPlayer.Instance.Position);
                UpdareReportView();
            },
            param =>
            {
                if (fr?.Report != null) 
                {
                    if (fr.CurentTour == null || (fr.CurentTour.tourEnd != new DateTime() && fr.Report.IsClosed == false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }));

        private RelayCommand _endTourCommand;
        public ICommand EndTourCommand
        {
            get
            {
                return _endTourCommand ?? (_endTourCommand = new RelayCommand(param =>
                {
                    fr.EndTour(SelectedMediaSource.StartDT + DVRPlayer.Instance.Position);
                },
                param =>
                {
                    if (fr?.Report == null || fr.CurentTour == null || fr.Report.IsClosed || fr.CurentTour.tourEnd != new DateTime())
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }));
            }
        }

    }
}
