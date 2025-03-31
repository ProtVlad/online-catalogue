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
        public ObservableCollection<Course> Courses { get; set; }
        public teacherHomeView()
        {
            InitializeComponent();
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
    }
    public class Course
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
