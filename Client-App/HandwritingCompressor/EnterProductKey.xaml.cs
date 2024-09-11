using HandwritingCompressor.Modules;
using HandwritingsCompressor.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HandwritingCompressor
{
    /// <summary>
    /// Interaction logic for EnterProductKey.xaml
    /// </summary>
    public partial class EnterProductKey : Window
    {
        public EnterProductKey()
        {
            InitializeComponent();
        }

        private void activateBtn_Click(object sender, RoutedEventArgs e)
        {
            var productKey = productKeyTextBox.Text.Trim();
            var keyValidator = new KeyValidator();
            var isValid = keyValidator.Validate(productKey);
            if(isValid)
            {
                ProductKeyManager.Save(productKey);
                MessageBox.Show("Restart program after pressing Ok btn");
                Application.Current.Shutdown();
                return;
            }
            MessageBox.Show($"Product key {productKey} is invalid");
        }
    }
}
