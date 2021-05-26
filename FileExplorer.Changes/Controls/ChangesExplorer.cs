using FileExplorer.Changes.Entities;
using FileExplorer.Changes.Events;
using FileExplorer.Changes.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileExplorer.Changes.Controls
{
    [TemplatePart(Name = "PART_AllFilesTabItem", Type = typeof(TabItem))]
    [TemplatePart(Name = "PART_NewFilesTabItem", Type = typeof(TabItem))]
    [TemplatePart(Name = "PART_ChangedFilesTabItem", Type = typeof(TabItem))]
    [TemplatePart(Name = "PART_FindButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_FindChangesButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_SaveStateButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_LoadStateButton", Type = typeof(Button))]
    public class ChangesExplorer : Control
    {        
        private DateTime _searchDateTime;
        private TabItem _allFilesTabItem;
        private TabItem _newFilesTabItem;
        private TabItem _changedFilesTabItem;
        private Button _findButton;
        private Button _findChangesButton;
        private Button _saveStateButton;
        private Button _loadStateButton;

        public event SearchStarted SearchStarted;
        public event SearchEnded SearchEnded;

        public static readonly DependencyProperty AllFilesProperty = DependencyProperty.Register(
            nameof(AllFiles), typeof(IEnumerable<FileEntity>), typeof(ChangesExplorer), new PropertyMetadata(null));

        public static readonly DependencyProperty NewFilesProperty = DependencyProperty.Register(
            nameof(NewFiles), typeof(IEnumerable<FileEntity>), typeof(ChangesExplorer), new PropertyMetadata(null));

        public static readonly DependencyProperty ChangedFilesProperty = DependencyProperty.Register(
            nameof(ChangedFiles), typeof(IEnumerable<FileEntity>), typeof(ChangesExplorer), new PropertyMetadata(null));

        public static readonly DependencyProperty FolderPathProperty = DependencyProperty.Register(
            nameof(FolderPath), typeof(string), typeof(ChangesExplorer), new PropertyMetadata(null));

        public IEnumerable<FileEntity> AllFiles
        {
            get { return (IEnumerable<FileEntity>)GetValue(AllFilesProperty); }
            set 
            { 
                _allFilesTabItem.Header = $"All Files ({value?.Count()})"; 
                SetValue(AllFilesProperty, value); 
            }
        }

        public IEnumerable<FileEntity> NewFiles
        {
            get { return (IEnumerable<FileEntity>)GetValue(NewFilesProperty); }
            set 
            {
                _newFilesTabItem.Header = $"New Files ({value?.Count()})";
                SetValue(NewFilesProperty, value); 
            }
        }

        public IEnumerable<FileEntity> ChangedFiles
        {
            get { return (IEnumerable<FileEntity>)GetValue(ChangedFilesProperty); }
            set 
            {
                _changedFilesTabItem.Header = $"Changed Files ({value?.Count()})";
                SetValue(ChangedFilesProperty, value); 
            }
        }

        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set { SetValue(FolderPathProperty, value); }
        }

        static ChangesExplorer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangesExplorer), new FrameworkPropertyMetadata(typeof(ChangesExplorer)));
        }

        public override void OnApplyTemplate()
        {
            _allFilesTabItem = (TabItem)Template.FindName("PART_AllFilesTabItem", this);
            _newFilesTabItem = (TabItem)Template.FindName("PART_NewFilesTabItem", this);
            _changedFilesTabItem = (TabItem)Template.FindName("PART_ChangedFilesTabItem", this);

            _findButton = (Button)Template.FindName("PART_FindButton", this);
            _findChangesButton = (Button)Template.FindName("PART_FindChangesButton", this);
            _saveStateButton = (Button)Template.FindName("PART_SaveStateButton", this);
            _loadStateButton = (Button)Template.FindName("PART_LoadStateButton", this);


            _findButton.Click += OnFindButtonClick;
            _findChangesButton.Click += OnFindChangesButtonClick;
            _saveStateButton.Click += OnSaveStateButtonClick;
            _loadStateButton.Click += OnLoadStateButtonClick;

            Initialise();
        }

        private void Initialise()
        {
            AllFiles = new List<FileEntity>();
            NewFiles = new List<FileEntity>();
            ChangedFiles = new List<FileEntity>();
        }

        private void OnLoadStateButtonClick(object sender, RoutedEventArgs e)
        {
            if (ChangeOperations.TryLoadFileState(out FileState fileState))
            {
                _searchDateTime = fileState.SearchDateTime;
                FolderPath = fileState.FolderPath;
                AllFiles = fileState.AllFiles;
                NewFiles = fileState.NewFiles;
                ChangedFiles = fileState.ChangedFiles;
            }
        }

        private void OnSaveStateButtonClick(object sender, RoutedEventArgs e)
        {
            var fileState = new FileState()
            {
                FolderPath = FolderPath,
                SearchDateTime = _searchDateTime,
                AllFiles = AllFiles?.ToList(),
                NewFiles = NewFiles?.ToList(),
                ChangedFiles = ChangedFiles?.ToList()
            };
            ChangeOperations.SaveFileState(fileState);
        }

        private async void OnFindButtonClick(object sender, RoutedEventArgs e)
        {
            SearchStarted?.Invoke();
            var folderPath = FolderPath;
            var error = string.Empty;

            await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(folderPath))
                    {
                        throw new Exception("No folder path has been specified. Please specify a folder path.");
                    }
                    _searchDateTime = DateTime.Now;
                    var allFiles = FileExplorerOperations.GetFilesForPath(folderPath);

                    Dispatcher.Invoke(() =>
                    {
                        Initialise();
                        AllFiles = allFiles;
                    });
                }
                catch (Exception exception)
                {
                    error = exception.Message;
                }
                finally
                {
                    SearchEnded?.Invoke(error);
                }
            });
        }

        private async void OnFindChangesButtonClick(object sender, RoutedEventArgs e)
        {
            SearchStarted?.Invoke();
            var filePath = FolderPath;
            var error = string.Empty;

            await Task.Run(() =>
            {
                try
                {
                    var files = FileExplorerOperations.GetFilesForPath(filePath);

                    Dispatcher.Invoke(() =>
                    {
                        NewFiles = files.Except(AllFiles);
                        ChangedFiles = files.Except(NewFiles).Where(x => _searchDateTime < x.ModifiedDateTime);
                    });
                }
                catch (Exception exception)
                {
                    error = exception.Message;
                }
                finally
                {
                    SearchEnded?.Invoke(error);
                }
            });
        }
    }
}
