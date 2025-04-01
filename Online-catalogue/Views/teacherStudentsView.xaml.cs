using System.Collections.Generic;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class teacherStudentsView : Window
    {
        public teacherStudentsView()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            List<Student> students = new List<Student>
            {
                new Student { Id = 1, LastName = "Popescu", FirstName = "Ana", Email = "ana.popescu@example.com" },
                new Student { Id = 2, LastName = "Ionescu", FirstName = "Mihai", Email = "mihai.ionescu@example.com" },
                new Student { Id = 3, LastName = "Dumitrescu", FirstName = "Elena", Email = "elena.dumitrescu@example.com" }
            };

            StudentsDataGrid.ItemsSource = students;
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
