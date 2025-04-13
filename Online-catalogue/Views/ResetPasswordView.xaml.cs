using Online_catalogue.Models;
using System;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class ResetPasswordView : Window
    {
        private User currentUser;

        public ResetPasswordView(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Parola nu poate fi goală.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                DatabaseService db = new DatabaseService();
                db.UpdateUserPassword(currentUser.Id, newPassword); // 👈 aici folosim ID-ul

                MessageBox.Show("Parola a fost resetată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la resetarea parolei:\n{ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
