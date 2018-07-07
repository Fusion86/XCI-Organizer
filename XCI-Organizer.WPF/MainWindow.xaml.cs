using Ookii.Dialogs.Wpf;
using System.Windows;
using System.Windows.Input;

namespace XCI_Organizer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Commands

        private void cmdSelectGamesFolder_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void cmdSelectGamesFolder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
            if (fbd.ShowDialog() == true)
            {
                if (vm.TrySetGamesPath(fbd.SelectedPath) == false)
                {
                    MessageBox.Show("Invalid path!", "Select Game Path");
                }
            }
        }

        private void cmdReloadGamesList_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = vm.GamesPath != null;

        private void cmdReloadGamesList_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        #endregion

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
    }
}
