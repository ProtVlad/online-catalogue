using Online_catalogue.Models;
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
            User loggedUser = dbService.AuthenticateUser(email, password);

            if (loggedUser != null)
            {
                MessageBox.Show($"Autentificare reușită!\nRol: {loggedUser.Rol}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                switch (loggedUser.Rol.ToLower())
                {
                    case "admin":
                        adminView adminView = new adminView();
                        this.Close();
                        adminView.ShowDialog();
                        break;
                    case "profesor":
                        teacherHomeView teacherHomeView = new teacherHomeView(loggedUser); // trimitem user-ul aici
                        this.Close();
                        teacherHomeView.ShowDialog();
                        break;
                    case "elev":
                        //new studentView(loggedUser).ShowDialog();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Email sau parolă incorectă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}