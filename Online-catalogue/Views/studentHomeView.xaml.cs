using Online_catalogue.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra principală pentru student, care afișează informațiile personale, cursurile și media generală.
    /// </summary>
    public partial class studentHomeView : Window
    {
        /// <summary>
        /// Utilizatorul logat în aplicație.
        /// </summary>
        public User LoggedInUser { get; set; }

        /// <summary>
        /// Colecția de cursuri la care este înscris studentul.
        /// </summary>
        public ObservableCollection<Curs> StudentCourses { get; set; }

        /// <summary>
        /// Media generală a notelor studentului.
        /// </summary>
        public double MediaGenerala { get; set; }

        /// <summary>
        /// Constructorul clasei studentHomeView.
        /// Inițializează fereastra cu informațiile despre utilizatorul logat, cursurile acestuia și media generală.
        /// </summary>
        /// <param name="user">Utilizatorul logat.</param>
        public studentHomeView(User user)
        {
            InitializeComponent();
            LoggedInUser = user;

            // Setează textul de bun venit
            WelcomeTextBlock.Text = $"Bun venit, {LoggedInUser.Nume} {LoggedInUser.Prenume}!";

            // Obține cursurile din baza de date
            var db = new DatabaseService();
            var cursuriDinDb = db.GetCourses();

            // Populează colecția de cursuri
            StudentCourses = new ObservableCollection<Curs>(cursuriDinDb);

            // Obține notele studentului și calculează media generală
            var toateNotele = db.GetNotesForStudent(LoggedInUser.Id).Where(n => n.IdUser == LoggedInUser.Id).ToList();
            if (toateNotele.Any())
            {
                MediaGenerala = toateNotele.Average(n => n.NotaValoare);
            }
            else
            {
                MediaGenerala = 0; // Dacă nu sunt note, media este 0
            }

            // Setează contextul de date pentru binding în UI
            DataContext = this;
        }

        /// <summary>
        /// Deschide fereastra de resetare a parolei pentru utilizatorul logat.
        /// </summary>
        /// <param name="sender">Butonul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            // Deschide fereastra de resetare a parolei
            var resetWindow = new ResetPasswordView(LoggedInUser);
            resetWindow.ShowDialog();
        }

        /// <summary>
        /// Deschide fereastra cu detalii despre cursul selectat de student.
        /// </summary>
        /// <param name="sender">Butonul sau elementul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void Course_Click(object sender, RoutedEventArgs e)
        {
            // Obține cursul selectat din contextul elementului
            var course = (sender as FrameworkElement)?.DataContext as Curs;

            if (course != null)
            {
                // Deschide fereastra de detalii pentru cursul selectat
                var detailsWindow = new studentCourseDetailsView(LoggedInUser, course);
                detailsWindow.ShowDialog();
            }
        }
    }
}
