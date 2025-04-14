using Online_catalogue.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra care afișează detaliile unui curs pentru un student, inclusiv notele și media acestuia.
    /// </summary>
    public partial class studentCourseDetailsView : Window
    {
        /// <summary>
        /// Numele cursului.
        /// </summary>
        public string NumeCurs { get; set; }

        /// <summary>
        /// Descrierea cursului.
        /// </summary>
        public string Descriere { get; set; }

        /// <summary>
        /// Lista notelor obținute de student la acest curs.
        /// </summary>
        public List<Nota> Note { get; set; }

        /// <summary>
        /// Media notelor obținute de student la acest curs.
        /// </summary>
        public double Media { get; set; }

        /// <summary>
        /// Constructorul clasei studentCourseDetailsView.
        /// Inițializează fereastra de detalii pentru cursul specificat și studentul care participă la acest curs.
        /// </summary>
        /// <param name="student">Studentul pentru care se afișează detaliile cursului.</param>
        /// <param name="curs">Cursul pentru care se afișează detaliile.</param>
        public studentCourseDetailsView(User student, Curs curs)
        {
            InitializeComponent();

            // Setează proprietățile pentru numele și descrierea cursului
            NumeCurs = curs.NumeCurs;
            Descriere = curs.Descriere;

            // Obține lista de note pentru studentul curent la acest curs
            var db = new DatabaseService();
            Note = db.GetNotesForStudent(student.Id)
                     .Where(n => n.IdUser == student.Id && n.IdCurs == curs.Id)
                     .ToList();

            // Calculează media notelor, dacă există note
            if (Note.Any())
            {
                Media = Note.Average(n => n.NotaValoare);
            }
            else
            {
                Media = 0; // Dacă nu există note, media este 0
            }

            // Setează contextul de date pentru binding în UI
            DataContext = this;
        }
    }
}
