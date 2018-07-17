using XCI_Organizer.Models;

namespace XCI_Organizer.Interfaces
{
    public interface IHFS0Object
    {
        XCI XCI { get; }
        string Name { get; }
        long Offset { get; }
        long Size { get; }
    }
}
