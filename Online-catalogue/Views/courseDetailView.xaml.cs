using Online_catalogue.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Fereastra care detaliază informațiile unui curs și permite adăugarea de note și studenți.
    /// </summary>
    public partial class courseDetailView : Window
    {
        /// <summary>
        /// Numele cursului.
        /// </summary>
        public string NumeCurs { get; set; }

        /// <summary>
        /// Lista de studenți cu notele acestora.
        /// </summary>
        public ObservableCollection<UserCuNote> Studenti { get; set; }

        /// <summary>
        /// ID-ul cursului.
        /// </summary>
        public int cursId { get; set; }

        /// <summary>
        /// Constructorul ferestrei care primește ID-ul cursului.
        /// </summary>
        /// <param name="courseId">ID-ul cursului pentru care se afișează detaliile.</param>
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

        /// <summary>
        /// Încarcă datele despre studenți și notele acestora din baza de date.
        /// </summary>
        private void LoadDataFromDatabase()
        {
            var db = new DatabaseService();

            var elevi = db.GetEleviPentruCurs(cursId);

            var toateNotele = db.GetNote(cursId)
                                .Where(n => n.IdCurs == cursId)
                                .ToList();

            // Alocă notele fiecărui elev
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

        /// <summary>
        /// Deschide fereastra pentru adăugarea unei note pentru un student selectat.
        /// După adăugarea notei, datele sunt reîncărcate.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul (butonul AdaugaNota).</param>
        /// <param name="e">Datele evenimentului.</param>
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

        /// <summary>
        /// Deschide fereastra pentru adăugarea unui student.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul (butonul AdaugaStudent).</param>
        /// <param name="e">Datele evenimentului.</param>
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

    /// <summary>
    /// Clasa care extinde clasa User și adaugă informații despre note.
    /// </summary>
    public class UserCuNote : User
    {
        /// <summary>
        /// Lista cu notele studentului.
        /// </summary>
        public List<int> Note { get; set; } = new List<int>();

        /// <summary>
        /// Proprietatea care returnează notele concatenate sub formă de șir de caractere.
        /// </summary>
        public string NoteConcatenate => string.Join(", ", Note);

        /// <summary>
        /// Actualizează lista de studenți din fereastra principală atunci când notele unui student se schimbă.
        /// </summary>
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
