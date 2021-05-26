using FileExplorer.Changes.Entities;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FileExplorer.Changes.Operations
{
    public static class FileInformationOperations
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out HandleFileInformation lpFileInformation);

        public static ulong GetUniqueFileIndex(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            using (var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                GetFileInformationByHandle(fileStream.SafeFileHandle.DangerousGetHandle(), 
                    out HandleFileInformation objectFileInfo);
                
                var fileIndex = (objectFileInfo.FileIndexHigh << 32) + objectFileInfo.FileIndexLow;
                return fileIndex;
            }
        }
    }
}
