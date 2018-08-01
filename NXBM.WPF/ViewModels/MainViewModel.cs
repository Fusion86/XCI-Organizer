using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace NXBM.WPF.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<object> Games { get; set; }
        public object SelectedGame { get; set; }

        public MainViewModel()
        {

        }
    }
}