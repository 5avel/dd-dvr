using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMLib
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
