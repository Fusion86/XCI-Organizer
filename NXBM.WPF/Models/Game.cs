using GalaSoft.MvvmLight;
using Primrose.Models;

namespace NXBM.WPF.Models
{
    public class Game : ObservableObject
    {
        public XCI XCI { get; }

        public string TitleId { get; set; }
        public string GameTitle { get; set; }
        public string ROMSize { get; set; }
        public string ROMUsedSpace { get; set; }
        public string CartSize { get; set; }
        public string Languages { get; set; }
        public int CartType { get; set; }
        public string Filename => XCI.Path;
        public string Developer { get; set; }
        public string GameRevision { get; set; }
        public string MasterkeyRevision { get; set; }

        public bool IsTrimmed { get; set; }

        public Game(XCI xci)
        {
            XCI = xci;
        }
    }
}
