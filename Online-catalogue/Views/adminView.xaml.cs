using Online_catalogue.Models;
using Online_catalogue.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Online_catalogue.Views
{
    /// <summary>
    /// Interaction logic for adminView.xaml
    /// </summary>
    public partial class adminView : Window
    {
        public adminView()
        {
            InitializeComponent();
            LoadUsers();  // Adaugă această linie pentru a încărca utilizatorii
        }

        private void LoadUsers()
        {
            DatabaseService dbService = new DatabaseService();
            List<User> users = dbService.GetUsers();
            UsersDataGrid.ItemsSource = users;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
             adminAddUserView adminAddUserView = new adminAddUserView();
            this.Close();
            adminAddUserView.ShowDialog();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                adminEditUserView editWindow = new adminEditUserView(selectedUser);
                editWindow.ShowDialog();

                // Reîncarcă utilizatorii după editare
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Te rog selectează un utilizator pentru a-l edita.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                // Nu permite ștergerea adminului
                if (selectedUser.Rol.ToLower() == "admin")
                {
                    MessageBox.Show("Nu poți șterge un utilizator cu rolul de ADMIN!", "Acțiune interzisă", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Confirmare ștergere pentru ceilalți utilizatori
                MessageBoxResult result = MessageBox.Show(
                    $"Sigur vrei să ștergi utilizatorul {selectedUser.Nume} {selectedUser.Prenume}?",
                    "Confirmare ștergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DatabaseService dbService = new DatabaseService();
                        dbService.DeleteUser(selectedUser.Id);

                        LoadUsers();

                        MessageBox.Show("Utilizatorul a fost șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la ștergere: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Te rog selectează un utilizator pentru a-l șterge.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
