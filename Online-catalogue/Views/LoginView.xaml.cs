using Online_catalogue.Models;
using System;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra de autentificare pentru utilizatori.
    /// Aceasta permite autentificarea utilizatorilor pe baza unui email și a unei parole.
    /// </summary>
    public partial class LoginView : Window
    {
        /// <summary>
        /// Constructorul clasei LoginView.
        /// Initializează componentele și testează conexiunea la baza de date.
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
            DatabaseService dbService = new DatabaseService();
            dbService.TestConnection();
        }

        /// <summary>
        /// Evenimentul pentru butonul de login.
        /// Verifică dacă email-ul și parola sunt corecte și redirecționează utilizatorul în funcție de rol.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul.</param>
        /// <param name="e">Datele evenimentului.</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            DatabaseService dbService = new DatabaseService();
            User loggedUser = dbService.AuthenticateUser(email, password);

            if (loggedUser != null)
            {
                MessageBox.Show($"Autentificare reușită!\nRol: {loggedUser.Rol}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                // Se verifică rolul utilizatorului și se deschide fereastra corespunzătoare
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
                        studentHomeView studentHomeView = new studentHomeView(loggedUser);
                        this.Close();
                        studentHomeView.ShowDialog();
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
