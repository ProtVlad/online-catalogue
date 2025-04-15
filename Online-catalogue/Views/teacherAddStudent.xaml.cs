using Online_catalogue.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra în care un profesor poate adăuga un student la un curs.
    /// Permite selectarea unui student și a unui curs din listele disponibile.
    /// </summary>
    public partial class teacherAddStudent : Window
    {
        /// <summary>
        /// Serviciul pentru accesarea bazei de date.
        /// </summary>
        private DatabaseService db;
        private int courseId;

        // Eveniment pentru a semnala că un student a fost adăugat
        public event Action<User> StudentAdaugat;


        /// <summary>
        /// Constructorul ferestrei de adăugare a studentului la curs.
        /// Inițializează componenta și încarcă datele necesare.
        /// </summary>
        public teacherAddStudent(int courseId)
        {
            InitializeComponent();
            db = new DatabaseService();
            this.courseId = courseId;
            LoadData();
        }

        /// <summary>
        /// Încarcă datele necesare pentru fereastra de adăugare student:
        /// 1. Elevii din baza de date.
        /// 2. Cursurile din baza de date.
        /// </summary>
        private void LoadData()
        {
            // Obținem elevii care nu sunt deja înscriși la cursul curent
            var eleviDisponibili = db.GetEleviDisponibili(courseId);

            // Creăm un câmp pentru afișare ușoară (Nume complet)
            foreach (var elev in eleviDisponibili)
            {
                elev.NumeComplet = $"{elev.Prenume} {elev.Nume}";
            }

            // Setăm lista de elevi în ComboBox
            StudentComboBox.ItemsSource = eleviDisponibili;
        }

        /// <summary>
        /// Adaugă studentul selectat la cursul selectat.
        /// Afișează un mesaj de succes sau eroare în funcție de selecțiile făcute.
        /// </summary>
        /// <param name="sender">Butonul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void AdaugaStudent_Click(object sender, RoutedEventArgs e)
        {
            // Obține studentul selectat
            var studentSelectat = StudentComboBox.SelectedItem as User;

            if (studentSelectat == null)
            {
                MessageBox.Show("Te rog selectează un student.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Salvează asocierea în baza de date (în `user_curs`)
            db.AddUserToCourse(studentSelectat.Id, courseId);

            // Emitere eveniment de adăugare student
            StudentAdaugat?.Invoke(studentSelectat); // Aici emitem evenimentul cu studentul

            // Închide fereastra după adăugare
            this.Close();
        }
    }
}
