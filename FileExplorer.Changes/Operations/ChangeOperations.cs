using FileExplorer.Changes.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileExplorer.Changes.Operations
{
    public static class ChangeOperations
    {
        public static bool TryLoadFileState(out FileState fileState)
        {
            fileState = FileState.Empty;

            var openFileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = Constants.FileFilter,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xmlSerialiser = new XmlSerializer(typeof(FileState));

                using (var fileReader = new StreamReader(openFileDialog.FileName))
                {
                    fileState = (FileState)xmlSerialiser.Deserialize(fileReader);
                }
            }

            return !fileState.IsEmpty;
        }

        public static void SaveFileState(FileState fileState)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = Constants.FileFilter,
                RestoreDirectory = true,
                FileName = $"File Explorer State {DateTime.Now:dd-MM-yyyy HH mm ss}"
            };

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xmlSerialiser = new XmlSerializer(typeof(FileState));

                using (var fileStream = File.Create(saveFileDialog.FileName))
                {
                    xmlSerialiser.Serialize(fileStream, fileState);
                }
            }
        }
    }
}
