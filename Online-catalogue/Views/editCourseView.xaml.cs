using Online_catalogue.Models;
using System;
using System.Linq;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class editCourseView : Window
    {
        private Curs course; // Obiectul care va ține cursul actualizat

        public editCourseView(int courseId)
        {
            InitializeComponent();

            // Încarcă cursul existent
            var db = new DatabaseService();
            course = db.GetCourseById(courseId);  // Presupunem că ai o metodă care îți aduce cursul după ID

            if (course != null)
            {
                // Completează câmpurile cu datele cursului existent
                NameTextBox.Text = course.NumeCurs;
                DescriptionTextBox.Text = course.Descriere;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (course != null)
            {
                // Actualizează obiectul Curs cu datele din TextBox-uri
                course.NumeCurs = NameTextBox.Text;
                course.Descriere = DescriptionTextBox.Text;

                // Salvează modificările în baza de date
                var db = new DatabaseService();
                db.UpdateCourse(course); // Metoda ta UpdateCourse din DatabaseService

                // Actualizează ObservableCollection cu modificările (real-time)
                var teacherHomeView = Application.Current.Windows.OfType<teacherHomeView>().FirstOrDefault();
                if (teacherHomeView != null)
                {
                    var updatedCourse = teacherHomeView.Courses.FirstOrDefault(c => c.Id == course.Id);
                    if (updatedCourse != null)
                    {
                        // În loc să căutăm cursul, UI-ul se va actualiza automat prin PropertyChanged
                        updatedCourse.NumeCurs = course.NumeCurs; // Modificare directă
                        updatedCourse.Descriere = course.Descriere; // Modificare directă
                    }
                }

                // Închide fereastra
                this.Close();
            }
        }

    }
}
