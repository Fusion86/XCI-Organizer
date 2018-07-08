using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace XCI_Organizer.WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            UpdateNSWDB();
        }

        #region Game List

        public string GamesPath { get; set; }

        public bool TrySetGamesPath(string path)
        {
            if (!Directory.Exists(path)) return false;
            GamesPath = path;
            return true;
        }

        #endregion

        #region NSWDB

        public NSWDB.NSWDB NSWDB = new NSWDB.NSWDB();

        public string NSWDBStatus
        {
            get
            {
                if (NSWDB.Releases.Count > 0)
                    return $"Loaded {NSWDB.Releases.Count} releases on " + DateTime.Now.ToLongTimeString();
                else
                    return "No information loaded";
            }
        }

        public string NSWDBStatusText => "NSWDB - " + NSWDBStatus;

        public async Task UpdateNSWDB()
        {
            await NSWDB.LoadRemote();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NSWDBStatusText)));
        }

        #endregion
    }
}
