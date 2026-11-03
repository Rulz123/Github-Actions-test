
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    public class Statistic
    {
        public static List<decimal> GetGraphDBData(int c_id, DateTime dateStart, DateTime dateEnd, int dropdownValueWC, int dropdownValueMU)
        {

            // We initialize the lists containing Waste Elements, the first list is used for storing the
            // Waste Elements that are within the selected date range and belong to the logged in company
            List<WasteElement> allWasteElements = WasteElement.GetWasteElements(c_id, dateStart, dateEnd);
            List<WasteElement> graphData = new List<WasteElement>();

            // We add each item from the allWasteElements list to the graphData list,
            // if they match the category and measurement unit
            foreach (var item in allWasteElements)
            {
                if (item.categoryID == dropdownValueWC && item.measurementUnit == dropdownValueMU)
                {
                    graphData.Add(item);
                }                
            }

            // We make a list with decimal values, for the amounts for each day displayed
            List<decimal> perDay = new List<decimal>();

            // We add the amounts for each day in the calendar of the items in graphData
            foreach (DateTime day in EachDay(dateStart, dateEnd))
            {
                perDay.Add(graphData.Where(item => item.dateTime.Date == day).Sum(item => item.amount));
            }

            // We return the list of decimal amounts per day
            return perDay;
        }

        // Method for cycling through a series of dates
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            // We go through each date in the date range and add a new DateTime to the day IEnumerable called day
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                // We return the IEnumerable list day
                yield return day;
        }
    }
}
