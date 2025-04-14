using Online_catalogue.Models;
using System.Windows;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Interacțiune pentru fereastra de adăugare a notei.
    /// Permite adăugarea unei note pentru un student la un anumit curs.
    /// </summary>
    public partial class addGrade : Window
    {
        private readonly int studentId;
        private readonly int courseId;

        /// <summary>
        /// Constructorul ferestrei de adăugare a notei.
        /// </summary>
        /// <param name="student">Obiectul <see cref="User"/> ce reprezintă studentul căruia i se va adăuga nota.</param>
        /// <param name="numeCurs">Numele cursului pentru care se adaugă nota.</param>
        /// <param name="idCurs">ID-ul cursului la care se adaugă nota.</param>
        public addGrade(User student, string numeCurs, int idCurs)
        {
            InitializeComponent();

            studentId = student.Id;
            courseId = idCurs;

            StudentNameText.Text = $"{student.Nume} {student.Prenume}";
            CourseNameText.Text = numeCurs;
        }

        /// <summary>
        /// Eveniment care se declanșează atunci când utilizatorul apasă butonul pentru a salva nota.
        /// Adaugă nota în baza de date dacă este validă.
        /// </summary>
        /// <param name="sender">Obiectul care a declanșat evenimentul (butonul de salvare a notei).</param>
        /// <param name="e">Datele evenimentului.</param>
        private void SalveazaNota_Click(object sender, RoutedEventArgs e)
        {
            // Verifică dacă nota introdusă este un număr între 1 și 10
            if (int.TryParse(NotaTextBox.Text, out int notaNoua) && notaNoua >= 1 && notaNoua <= 10)
            {
                var db = new DatabaseService();
                db.AddNota(new Nota
                {
                    IdUser = studentId,
                    IdCurs = courseId,
                    NotaValoare = notaNoua
                });

                MessageBox.Show("Nota a fost adăugată cu succes.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Introduceți o notă validă între 1 și 10.");
            }
        }
    }
}
