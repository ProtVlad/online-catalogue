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
        private User LoggedInUser { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public teacherHomeView(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            WelcomeTextBlock.Text = $"Bun venit, {LoggedInUser.Nume} {LoggedInUser.Prenume}!";
        }

        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            addCourseView addCourseWindow = new addCourseView(); 
            addCourseWindow.ShowDialog();
            
        }

        private void ViewAllStudents_Click(object sender, RoutedEventArgs e)
        {
            var viewStudentsWindow = new teacherStudentsView();
            viewStudentsWindow.ShowDialog();
        }
        private void Course_MouseDown(object sender, RoutedEventArgs e)
        {
            var course = (sender as FrameworkElement).DataContext as Course;

            if (course != null)
            {
                courseDetailView courseDetailsWindow = new courseDetailView();
                courseDetailsWindow.ShowDialog();
            }
        }
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordView(LoggedInUser);
            resetWindow.ShowDialog();
        }
    }
    public class Course
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
