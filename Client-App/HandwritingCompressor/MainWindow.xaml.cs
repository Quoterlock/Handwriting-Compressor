using HandwritingCompressor.Exceptions;
using HandwritingCompressor.Modules.Interfaces;
using HandwritingsCompressor.Modules;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace HandwritingCompressor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedFilePath = string.Empty;
        private readonly ImagesManager _imagesManager;
        private readonly IProductKeyManager _keyManager;
        private bool _isActivated;

        public MainWindow(ImagesManager imagesManager, IProductKeyManager keyManager)
        {
            _imagesManager = imagesManager;
            _keyManager = keyManager;
            InitializeComponent();
            IsActivated = _keyManager.IsActivated();
        }

        public bool IsActivated
        {
            get { return _isActivated; }
            set
            {
                _isActivated = value;
                UpdateDemoLayout(value);
            }
        }

        private void UpdateDemoLayout(bool value)
        {
            if (value)
            {
                demoNotificationBar.Height = new GridLength(0);
                exportAllBtn.IsEnabled = true;
                this.Title = "Handwriting Compressor";
            }
            else
            {
                demoNotificationBar.Height = new GridLength(24);
                exportAllBtn.IsEnabled = false;
                this.Title = "Handwriting Compressor (Demo)";
            }
        }

        private void UpdatePreview()
        {
            var config = _imagesManager.GetInfo(_selectedFilePath);
            brightnessSlider.Value = config.Brightness;
            contrastSlider.Value = config.Contrast;
            negativeCheckBox.IsChecked = config.Negative;
            previewImage.Source = _imagesManager.GetPreview(_selectedFilePath);
        }

        private void updatePreviewBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdatePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void contrastSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            contrastLabel.Content = $"Contrast ({(int)contrastSlider.Value}%)";
            _imagesManager.UpdateConfig(_selectedFilePath, null, (int)contrastSlider.Value, null);
        }

        private void brightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            brightnessLabel.Content = $"Brightness ({(int)brightnessSlider.Value}%)";
            _imagesManager.UpdateConfig(_selectedFilePath, (int)brightnessSlider.Value, null, null);
        }

        private void previewScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (previewImage != null && previewImage.Source != null)
            {
                int originalW = (int)previewImage.Source.Width;
                int originalH = (int)previewImage.Source.Height;

                int scale = (int)previewScale.Value;
                int width = originalW * scale / 100;
                int height = originalH * scale / 100;

                previewImage.Width = width;
                previewImage.Height = height;
            }
        }

        private void addFileBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Images (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    _imagesManager.Add(openFileDialog.FileNames);
                    UpdateFilesList();
                }
            }
            catch (NotActivatedProductException ex)
            {
                MessageBox.Show($"Activate to work with multiple files or files larger than 1MB.", "Activation");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occured during opening: {ex}");
            }
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath)) 
            {
                MessageBox.Show("Select file first");
                return;
            }
            
            try
            {
                string dir = SelectDirectory();
                if (string.IsNullOrEmpty(dir))
                    return;
                _imagesManager.ExportSelected(_selectedFilePath, dir);
                MessageBox.Show("Saved");
            }
            catch (NotActivatedProductException ex)
            {
                MessageBox.Show($"Activate to work with multiple files or files larger than 1MB.", "Activation");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occured during saving: {ex}");
            }
        }

        private string SelectDirectory()
        {
            var folderDialog = new OpenFolderDialog();

            if (folderDialog.ShowDialog() == true)
            {
                return folderDialog.FolderName;
            }
            return "";
        }

        private void filesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filesListView.SelectedIndex >= 0
                && filesListView.SelectedIndex < filesListView.Items.Count)
            {
                _selectedFilePath = (string)filesListView.SelectedValue;
                UpdatePreview();
            }
        }

        private void removeFileBtn_Click(object sender, RoutedEventArgs e)
        {
            _imagesManager.Remove((string)filesListView.SelectedValue);
            UpdateFilesList();
        }

        private void UpdateFilesList()
        {
            filesListView.ItemsSource = _imagesManager.GetPaths();
            filesListView.SelectedIndex = 0;
        }

        private void exportAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if(_imagesManager.GetPaths().Length <= 0)
            {
                MessageBox.Show("Add file first");
                return;
            }

            try
            {
                string dir = SelectDirectory();
                if (string.IsNullOrEmpty(dir))
                    return;

                _imagesManager.Export(dir);
                MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occured during saving: {ex}");
            }
        }

        private void enterProductKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new EnterProductKey(_keyManager);
            dialogWindow.ShowDialog();
            IsActivated = dialogWindow.IsActivated;
        }
    }
}