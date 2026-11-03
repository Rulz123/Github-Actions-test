using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    //Class written by Keemon and Natasha
    public class CSV
    {
        //Done by Natasha
        public static List<WasteElement> ImportCSV(string fileName)
        {
            //We make a WasteElement list.
            List<WasteElement> listCSV = new List<WasteElement>();

            try
            {
                //We read the chosen CSV file.
                string lines = File.ReadAllText(fileName);

                //We make a configuration where we tell it there is quotation marks around each field and each field is
                //separated by a comma and the CSV file has no header row.
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    Quote = '"',
                    HasHeaderRecord = false,
                    Encoding = Encoding.UTF8
                };

                //We instatiate a StreamReader that we use to instantiate the CSV reader with the specified configuration.
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, config))
                {
                    //We tell the CSV reader how to interpret each row as a waste element using a Map class.
                    csv.Context.RegisterClassMap<WasteElementMap>();
                    //We get each row using the GetRecords method as waste elements.
                    //We can use GetRecords becuase we've mapped the waste element class.
                    var rows = csv.GetRecords<WasteElement>();
                    //We insert the rows into our list.
                    listCSV = rows.ToList<WasteElement>();
                }

                //We return the list.
                return listCSV;
            }
            catch (Exception e)
            {
                //In case of an exception we throw an error message
                MessageBox.Show("Filen kunne ikke findes.\n\nSystem fejlbesked:\n" + e.Message);
                return listCSV;
            }
        }

        //Done by Keemon
        public static void ExportCSV(int c_id, DateTime dateStart, DateTime dateEnd)
        {
            //We make a list of WasteElement to contain the data to export, based on the current company ID (the logged in user)
            //as well as the specified time period.
            List<WasteElement> exportedData = WasteElement.GetWasteElements(c_id, dateStart, dateEnd.AddDays(1));

            //We make a configuration to separate each field by comma, and to include quotation around every field.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                ShouldQuote = new ShouldQuote((field) => true),
                Encoding = Encoding.GetEncoding(1252)
            };

            string fileCompanyName = MainWindow.currentCompanyName.Replace(".", "").Replace("/", "").Replace(" ","").Replace("\\","").Replace("!","").Replace("?","");

            //We instatiate a StreamWriter to save the CSV file we export to our Dropzone folder,
            //saving it with the company name as its name and then instatiate a CsvWriter to use the specified configuration and StreamWriter.
            using (var writer = new StreamWriter(@"C:\Dropzone\" + fileCompanyName + ".csv", false, Encoding.GetEncoding(1252)))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                //We then write each item from the exportedData list into the CSV fields in the required order. 
                foreach (var item in exportedData)
                {
                    csvWriter.WriteField(item.id);
                    csvWriter.WriteField(item.amount);
                    csvWriter.WriteField(item.measurementUnit);
                    csvWriter.WriteField(item.categoryID);
                    csvWriter.WriteField(item.description);
                    csvWriter.WriteField(item.responsibleEmployee);
                    csvWriter.WriteField(item.companyID);
                    csvWriter.WriteField(item.dateTimePlainText);
                    csvWriter.NextRecord();
                }
                //We clear the buffer for the writer.
                writer.Flush();
            }

            Log.CreateLog("CSV for " + fileCompanyName + " exported ");
        }
    }
}
