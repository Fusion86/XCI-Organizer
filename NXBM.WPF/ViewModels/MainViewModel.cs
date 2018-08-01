using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace NXBM.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public object SelectedGame { get; set; }

        public Control SidePanel { get; set; }

        public MainViewModel()
        {

        }
    }
}