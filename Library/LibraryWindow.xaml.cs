using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            InitializeComponent();
            GlobalMethods.LibraryMain = this;
            GlobalMethods.GetUserInformation();
            GlobalMethods.WaitForMethodRun();
            GlobalMethods.GetBooksInformation();
            GlobalMethods.AdminPanelButtonVisibility();
        }

        private void btnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adm = new AdminPanel();
            adm.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LibraryOperations.Search(this);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            LibraryOperations.FilterForAmount(this);
        }

        private void cmbxForListing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LibraryOperations.ListiningTheBooks(this);
        }

        private void txtbxNameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LibraryOperations.FastSearch(this);
        }

        private void btnGetSelectedBook_Click(object sender, RoutedEventArgs e)
        {
            LibraryOperations.TakenTheBook(this);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            GlobalMethods.TryLogout();
        }

        private void btnBooksOnMe_Click(object sender, RoutedEventArgs e)
        {
            TheBooksOnMe theBooks = new TheBooksOnMe();
            theBooks.Show();
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            Notifications noti = new Notifications();
            noti.Show();
        }
    }
}
