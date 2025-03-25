using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for adminAddUserView.xaml
    /// </summary>
    public partial class adminAddUserView : Window
    {
        public adminAddUserView()
        {
            InitializeComponent();
        }

       

private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(IdTextBox.Text) ||
            string.IsNullOrEmpty(LastNameTextBox.Text) ||
            string.IsNullOrEmpty(FirstNameTextBox.Text) ||
             RoleComboBox.SelectedItem == null ||
            string.IsNullOrEmpty(EmailTextBox.Text))
        {
            MessageBox.Show("Toate câmpurile trebuie completate!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            return; 
        }

        // Validare email
        string email = EmailTextBox.Text;
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; 
        if (!Regex.IsMatch(email, emailPattern))
        {
            MessageBox.Show("Adresa de email nu este validă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            return; 
        }

        // Dacă totul este în regulă, închide fereastra
        this.Close();
    }


}
}
