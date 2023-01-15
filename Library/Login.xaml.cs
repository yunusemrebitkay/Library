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

namespace Library
{
    /// <summary>
    /// Login.xaml etkileşim mantığı
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            //We send the information in the login window to class Logging
            Logging.LoginMain = this;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Register RgWPF = new Register();
            RgWPF.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Logging.TryLogin();
        }
    }
}
