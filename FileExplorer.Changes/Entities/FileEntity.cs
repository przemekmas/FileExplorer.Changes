using System;

namespace FileExplorer.Changes.Entities
{
    public class FileEntity : IEquatable<FileEntity>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public ulong FileIndex { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool Equals(FileEntity other)
        {
            return FileIndex == other.FileIndex;
        }

        public override int GetHashCode()
        {
            return FileIndex.GetHashCode();
        }
    }
}
