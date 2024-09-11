using HandwritingCompressor.Modules.Interfaces;
using HandwritingsCompressor.Exceptions;
using System.Windows;


namespace HandwritingCompressor
{
    /// <summary>
    /// Interaction logic for EnterProductKey.xaml
    /// </summary>
    public partial class EnterProductKey : Window
    {
        private readonly IProductKeyManager _keyManager;
        public EnterProductKey(IProductKeyManager keyManager)
        {
            _keyManager = keyManager;
            InitializeComponent();
        }

        private void activateBtn_Click(object sender, RoutedEventArgs e)
        {
            var productKey = productKeyTextBox.Text.Trim();
            if (string.IsNullOrEmpty(productKey))
                return;
            try // try to activate product
            {
                _keyManager.Activate(productKey);
                this.Close();
            } 
            catch(InvalidProductKeyException ex)
            {
                MessageBox.Show(
                    $"Product key \"{productKey}\" is not valid", 
                    "Invalid key");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured");
            }
        }
    }
}
