
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
    public class MeasurementUnit
    {
        // We define MeasurementUnit's expected variables
        public int id { get; set; }
        public string name { get; set; }

        // A method for getting all measurement units from the database
        public static List<MeasurementUnit> GetMeasurementUnits()
        {
            // We initialize a list of measurement units
            List<MeasurementUnit> measurementUnits = new List<MeasurementUnit>();

            // We instantiate the connection to the database
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);

            try
            {
                // We open the connection
                cnn.Open();

                // We instantiate an adapter
                SqlDataAdapter adapter = new SqlDataAdapter();

                // We define the SQL command, that selects all rows from the MeasurementUnit table
                string sql = "SELECT * FROM MeasurementUnit";

                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    // We instantiate a reader that executes the SQL command
                    var dataReader = command.ExecuteReader();

                    // While the reader is reading, we grab each of the table rows one by one
                    while (dataReader.Read())
                    {
                        // We create a new MeasurementUnit object, and set the ID and Name to that of the database row
                        measurementUnits.Add(new MeasurementUnit()
                        {
                            id = Convert.ToInt32(dataReader.GetValue(0)),
                            name = Convert.ToString(dataReader.GetValue(1))
                        });
                    }

                    // We delete the command and close the connection
                    command.Dispose();
                    cnn.Close();

                    // We return the list of measurement units
                    return measurementUnits;
                }
            }
            catch (Exception e)
            {
                // We instantiate an empty MeasurementUnit list
                List<MeasurementUnit> emptyList = new List<MeasurementUnit>();
                cnn.Close();
                Log.CreateLog(e + " ");
                // We display an error message to the user
                MessageBox.Show("Database forbindelsen kunne ikke oprettes Measurement Unit.\n\nSystem fejlbesked:\n" + e.Message);

                // We return the empty list, as the method expects a list returned
                return emptyList;
            }
            finally
            {
                // We close the connection if it is for some reason still open
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }
        }
    }
}
