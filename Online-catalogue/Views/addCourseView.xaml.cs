using Online_catalogue.Models;
using System;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Interacțiune pentru fereastra de adăugare a unui curs.
    /// Permite utilizatorului să adauge un nou curs în aplicație.
    /// </summary>
    public partial class addCourseView : Window
    {
        private int ProfessorId { get; set; }
        
        /// <summary>
        /// Constructorul ferestrei de adăugare a cursului.
        /// Inițializează componentele vizuale ale ferestrei.
        /// </summary>
        public addCourseView(int professorId)
        {
            InitializeComponent();
            ProfessorId = professorId;
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

            // Crearea obiectului Curs
            var newCourse = new Curs
            {
                NumeCurs = NameTextBox.Text,
                Descriere = DescriptionTextBox.Text
            };

            // Salvarea cursului în baza de date
            var db = new DatabaseService();
            db.AddCourse(newCourse); // Adăugăm cursul în DB și obținem id-ul

            // Salvarea legăturii între profesor și curs în tabela user_curs
            db.AddUserCourseLink(ProfessorId, newCourse.Id); // Legăm profesorul de curs

            MessageBox.Show("Cursul a fost salvat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            // Închidem fereastra și returnăm true pentru succes
            this.DialogResult = true;
            this.Close();
        }


    }
}
