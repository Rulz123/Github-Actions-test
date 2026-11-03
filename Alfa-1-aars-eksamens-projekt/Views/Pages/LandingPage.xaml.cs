using Alfa_1_aars_eksamens_projekt.Classes;
using Alfa_1_aars_eksamens_projekt.Views.Partials;
using Alfa_1_aars_eksamens_projekt.Views.Popups;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alfa_1_aars_eksamens_projekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : Page
    {
        // We initialize variables that are used several times throughout the landing pages methods
        public DateTime startDate = DateTime.Now.AddMonths(-1);
        public DateTime endDate = DateTime.Now;
        RegisterWasteElement registerWasteElement = new RegisterWasteElement();
        public static int wasteElementEditID;

        // Section By Natasha
        // When loading up the page
        public LandingPage()
        {
            InitializeComponent();

            // Set the selected dates in the comboboxes on the page to the start date (a month ago) and end date (today)
            SortDateTimeStart.SelectedDate = startDate;
            SortDateTimeEnd.SelectedDate = endDate;

            // We make a list where we get all waste elements in the database that are made by the company that is logged in
            // and are within the data range specified by startDate and endDate
            List<WasteElement> wasteElements = WasteElement.GetWasteElements(MainWindow.currentCompanyID, startDate, endDate);
            
            // We set the listview's content to display the waste elements
            WasteElementListView.ItemsSource = wasteElements;
        }

        // Update listview waste element items when startDate calendar closes
        private void SortDateTimeStart_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // We grab the startDate from the selection made in the calendar
            DateTime startDate = (DateTime)SortDateTimeStart.SelectedDate;
            DateTime endDate = (DateTime)SortDateTimeEnd.SelectedDate;

            // We select all the waste elements from the database that are made by the currently logged in company
            // and are within the date range selected
            List<WasteElement> wasteElements = WasteElement.GetWasteElements(MainWindow.currentCompanyID, startDate, endDate);

            // We clear the listview and re-display the newly selected waste elements
            WasteElementListView.ItemsSource = null;
            WasteElementListView.Items.Clear();
            WasteElementListView.Items.Refresh();
            WasteElementListView.ItemsSource = wasteElements;
        }

        // Update listview waste element items when endDate calendar closes
        private void SortDateTimeEnd_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // We grab the endDate from the selection made in the calendar
            DateTime endDate = (DateTime)SortDateTimeEnd.SelectedDate;
            endDate.AddDays(1);

            // We select all the waste elements from the database that are made by the currently logged in company
            // and are within the date range selected
            List<WasteElement> wasteElements = WasteElement.GetWasteElements(MainWindow.currentCompanyID, startDate, endDate);

            // We clear the listview and re-display the newly selected waste elements
            WasteElementListView.ItemsSource = null;
            WasteElementListView.Items.Clear();
            WasteElementListView.Items.Refresh();
            WasteElementListView.ItemsSource = wasteElements;

        }

        // Button click to register new waste elements for the company
        private void btn_RegisterWasteElement_Click(object sender, RoutedEventArgs e)
        {
            // We initialize and display the Register Waste Element window
            registerWasteElement.Show();
        }

        // Button to delete specific waste element from database
        private void DeleteWE_Click(object sender, RoutedEventArgs e)
        {
            // Grab button data from sender, interpret as button control
            var btn = sender as Button;

            // Find the Person object data in the row of the button
            var wasteElement= btn.DataContext as WasteElement;

            try
            {
                // We atttempt to delete the waste element with the ID we have gotten from the button data context
                // from the listview
                WasteElement.DeleteWasteElement(wasteElement.id);

                // We display a success message if no error is thrown
                MessageBox.Show("Slettet fra databasen");

                // We redirect to landing page to refresh all the content
                MainWindow.mvvm.SwapPageContent("LandingPage");
            }
            catch (Exception ex)
            {
                // If we encounter an error, we display an error message to the user
                MessageBox.Show("Noget gik galt \n" + ex);
                Log.CreateLog(ex + " ");
            }
        }

        // Button to edit specific waste element
        private void EditWE_Click(object sender, RoutedEventArgs e)
        {
            // Grab button data from sender, interpret as button control
            var btn = sender as Button;

            // Find the waste element object data in the row of the button
            var wasteElement = btn.DataContext as WasteElement;

            // We initialize the Edit Waste Element window
            EditWasteElement window = new EditWasteElement(wasteElement);

            // We display the Edit Waste Element window, where the user can edit the waste element
            window.Show();
        }

        // Button to export company data to CSV, within the selected date range
        private void btn_ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //We set the start and end date to the selected dates in the date time pickers.
                startDate = (DateTime)SortDateTimeStart.SelectedDate;
                endDate = (DateTime)SortDateTimeEnd.SelectedDate;
                // We run the method from the CSV class, sending in the currentCompanyID, startDate and endDate as parameters
                CSV.ExportCSV(MainWindow.currentCompanyID, startDate, endDate);
                //We let the user know that the export was successful and where to find the exported CSV file.
                MessageBox.Show("Affalds elementer i den givne tidsperiode er nu eksporteret til Dropzone mappen på C: drevet.");
            }
            catch (Exception ex)
            {
                Log.CreateLog(ex + " ");
                MessageBox.Show("Kan ikke få adgang til filen da den allerede er i brug.\n" + ex.Message);
            }
            
        }
    }
}
