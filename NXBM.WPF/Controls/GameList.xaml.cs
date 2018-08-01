using System.Windows;
using System.Windows.Controls;

namespace NXBM.WPF.Controls
{
    /// <summary>
    /// Interaction logic for GameList.xaml
    /// </summary>
    public partial class GameList : UserControl
    {
        public GameList()
        {
            InitializeComponent();
        }
        
        public string SelectedGame
        {
            get { return (string)GetValue(SelectedGameProperty); }
            set { SetValue(SelectedGameProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedGameProperty =
            DependencyProperty.Register(nameof(SelectedGame), typeof(string), typeof(GameList), new PropertyMetadata(""));
    }
}
