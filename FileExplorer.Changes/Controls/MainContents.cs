using System.Windows;
using System.Windows.Controls;

namespace FileExplorer.Changes.Controls
{
    [TemplatePart(Name = "PART_ChangesExplorer", Type = typeof(ChangesExplorer))]
    [TemplatePart(Name = "PART_LoadingModal", Type = typeof(LoadingModal))]
    public class MainContents : Control
    {
        private ChangesExplorer _changesExplorer;
        private LoadingModal _loadingModal;

        static MainContents()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainContents), new FrameworkPropertyMetadata(typeof(MainContents)));
        }

        public override void OnApplyTemplate()
        {
            _changesExplorer = (ChangesExplorer)Template.FindName("PART_ChangesExplorer", this);
            _loadingModal = (LoadingModal)Template.FindName("PART_LoadingModal", this);

            _changesExplorer.SearchStarted += OnSearchStarted;
            _changesExplorer.SearchEnded += OnSearchEnded;
        }

        private void OnSearchEnded(string error)
        {
            Dispatcher.Invoke(() =>
            {
                _loadingModal.Visibility = Visibility.Collapsed;
            });

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSearchStarted()
        {
            Dispatcher.Invoke(() =>
            {
                _loadingModal.Visibility = Visibility.Visible;
            });
        }
    }
}
