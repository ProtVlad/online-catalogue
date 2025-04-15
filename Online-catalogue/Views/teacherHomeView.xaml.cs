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
    /// Interaction logic for teacherHomeView.xaml
    /// </summary>
    public partial class teacherHomeView : Window
    {
        public ObservableCollection<Curs> Courses { get; set; }
        private User LoggedInUser { get; set; }

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





        private void ViewAllStudents_Click(object sender, RoutedEventArgs e)
        {
            var viewStudentsWindow = new teacherStudentsView();
            viewStudentsWindow.ShowDialog();
        }
        private void Course_MouseDown(object sender, RoutedEventArgs e)
        {
            var course = (sender as FrameworkElement).DataContext as Curs;

            if (course != null)
            {
                var courseDetailWindow = new courseDetailView(course.Id, course.NumeCurs);
                courseDetailWindow.ShowDialog();
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordView(LoggedInUser);
            resetWindow.ShowDialog();
        }
    }

}
