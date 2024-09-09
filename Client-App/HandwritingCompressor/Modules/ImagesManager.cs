using HandwritingsCompressor.Exceptions;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HandwritingsCompressor.Modules
{
    public class ImagesManager
    {
        private List<StoredImage> _images = [];

        public ImagesManager() { }

        public void Add(params string[] paths)
        {
            if (paths.Length > 1)
            {
                // TODO:Verify user product key
            }


            foreach (var path in paths)
            {
                if (!IsValid(path))
                    throw new ArgumentException(
                        $"File doesn't exist or filetype is not supported (Path:{path})");
                _images.Add(new StoredImage
                {
                    Brightness = 0,
                    Contrast = 0,
                    Path = path
                });
            }
        }

        public void Remove(string filePath)
        {
            var index = _images.FindIndex(i => i.Path.Equals(filePath));
            _images.RemoveAt(index);
        }
        
        public void UpdateConfig(string filePath, int? brightness, int? contrast, bool? isNegative)
        {
            var index = _images.FindIndex(i => i.Path.Equals(filePath));

            if (index == -1)
                throw new ArgumentException($"path not found (path:{filePath})");

            if (brightness != null)
                _images[index].Brightness = brightness.Value;
            if(contrast != null)
                _images[index].Contrast = contrast.Value;
            if (isNegative != null)
                _images[index].Negative = isNegative.Value;
        }

        public BitmapSource GetPreview(string file)
        {
            var index = _images.FindIndex(i => i.Path.Equals(file));
            if (index == -1)
                throw new ArgumentException($"Image with path:{file} was not found");

            var imgItem = _images[index];
            var img = ItemToMagicImage(imgItem);
            return ToBitmapSource(img);
        }

        public StoredImage GetInfo(string file)
        {
            var index = _images.FindIndex(i => i.Path.Equals(file));
            if (index == -1)
                throw new ArgumentException($"Image with path:{file} was not found");

            return _images[index];
        }

        public void ExportSelected(string filePath, string resultDirPath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"File doesn't exist (path:{filePath})");
            if (!Directory.Exists(resultDirPath))
                throw new ArgumentException($"Directory doesn't exist (path:{resultDirPath})");
            ExportFile(filePath, resultDirPath);
        }

        private void ExportFile(string filePath, string resultDirPath)
        {
            try
            {
                var img = GetInfo(filePath);
                var resultFileName = GetNewFileName(filePath, resultDirPath);
                MagickImage outImg = ItemToMagicImage(img);
                outImg.Write(resultFileName);
            }
            catch (Exception ex)
            {
                // TODO: handle exception properly
                MessageBox.Show($"Error occured during image processing. Details: {ex.Message}");
            }
        }

        private string GetNewFileName(string filePath, string resultDirPath)
        {
            var fileInfo = new FileInfo(filePath);
            var newPath = Path.Combine(resultDirPath, $"{fileInfo.Name}.tif");
            if (!File.Exists(newPath))
                return newPath;

            int counter = 0;
            while (true)
            {
                newPath = Path.Combine(resultDirPath, $"{fileInfo.Name}({counter}).tif");
                if (!File.Exists(newPath))
                    return newPath;
                counter++;
            }
        }

        public void Export(string resultDirPath)
        {
            if (_images.Count > 1)
            {
                // TODO:Verify user product key
                if (true)
                    throw new InvalidProductKeyException("");
            }

            if (!Directory.Exists(resultDirPath))
                throw new ArgumentException($"{resultDirPath} is not an existing directory");

            foreach (var img in _images)
                ExportFile(img.Path, resultDirPath);
        }

        public string[] GetPaths() 
            => _images.Select(i => i.Path).ToArray();

        private MagickImage ItemToMagicImage(StoredImage storedImage)
        {
            int negativeModifier = storedImage.Negative ? 200 : 0;
            MagickImage outImg = new MagickImage(storedImage.Path);
            outImg.BrightnessContrast(
                new Percentage(storedImage.Brightness),
                new Percentage(storedImage.Contrast + negativeModifier));
            outImg.ColorType = ColorType.Bilevel;
            outImg.Depth = 1;// b&w
            outImg.Settings.Compression = CompressionMethod.Group4;
            return outImg;
        }

        private bool IsValid(string path)
        {
            var extention = path.Split('.').Last() ?? string.Empty;
            return !(!File.Exists(path)
                || !(extention.Equals("png") || extention.Equals("jpeg") || extention.Equals("jpg")));
        }
        private BitmapSource ToBitmapSource(MagickImage magickImage)
        {
            // Convert MagickImage to byte array
            byte[] bytes = magickImage.ToByteArray(MagickFormat.Bmp);

            // Create BitmapSource from byte array
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new System.IO.MemoryStream(bytes);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }

    public class StoredImage
    {
        public string Path { get; set; }
        public int Brightness { get; set; }
        public int Contrast { get; set; }
        public bool Negative { get; set; }
    }
}
