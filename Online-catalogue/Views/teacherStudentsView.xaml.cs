using Online_catalogue.Models;
using System.Collections.Generic;
using System.Linq;
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
            DatabaseService db = new DatabaseService();
            List<User> studenti = db.GetUsers().Where(u => u.Rol == "elev").ToList();

            StudentsDataGrid.ItemsSource = studenti;
        }

    }

}
