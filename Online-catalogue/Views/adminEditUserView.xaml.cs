using Online_catalogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for adminEditUserView.xaml
    /// </summary>
    public partial class adminEditUserView : Window
    {
        private User _selectedUser;

        public adminEditUserView(User user)
        {
            InitializeComponent();

            // Inițializare câmpuri cu datele utilizatorului selectat
            _selectedUser = user;

            LastNameTextBox.Text = _selectedUser.Nume;
            FirstNameTextBox.Text = _selectedUser.Prenume;
            RoleComboBox.SelectedItem = RoleComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _selectedUser.Rol);
            EmailTextBox.Text = _selectedUser.Email;
            PasswordBox.Password = _selectedUser.Parola;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validarea câmpurilor
            if (string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                RoleComboBox.SelectedItem == null &&
                _selectedUser.Rol.ToLower() != "admin" ||  // Acceptă null doar pentru admin
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Toate câmpurile trebuie completate!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validare email
            string email = EmailTextBox.Text;
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Adresa de email nu este validă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificare modificare rol admin
            string rolNou = RoleComboBox.SelectedItem != null
                ? ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString()
                : "";

            if (_selectedUser.Rol.ToLower() == "admin")
            {
                if (string.IsNullOrWhiteSpace(rolNou))
                {
                    rolNou = "admin"; // Nu s-a ales nimic → păstrăm admin
                }
                else if (rolNou.ToLower() != "admin")
                {
                    MessageBox.Show("Nu poți modifica rolul unui utilizator cu rolul ADMIN!", "Acțiune interzisă", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            _selectedUser.Nume = LastNameTextBox.Text;
            _selectedUser.Prenume = FirstNameTextBox.Text;
            _selectedUser.Rol = rolNou;
            _selectedUser.Email = EmailTextBox.Text;
            _selectedUser.Parola = PasswordBox.Password;

            try
            {
                DatabaseService db = new DatabaseService();
                db.UpdateUser(_selectedUser);

                MessageBox.Show("Utilizatorul a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea utilizatorului:\n{ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
