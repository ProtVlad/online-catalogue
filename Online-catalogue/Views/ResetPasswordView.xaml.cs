using Online_catalogue.Models;
using System;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra pentru resetarea parolei utilizatorului.
    /// Permite utilizatorului să își schimbe parola.
    /// </summary>
    public partial class ResetPasswordView : Window
    {
        /// <summary>
        /// Utilizatorul curent pentru care se resetează parola.
        /// </summary>
        private User currentUser;

        /// <summary>
        /// Constructorul clasei ResetPasswordView.
        /// Initializează fereastra de resetare a parolei pentru utilizatorul specificat.
        /// </summary>
        /// <param name="user">Utilizatorul căruia îi vom reseta parola.</param>
        public ResetPasswordView(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        /// <summary>
        /// Evenimentul care se declanșează când utilizatorul apasă butonul de resetare a parolei.
        /// Verifică validitatea noii parole și actualizează parola utilizatorului în baza de date.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul.</param>
        /// <param name="e">Datele evenimentului.</param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Obținem noua parolă din caseta de text
            string newPassword = NewPasswordBox.Password;

            // Verificăm dacă parola nu este goală
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Parola nu poate fi goală.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Creăm obiectul de serviciu pentru a actualiza parola
                DatabaseService db = new DatabaseService();
                db.UpdateUserPassword(currentUser.Id, newPassword); // aici folosim ID-ul utilizatorului pentru actualizare

                // Afișăm un mesaj de succes
                MessageBox.Show("Parola a fost resetată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Închidem fereastra după resetarea parolei
            }
            catch (Exception ex)
            {
                // Dacă apare o eroare, afișăm mesajul corespunzător
                MessageBox.Show($"Eroare la resetarea parolei:\n{ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
