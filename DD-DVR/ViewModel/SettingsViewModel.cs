using DD_DVR.Data;
using MVVMLib;

namespace DD_DVR.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            var config = ConfigurationRepository.LoadObjFromFile();
            _videoLibPath = config.OutputVodeoDir;
            _licKey = config.Key;
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

        private string _licKey;
        public string LicKey
        {
            get => _licKey;
            set
            {
                _licKey = value;
                var obj = ConfigurationRepository.LoadObjFromFile();
                obj.Key = value;
                ConfigurationRepository.SaveObjToFile(obj);
                OnPropertyChanged();
            }
        }
    }
}
//TODO: Разработать viewModel для окна настроек