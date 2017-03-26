using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMLib
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged<T>(Expression<Func<T>> changedProperty)
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        string propName = ((MemberExpression)changedProperty.Body).Member.Name;
        //        handler.Invoke(this, new PropertyChangedEventArgs(propName));
        //    }
        //}

        /// <summary>
        /// Новый OnPropertyChanged которому вообще не нужно передавать ни свойство ни название свойства)))
        /// </summary>
        /// <param name="propertyName"> Заполняется автоматически названием вызываюшего члена!</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Тоже самое 
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));



    }
}
