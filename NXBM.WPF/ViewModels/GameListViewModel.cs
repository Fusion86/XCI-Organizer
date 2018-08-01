using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.IO;

namespace NXBM.WPF.ViewModels
{
    public class GameListViewModel : ViewModelBase
    {
        public ObservableCollection<string> Sources { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Games { get; set; } = new ObservableCollection<string>();

        public RelayCommand AddSourceCommand { get; }
        public RelayCommand<string> RemoveSourceCommand { get; }

        public GameListViewModel()
        {
            AddSourceCommand = new RelayCommand(AddSource, CanAddSource);
            RemoveSourceCommand = new RelayCommand<string>(RemoveSource, CanRemoveSource);
        }

        private void AddSource()
        {
            string path = @"L:\ROM\Nintendo Switch";

            // Don't add duplicates
            if (Sources.Contains(path))
                return;

            Sources.Add(path);

            // Add files inside directory to the games list
            foreach (string file in Directory.GetFiles(path))
            {
                Games.Add(file);
            }
        }

        private bool CanAddSource()
        {
            return true;
        }

        private void RemoveSource(string path)
        {
            Sources.Remove(path);

            // Remove files inside directory from the games list
            foreach (string file in Directory.GetFiles(path))
            {
                Games.Remove(file);
            }
        }

        private bool CanRemoveSource(string path)
        {
            return true;
        }
    }
}
