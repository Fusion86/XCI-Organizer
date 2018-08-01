namespace Primrose.Interfaces
{
    /// <summary>
    /// FileSystem object
    /// </summary>
    public interface IFSObject
    {
        string Path { get; }
        string Name { get; }
        long Offset { get; }
        long Size { get; }
    }
}
