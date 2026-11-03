using Alfa_1_aars_eksamens_projekt.Classes;
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

namespace Alfa_1_aars_eksamens_projekt.Views.Popups
{   
    /// <summary>
    /// Interaction logic for CreateResponsibleEmployee.xaml
    /// </summary>
    public partial class CreateResponsibleEmployee : Window
    {
        public CreateResponsibleEmployee()
        {
            InitializeComponent();
            EmployeeName.Focus();
        }
        //We set a method for the button click event to create responsible employees. 
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //We do a try loop to run our methods.
            try
            {
                //We setup a string called name for our employee name that is set to a text.
                string name = EmployeeName.Text;
                //We call the responsible employee class and the method to create a responsible employee from the said class, and using the logged in company ID to associate it with.
                ResponsibleEmployee.CreateResponsibleEmployees(name, MainWindow.currentCompanyID);
                //We setup a if loop to check if the string for the name is empty.
                if (string.IsNullOrEmpty(name))
                {
                    //We show a messagebox that says to write a name in case the string is empty.
                    MessageBox.Show("Venligst skriv medarbejders fulde navn");
                    //We return to the original popup window. 
                    return;
                }

                //Closes the window after it has run.
                Close();

                //We refresh the Company Profile page to show added responsible employee.
                MainWindow.mvvm.SwapPageContent("CompanyProfile");

            }
            //We catch errors that has happened in the loops.
            catch (Exception ex)
            {
                Log.CreateLog(ex + " ");
                //We show a messagebox with the error message. 
                MessageBox.Show("Noget gik galt \n" + ex);
            }

        }      
    }
}
