using System;
using System.Collections.Generic;
using System.IO;
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

    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
            AdminOperations.AdminMain = this;
            AdminOperations.GetTakenInformationForAdmin();
            AdminOperations.GetBooksInformationForAdmin();
            AdminOperations.GetTeachersInformationForAdmin();
            AdminOperations.GetReturnApprovals();
        }

        private void btnTeacherApproval_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.TeacherApproval();
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.AddThebook();
        }


        private void btnReturnApproval_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.ReturnApproval();
        }

        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.SaveSetting();
        }
    }
}
