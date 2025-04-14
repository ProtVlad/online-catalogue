using Online_catalogue.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class studentCourseDetailsView : Window
    {
        public string NumeCurs { get; set; }
        public string Descriere { get; set; }
        public List<Nota> Note { get; set; }
        public double Media { get; set; }

        public studentCourseDetailsView(User student, Curs curs)
        {
            InitializeComponent();

            NumeCurs = curs.NumeCurs;
            Descriere = curs.Descriere;

            var db = new DatabaseService();
            Note = db.GetNotesForStudent(student.Id)
                     .Where(n => n.IdUser == student.Id && n.IdCurs == curs.Id)
                     .ToList();

            if (Note.Any())
            {
                Media = Note.Average(n => n.NotaValoare);
            }
            else
            {
                Media = 0;
            }

            DataContext = this;
        }
    }
}
