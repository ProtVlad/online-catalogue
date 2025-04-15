using Online_catalogue.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Online_catalogue.Views
{
    public partial class teacherAddStudent : Window
    {
        private DatabaseService db;
        private int courseId;

        // Eveniment pentru a semnala că un student a fost adăugat
        public event Action<User> StudentAdaugat;

        public teacherAddStudent(int courseId)
        {
            InitializeComponent();
            db = new DatabaseService();
            this.courseId = courseId;
            LoadData();
        }

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

        private void AdaugaStudent_Click(object sender, RoutedEventArgs e)
        {
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
