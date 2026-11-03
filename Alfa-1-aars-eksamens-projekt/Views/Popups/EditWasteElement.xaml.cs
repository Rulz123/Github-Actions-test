using Alfa_1_aars_eksamens_projekt.Classes;
using Alfa_1_aars_eksamens_projekt.Views.Pages;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt.Views.Popups
{
    /// <summary>
    /// Interaction logic for EditWasteElement.xaml
    /// </summary>
    public partial class EditWasteElement : Window
    {
        public WasteElement wasteElement;

        public EditWasteElement(WasteElement wasteElementEdit)
        {
            InitializeComponent();

            wasteElement = wasteElementEdit;

            PickAmount.Focus();

            //We create a variable to make a list of wastecategories, and call the GetWasteCategories method from the WasteCategory class.

            List<WasteCategory> wasteCategories = WasteCategory.GetWasteCategories();
            //We create a variable to make a list of measurement units, and call the GetMeasurementUnits method from the MeasurementUnits class.
            List<MeasurementUnit> measurementUnits = MeasurementUnit.GetMeasurementUnits();
            //We create a variable to make a list of responsible employees, and call the GetResponsibleEmployee method, linked to the companyID that is locked in. 
            List<ResponsibleEmployee> responsibleEmployees = ResponsibleEmployee.GetResponsibleEmployees(MainWindow.currentCompanyID);

            //We setup a foreach loop to look through the waste categories in the database, and add them to the combobox for the category combobox.
            foreach (WasteCategory WC in wasteCategories)
                PickCategory.Items.Add(WC);
            //We setup a foreach loop to look through the measurement units in the database, and add them to the measurement units combobox.
            foreach (MeasurementUnit MU in measurementUnits)
                PickMU.Items.Add(MU);
            //We setup a foreach loop to look through the responsible employees in the database, and add them to the measurement units in the combobox.
            foreach (ResponsibleEmployee RE in responsibleEmployees)
                PickRE.Items.Add(RE);

            //We setup the textbox's and adds them to the wateElement variable to add them to the list.
            PickAmount.Text = wasteElement.amount.ToString();
            GiveDescription.Text = wasteElement.description;

            //We setup a foreach loop to look through the items in the combobox for measurement units dropdown menu.
            foreach (MeasurementUnit item in PickMU.Items)
                //We make a if statement to add the name to a string, and make sure it is equal to the measurement unit name in the wasteElement list and database.
                if (item.name.ToString() == wasteElement.measurementUnitName)
                {
                    //If the item is the name, we set it to be the selected item.
                    PickMU.SelectedItem = item;
                    //We break out of the if statement.
                    break;
                }
            //We setup a foreach loop to look through the responsible employees in the combobox dropdown menu. 
            foreach (ResponsibleEmployee item in PickRE.Items)
                //We setup a if statement, to check if the selected item as a string is the same as the responsibleEmployee in the wasteElement list and database.
                if (item.name.ToString() == wasteElement.responsibleEmployee)
                {
                    //If the item is the correct name chosen, we set it to be the selected item.
                    PickRE.SelectedItem = item;
                    //We break out of the if statement.
                    break;
                }

            //We setup a foreach loop to look through the categories in the category combobox dropdown menu.
            foreach (WasteCategory item in PickCategory.Items)

                //We setup a if statement to look see if the selected category as a string is the same as the list from wasteElement and database. 
                if (item.name.ToString() == wasteElement.category)
                {
                    //If the item is the correct name, we set it to be the selected item. 
                    PickCategory.SelectedItem = item;
                    //We break out of the if statement.
                    break;
                }

        }
        //We setup a method for the button click event from xaml.
        private void EditWE_Click(object sender, RoutedEventArgs e)
        {
            //We setup a try loop to try and run our click event.
            try
            {
                //we make a if statement to see if the strings in give description and pick amount is empty, and we see if the user has not selected anything in the combobox with the -1.

                if (string.IsNullOrEmpty(GiveDescription.Text) || string.IsNullOrEmpty(PickAmount.Text) || PickCategory.SelectedIndex == -1 || PickRE.SelectedIndex == -1 || PickMU.SelectedIndex == -1)
                {
                    //If any of them is empty or not selected, we show a messagebox telling the users to fill out all the fields.
                    MessageBox.Show("Udfyld venligst alle felter");
                    //we return to the pop up window.
                    return;
                }
                //If the fields all have a input we set what happens here.
                else
                {
                    //We add the selected combobox item from measurement unit combobox to be added to the list.
                    MeasurementUnit selectedMU = (MeasurementUnit)PickMU.SelectedItem;
                    ////We add the selected combobox item from category combobox to be added to the list.
                    WasteCategory selectedCategory = (WasteCategory)PickCategory.SelectedItem;
                    //We add the selected combobox item from responsible employee unit combobox to be added to the list.
                    ResponsibleEmployee selectedRE = (ResponsibleEmployee)PickRE.SelectedItem;
                    //We call the EditWasteElement to change the selected firelds to the waste element that is in the database, and look for the wasteElementEditID from the LandingPage, and make sure we have the correct element to edit.
                    WasteElement.EditWasteElement(wasteElement.id, selectedMU.id, selectedCategory.id, selectedRE.id, Convert.ToDecimal(PickAmount.Text), GiveDescription.Text);
                }


                //Closes the window
                Close();

                //To refresh the Company Profile page to show edited waste element.
                MainWindow.mvvm.SwapPageContent("LandingPage");

            }
            //We catch the errors
            catch (Exception ex)
            {
                //We display a messagebox and show the error in the messagebox. 
                Log.CreateLog(ex + " ");
                MessageBox.Show("Noget gik galt \n" + ex);
            }
        }
    }
}

