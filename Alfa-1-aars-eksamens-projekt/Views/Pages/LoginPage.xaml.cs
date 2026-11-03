
using System.Windows;
using System.Windows.Controls;
using Alfa_1_aars_eksamens_projekt.Views.Partials;
using Alfa_1_aars_eksamens_projekt.Classes;

namespace Alfa_1_aars_eksamens_projekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        Company login = new Company();

        public LoginPage()
        {
            InitializeComponent();
            username.Focus();
        }
        //We create a method for the login button from xaml.
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //We set a bool for the succes for a login to be the input from username textbox and password textbox.
            bool success = login.Login(username.Text, password.Password);

            //If there is text in both fields the login is a success.
            if (success == true)
            {
                Log.CreateLog(MainWindow.currentCompanyName + " login ");
                //We swap the page to the LandingPage after succesful login.
                MainWindow.mvvm.SwapPageContent("LandingPage");
            }
           
        }
    }
}
