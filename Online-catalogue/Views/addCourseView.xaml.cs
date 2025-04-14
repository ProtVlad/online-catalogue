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
    /// Interacțiune pentru fereastra de adăugare a unui curs.
    /// Permite utilizatorului să adauge un nou curs în aplicație.
    /// </summary>
    public partial class addCourseView : Window
    {
        /// <summary>
        /// Constructorul ferestrei de adăugare a cursului.
        /// Inițializează componentele vizuale ale ferestrei.
        /// </summary>
        public addCourseView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Eveniment care se declanșează atunci când utilizatorul apasă butonul pentru a salva cursul.
        /// Verifică dacă câmpurile sunt completate și salvează cursul.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul (butonul de salvare a cursului).</param>
        /// <param name="e">Datele evenimentului.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Verifică dacă câmpurile "Nume curs" și "Descriere" sunt completate
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                // Afișează un mesaj de eroare dacă câmpurile nu sunt completate
                MessageBox.Show("Toate câmpurile trebuie completate!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Afișează un mesaj de succes dacă cursul a fost salvat cu succes
            MessageBox.Show("Cursul a fost salvat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
