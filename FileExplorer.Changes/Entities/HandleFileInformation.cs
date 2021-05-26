using System.Runtime.InteropServices.ComTypes;

namespace FileExplorer.Changes.Entities
{
    public struct HandleFileInformation
    {
        public uint FileAttributes { get; set; }
        public FILETIME CreationTime { get; set; }
        public FILETIME LastAccessTime { get; set; }
        public FILETIME LastWriteTime { get; set; }
        public uint VolumeSerialNumber { get; set; }
        public uint FileSizeHigh { get; set; }
        public uint FileSizeLow { get; set; }
        public uint NumberOfLinks { get; set; }
        public uint FileIndexHigh { get; set; }
        public uint FileIndexLow { get; set; }
    }
}