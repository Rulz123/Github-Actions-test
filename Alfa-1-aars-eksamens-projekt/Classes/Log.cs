using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    public class Log
    {
        int id { get; set; }
        string message { get; set; }

        // Made by Natasha
        public static void CreateLog(string message)
        {
            DateTime dateTime = DateTime.Now;

            //Sets up a connection to the database.
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);

            //The sql query's to the database to insert a log
            string sql = "INSERT INTO Logs (L_Name, L_Date) VALUES ('" + message + "by " + GetIP() + "', '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            //We try to open a connection to the database.
            try
            {
                cnn.Open();

                //We make a call name for the adapter. 
                SqlDataAdapter adapter = new SqlDataAdapter();

                //Use the sql query to create the responsible employee in the database. 
                using (SqlCommand Sqlcommand = new SqlCommand(sql, cnn))
                {
                    //We use the query command, and call an execute for the query to insert a log to the database.
                    Sqlcommand.ExecuteReader();
                    //We dispose of the query command.
                    Sqlcommand.Dispose();
                    //We close connection to the database.
                    cnn.Close();
                }
            }
            //We catch errors that occured in the loops.
            catch (Exception ex)
            {
                //Disconnects in case of error
                cnn.Close();
                //We make the program display the error.
                throw ex;
            }
        }

        // Made by Natasha
        public static string GetIP()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");

            foreach (IPAddress hostAddress in hostAddresses)
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(hostAddress) &&  // ignore loopback addresses
                    !hostAddress.ToString().StartsWith("169.254."))  // ignore link-local addresses
                    return hostAddress.ToString();
            }
            return null; // or IPAddress.None if none found
        }
    }
}
