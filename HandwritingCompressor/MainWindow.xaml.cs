﻿using HandwritingsCompressor.Exceptions;
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
        private readonly ImagesManager _imagesManager = new();
        public MainWindow()
        {
            InitializeComponent();
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
                openFileDialog.Filter = "Images (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    _imagesManager.Add(openFileDialog.FileNames);
                    UpdateFilesList();
                }
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
                string dir = "";
                do
                {
                    dir = SelectDirectory();
                }
                while (string.IsNullOrEmpty(dir));

                _imagesManager.ExportSelected(_selectedFilePath, dir);
                MessageBox.Show("Saved");
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
                MessageBox.Show("Select file first");
                return;
            }

            try
            {
                string dir = "";
                do
                {
                    dir = SelectDirectory();
                }
                while (string.IsNullOrEmpty(dir));

                _imagesManager.Export(dir);
                MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occured during saving: {ex}");
            }
        }
    }
}