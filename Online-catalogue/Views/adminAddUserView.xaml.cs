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
    /// Interacțiune pentru fereastra de adăugare a unui utilizator.
    /// Permite unui administrator să adauge un utilizator în sistem.
    /// </summary>
    public partial class adminAddUserView : Window
    {
        /// <summary>
        /// Constructorul ferestrei de adăugare a utilizatorului.
        /// Inițializează componentele vizuale ale ferestrei.
        /// </summary>
        public adminAddUserView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Eveniment care se declanșează atunci când utilizatorul apasă butonul pentru a salva un nou utilizator.
        /// Validază datele introduse și adaugă utilizatorul în baza de date.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul (butonul de salvare a utilizatorului).</param>
        /// <param name="e">Datele evenimentului.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Verifică dacă toate câmpurile sunt completate
            if (string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                RoleComboBox.SelectedItem == null ||
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

            string nume = LastNameTextBox.Text;
            string prenume = FirstNameTextBox.Text;
            string rol = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
            string parola = PasswordBox.Password;
            DateTime createdAt = DateTime.Now;

            try
            {
                // Adaugă utilizatorul în baza de date
                DatabaseService db = new DatabaseService();
                db.InsertUser(nume, prenume, rol, email, parola);

                MessageBox.Show("Utilizator adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // Afișează eroare în cazul în care salvarea în baza de date eșuează
                MessageBox.Show($"Eroare la salvare în baza de date:\n{ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Închide fereastra curentă
            this.Close();
        }

        /// <summary>
        /// Eveniment care se declanșează atunci când fereastra este închisă.
        /// Deschide fereastra de administrare a utilizatorilor.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul de închidere.</param>
        /// <param name="e">Datele evenimentului de închidere.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Deschide fereastra adminView după ce fereastra curentă este închisă
            adminView adminViewWindow = new adminView();
            adminViewWindow.Show();  // Deschide fereastra adminView
        }
    }
}
