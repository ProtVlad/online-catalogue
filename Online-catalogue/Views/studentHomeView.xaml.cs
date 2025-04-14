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

        public studentHomeView(User user)
        {
            InitializeComponent();
            LoggedInUser = user;

            WelcomeTextBlock.Text = $"Bun venit, {LoggedInUser.Nume} {LoggedInUser.Prenume}!";

            var db = new DatabaseService();

            var cursuriDinDb = db.GetCourses();

            StudentCourses = new ObservableCollection<Curs>(cursuriDinDb);

            var toateNotele = db.GetNotesForStudent(LoggedInUser.Id).Where(n => n.IdUser == LoggedInUser.Id).ToList();

            if (toateNotele.Any())
            {
                MediaGenerala = toateNotele.Average(n => n.NotaValoare);
            }
            else
            {
                MediaGenerala = 0;
            }

            DataContext = this;
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordView(LoggedInUser);
            resetWindow.ShowDialog();
        }

        private void Course_Click(object sender, RoutedEventArgs e)
        {
            var course = (sender as FrameworkElement)?.DataContext as Curs;

            if (course != null)
            {
                var detailsWindow = new studentCourseDetailsView(LoggedInUser, course);
                detailsWindow.ShowDialog();
            }
        }

    }
}
