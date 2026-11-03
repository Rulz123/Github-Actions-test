using Alfa_1_aars_eksamens_projekt.Views.Pages;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

//Lavet af Nichlas

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    public class ResponsibleEmployee
    {
        public string name { get; set; }

        public int id { get; set; }

        public int companyID { get; set; }


        public static List<ResponsibleEmployee> GetResponsibleEmployees(int companyID)
        {
            //We setup a call for a list to insert responsible employees into. 
            List<ResponsibleEmployee> responsibleEmployees = new List<ResponsibleEmployee>();

            //Create connection to the database using Sql
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
           
            //Try to open connection
            try
            {
                

                //Use SqlDataAdapter to read and write what gets put into the database. 
                SqlDataAdapter adapter = new SqlDataAdapter();

                // sql query to look for responsible employees that has logged in company ID in the database.
                string sql = "SELECT * FROM CompanyResponsibleEmployee WHERE CRE_C_ID = " + companyID;
                List<int> responsibleEmployeeIDs = new List<int>();

                cnn.Open();

                //Use the adapter to issue commands for the database. 
                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    //set dataReader as variable to call a execute on the sql command.
                    var dataReader = command.ExecuteReader();

                    // While loot for datareader to find responsible employees to add to list.
                    while (dataReader.Read())
                    {
                        //Add the responsible employees to a list, with the Employee ID and their name. 
                        responsibleEmployeeIDs.Add(Convert.ToInt32(dataReader.GetValue(2)));
                    }

                    // We delete the command and close the connection
                    command.Dispose();
                    cnn.Close();
                }

                foreach (int item in responsibleEmployeeIDs)
                {
                    cnn.Open();
                    string sql1 = "SELECT * FROM ResponsibleEmployee WHERE RE_ID = " + item;

                    //Use the adapter to issue commands for the database. 
                    using (SqlCommand command = new SqlCommand(sql1, cnn))
                    {
                        //set dataReader as variable to call a execute on the sql command.
                        var dataReader = command.ExecuteReader();

                        // While loot for datareader to find responsible employees to add to list.
                        while (dataReader.Read())
                        {
                            //Add the responsible employees to a list, with the Employee ID and their name. 
                            responsibleEmployees.Add(new ResponsibleEmployee()
                            {
                                id = Convert.ToInt32(dataReader.GetValue(0)),
                                name = Convert.ToString(dataReader.GetValue(1))
                            });
                        }

                        // We delete the command and close the connection
                        command.Dispose();
                        cnn.Close();
                    }
                }

                //We return the responsible employees list so we can display it.
                return responsibleEmployees;
            }

            //We catch erros with this catch loop.
            catch (Exception e)
            {
                //We create a empty list in case of errors.
                List<ResponsibleEmployee> emptyList = new List<ResponsibleEmployee>();
                cnn.Close();
                Log.CreateLog(e + " ");
                //We have a messagebox showing us the error(s) that occured. 
                MessageBox.Show("Database forbindelsen kunne ikke oprettes - Responsible Employee.\n\nSystem fejlbesked:\n" + e.Message);

                //We return the empty list.
                return emptyList;
            }
            finally
            {
                //We check to see that the connection to the database is closed, and if not we close it. 
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }
        }

        //Method to create the responsible employees
        public static void CreateResponsibleEmployees(string name, int c_id)
        {
            //Sets up a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);

            //The sql query's to the database to insert and select responsible employees. 

            string sql = "INSERT INTO ResponsibleEmployee (RE_Name) VALUES ('" + name + "')";
            string sql2 = "SELECT TOP 1 * FROM ResponsibleEmployee ORDER BY RE_ID DESC";

            //We try to open a connection to the database.
            try
            {
                cnn.Open();

                //We make a call name for the adapter. 
                SqlDataAdapter adapter = new SqlDataAdapter();

                //Use the sql query to create the responsible employee in the database. 
                using (SqlCommand Sqlcommand = new SqlCommand(sql, cnn))
                {
                    //We use the query command, and call an execute for the query to insert a responsible employee to the database.
                    Sqlcommand.ExecuteReader();
                    //We dispose of the query command.
                    Sqlcommand.Dispose();
                    //We close connection to the database.
                    cnn.Close();

                }

                //We set a start int for the responsible employee ID to start from.
                int reid = 0;

                cnn.Open();

                //We setup the second SQL query to select responsible employees from the database with the newest as the top. 
                using (SqlCommand Sqlcommand2 = new SqlCommand(sql2, cnn))
                {
                    // We set a parameter for the sql query as a variable.
                    var dataReader = Sqlcommand2.ExecuteReader();
                    //We make the datareader read in the database to find the responsible employee.
                    while (dataReader.Read())
                    {
                        //We convert the responsible employee ID from the database to an int. 
                        reid = Convert.ToInt32(dataReader.GetValue(0));
                               

                    }
                    //We dispose of the SQL command.
                    Sqlcommand2.Dispose();
                    //We close connection to the database.
                    cnn.Close();
                }

                cnn.Open();
                
                //SQL query to insert ID's into database helpers to key together responsible ID and company ID.
                string sql3 = "INSERT INTO CompanyResponsibleEmployee (CRE_C_ID, CRE_RE_ID) VALUES (" + c_id + ", " + reid + ")";

                //Use sql3 query to insert into the company responsible employee helper table.
                using (SqlCommand Sqlcommand3 = new SqlCommand(sql3, cnn))
                {
                    //We exectue the reader to send SQL query to the database.
                    Sqlcommand3.ExecuteReader();
                    //We dispose the command after it's run.
                    Sqlcommand3.Dispose();
                    //We close connection to the database.
                    cnn.Close();
                    //We display a messagebox to show that we succeeded with the wanted query.
                    MessageBox.Show("Det lykkedes!");
                }

                Log.CreateLog("Responsible employee named " + name + " for company " + MainWindow.currentCompanyName + " created ");
            }
            //We catch errors that occured in the loops.
            catch (Exception ex)
            {
                //Disconnects in case of error
                cnn.Close();
                Log.CreateLog(ex + " ");
                //We make the program display the error.
                throw ex;
            }

        }

        //Method to edit the responsible employees.
        public static void EditResponsibleEmployee(string name)
        {
            //We setup the connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
            
            
            //SQL query to update name in responsible employee. 
            string sql = "UPDATE ResponsibleEmployee SET RE_Name='" + name + "' WHERE RE_ID = " + CompanyProfile.responsibleEmployeeEditID;

            //We try and open a connection.
            try
            {
                //We open a connection to the database.
                cnn.Open();

                //We call  the adapter to insert data into the database.
                SqlDataAdapter adapter = new SqlDataAdapter();

                //Using the update query to update name of responsible employee.
                using (SqlCommand Sqlcommand = new SqlCommand(sql, cnn))
                {
                    //We call the reader to use the SQL query to send data to the database.
                    Sqlcommand.ExecuteReader();
                    //We dispose of the finished SQL query.
                    Sqlcommand.Dispose();
                    //We close the connection to the database.
                    cnn.Close();

                    Log.CreateLog("Responsible employee with ID " + CompanyProfile.responsibleEmployeeEditID + " for company " + MainWindow.currentCompanyName + " edited ");

                    //We display a messagebox to show that we succeeded to update the name.
                    MessageBox.Show("Det lykkedes!");
                }
            }
            //We catch errors in the loops.
            catch(Exception ex)
            {
                //We close the connection to the database in case of errors.
                cnn.Close();
                Log.CreateLog(ex + " ");
                //We make the program display errors.
                throw ex;               
            }
        }

        //Method for deleting responsible employees. 
        public static void DeleteResponsibleEmployee(int RE_id)
        {
            //We setup a connection to the database. 
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);

            //SQL query to delete responsible employee from database, using the responsible employees ID.
            string sql = "DELETE FROM ResponsibleEmployee WHERE RE_ID=" + RE_id;

            //We try to open a connection to the database.
            try
            {
                //Open connection to the database.
                cnn.Open();

                //We call  the adapter to insert data into the database.
                SqlDataAdapter adapter = new SqlDataAdapter();

                //Using delete query to delete ResponsibleEmployee from database.
                using (SqlCommand Sqlcommand = new SqlCommand(sql, cnn))
                {
                    //We execute the reader to call the query in the database.
                    Sqlcommand.ExecuteReader();
                    //We dispose of the query after it has run.
                    Sqlcommand.Dispose();
                    //We close the connection to the database.
                    cnn.Close();
                }

                //Delete query to delete company responsible employee from company responsible employee helper tables using responsible employee ID.
                string sql2 = "DELETE FROM CompanyResponsibleEmployee WHERE CRE_RE_ID=" + RE_id;

                //We open connection to the database.
                cnn.Open();

                //Using delete query to delete company responsible employee from helper tables.
                using (SqlCommand Sqlcommand = new SqlCommand(sql2, cnn))
                {
                    //We execute reader to send the SQL query to the database and run query.
                    Sqlcommand.ExecuteReader();
                    //We dispose of the query after it has run.
                    Sqlcommand.Dispose();
                    //We close connection to the database.
                    cnn.Close();

                    Log.CreateLog("Responsible employee with ID " + RE_id + " deleted ");
                }
            }

            //We catch errors in the loops.
            catch (Exception ex)
            {
                //We close connection to the database in case of errors.
                cnn.Close();
                Log.CreateLog(ex + " ");
                //We make the program display error. 
                throw ex;
            }
        }


    }
}

