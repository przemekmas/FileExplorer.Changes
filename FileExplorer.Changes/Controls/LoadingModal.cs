using System;
using System.Windows;
using System.Windows.Controls;

namespace FileExplorer.Changes.Controls
{
    [TemplatePart(Name = "PART_ModalBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_ModalBackgroundBorder", Type = typeof(Border))]
    public class LoadingModal : Control
    {
        private Border _modalBorder;
        private Border _modalBackgroundBorder;

        static LoadingModal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingModal), new FrameworkPropertyMetadata(typeof(LoadingModal)));
        }

        public override void OnApplyTemplate()
        {
            _modalBorder = (Border)Template.FindName("PART_ModalBorder", this);
            _modalBackgroundBorder = (Border)Template.FindName("PART_ModalBackgroundBorder", this);

            _modalBackgroundBorder.SizeChanged += OnBackgroundBorderSizeChanged;
        }

        private void OnBackgroundBorderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var modalBackgroundWidth = _modalBackgroundBorder.ActualWidth;
            var modalBackgroundHeight = _modalBackgroundBorder.ActualHeight;

            _modalBorder.Width = modalBackgroundWidth / 3;
            _modalBorder.Height = modalBackgroundHeight / 3;
        }
    }
}
