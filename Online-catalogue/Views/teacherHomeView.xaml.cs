using Online_catalogue.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Fereastra principală pentru profesor, care permite vizualizarea cursurilor, 
    /// adăugarea unui curs, vizualizarea studenților și resetarea parolei.
    /// </summary>
    public partial class teacherHomeView : Window
    {
        /// <summary>
        /// Lista de cursuri disponibile pentru profesor.
        /// </summary>
        public ObservableCollection<Curs> Courses { get; set; }

        /// <summary>
        /// Utilizatorul care este conectat (profesorul).
        /// </summary>
        private User LoggedInUser { get; set; }

        /// <summary>
        /// Constructorul ferestrei care primește un utilizator conectat și încarcă cursurile din baza de date.
        /// </summary>
        /// <param name="user">Utilizatorul conectat.</param>
        public teacherHomeView(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            WelcomeTextBlock.Text = $"Bun venit, {LoggedInUser.Nume} {LoggedInUser.Prenume}!";

            var db = new DatabaseService();

            // Filtrăm cursurile pentru profesorul logat
            var cursuriDinDb = db.GetCoursesForTeacher(LoggedInUser.Id); // modifică în funcție de implementarea ta

            Courses = new ObservableCollection<Curs>(cursuriDinDb);

            DataContext = this;
        }

        /// <summary>
        /// Deschide fereastra pentru adăugarea unui nou curs.
        /// </summary>
        /// <param name="sender">Butonul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            // Deschidem fereastra de adăugare curs cu ID-ul profesorului
            addCourseView addCourseWindow = new addCourseView(LoggedInUser.Id);

            // Așteptăm ca fereastra de adăugare să fie închisă
            bool? dialogResult = addCourseWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                // Când fereastra de adăugare curs este închisă, actualizăm lista de cursuri
                var db = new DatabaseService();
                var cursuriDinDb = db.GetCourses(); // Reîncarcă lista de cursuri din DB
                Courses.Clear(); // Golim lista existentă
                foreach (var curs in cursuriDinDb) // Adăugăm noile cursuri
                {
                    Courses.Add(curs);
                }
            }
        }

        private void EditCourse_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var courseId = (int)button.Tag;

            // Creezi o instanță a ferestrei de editare și îi treci ID-ul cursului
            var editCourseWindow = new editCourseView(courseId);

            // Afișezi fereastra de editare
            editCourseWindow.ShowDialog();
        }


        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            if (button.Tag is int courseId)
            {
                var courseToRemove = Courses.FirstOrDefault(c => c.Id == courseId);
                if (courseToRemove != null)
                {
                    // Confirmare opțională
                    var result = MessageBox.Show("Ești sigur că vrei să ștergi acest curs?", "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        Courses.Remove(courseToRemove);

                        // Dacă vrei să o ștergi și din DB:
                        var db = new DatabaseService();
                        db.DeleteCourse(courseId); // asigură-te că ai metoda asta implementată
                    }
                }
            }
        }





        /// <summary>
        /// Deschide fereastra care afișează toți studenții.
        /// </summary>
        /// <param name="sender">Butonul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void ViewAllStudents_Click(object sender, RoutedEventArgs e)
        {
            var viewStudentsWindow = new teacherStudentsView();
            viewStudentsWindow.ShowDialog();
        }

        /// <summary>
        /// Deschide fereastra cu detalii pentru cursul selectat.
        /// </summary>
        /// <param name="sender">Elementul de UI care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void Course_MouseDown(object sender, RoutedEventArgs e)
        {
            var course = (sender as FrameworkElement).DataContext as Curs;

            if (course != null)
            {
                var courseDetailWindow = new courseDetailView(course.Id, course.NumeCurs);
                courseDetailWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Deschide fereastra de resetare a parolei pentru utilizatorul curent.
        /// </summary>
        /// <param name="sender">Butonul care a declanșat evenimentul.</param>
        /// <param name="e">Evenimentul de click.</param>
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordView(LoggedInUser);
            resetWindow.ShowDialog();
        }
    }
}
