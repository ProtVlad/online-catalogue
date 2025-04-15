using Online_catalogue.Models;
using System;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class addCourseView : Window
    {
        private int ProfessorId { get; set; }

        public addCourseView(int professorId)
        {
            InitializeComponent();
            ProfessorId = professorId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
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
