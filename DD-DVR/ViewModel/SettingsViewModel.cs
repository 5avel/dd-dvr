using DD_DVR.Data;
using MVVMLib;

namespace DD_DVR.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            _videoLibPath = new Repository().LoadObjFromFile<ConfigurationRepository>().OutputVodeoDir;
        }

        private string _videoLibPath;
        public string VideoLibPath
        {
            get =>  _videoLibPath;
            set
            {
                _videoLibPath = value;
                 new Repository().LoadObjFromFile<ConfigurationRepository>().OutputVodeoDir = value;
                 new Repository().SaveObjToFile<ConfigurationRepository>();
                OnPropertyChanged();
            }
        }
    }
}
//TODO: Разработать viewModel для окна настроек