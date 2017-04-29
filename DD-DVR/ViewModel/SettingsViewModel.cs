using MVVMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.ViewModel
{
    

    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            VideoLibPath = "test";
        }

        private string _videoLibPath;
        public string VideoLibPath
        {
            get =>  _videoLibPath;
            set
            {
                _videoLibPath = value;
                OnPropertyChanged();
            }
        }
    }
}
//TODO: Разработать viewModel для окна настроек