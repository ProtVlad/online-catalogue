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
        public ObservableCollection<UserCuNote> Studenti { get; set; }

        private int cursId = 1; // <-- modifică cu ID-ul real al cursului dacă ai unul

        public courseDetailView(int courseId)
        {
            InitializeComponent();

            NumeCurs = "Matematică";//SCHIMBARE NUME!!!!
            Studenti = new ObservableCollection<UserCuNote>();

            LoadDataFromDatabase();

            DataContext = this;
        }

        private void LoadDataFromDatabase()
        {
            var db = new DatabaseService();

            var elevi = db.GetUsers().Where(u => u.Rol == "elev").ToList();

            var toateNotele = db.GetNote(cursId).Where(n => n.IdCurs == cursId).ToList();

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
            var fereastraAdaugare = new teacherAddStudent();
            fereastraAdaugare.ShowDialog();
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
