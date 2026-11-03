using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    public class WasteCategory
    {
        //We make the program get the id as a int automatically and set it where we call the GetWasteCategories.
        public int id { get; set; }
        //We also make the program get the name as a string automatically and set it where we call the GetWasteCategories.
        public string name { get; set; }
        //We make a method to get the waste categories out of the database.
        public static List<WasteCategory> GetWasteCategories()
        {
            //We make a variable that we can use to create a list for the items from the database.
            List<WasteCategory> wasteCategories = new List<WasteCategory>();
            //We setup a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
            //We try to open the connection with the try loop.
            try
            {
                //We open the connection to the database.
                cnn.Open();
                //We use a adapter to convert sql over to Sql.
                SqlDataAdapter adapter = new SqlDataAdapter();
                //We create a string for the sql query, to tell the database what to do.
                string sql = "SELECT * FROM WasteCategory";
                //We use the query we made while connected.
                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    //We execute the command by letting the database read and input it.
                    var dataReader = command.ExecuteReader();

                    //We setup a while loop to do something while the command is beeing executed.
                    while (dataReader.Read())
                    {
                        //We make a list to add what we need to.
                        wasteCategories.Add(new WasteCategory()
                        {
                            //We convert the ID from the database for the category to a int.
                            id = Convert.ToInt32(dataReader.GetValue(0)),
                            //We convert the name to a string from the database.
                            name = Convert.ToString(dataReader.GetValue(1))
                        });


                    }

                    // We delete the command and close the connection
                    command.Dispose();
                    cnn.Close();
                    //We return the list.
                    return wasteCategories;

                }
            }
            //We setup a catch to catch error(s).
            catch (Exception e)
            {
                //We create a empty list in case of error(s).
                List<WasteCategory> emptyList = new List<WasteCategory>();
                cnn.Close();
                Log.CreateLog(e + " ");
                //We show a messagebox that tells us there is a error, and what the error is.
                MessageBox.Show("Database forbindelsen kunne ikke oprettes - Waste Category.\n\nSystem fejlbesked:\n" + e.Message);
                //We return the empty list.
                return emptyList;
            }
            //We make a final statement.
            finally
            {
                //We check if the connection to the database i closed, and if not, we close it.
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }
        }
    }

}
