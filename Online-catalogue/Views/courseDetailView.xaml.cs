using Online_catalogue.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class courseDetailView : Window
    {
        public string NumeCurs { get; set; }

        public int cursId { get; set; }
        public ObservableCollection<UserCuNote> Studenti { get; set; }

        public courseDetailView(int courseId, string numeCurs)
        {
            InitializeComponent();

            // Setează Numele cursului și ID-ul cursului
            NumeCurs = numeCurs;
            cursId = courseId;

            Studenti = new ObservableCollection<UserCuNote>();

            // Încarcă datele inițiale din baza de date
            LoadDataFromDatabase();

            DataContext = this;
        }

        private void LoadDataFromDatabase()
        {
            var db = new DatabaseService();

            var elevi = db.GetEleviPentruCurs(cursId);

            var toateNotele = db.GetNote(cursId)
                                .Where(n => n.IdCurs == cursId)
                                .ToList();

            foreach (var elev in elevi)
            {
                var noteElev = toateNotele
                    .Where(n => n.IdUser == elev.Id)
                    .Select(n => n.NotaValoare)
                    .ToList();

                Studenti.Add(new UserCuNote
                {
                    Id = elev.Id,
                    Nume = elev.Nume,
                    Prenume = elev.Prenume,
                    Email = elev.Email,
                    Note = noteElev
                });
            }
        }

        private void AdaugaNota_Click(object sender, RoutedEventArgs e)
        {
            var student = (sender as FrameworkElement)?.DataContext as UserCuNote;
            if (student != null)
            {
                var fereastra = new addGrade(student, NumeCurs, cursId);
                fereastra.ShowDialog();

                // Reîncarcă datele pentru a include noua notă
                Studenti.Clear();
                LoadDataFromDatabase();
            }
        }

        private void AdaugaStudent_Click(object sender, RoutedEventArgs e)
        {
            // Creează fereastra de adăugare student
            var fereastraAdaugare = new teacherAddStudent(cursId);

            // Abonăm la evenimentul de adăugare student
            fereastraAdaugare.StudentAdaugat += (student) =>
            {
                // Adăugăm studentul nou în lista de studenți
                var toateNotele = new DatabaseService().GetNote(cursId)
                                                        .Where(n => n.IdCurs == cursId)
                                                        .ToList();
                var noteElev = toateNotele
                    .Where(n => n.IdUser == student.Id)
                    .Select(n => n.NotaValoare)
                    .ToList();

                Studenti.Add(new UserCuNote
                {
                    Id = student.Id,
                    Nume = student.Nume,
                    Prenume = student.Prenume,
                    Email = student.Email,
                    Note = noteElev
                });
            };

            fereastraAdaugare.ShowDialog();
        }

        private void StergeStudent_Click(object sender, RoutedEventArgs e)
        {
            var student = (sender as FrameworkElement)?.DataContext as UserCuNote;
            if (student != null)
            {
                var db = new DatabaseService();

                // Șterge studentul din user_curs (relația dintre student și curs)
                db.RemoveStudentFromCourse(student.Id, cursId);

                // Șterge studentul din lista Studenti
                Studenti.Remove(student);
            }
        }
    }

    public class UserCuNote : User
    {
        public List<int> Note { get; set; } = new List<int>();

        public string NoteConcatenate => string.Join(", ", Note);

        public void OnNoteChanged()
        {
            var fereastra = Application.Current.Windows.OfType<courseDetailView>().FirstOrDefault();
            var index = fereastra?.Studenti.IndexOf(this);
            if (index >= 0 && index < fereastra?.Studenti.Count)
            {
                fereastra.Studenti[index.Value] = fereastra.Studenti[index.Value];
            }
        }
    }
}
