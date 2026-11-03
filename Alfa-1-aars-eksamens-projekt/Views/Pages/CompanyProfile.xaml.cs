using Alfa_1_aars_eksamens_projekt.Classes;
using Alfa_1_aars_eksamens_projekt.Views.Popups;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Alfa_1_aars_eksamens_projekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for CompanyProfile.xaml
    /// </summary>
    public partial class CompanyProfile : Page
    {
        public static int responsibleEmployeeEditID;

        public CompanyProfile()
        {
            InitializeComponent();

            CompanyName.Content = MainWindow.currentCompanyName;

            List<ResponsibleEmployee> responsibleEmployees = ResponsibleEmployee.GetResponsibleEmployees(MainWindow.currentCompanyID);
            ResponsibleEmployeeListView.ItemsSource = responsibleEmployees;

        }

        private void createRE_Click(object sender, RoutedEventArgs e)
        {
            CreateResponsibleEmployee window = new CreateResponsibleEmployee();

            window.Show();
        }

        private void EditRE_Click(object sender, RoutedEventArgs e)
        {
            // Grab button data from sender, interpret as button control
            var btn = sender as Button;

            // Find the Person object data in the row of the button
            var responsibleEmployee = btn.DataContext as ResponsibleEmployee;

            responsibleEmployeeEditID = responsibleEmployee.id;

            EditResponsibleEmployee window = new EditResponsibleEmployee();

            window.Show();
        }

        private void DeleteRE_Click(object sender, RoutedEventArgs e)
        {

            // Grab button data from sender, interpret as button control
            var btn = sender as Button;

            // Find the Person object data in the row of the button
            var responsibleEmployee = btn.DataContext as ResponsibleEmployee;

            try
            {

                ResponsibleEmployee.DeleteResponsibleEmployee(responsibleEmployee.id);
                Log.CreateLog("Responsible Employee with ID: " + responsibleEmployee.id + " deleted by ");
                MessageBox.Show("Slettet fra databasen");
                MainWindow.mvvm.SwapPageContent("CompanyProfile");
            }
            catch (Exception ex)
            {
                Log.CreateLog(ex + " ");
                MessageBox.Show("Noget gik galt \n" + ex);
            }
        }
    }
}
