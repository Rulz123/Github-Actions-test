using Alfa_1_aars_eksamens_projekt.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alfa_1_aars_eksamens_projekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for RegisterCompanyProfile.xaml
    /// </summary>
   
    
    public partial class RegisterCompanyProfile : Page
    {
        //We instantiate a new Company object
        Company registerUser = new Company();

        //We have different bool variables in order to validate the user input
        public bool validUser = false;
        public bool passwordValid = false;
        public bool usernameValid = false;
        public bool companynameValid = false;

        //We instantiate another new Comapny object
        public static Company companyHelper = new Company();

        //We instantiate Company list
        public List<Company> companies = Company.GetCompanies();


        public RegisterCompanyProfile()
        {
            InitializeComponent();
            companyname.Focus();
        }

        //Gamze

        private void RegisterUser_Click(object sender, RoutedEventArgs e)
        {
           //Instantiate the variables from the frontend(xaml)
            string companyName = companyname.Text.Trim();
            string userName = username.Text.Trim();
            string passwordLogin = password.Password.Trim();

            //We check that the ValidUser and the consent checkbox is true
            if (validUser == true && consent.IsChecked == true)
            {
                //We send the variables to the insertCompany method 
                registerUser.insertCompany(companyName, userName, passwordLogin);

                //We try to login the new user after registering
                bool success = registerUser.Login(userName, passwordLogin);

                //If the login was successfull
                if (success == true)
                {
                    //We display a success message and redirect to the landingpage  
                    MessageBox.Show("Firma profilen blev oprettet");
                    MainWindow.mvvm.SwapPageContent("LandingPage");
                }
                else
                {
                    //We display an error message
                    MessageBox.Show("Du kunne ikke automatisk logges ind, prøv igen via forsiden");
                }
            }
            else
            {    //If the consent checkbox is false the box changes color to red 
                if (consent.IsChecked == false)
                {
                    //changes to red
                    consent.Foreground = new SolidColorBrush(Color.FromRgb(217, 87, 31));
                }
                else
                {
                    //Is green
                    consent.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
                //We display an error message
                MessageBox.Show("Udfyld venligst alle felter korrekt,\nog afkryds at du accepterer vores betingelser");
            }
        }

        private void fieldValidator(object sender, TextChangedEventArgs e)
        {
            //Natasha
            string companyName = companyname.Text.Trim();
            string userName = username.Text.Trim();

            if (companyName == "")
            {
                companyname.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                companyname.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                companynameOutput.Content = "Skal udfyldes";
                companynameValid = false;
            }
            else
            {
                companyname.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                companyname.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                companynameOutput.Content = "";
                companynameValid = true;
            }

            if (userName == "")
            {
                username.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                username.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                usernameOutput.Content = "Skal udfyldes";
                usernameValid = false;
            }

            if (companies.Exists(x => x.username == userName) && userName != "")
            {
                username.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                username.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                usernameOutput.Content = "Brugernavn allerede i brug";
                usernameValid = false;
            }
            else if (!companies.Exists(x => x.username == userName) && userName != "")
            {
                username.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                username.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                usernameOutput.Content = "";
                usernameValid = true;
            }

            if (usernameValid == true && passwordValid == true && companynameValid == true)
            {
                validUser = true;
            }
            else
            {
                validUser = false;
            }
        }

        private void fieldValidatorPassword(object sender, RoutedEventArgs e)
        {
            //Gamze

            //Everytime score matches with a character it increases with one ekstra point
            int score = 0;
            string passwordLogin = password.Password.Trim();


            if (passwordLogin.Length > 0)
                score++;
            if (passwordLogin.Length > 7)
                score++;
            if (passwordLogin.Length > 11)
                score++;
            //The characters get checked one by one, if there are numbers and letters the score will increase with 1. 
            if (passwordLogin.Any(char.IsNumber) == true && passwordLogin.Any(char.IsLetter) == true)
                score++;
           
            //linq takes the input text and checks it with Any.
            //Char is taking every character and checks it with upper(bool), if it is a capital letter. 
            if (passwordLogin.Any(char.IsUpper) == true && passwordLogin.Any(char.IsLower) == true)
                score++;
           
            
            //Checks if the characters is letters or digits, if not then there will be a match and the score will increase with one
            if (passwordLogin.Any(ch => !Char.IsLetterOrDigit(ch)) == true)
                score++;



           
            //The switch checks the value of score
            switch (score)
            {
                case 0:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                    password.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                    passwordOutput.Content = "Udfyld venligst kodeord";
                    passwordValid = false;

                    break;
                case 1:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(217, 87, 31));
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "Meget svag - øg kompleksitet";
                    passwordValid = false;
                    break;
                case 2:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(209, 118, 0));
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "Svag - øg kompleksitet";
                    passwordValid = false;
                    break;
                case 3:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(201, 136, 0));
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "Normal";
                    passwordValid = true;
                    break;
                case 4:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(183, 163, 0));
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "Stærk";
                    passwordValid = true;
                    break;
                case 5:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(168, 179, 3));
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "Meget stærk";
                    passwordValid = true;
                    break;
                case 6:
                    passwordOutput.Foreground = new SolidColorBrush(Color.FromRgb(149, 194, 43));
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "Super stærk";
                    passwordValid = true;
                    break;
                //If no cases match 
                default:
                    password.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                    password.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                    passwordOutput.Content = "";
                    passwordValid = false;
                    break;
            }

            if (usernameValid == true && passwordValid == true && companynameValid == true)
            {
                validUser = true;
            }
            else
            {
                validUser = false;
            }
        }
    }
}