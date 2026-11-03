using System.Windows;
using System.Windows.Controls;

namespace Alfa_1_aars_eksamens_projekt.Views.Partials
{
    /// <summary>
    /// Interaction logic for NavBar.xaml
    /// </summary>
    /// User Control done by Keemon
    public partial class NavBar : UserControl
    {
        public NavBar()
        {
            InitializeComponent();
        }

        private void frontPage_Click(object sender, RoutedEventArgs e)
        {
            //Swaps the PageContent to the landing page
            MainWindow.mvvm.SwapPageContent("LandingPage");
        }

        private void companyProfile_Click(object sender, RoutedEventArgs e)
        {
            //Swaps the PageContent to the company profile page
            MainWindow.mvvm.SwapPageContent("CompanyProfile");
        }

        private void statistics_Click(object sender, RoutedEventArgs e)
        {
            //Swaps the PageContent to the statistics page
            MainWindow.mvvm.SwapPageContent("Statistics");
        }

        private void wasteInformation_Click(object sender, RoutedEventArgs e)
        {
            //Swaps the PageContent to the waste information page
            MainWindow.mvvm.SwapPageContent("WasteInformation");
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            //Swaps the PageContent to the login page
            MainWindow.mvvm.SwapPageContent("LoginPage");
        }
    }
}
