using System.Windows.Input;

namespace XCI_Organizer.WPF
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand SelectGamesFolder = new RoutedUICommand
            (
                "Select Games Folder",
                "Select Games Folder",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.O, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand ReloadGamesList = new RoutedUICommand
            (
                "Reload Games List",
                "Reload Games List",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.R, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand UpdateNSWDB = new RoutedUICommand
            (
                "Update NSWDB",
                "Update NSWDB",
                typeof(CustomCommands),
                new InputGestureCollection() { }
            );

        public static readonly RoutedUICommand BrowseNSWDB = new RoutedUICommand
            (
                "Browse NSWDB",
                "Browse NSWDB",
                typeof(CustomCommands),
                new InputGestureCollection() { }
            );
    }
}
