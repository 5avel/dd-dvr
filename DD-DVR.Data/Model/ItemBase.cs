using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data.Model
{
    public class ItemBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Новый OnPropertyChanged которому вообще не нужно передавать ни свойство ни название свойства)))
        /// </summary>
        /// <param name="propertyName"> Заполняется автоматически названием вызываюшего члена!</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
