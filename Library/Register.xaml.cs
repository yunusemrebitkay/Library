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
    /// Register.xaml etkileşim mantığı
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Registering.RegisterMain = this;
            RegisterStatus.RegisterMain = this;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            Registering.RegisteringUser();

        }
    }
}
