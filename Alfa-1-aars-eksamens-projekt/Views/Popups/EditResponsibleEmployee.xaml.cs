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
    /// Interaction logic for EditResponsibleEmployee.xaml
    /// </summary>
    public partial class EditResponsibleEmployee : Window
    {       

        public EditResponsibleEmployee()
        {
            InitializeComponent();
            EmployeeName.Focus();
        }
      
        //We create a method for the edit button click from xaml. 
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            //We try and run the functions in the button click.
            try
            {
                //We setup a string for the name, that is defined as text.
                string name = EmployeeName.Text;                
                //We check if the name string is empty.
                if (string.IsNullOrEmpty(name))
                {
                    //If the name string is empty we show a messagebox, telling the user to input the employees name.
                    MessageBox.Show("Venligst skriv medarbejderens fulde navn");
                    //We return to the original pop up window.
                    return;
                }
                //We setup the function that happens if the name string is not empty.
                else
                {
                    //If the name string is not empty, we call for the class responsible employee, to use the method to edite the responsible employee.
                    ResponsibleEmployee.EditResponsibleEmployee(name);
                }

                //We close the pop up window after it has run the button click.
                Close();

                //To refresh the Company Profile page to show added responsible employee.
                MainWindow.mvvm.SwapPageContent("CompanyProfile");

            }
            //We catch errors that happened in the loops
            catch (Exception ex)
            {
                Log.CreateLog(ex + " ");
                //We display a messagebox that shows the error.
                MessageBox.Show("Noget gik galt \n" + ex);
            }
        }
    }
}
