using FileExplorer.Changes.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Changes.Operations
{
    public static class FileExplorerOperations
    {
        public static IEnumerable<FileEntity> GetFilesForPath(string path)
        {
            var files = new List<FileEntity>();

            GetAllFilesInFolder(files, path);

            return files;
        }
        
        public static IEnumerable<FileEntity> GetChangedFilesForPath(string path)
        {
            return new List<FileEntity>();
        }

        private static void GetAllFilesInFolder(List<FileEntity> files, string path)
        {
            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(file);
                    var fileIndex = FileInformationOperations.GetUniqueFileIndex(file);

                    files.Add(new FileEntity()
                    {
                        Name = fileInfo.Name,
                        Path = fileInfo.FullName,
                        FileIndex = fileIndex,
                        ModifiedDateTime = fileInfo.LastWriteTime
                    });
                }
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                GetAllFolders(files, path);
            }
        }

        private static void GetAllFolders(List<FileEntity> files, string path)
        {
            try
            {
                foreach (var folder in Directory.GetDirectories(path))
                {
                    GetAllFilesInFolder(files, folder);
                }
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
