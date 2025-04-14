using Online_catalogue.Models;
using System.Windows;

namespace Online_catalogue.Views
{
    public partial class addGrade : Window
    {
        private readonly int studentId;
        private readonly int courseId;

        public addGrade(User student, string numeCurs, int idCurs)
        {
            InitializeComponent();

            studentId = student.Id;
            courseId = idCurs;

            StudentNameText.Text = $"{student.Nume} {student.Prenume}";
            CourseNameText.Text = numeCurs;
        }

        private void SalveazaNota_Click(object sender, RoutedEventArgs e)
        {
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
