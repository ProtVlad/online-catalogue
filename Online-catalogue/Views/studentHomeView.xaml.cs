using Online_catalogue.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Online_catalogue.Views
{
    public partial class studentHomeView : Window
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Curs> StudentCourses { get; set; }
        public double MediaGenerala { get; set; }

        // Constructorul ferestrei
        public studentHomeView(User user)
        {
            InitializeComponent();
            LoggedInUser = user;

            // Setăm textul de bun venit
            WelcomeTextBlock.Text = $"Bun venit, {LoggedInUser.Nume} {LoggedInUser.Prenume}!";

            var db = new DatabaseService();

            // Obținem cursurile din baza de date
            var cursuriDinDb = db.GetCourses();

            // Adăugăm cursurile într-o colecție observable
            StudentCourses = new ObservableCollection<Curs>(cursuriDinDb);

            // Calculăm media generală pe baza notelor
            var toateNotele = db.GetNotesForStudent(LoggedInUser.Id).Where(n => n.IdUser == LoggedInUser.Id).ToList();

            if (toateNotele.Any())
            {
                // Media notelor
                MediaGenerala = toateNotele.Average(n => n.NotaValoare);
            }
            else
            {
                MediaGenerala = 0;
            }

            // Setăm DataContext-ul pentru Binding
            DataContext = this;
        }

        // Metoda pentru resetarea parolei
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordView(LoggedInUser);
            resetWindow.ShowDialog();
        }

        // Metoda pentru a selecta un curs și a afișa detalii
        private void Course_Click(object sender, RoutedEventArgs e)
        {
            var course = (sender as FrameworkElement)?.DataContext as Curs;

            if (course != null)
            {
                // Afișează mesaj cu numele cursului selectat
                MessageBox.Show($"Ai selectat cursul: {course.NumeCurs}", "Detalii Curs", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
