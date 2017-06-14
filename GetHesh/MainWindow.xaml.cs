using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace GetHesh
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string GetHashString()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Product, SerialNumber, Manufacturer FROM Win32_BaseBoard");

            ManagementObjectCollection information = searcher.Get();

            string s = "";
            foreach (ManagementObject obj in information)
            {
                foreach (PropertyData data in obj.Properties)
                {
                    //Console.WriteLine(string.Format("{0} = {1}", data.Name, data.Value));
                    s += data.Value;
                }

            }
            s += "omyr;@#$";
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        private void btnGetHash_Click(object sender, RoutedEventArgs e)
        {
            tbHash.Text = GetHashString();
        }
    }
}
