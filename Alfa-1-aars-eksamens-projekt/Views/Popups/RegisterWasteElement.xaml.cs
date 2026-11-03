using Alfa_1_aars_eksamens_projekt.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alfa_1_aars_eksamens_projekt.Views.Popups
{
    /// <summary>
    /// Interaction logic for RegisterWasteElement.xaml
    /// </summary>
    public partial class RegisterWasteElement : Window
    {
        //We set bool values to false for use further down.
        public bool validAmount = false;
        public bool validSelection = false;
        public bool PickAmountValid = false;
        public bool PickMUValid = false;
        public bool PickCategoryValid = false;
        public bool PickREValid = false;
        //We make a public instance of the register waste element class
        public RegisterWasteElement()
        {


            InitializeComponent();

            PickAmount.Focus();
            //We create lists to insert the information from the 3 different classes, where we use the differetn Get methods.
            List<WasteCategory> wasteCategories = WasteCategory.GetWasteCategories();
            List<MeasurementUnit> measurementUnits = MeasurementUnit.GetMeasurementUnits();
            List<ResponsibleEmployee> responsibleEmployees = ResponsibleEmployee.GetResponsibleEmployees(MainWindow.currentCompanyID);

            //Here we look through all the items in the lists, and add them to the comboboxes
            foreach (WasteCategory WC in wasteCategories)
                PickCategory.Items.Add(WC.name);
            foreach (MeasurementUnit MU in measurementUnits)
                PickMU.Items.Add(MU.name);
            foreach (ResponsibleEmployee RE in responsibleEmployees)
                PickRE.Items.Add(RE.name);
        }
        //We create a method for the button click from xaml to register waste element. 
        private void RegisterWE_Click(object sender, RoutedEventArgs e)
        {
            //We create a if statement to see if there is information input in the different fields.
            if (validAmount == true && validSelection == true)
            {
                //We convert the inputted information to the different data types from the combo boxes and text boxes.
                decimal amount = Convert.ToDecimal(PickAmount.Text);
                string description = GiveDescription.Text;
                DateTime dateTime = DateTime.Now;
                string responsibleEmployee = PickRE.SelectedItem.ToString();
                int measurementUnit = PickMU.SelectedIndex;
                int category = PickCategory.SelectedIndex;

                //We call the WasteElement class and use the CreateWasteElement method to input the converted data into the database.
                WasteElement.CreateWasteElement(amount, description, dateTime, responsibleEmployee, measurementUnit, category);

                //We close the popup window upen completion.
                Close();

                //We refresh the LandingPage so the user can see the newly added waste element and its information.
                MainWindow.mvvm.SwapPageContent("LandingPage");
            }
            //We setup what the app will do in case of failed validation.
            else
            {
                //We show a messagebox with a message to input information in all fields.
                MessageBox.Show("Input i alle felter");
            }
        }

        //We create a method to validate the textbox input.
        private void fieldValidator(object sender, TextChangedEventArgs e)
        {
            //Lavet af Nichlas
            //We create a string for the textbox for amount to be put into.
            string amount = PickAmount.Text.ToString().Trim();
            decimal value;

            //We setup a if statement, to check if the textbox for amount is empty.
            if (amount == "")
            {
                //We setup background and border colour to change in case of no input.
                PickAmount.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                PickAmount.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                //We add a message to a label to be shown next to the amount textbox.
                amountOutput.Content = "Skal udfyldes";
                //We keep the validation as false.
                PickAmountValid = false;
            }
            else if (!Decimal.TryParse(amount, out value))
            {
                //We setup background and border colour to change in case of no input.
                PickAmount.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                PickAmount.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                //We add a message to a label to be shown next to the amount textbox.
                amountOutput.Content = "Skal være et tal";
                //We keep the validation as false.
                PickAmountValid = false;
            }
            //We setup what the validation will do when proper information is put in.
            else
            {
                //We setup a change of colour for the textbox and border.
                PickAmount.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                PickAmount.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                //We keep the label next to the textbox empty
                amountOutput.Content = "";
                //We set the validation to be true so it is valid.
                PickAmountValid = true;
            }
            //We check if the validation of amount is valid in if statement.
            if (PickAmountValid == true)
            {
                //We set the valid amount to be accepted.
                validAmount = true;
            }
            //We create the outcome if the validation of amount is not valid.
            else
            {
                //We set the valid amount to false so it send the input message to prompt the user to input data.
                validAmount = false;
            }
        }

        //We create a method to validate the combobox choices.
        private void selectionChangedValidator(object sender, System.EventArgs e)
        {
            // Lavet af Nichlas

            //We create a if statement to check if the set string is empty, to see if the user chose a item in the combobox.
            if (PickMU.SelectedIndex == -1)
            {
                //If the string is empty, it changes colours of the background and the border.
                PickMU.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                PickMU.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                //We set in a text to a label next to the combobox, to tell the user to choose an item.
                MUOutput.Content = "Vælg 1 måle enhed";
                //We set the measurement unit validation as false.
                PickMUValid = false;
            }
            //We setup what happens if the string is not empty.
            else
            {
                //If the string is not empty the background and border colour changes.
                PickMU.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                PickMU.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                //No text is set to the label next to the combobox.
                MUOutput.Content = "";
                //We set the measurement unit validation as true. 
                PickMUValid = true;
            }

            //We create a if statement to check if the set string is empty, to see if the user chose a item in the combobox.
            if (PickCategory.SelectedIndex == -1)
            {
                //If the string is empty, it changes colours of the background and the border.
                PickCategory.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                PickCategory.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                //We set in a text to a label next to the combobox, to tell the user to choose an item.
                CategoryOutput.Content = "Vælg 1 kategori";
                //We set the category validation as false.
                PickCategoryValid = false;
            }
            //We setup what happens if the string is not empty.
            else
            {
                //If the string is not empty the background and border colour changes.
                PickCategory.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                PickCategory.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                //No text is set to the label next to the combobox.
                CategoryOutput.Content = "";
                //We set the category validation as true. 
                PickCategoryValid = true;
            }

            //We create a if statement to check if the set string is empty, to see if the user chose a item in the combobox.
            if (PickMU.SelectedIndex == -1)
            {
                //If the string is empty, it changes colours of the background and the border.
                PickRE.Background = new SolidColorBrush(Color.FromRgb(246, 216, 206));
                PickRE.BorderBrush = new SolidColorBrush(Color.FromRgb(217, 65, 48));
                //We set in a text to a label next to the combobox, to tell the user to choose an item.
                REOutput.Content = "Vælg 1 ansvarlig medarbejder";
                //We set the responsible employee validation as false.
                PickREValid = false;
            }
            //We setup what happens if the string is not empty.
            else
            {
                //If the string is not empty the background and border colour changes.
                PickRE.Background = new SolidColorBrush(Color.FromRgb(233, 246, 203));
                PickRE.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 217, 48));
                //No text is set to the label next to the combobox.
                REOutput.Content = "";
                //We set the responsible employee validation as true.
                PickREValid = true;
            }

            //We check in the if statement, if the different combobox validations are true.
            if (PickMUValid == true && PickCategoryValid == true && PickREValid == true)
            {
                //If they are all true, the validation is set true.
                validSelection = true;
            }
            //We setup what happens if the check has one or more false validations.
            else
            {
                //We set the validation to false if one of the validations or more is false.
                validSelection = false;
            }
        }
    }
}
