using System.ComponentModel;
using System.IO;

namespace XCI_Organizer.WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Game List

        public string GamesPath { get; set; }

        public bool TrySetGamesPath(string path)
        {
            if (!Directory.Exists(path)) return false;
            GamesPath = path;
            return true;
        }

        #endregion
    }
}
