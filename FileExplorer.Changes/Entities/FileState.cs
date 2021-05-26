using System;
using System.Collections.Generic;

namespace FileExplorer.Changes.Entities
{
    public struct FileState
    {
        public string FolderPath { get; set; }
        public DateTime SearchDateTime { get; set; }
        public List<FileEntity> AllFiles { get; set; }
        public List<FileEntity> NewFiles { get; set; }
        public List<FileEntity> ChangedFiles { get; set; }
        public bool IsEmpty { get; set; }

        public static FileState Empty 
        { 
            get 
            {
                return new FileState() { IsEmpty = true };
            } 
        }
    }
}
