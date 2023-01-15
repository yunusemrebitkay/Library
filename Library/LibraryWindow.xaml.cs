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
    /// LibraryWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            InitializeComponent();
            GlobalMethods.LibraryMain = this;
        }

        private void btnAdminPanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmbxForListing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void txtbxNameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnGetSelectedBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBooksOnMe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
