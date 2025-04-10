﻿using System;
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
            if (string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                RoleComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password))
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

            string nume = LastNameTextBox.Text;
            string prenume = FirstNameTextBox.Text;
            string rol = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
            string parola = PasswordBox.Password;
            DateTime createdAt = DateTime.Now;

            try
            {
                DatabaseService db = new DatabaseService();
                db.InsertUser(nume, prenume, rol, email, parola, createdAt);

                MessageBox.Show("Utilizator adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvare în baza de date:\n{ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Dacă totul este în regulă, închide fereastra
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Deschidem fereastra de Admin după ce se închide fereastra curentă
            adminView adminViewWindow = new adminView();
            adminViewWindow.Show();  // Deschide fereastra adminView
        }

    }

}
