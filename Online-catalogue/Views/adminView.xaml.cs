using Online_catalogue.Views;
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

namespace Online_catalogue.Views
{
    /// <summary>
    /// Interaction logic for adminView.xaml
    /// </summary>
    public partial class adminView : Window
    {
        public adminView()
        {
            InitializeComponent();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
             adminAddUserView adminAddUserView = new adminAddUserView();
            adminAddUserView.ShowDialog();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            //if (UsersDataGrid.SelectedItem is User selectedUser)
            //{
            //    EditUserWindow editWindow = new EditUserWindow(selectedUser);
            //    editWindow.ShowDialog();

            //    LoadUsers();
            //}

            //cred ca putem folosi si adminAddUserView
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            //if (UsersDataGrid.SelectedItem is User selectedUser)
            //{
            //    MessageBoxResult result = MessageBox.Show($"Sigur vrei să ștergi utilizatorul {selectedUser.LastName}?",
            //                                              "Confirmare ștergere",
            //                                              MessageBoxButton.YesNo,
            //                                              MessageBoxImage.Warning);

            //    if (result == MessageBoxResult.Yes)
            //    {
            //        DeleteUser(selectedUser.Id);

            //        LoadUsers();
            //    }
            //}
        }

        // Metoda care șterge utilizatorul din baza de date
        private void DeleteUser(int userId)
        {
           // Users.Remove(Users.FirstOrDefault(u => u.Id == userId));
        }


    }
}
