using Online_catalogue.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra care permite profesorului să vizualizeze lista de studenți.
    /// </summary>
    public partial class teacherStudentsView : Window
    {
        /// <summary>
        /// Constructorul ferestrei care inițializează componentele și încarcă lista de studenți.
        /// </summary>
        public teacherStudentsView()
        {
            InitializeComponent();
            LoadStudents();
        }

        /// <summary>
        /// Încarcă lista de studenți din baza de date și o afișează în DataGrid.
        /// </summary>
        private void LoadStudents()
        {
            DatabaseService db = new DatabaseService();
            List<User> studenti = db.GetUsers().Where(u => u.Rol == "elev").ToList();

            StudentsDataGrid.ItemsSource = studenti;
        }
    }
}
