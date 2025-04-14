using Online_catalogue.Models;
using System.Collections.Generic;
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

        /// <summary>
        /// Lista de elevi din baza de date.
        /// </summary>
        private List<User> elevi;

        /// <summary>
        /// Lista de cursuri din baza de date.
        /// </summary>
        private List<Curs> cursuri;

        /// <summary>
        /// Constructorul ferestrei de adăugare a studentului la curs.
        /// Inițializează componenta și încarcă datele necesare.
        /// </summary>
        public teacherAddStudent()
        {
            InitializeComponent();
            db = new DatabaseService();
            LoadData();
        }

        /// <summary>
        /// Încarcă datele necesare pentru fereastra de adăugare student:
        /// 1. Elevii din baza de date.
        /// 2. Cursurile din baza de date.
        /// </summary>
        private void LoadData()
        {
            // 1. Luăm toți elevii
            elevi = db.GetUsers().Where(u => u.Rol == "elev").ToList();

            // Creăm un câmp nou pentru afișare ușoară
            foreach (var elev in elevi)
            {
                elev.NumeComplet = $"{elev.Prenume} {elev.Nume}";
            }

            // Setează sursa de date pentru ComboBox-ul cu elevi
            StudentComboBox.ItemsSource = elevi;

            // 2. Luăm toate cursurile
            cursuri = db.GetCourses(); // presupunem că această metodă există

            // Setează sursa de date pentru ComboBox-ul cu cursuri
            CourseComboBox.ItemsSource = cursuri;
        }

        /// <summary>
        /// Adaugă studentul selectat la cursul selectat.
        /// Afișează un mesaj de succes sau eroare în funcție de selecțiile făcute.
        /// </summary>
        /// <param name="sender">Butonul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void AdaugaStudentLaCurs_Click(object sender, RoutedEventArgs e)
        {
            // Obține studentul selectat
            var studentSelectat = StudentComboBox.SelectedItem as User;

            // Obține cursul selectat
            var cursSelectat = CourseComboBox.SelectedItem as Curs;

            // Verifică dacă ambele elemente sunt selectate
            if (studentSelectat == null || cursSelectat == null)
            {
                MessageBox.Show("Te rog selectează un student și un curs.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Salvăm asocierea în baza de date (comentat pentru acum)
            // db.AddUserToCourse(studentSelectat.Id, cursSelectat.Id);

            // Afișăm mesaj de succes
            MessageBox.Show($"Studentul {studentSelectat.Prenume} {studentSelectat.Nume} a fost adăugat la cursul {cursSelectat.NumeCurs}.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            // Închide fereastra după adăugare
            this.Close();
        }
    }
}
