using Online_catalogue.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Online_catalogue.Views
{
    public partial class teacherAddStudent : Window
    {
        private DatabaseService db;
        private List<User> elevi;
        private List<Curs> cursuri;

        public teacherAddStudent()
        {
            InitializeComponent();
            db = new DatabaseService();
            LoadData();
        }

        private void LoadData()
        {
            // 1. Luăm toți elevii
            elevi = db.GetUsers().Where(u => u.Rol == "elev").ToList();

            // Creăm un câmp nou pentru afișare ușoară
            foreach (var elev in elevi)
            {
                elev.NumeComplet = $"{elev.Prenume} {elev.Nume}";
            }

            StudentComboBox.ItemsSource = elevi;

            // 2. Luăm toate cursurile
            cursuri = db.GetCourses(); // presupunem că această metodă există
            CourseComboBox.ItemsSource = cursuri;
        }

        private void AdaugaStudentLaCurs_Click(object sender, RoutedEventArgs e)
        {
            var studentSelectat = StudentComboBox.SelectedItem as User;
            var cursSelectat = CourseComboBox.SelectedItem as Curs;

            if (studentSelectat == null || cursSelectat == null)
            {
                MessageBox.Show("Te rog selectează un student și un curs.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Salvăm asocierea în baza de date
          //  db.AddUserToCourse(studentSelectat.Id, cursSelectat.Id);

            MessageBox.Show($"Studentul {studentSelectat.Prenume} {studentSelectat.Nume} a fost adăugat la cursul {cursSelectat.NumeCurs}.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
