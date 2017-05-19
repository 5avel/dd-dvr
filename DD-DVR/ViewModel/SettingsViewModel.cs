using MVVMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD_DVR.Data;

namespace DD_DVR.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            _videoLibPath = ConfigurationRepository.Instanse.OutputVodeoDir;
        }

        private string _videoLibPath;
        public string VideoLibPath
        {
            get =>  _videoLibPath;
            set
            {
                _videoLibPath = value;
                ConfigurationRepository.Instanse.OutputVodeoDir = value;
                ConfigurationRepository.Instanse.Save();
                OnPropertyChanged();
            }
        }
    }
}
//TODO: Разработать viewModel для окна настроек