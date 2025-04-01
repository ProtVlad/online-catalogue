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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DatabaseService dbService = new DatabaseService();
            dbService.TestConnection();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            DatabaseService dbService = new DatabaseService();
            string userRole = dbService.AuthenticateUser(email, password);

            if (userRole != null) // Dacă autentificarea reușește
            {
                MessageBox.Show($"Autentificare reușită!\nRol: {userRole}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Poți deschide o nouă fereastră aici
            }
            else
            {
                MessageBox.Show("Email sau parolă incorectă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}