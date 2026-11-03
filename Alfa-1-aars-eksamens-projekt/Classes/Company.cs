using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    //Gamze
    public class Company
    {

        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int C_id { get; set; }

        //GetCompanies method is so that we can get registered companies out of the company List. 
        public static List<Company> GetCompanies()
        {
            //We make a List of type Company. 
            List<Company> company = new List<Company>();

            //Sql connection string, to get connection to the database. 
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
            try
            {
                cnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                string sql = "SELECT * FROM Company";

                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    //Here we read the data in the Company table 
                    var dataReader = command.ExecuteReader();

                    //While it reads the data, execute the code in the while loop. 
                    while (dataReader.Read())
                    {

                        //We add the company to the company List.
                        company.Add(new Company()
                        {
                            C_id = Convert.ToInt32(dataReader.GetValue(0)),
                            name = Convert.ToString(dataReader.GetValue(1)),
                            username = Convert.ToString(dataReader.GetValue(2)),
                            password = Convert.ToString(dataReader.GetValue(3))
                        });


                    }

                    if (company == null)
                    {
                        MessageBox.Show("Problem med at hente companies");
                    }

                    // We delete the command and close the connection.
                    command.Dispose();
                    cnn.Close();
                    return company;

                }
            }
            catch (Exception e)
            {
                //Error message, in case of an error.
                List<Company> emptyList = new List<Company>();
                cnn.Close();
                Log.CreateLog(e + " ");
                MessageBox.Show("Database forbindelsen kunne ikke oprettes - Company.\n\nSystem fejlbesked:\n" + e.Message);
                return emptyList;
            }
            finally
            {
                //If cnn is not null and cnn connection is open, then close cnn.
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }
        }


        //The insertCompany method is for registration of new employees.
        public void insertCompany(string name, string username, string password)
        {

            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);


            //The sql query inserts the company name, usernsame and password into the Company table. 
            string sql = "INSERT INTO Company (C_Name, C_Username, C_Password) VALUES ('" + name + "', '" + username + "', '" + password + "')";


            try
            {
                cnn.Open();


                SqlDataAdapter adapter = new SqlDataAdapter();


                using (SqlCommand Sqlcommand3 = new SqlCommand(sql, cnn))
                {
                    Sqlcommand3.ExecuteReader();
                    Sqlcommand3.Dispose();
                    cnn.Close();
                }

                Log.CreateLog("Company named " + name + " created ");
            }
            catch (Exception ex)
            {
                //Disconnects in case of error
                cnn.Close();
                Log.CreateLog(ex + " ");

                throw ex;
            }

        }

        //The Login method, does the Login part of the app. 
        public bool Login(string username, string password)
        {
            //We call the company List and set it to GetCompanies()
            List<Company> company = GetCompanies();

            //Here we search for a username in the list that matches what the employee has written as a username
            Company matchingCompany = company.FindLast(
            delegate (Company comp)
            {
                string usernameMatch = username;
                return usernameMatch == comp.username;
            });

            //We check if matchingCompany is empty
            if (matchingCompany != null)
            {
                //If the password matches the input, we set the currentCompany name and ID to match.
                if (matchingCompany.password == password)
                {
                    MainWindow.currentCompanyName = matchingCompany.name;
                    MainWindow.currentCompanyID = matchingCompany.C_id;

                    return true;
                }
                else
                { //Error message if there was not any matches for the password
                    MessageBox.Show("Forkert brugernavn eller kode.");
                    return false;
                }
            }
            else
            {
                // Error if the text boxes are null (empty)
                MessageBox.Show("Forkert brugernavn eller kode.");
                return false;
            }
        }
    }
}











