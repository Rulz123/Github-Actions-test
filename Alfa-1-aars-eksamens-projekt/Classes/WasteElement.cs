using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    //Made by Natasha
    public class WasteElement
    {
        //get and set is used to automatically get the variable you want, and then set it where it is called. 
        //We setup a getter and setter for the waste element ID.
        public int id { get; set; }
        //We setup a getter and setter for the amount of the waste element.
        public decimal amount { get; set; }
        //We create a getter and setter for the measurement unit of the waste element.
        public int measurementUnit { get; set; }
        //We create a getter and setter for the measurement unit name of the waste element.
        public string measurementUnitName { get; set; }
        //We create a getter and setter for the category ID of the waste element.
        public int categoryID { get; set; }
        //We create a getter and setter for the category name of the waste element.
        public string category { get; set; }
        //We create a getter and setter for the description of a waste element.
        public string description { get; set; }
        //We create a getter and setter for the responsible employee names that is logging or logged the waste element.
        public string responsibleEmployee { get; set; }
        //We create a getter and setter for the company ID that is loggine the waste element. 
        public int companyID { get; set; }
        //We create a getter and setter DateTime to set the date of the waste element.
        public DateTime dateTime { get; set; }
        //We create a getter and setter for the string formatted DateTime of the waste element.        
        public string dateTimePlainText { get; set; }
        public int responsibleEmployeeID { get; set; }

        //Done by Natasha
        //We create a method to pull out a waste element list
        public static List<WasteElement> GetWasteElements(int c_id, DateTime dateStart, DateTime dateEnd)
        {
            //We create a variable to make new lists with.
            List<WasteElement> wasteElements = new List<WasteElement>();
            //We setup a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
            //We try to open the connection to the database.
            try
            {
                //We try and establish database connection.
                cnn.Open();

                //We setup a adapter for the sql queries to Sql
                SqlDataAdapter adapter = new SqlDataAdapter();

                ////We create a query to pull out destinct information related to the waste element, like the waste element itself, the waste category, the company id etc. and use the helper tables to make the process more precise.
                //string sql = "SELECT DISTINCT WE., WC., MU., C.C_ID, RE.RE_Name, WasteElementMeasurementUnit., WasteElementCategory., WasteElementResponsibleEmployee., CompanyWasteElement.* FROM WasteElement WE INNER JOIN WasteCategory WC ON WasteElementCategory.WEC_WE_ID = WE.WE_ID AND WC.WC_ID = WasteElementCategory.WEC_WC_ID INNER JOIN MeasurementUnit MU ON WasteElementMeasurementUnit.WEMU_WE_ID = WE.WE_ID AND WasteElementMeasurementUnit.WEMU_MU_ID = MU.MU_ID INNER JOIN ResponsibleEmployee RE ON WasteElementResponsibleEmployee.WERE_WE_ID = WE.WE_ID AND WasteElementResponsibleEmployee.WERE_RE_ID = RE.RE_ID INNER JOIN Company C ON CompanyWasteElement.CWE_WE_ID = WE.WE_ID AND C.C_ID = CompanyWasteElement.CWE_C_ID WHERE WasteElement.WE_DateTime >= '" + dateStart.ToString("yyyy-MM-dd HH:mm:ss") + "' AND WasteElement.WE_DateTime <= '" + dateEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Company.C_ID = " + c_id;

                List<WasteElement> allWasteElements = new List<WasteElement>();
                //We create a query to pull out destinct information related to the waste element, like the waste element itself, the waste category, the company id etc. and use the helper tables to make the process more precise.
                string sql = "SELECT * FROM WasteElement WHERE WasteElement.WE_DateTime >= '" + dateStart.ToString("yyyy-MM-dd HH:mm:ss") + "' AND WasteElement.WE_DateTime <= '" + dateEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";

                //We use the sql query
                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    //We execute the command, and send it to the database to run.
                    var dataReader = command.ExecuteReader();

                    // We set up a while loop, for while the dataReader is reading the output.
                    while (dataReader.Read())
                    {
                        //We add the information to the wasteElements list, while the datareader is reading the output.
                        wasteElements.Add(new WasteElement()
                        {
                            //We convert the different outputs, to the wanted variables, so the getters and setters can do work.
                            id = Convert.ToInt32(dataReader.GetValue(0)),
                            amount = Convert.ToDecimal(dataReader.GetValue(1)),
                            description = Convert.ToString(dataReader.GetValue(2)),
                            dateTime = dataReader.GetDateTime(3),
                            dateTimePlainText = Convert.ToDateTime(dataReader.GetDateTime(3)).ToString("yyyy:MM:dd HH:mm")
                        });
                    }

                    dataReader.Close();
                    // We delete the command and close the connection
                    command.Dispose();
                }

                foreach (var item in wasteElements)
                {
                    // We grab all companyIDs for each waste element
                    string sql1 = "SELECT * FROM CompanyWasteElement WHERE CWE_WE_ID = " + item.id;

                    cnn.Close();
                    cnn.Open();

                    using (SqlCommand command = new SqlCommand(sql1, cnn))
                    {
                        //We execute the command, and send it to the database to run.
                        var dataReader = command.ExecuteReader();

                        // We set up a while loop, for while the dataReader is reading the output.
                        while (dataReader.Read())
                        {
                            //We add the information to the wasteElements list, while the datareader is reading the output.
                            item.companyID = Convert.ToInt32(dataReader.GetValue(2));
                        }

                        dataReader.Close();
                        // We delete the command and close the connection
                        command.Dispose();
                    }

                    // If the companyID matches the logged in companies ID
                    if (MainWindow.currentCompanyID == item.companyID)
                    {
                        // We get the waste category ID
                        string sql2 = "SELECT * FROM WasteElementCategory WHERE WEC_WE_ID = " + item.id;

                        cnn.Close();
                        cnn.Open();

                        using (SqlCommand command = new SqlCommand(sql2, cnn))
                        {
                            //We execute the command, and send it to the database to run.
                            var dataReader = command.ExecuteReader();

                            // We set up a while loop, for while the dataReader is reading the output.
                            while (dataReader.Read())
                            {
                                //We add the information to the wasteElements list, while the datareader is reading the output.
                                item.categoryID = Convert.ToInt32(dataReader.GetValue(2));
                            }

                            dataReader.Close();
                            // We delete the command and close the connection
                            command.Dispose();
                        }

                        cnn.Close();
                        cnn.Open();

                        // We get the measurement unit ID
                        string sql3 = "SELECT * FROM WasteElementMeasurementUnit WHERE WEMU_WE_ID = " + item.id;
                        using (SqlCommand command = new SqlCommand(sql3, cnn))
                        {
                            //We execute the command, and send it to the database to run.
                            var dataReader = command.ExecuteReader();

                            // We set up a while loop, for while the dataReader is reading the output.
                            while (dataReader.Read())
                            {
                                //We add the information to the wasteElements list, while the datareader is reading the output.
                                item.measurementUnit = Convert.ToInt32(dataReader.GetValue(2));
                            }

                            dataReader.Close();
                            // We delete the command and close the connection
                            command.Dispose();
                        }

                        cnn.Close();
                        cnn.Open();

                        // We get the responsible employee ID
                        string sql4 = "SELECT * FROM WasteElementResponsibleEmployee WHERE WERE_WE_ID = " + item.id;
                        using (SqlCommand command = new SqlCommand(sql4, cnn))
                        {
                            //We execute the command, and send it to the database to run.
                            var dataReader = command.ExecuteReader();

                            // We set up a while loop, for while the dataReader is reading the output.
                            while (dataReader.Read())
                            {
                                //We add the information to the wasteElements list, while the datareader is reading the output.
                                item.responsibleEmployeeID = Convert.ToInt32(dataReader.GetValue(2));
                            }

                            dataReader.Close();
                            // We delete the command and close the connection
                            command.Dispose();
                        }

                        // We get the category name
                        string sql5 = "SELECT * FROM WasteCategory WHERE WC_ID = " + item.categoryID;

                        cnn.Close();
                        cnn.Open();

                        using (SqlCommand command = new SqlCommand(sql5, cnn))
                        {
                            //We execute the command, and send it to the database to run.
                            var dataReader = command.ExecuteReader();

                            // We set up a while loop, for while the dataReader is reading the output.
                            while (dataReader.Read())
                            {
                                //We add the information to the wasteElements list, while the datareader is reading the output.
                                item.category = Convert.ToString(dataReader.GetValue(1));
                            }

                            dataReader.Close();
                            // We delete the command and close the connection
                            command.Dispose();
                        }

                        // We get the responsible employee name
                        string sql6 = "SELECT * FROM ResponsibleEmployee WHERE RE_ID = " + item.responsibleEmployeeID;

                        cnn.Close();
                        cnn.Open();

                        using (SqlCommand command = new SqlCommand(sql6, cnn))
                        {
                            //We execute the command, and send it to the database to run.
                            var dataReader = command.ExecuteReader();

                            // We set up a while loop, for while the dataReader is reading the output.
                            while (dataReader.Read())
                            {
                                //We add the information to the wasteElements list, while the datareader is reading the output.
                                item.responsibleEmployee = Convert.ToString(dataReader.GetValue(1));
                            }

                            dataReader.Close();
                            // We delete the command and close the connection
                            command.Dispose();
                        }

                        // We get the measurement unit name
                        string sql7 = "SELECT * FROM MeasurementUnit WHERE MU_ID = " + item.measurementUnit;

                        cnn.Close();
                        cnn.Open();

                        using (SqlCommand command = new SqlCommand(sql7, cnn))
                        {
                            //We execute the command, and send it to the database to run.
                            var dataReader = command.ExecuteReader();

                            // We set up a while loop, for while the dataReader is reading the output.
                            while (dataReader.Read())
                            {
                                //We add the information to the wasteElements list, while the datareader is reading the output.
                                item.measurementUnitName = Convert.ToString(dataReader.GetValue(1));
                            }

                            dataReader.Close();
                            // We delete the command and close the connection
                            command.Dispose();
                        }

                        allWasteElements.Add(item);
                    }

                    cnn.Close();                
                }
                return allWasteElements;
            }
            //We setup a catch to catch error(s).
            catch (Exception e)
            {
                //We create a empty list in case of error(s).
                List<WasteElement> emptyList = new List<WasteElement>();
                Log.CreateLog(e + " ");
                //We display a messagebox that tells the user a error occured, and shows the error.
                MessageBox.Show("Database forbindelsen kunne ikke oprettes - Waste Element - GetWasteElements.\n\nSystem fejlbesked:\n" + e.Message);
                //We return the empty list.
                return emptyList;
            }
            //We create a final statement.
            finally
            {
                //We check if the connection to the database is closed or open, and if open we close the connection.
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }
        }

        //Done by Natasha
        //We create a method to create a waste element. 
        public static void CreateWasteElement(decimal amount, string description, DateTime dateTime, string dropdownValueRE, int dropdownValueMU, int dropdownValueC)
        {
            //We make a variable to make a new list of responsible employees, and calls the GetResponsibleEmployees through the ResponsibleEmployee class, and use the company ID that is logged in on the MainWindow.
            List<ResponsibleEmployee> responsibleEmployees = ResponsibleEmployee.GetResponsibleEmployees(MainWindow.currentCompanyID);
            //We set the first wasteElementID to be 0.
            int wasteElementID = 0;
            //We create a variable to look through the last made responsible employee in the responsibleEmployees list. 
            //We then check if the name matches the responsible employee from the dropdown menu of the combobox.
            ResponsibleEmployee matchingEmployee = responsibleEmployees.FindLast(
           delegate (ResponsibleEmployee emp)
            {
                //We check the selected employee from the dropbox.
                string nameMatch = dropdownValueRE;
                //We return the employees name.
                return nameMatch == emp.name;
            });

            //We setup a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);

            //We create a sql query to insert amount, description and datetime into the database in the WasteElement table with dateTime, and we bind the data to the variables.
            string sqlInsertWE = "INSERT INTO WasteElement (WE_Amount, WE_Description, WE_DateTime) VALUES (" + amount + ", '" + description + "', '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            //We setup a sql query to pull out 1 of the WasteElements, and it is the newest one.
            string sqlSelectWE = "SELECT TOP 1 * FROM WasteElement ORDER BY WE_ID DESC";


            //We try and open a connection and run our functions.
            try
            {
                //We open a connection to the database.
                cnn.Open();

                //We use an adapter for sql queries to Sql.
                SqlDataAdapter adapter = new SqlDataAdapter();

                //We use the sql query to insert a waste element to the database. 
                using (SqlCommand Sqlcommand = new SqlCommand(sqlInsertWE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand.ExecuteReader();
                    //We delete the command.
                    Sqlcommand.Dispose();
                    //We close the connection to the database.
                    cnn.Close();

                }

                //We open a connection to the database.
                cnn.Open();

                //We use the sql query to get the newest waste element.
                using (SqlCommand Sqlcommand2 = new SqlCommand(sqlSelectWE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    var dataReader = Sqlcommand2.ExecuteReader();
                    //We create a while loop for while it reads the output.
                    while (dataReader.Read())
                    {
                        //We convert the waste element ID to an int.
                        wasteElementID = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    //We delete the command.
                    Sqlcommand2.Dispose();
                    //We close the connection to the database.
                    cnn.Close();
                }
                //We setup a dropdown menu value via the category ID, and +1 because the database starts from 1 and not 0 like C#.
                int dropdownValueCUpdated = dropdownValueC + 1;
                //We setup a dropdown menu value via the measurement unit ID, and +1 because the database starts from 1 and not 0 like C#.
                int dropdownValueMUUpdated = dropdownValueMU + 1;
                // We create SQL queries to join the waste element ID to the helper tables, to make the searches in the database more precise.
                string sqlInsertWERE = "INSERT INTO WasteElementResponsibleEmployee (WERE_WE_ID, WERE_RE_ID) VALUES (" + wasteElementID + ", " + matchingEmployee.id + ")";
                string sqlInsertWEMU = "INSERT INTO WasteElementMeasurementUnit (WEMU_WE_ID, WEMU_MU_ID) VALUES (" + wasteElementID + ", " + dropdownValueMUUpdated + ")";
                string sqlInsertWEC = "INSERT INTO WasteElementCategory (WEC_WE_ID, WEC_WC_ID) VALUES (" + wasteElementID + ", " + dropdownValueCUpdated + ")";
                string sqlInsertCWE = "INSERT INTO CompanyWasteElement (CWE_WE_ID, CWE_C_ID) VALUES (" + wasteElementID + ", " + MainWindow.currentCompanyID + ")";
                //We open a connection to the database.
                cnn.Open();


                //We use the sql query to insert ID's into the WasteElementREsponsibleEmployee helper table in the database.
                using (SqlCommand Sqlcommand3 = new SqlCommand(sqlInsertWERE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand3.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand3.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open the connection to the database.
                cnn.Open();



                //We use the sql query to insert ID's into the WasteElementMEasurementUnit helper table.              
                using (SqlCommand Sqlcommand4 = new SqlCommand(sqlInsertWEMU, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand4.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand4.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open the connection to the database.
                cnn.Open();



                //We use the sql query to insert ID's into the WasteElementCategory helper table.
                using (SqlCommand Sqlcommand5 = new SqlCommand(sqlInsertWEC, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand5.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand5.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open the connection to the database.
                cnn.Open();


                //Use sql3 query to insert into the CompanyWasteElement helper table.
                using (SqlCommand Sqlcommand6 = new SqlCommand(sqlInsertCWE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand6.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand6.Dispose();
                    //We close the connection.
                    cnn.Close();

                    Log.CreateLog("Waste Element with ID " + wasteElementID + " created ");

                    //We show a messagebox that says it succeeded. 
                    MessageBox.Show("Det lykkedes!");
                }

            }
            //We setup a catch to catch error(s).
            catch (Exception ex)
            {
                //Disconnects in case of error.
                cnn.Close();
                Log.CreateLog(ex + " ");
                //We make the program show the error.
                throw ex;
            }

        }
        //Done by Natasha
        //We create a method to edit waste elements.
        public static void EditWasteElement(int wasteElementID, int dropdownValueMU, int dropdownValueC, int dropdownValueRE, decimal amount, string description)
        {
            //We setup a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
            //We create a try loop to try and run the functions.
            try
            {
                //We create SQL commands for updates to waste element and the helper tables, and bind the variables that can change.
                string sqlUpdateWERE = "UPDATE WasteElementResponsibleEmployee SET WERE_RE_ID = " + dropdownValueRE + " WHERE WERE_WE_ID = " + wasteElementID;
                string sqlUpdateWEMU = "UPDATE WasteElementMeasurementUnit SET WEMU_MU_ID = " + dropdownValueMU + " WHERE WEMU_WE_ID = " + wasteElementID;
                string sqlUpdateWEC = "UPDATE WasteElementCategory SET WEC_WC_ID = " + dropdownValueC + " WHERE WEC_WE_ID = " + wasteElementID;
                string sqlUpdateCWE = "UPDATE CompanyWasteElement SET CWE_C_ID = " + MainWindow.currentCompanyID + " WHERE CWE_WE_ID = " + wasteElementID;
                string sqlUpdateWE = "UPDATE WasteElement SET WE_Amount = " + amount + ", WE_Description = '" + description + "' WHERE WE_ID = " + wasteElementID;

                //We open a connection to the database.
                cnn.Open();


                //We use the sql command to update WasteElementResponsibleEmployee table
                using (SqlCommand Sqlcommand3 = new SqlCommand(sqlUpdateWERE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand3.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand3.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open a connection to the database.
                cnn.Open();



                //We use the sql command to update WasteElementMeasurementUnit table
                using (SqlCommand Sqlcommand4 = new SqlCommand(sqlUpdateWEMU, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand4.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand4.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open a connection to the database.
                cnn.Open();



                //We use the sql command to update WasteElementCatergory table
                using (SqlCommand Sqlcommand5 = new SqlCommand(sqlUpdateWEC, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand5.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand5.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open a connection to the database.
                cnn.Open();


                //We use the sql command to update CompanyWasteElement table
                using (SqlCommand Sqlcommand6 = new SqlCommand(sqlUpdateCWE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand6.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand6.Dispose();
                    //We close the connection.
                    cnn.Close();
                }
                //We open a connection to the database.
                cnn.Open();

                //We use the sql command to update WasteElement table
                using (SqlCommand Sqlcommand7 = new SqlCommand(sqlUpdateWE, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand7.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand7.Dispose();
                    //We close the connection.
                    cnn.Close();
                    MessageBox.Show("Det lykkedes!");
                    Log.CreateLog("Waste Element with ID " + wasteElementID + " edited ");
                }

            }
            //We setup a catch to catch the error(s)
            catch (Exception ex)
            {
                //Disconnects in case of error
                cnn.Close();
                Log.CreateLog(ex + " ");
                //We make the program give us the error.
                throw ex;
            }
        }

        //Done by Natasha
        public static void DeleteWasteElement(int WE_id)
        {
            //We setup a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);

            //Delete query to database to delete from WasteElement table.
            string sql = "DELETE FROM WasteElement WHERE WE_ID=" + WE_id;

            try
            {
                //We open a connection to the database.
                cnn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();

                //Using delete query to delete WasteElement.
                using (SqlCommand Sqlcommand = new SqlCommand(sql, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand.Dispose();
                    //We close the connection.
                    cnn.Close();


                }

                //Delete query to delete from CompanyWasteElement helper tablet.
                string sql2 = "DELETE FROM CompanyWasteElement WHERE CWE_WE_ID=" + WE_id;

                //We open a connection to the database.
                cnn.Open();

                //Using delete query to delete from CompanyWasteElement.
                using (SqlCommand Sqlcommand = new SqlCommand(sql2, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand.Dispose();
                    //We close the connection.
                    cnn.Close();


                }

                //Delete query to delete from WasteElementCategory  helper tablet.
                string sql3 = "DELETE FROM WasteElementCategory WHERE WEC_WE_ID=" + WE_id;

                //We open a connection to the database.
                cnn.Open();

                //Using delete query to delete from WasteElementCategory.
                using (SqlCommand Sqlcommand = new SqlCommand(sql3, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand.Dispose();
                    //We close the connection.
                    cnn.Close();


                }

                //Delete query to delete from WasteElementMeasurementUnit helper tablet.
                string sql4 = "DELETE FROM WasteElementMeasurementUnit WHERE WEMU_WE_ID=" + WE_id;

                //We open a connection to the database.
                cnn.Open();

                //Using delete query to delete from WasteElementMeasurementUnit table.
                using (SqlCommand Sqlcommand = new SqlCommand(sql4, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand.Dispose();
                    //We close the connection.
                    cnn.Close();


                }

                //Delete query to delete from WasteElementREsponsibleEmployee helper tablet.
                string sql5 = "DELETE FROM WasteElementResponsibleEmployee WHERE WERE_WE_ID=" + WE_id;

                //We open a connection to the database.
                cnn.Open();

                //Using delete query to delete WasteElementREsponsibleEmployee table.
                using (SqlCommand Sqlcommand = new SqlCommand(sql5, cnn))
                {
                    //We execute the sql command and send it to the database to read.
                    Sqlcommand.ExecuteReader();
                    //We delete the command after it has run.
                    Sqlcommand.Dispose();
                    //We close the connection.
                    cnn.Close();
                }

                Log.CreateLog("Waste Element with ID " + WE_id + " deleted ");
            }
            //We setup a catch to catch the error(s)
            catch (Exception ex)
            {
                //We close the connection.
                cnn.Close();
                Log.CreateLog(ex + " ");
                //We make the program show teh error.
                throw ex;
            }
        }

    }
}
