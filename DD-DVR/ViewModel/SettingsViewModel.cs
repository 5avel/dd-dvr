using DD_DVR.Data;
using MVVMLib;

namespace DD_DVR.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            _videoLibPath = ConfigurationRepository.LoadObjFromFile().OutputVodeoDir;
        }

        private string _videoLibPath;
        public string VideoLibPath
        {
            get =>  _videoLibPath;
            set
            {
                _videoLibPath = value;
                var obj = ConfigurationRepository.LoadObjFromFile();
                obj.OutputVodeoDir = value;
                ConfigurationRepository.SaveObjToFile(obj);
                OnPropertyChanged();
            }
        }
    }
}
//TODO: Разработать viewModel для окна настроек