using Alfa_1_aars_eksamens_projekt.Classes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Alfa_1_aars_eksamens_projekt.Views.Pages
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    /// Code for the statistics page primarily written by Keemon.
    public partial class Statistics : Page
    {
        //We define a SeriesCollection to hold our different Line Chart points.
        public SeriesCollection SeriesCollection { get; set; }
        //We define a string array of labels for the Chart, bound to the X-axis.
        public string[] Labels { get; set; }
        //The YFormatter defines the format of the Y-axis, which allow us to display a title and the values for amount of waste as labels displayed on the graph.
        public Func<decimal, string> YFormatter { get; set; }
        //We set an initial start date to one month in the past.
        public DateTime startDate = DateTime.Now.AddMonths(-1);
        //We set an initial end date to the current date.
        public DateTime endDate = DateTime.Now;
        object threadLock = new object();
        public string path = @"C:\Dropzone\";
        public List<string> csvFiles = new List<string>();

        //Done by Keemon
        //In the constructor for the statistics page, we set all default values for our statistics line chart, and get the default data we need loaded.
        public Statistics()
        {
            InitializeComponent();

            //Here we get a list of the different waste categories in order to populate our waste category comboboxes.
            List<WasteCategory> wasteCategories = WasteCategory.GetWasteCategories();
            //Here we get a list of the different measurement units in order to populate our measurement unit combobox.
            List<MeasurementUnit> measurementUnits = MeasurementUnit.GetMeasurementUnits();

            //We set our date time pickers to the initial start and end date.
            startDateLineGraph.SelectedDate = startDate;
            endDateLineGraph.SelectedDate = endDate;

            //We populate our waste category and measurement unit combo boxes.
            foreach (WasteCategory WC in wasteCategories)
                WCLineGraph.Items.Add(WC.name);
            foreach (WasteCategory WC in wasteCategories)
                WCLineGraph_WCCompare.Items.Add(WC.name);
            foreach (MeasurementUnit MU in measurementUnits)
                MULineGraph.Items.Add(MU.name);

            //Default Waste Category loaded when loading the page (First category in Database).
            foreach (string item in WCLineGraph.Items)
                if (item == "Batterier")
                {
                    WCLineGraph.SelectedItem = item;
                    break;
                }

            //Default Waste Category 2 loaded when loading the page (Second category in Database).
            foreach (string item in WCLineGraph_WCCompare.Items)
                if (item == "Biler")
                {
                    WCLineGraph_WCCompare.SelectedItem = item;
                    break;
                }

            //Default Measurement Unit loaded when loading the page (First measurement unit in Database).
            foreach (string item in MULineGraph.Items)
                if (item == "Colli")
                {
                    MULineGraph.SelectedItem = item;
                    break;
                }

            //We get the CSV files that are in our Dropzone folder
            csvFiles = Directory.GetFiles(path, "*.csv", SearchOption.AllDirectories).ToList();
            foreach (var file in csvFiles)
            {
                CSVLineGraph.Items.Add(file);
            }

            //Each of the selected waste categories and measurement unit is saved as variables. + 1 is added to the selected index, as the database values count from 1 instead of 0.
            int selectedMU = MULineGraph.SelectedIndex + 1;
            int selectedWC = WCLineGraph.SelectedIndex + 1;
            int selectedWC2 = WCLineGraph_WCCompare.SelectedIndex + 1;

            //We get the graph data with the default values for the two waste categories to compare.
            //Our current company ID that we are logged in with, the selected waste categories, and the measurement unit,
            //as well as the start and end date are passed to the GetGraphDBData method in order to get the relevant waste data.
            List<decimal> graphData = Statistic.GetGraphDBData(MainWindow.currentCompanyID, startDate, endDate, selectedWC, selectedMU);
            List<decimal> graphData2 = Statistic.GetGraphDBData(MainWindow.currentCompanyID, startDate, endDate, selectedWC2, selectedMU);

            //We make a chart values collection to contain the data of the registered waste amounts.
            ChartValues<decimal> amountOverTime = new ChartValues<decimal>();
            ChartValues<decimal> amountOverTime2 = new ChartValues<decimal>();

            //We add the graph data to the chart values collection.
            foreach (decimal item in graphData)
            {
                amountOverTime.Add(item);
            }
            foreach (decimal item in graphData2)
            {
                amountOverTime2.Add(item);
            }

            //We add two Line Series to our SeriesCollection, one for each of the two selected waste categories, which will be shown on the graph
            //The SeriesCollection gets added to our chart with a binding in the chart xaml code.
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Affaldstype 1 over tid",
                    DataLabels = true,
                    LineSmoothness = 0,
                    Opacity = 0.5,
                    Values = amountOverTime
                },
                new LineSeries
                {
                    Title = "Affaldstype 2 over tid",
                    DataLabels = true,
                    LineSmoothness = 0,
                    Opacity = 0.5,
                    Values = amountOverTime2
                }
            };

            //We set date labels on the graph X axis, by first adding each day between the start and end date to a list of strings.
            List<string> dateLabels = new List<string>();
            foreach (DateTime day in Statistic.EachDay(startDate, endDate))
            {
                dateLabels.Add(day.Date.ToString("yyyy-MM-dd"));
            }
            //Then we add our date labels to the labels string array which is bound to the X-axis in the xaml code for the chart.
            Labels = dateLabels.ToArray();

            //We add the waste amount value as labels for the Y-Axis.
            YFormatter = value => value.ToString();

            //We add a title for the Y-Axis to display the measurement unit that's chosen.
            type.Title = MULineGraph.SelectedItem.ToString();

            //We set the DataContext for our bindings to this page.
            DataContext = this;
        }

        //Done by Keemon
        //Drop down menu for the first waste category to update the chart with specified values when the dropdown menu closes.
        private void WCLineGraph_DropDownClosed(object sender, EventArgs e)
        {
            //We set our date pickers to the selected dates, in order to get waste data for the dates you choose.
            startDate = (DateTime)startDateLineGraph.SelectedDate;
            endDate = (DateTime)endDateLineGraph.SelectedDate;

            //We save the selected waste category and selected measurement unit as variables.
            //+ 1 is added to the selected index, as the database values count from 1 instead of 0.
            int selectedWC = WCLineGraph.SelectedIndex + 1;
            int selectedMU = MULineGraph.SelectedIndex + 1;

            //Here we first check if a CSV has already been imported, if not, we can update the graph line series for the first waste category.
            if (CSVLineGraph.SelectedItem == null)
            {
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
            }
            //If a CSV has already been imported, when updating the waste category,
            //we make sure to update the CSV line series as well to use the same newly selected waste category.
            else if (SeriesCollection.Count > 2)
            {
                //We save the selected CSV file in order to import the new data for the newly selected waste category.
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
        }

        //Done by Keemon
        //Drop down menu for the measurement unit to update the chart with specified values when the dropdown menu closes.
        private void MULineGraph_DropDownClosed(object sender, EventArgs e)
        {
            //We set our date pickers to the selected dates, in order to get waste data for the dates you choose.
            startDate = (DateTime)startDateLineGraph.SelectedDate;
            endDate = (DateTime)endDateLineGraph.SelectedDate;

            //We save both the selected waste categories and the selected measurement unit as variables.
            //+ 1 is added to the selected index, as the database values count from 1 instead of 0.
            int selectedWC = WCLineGraph.SelectedIndex + 1;
            int selectedWC2 = WCLineGraph_WCCompare.SelectedIndex + 1;
            int selectedMU = MULineGraph.SelectedIndex + 1;

            //Here we first check if a CSV has already been imported, if not, we only update the two line series for the first and second waste category
            //with the newly selected measurement unit.
            if (CSVLineGraph.SelectedItem == null)
            {
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
            }
            //If a CSV has already been imported, when updating the measurement unit,
            //we make sure to update the CSV line series as well to use the same newly selected measurement unit.
            else if (SeriesCollection.Count > 2)
            {
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
            else
            {
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
        }

        //Done by Keemon
        //Drop down menu for the second waste category to update the chart with specified values when the dropdown menu closes.
        private void WCLineGraph_WCCompare_DropDownClosed(object sender, EventArgs e)
        {
            //We set our date pickers to the selected dates, in order to get waste data for the dates you choose.
            startDate = (DateTime)startDateLineGraph.SelectedDate;
            endDate = (DateTime)endDateLineGraph.SelectedDate;

            //We save the selected waste category and selected measurement unit as variables.
            //+ 1 is added to the selected index, as the database values count from 1 instead of 0.
            int selectedWC = WCLineGraph_WCCompare.SelectedIndex + 1;
            int selectedMU = MULineGraph.SelectedIndex + 1;

            //We then update the graph with the newly selected second waste category.
            UpdateGraphWC2(startDate, endDate, selectedWC, selectedMU);
        }

        // Done by Natasha
        //Drop down menu to select desired CSV file to import data from, from the dropzone folder.
        private void CSVLineGraph_DropDownClosed(object sender, EventArgs e)
        {
            //We set our date pickers to the selected dates, in order to get waste data for the dates you choose.
            startDate = (DateTime)startDateLineGraph.SelectedDate;
            endDate = (DateTime)endDateLineGraph.SelectedDate;

            //If no CSV file was selected, we ask the user to pick one again.
            if (CSVLineGraph.SelectedItem == null)
            {
                MessageBox.Show("Vælg CSV fil igen");
            }
            else
            {
                //We save the selected CSV, waste category and measurement unit as variables.
                //+ 1 is added to the selected index, as the database values count from 1 instead of 0.
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                int selectedMU = MULineGraph.SelectedIndex + 1;
                int selectedWC = WCLineGraph.SelectedIndex + 1;

                //We update the graph with the imported CSV data, based on the selected date, waste category, measurement unit and CSV file.
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
        }

        //Done by Keemon
        //This method updates the graph line series for the first waste category
        private void UpdateGraphWC(DateTime dateStart, DateTime dateEnd, int selectedWC, int selectedMU)
        {
            //We get the graph data based on our selections and put it in a decimal list.
            List<decimal> graphData = Statistic.GetGraphDBData(MainWindow.currentCompanyID, dateStart, dateEnd, selectedWC, selectedMU);

            //We create a collection of chart values.
            ChartValues<decimal> amountOverTime = new ChartValues<decimal>();

            //We add the graph data to the chart values collection
            foreach (var item in graphData)
            {
                amountOverTime.Add(item);
            }

            //We update the line series for the first waste category to contain the updated graph data
            SeriesCollection[0].Values = amountOverTime;

            //We set date labels on the graph X axis, by first adding each day between the start and end date to a list of strings.
            List<string> dateLabels = new List<string>();
            foreach (DateTime day in Statistic.EachDay(startDate, endDate))
            {
                dateLabels.Add(day.Date.ToString("yyyy-MM-dd"));
            }
            //Then we add our date labels to the labels string array which is bound to the X-axis in the xaml code for the chart.
            Labels = dateLabels.ToArray();

            //We add the waste amount value as labels for the Y-Axis.
            YFormatter = value => value.ToString();

            //We add a title for the Y-Axis to display the measurement unit that's chosen.
            type.Title = MULineGraph.SelectedItem.ToString();

            //We set the DataContext for our bindings to this page.
            DataContext = this;

            //The chart is updated
            lineChart.Update(true);
        }

        //Done by Keemon
        //This method updates the graph line series for the second waste category
        private void UpdateGraphWC2(DateTime dateStart, DateTime dateEnd, int selectedWC, int selectedMU)
        {
            //We get the graph data based on our selections and put it in a decimal list.
            List<decimal> graphData = Statistic.GetGraphDBData(MainWindow.currentCompanyID, dateStart, dateEnd, selectedWC, selectedMU);

            //We create a collection of chart values.
            ChartValues<decimal> amountOverTime = new ChartValues<decimal>();

            //We add the graph data to the chart values collection
            foreach (var item in graphData)
            {
                amountOverTime.Add(item);
            }

            //We update the line series for the second waste category to contain the updated graph data,
            //and make sure that the Line Series is already added, otherwise we add a new line series,
            //in case the second waste category line series wasnt already added to the graph when the page loaded.
            if (SeriesCollection.Count > 1 && selectedWC == WCLineGraph_WCCompare.SelectedIndex + 1)
            {
                SeriesCollection[1].Values = amountOverTime;
            }
            else if (SeriesCollection.Count < 2 && selectedWC == WCLineGraph_WCCompare.SelectedIndex + 1)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = "Affaldstype 2 over tid",
                    LineSmoothness = 0,
                    Opacity = 0.5,
                    Values = amountOverTime
                });
            }
            else
            {
                SeriesCollection[0].Values = amountOverTime;
            }

            //We set date labels on the graph X axis, by first adding each day between the start and end date to a list of strings.
            List<string> dateLabels = new List<string>();
            foreach (DateTime day in Statistic.EachDay(startDate, endDate))
            {
                dateLabels.Add(day.Date.ToString("yyyy-MM-dd"));
            }
            //Then we add our date labels to the labels string array which is bound to the X-axis in the xaml code for the chart.
            Labels = dateLabels.ToArray();

            //We add the waste amount value as labels for the Y-Axis.
            YFormatter = value => value.ToString();

            //We add a title for the Y-Axis to display the measurement unit that's chosen.
            type.Title = MULineGraph.SelectedItem.ToString();

            //We set the DataContext for our bindings to this page.
            DataContext = this;

            //The chart is updated
            lineChart.Update(true);
        }

        //Done by Natasha
        private void UpdateGraphCSV(DateTime dateStart, DateTime dateEnd, int selectedWC, int selectedMU, string selectedCSV)
        {
            //We run our CSV import method passing the selected CSV into it, and adding the waste element data from
            //the CSV file to a WasteElement list.
            List<WasteElement> csvData = CSV.ImportCSV(selectedCSV);

            //We create a collection of chart values.
            ChartValues<decimal> amountOverTime = new ChartValues<decimal>();

            //Then we find the CSV data that matches our chosen waste category and measurement unit,
            //for each day in the selected time period.
            foreach (DateTime day in Statistic.EachDay(dateStart, dateEnd))
            {
                //We create a WasteElement list to contain CSV date matches.
                List<WasteElement> csvDateMatch = new List<WasteElement>();

                //We find all matching CSV waste elements.
                csvDateMatch = csvData.FindAll(
                    delegate (WasteElement we)
                    {
                        //We take the elements dateTimePlainText and parse it as a DateTime object.
                        we.dateTime = DateTime.ParseExact(we.dateTimePlainText, "yyyy:MM:dd HH:mm", CultureInfo.InvariantCulture);
                        //If the waste elements date time, category and measurement unit matches, we return true, else false.
                        if (we.dateTime.Date == day && we.categoryID == selectedWC && we.measurementUnit == selectedMU)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                );

                //If the list of waste elements for the current day in the foreach has any items, we take the sum 
                //of the items amount values and add them to the current day in our chart values collection.
                if (csvDateMatch != null)
                {
                    decimal amountSum = csvDateMatch.Sum(x => Convert.ToDecimal(x.amount));

                    amountOverTime.Add(amountSum);
                }
                else
                {
                    //If there are no waste elements for the current day, we add the decimal amount of 0 to that day.
                    amountOverTime.Add(0);
                }
            }

            //We check if a CSV has already been added and update its Line Series, otherwise we add a new Line Series to the SeriesCollection.
            if (SeriesCollection.Count > 2)
            {
                SeriesCollection[2].Values = amountOverTime;
            }
            else
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = "CSV over tid - Affaldstype 1",
                    DataLabels = true,
                    LineSmoothness = 0,
                    Opacity = 0.5,
                    Values = amountOverTime
                });
            }

            //We set date labels on the graph X axis, by first adding each day between the start and end date to a list of strings.
            List<string> dateLabels = new List<string>();
            foreach (DateTime day in Statistic.EachDay(startDate, endDate))
            {
                dateLabels.Add(day.Date.ToString("yyyy-MM-dd"));
            }
            //Then we add our date labels to the labels string array which is bound to the X-axis in the xaml code for the chart.
            Labels = dateLabels.ToArray();

            //We add the waste amount value as labels for the Y-Axis.
            YFormatter = value => value.ToString();

            //We add a title for the Y-Axis to display the measurement unit that's chosen.
            type.Title = MULineGraph.SelectedItem.ToString();

            //We set the DataContext for our bindings to this page.
            DataContext = this;

            //The chart is updated
            lineChart.Update(true);
        }

        //Done by Keemon
        private void endDateLineGraph_CalendarClosed(object sender, RoutedEventArgs e)
        {
            //We set our date pickers to the selected dates, in order to get waste data for the dates you choose.
            startDate = (DateTime)startDateLineGraph.SelectedDate;
            endDate = (DateTime)endDateLineGraph.SelectedDate;

            //We save both the selected waste categories and the selected measurement unit as variables.
            //+ 1 is added to the selected index, as the database values count from 1 instead of 0.
            int selectedWC = WCLineGraph.SelectedIndex + 1;
            int selectedWC2 = WCLineGraph_WCCompare.SelectedIndex + 1;
            int selectedMU = MULineGraph.SelectedIndex + 1;

            //Here we first check if a CSV has already been imported, if not, we only update the two line series for the first and second waste category
            //with the newly selected measurement unit.
            if (CSVLineGraph.SelectedItem == null)
            {
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
            }
            //If a CSV has already been imported, when updating the measurement unit,
            //we make sure to update the CSV line series as well to use the same newly selected measurement unit.
            else if (SeriesCollection.Count > 2)
            {
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
            else
            {
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
        }

        //Done by Keemon
        private void startDateLineGraph_CalendarClosed(object sender, RoutedEventArgs e)
        {
            //We set our date pickers to the selected dates, in order to get waste data for the dates you choose.
            startDate = (DateTime)startDateLineGraph.SelectedDate;
            endDate = (DateTime)endDateLineGraph.SelectedDate;

            //We save both the selected waste categories and the selected measurement unit as variables.
            //+ 1 is added to the selected index, as the database values count from 1 instead of 0.
            int selectedWC = WCLineGraph.SelectedIndex + 1;
            int selectedWC2 = WCLineGraph_WCCompare.SelectedIndex + 1;
            int selectedMU = MULineGraph.SelectedIndex + 1;

            //Here we first check if a CSV has already been imported, if not, we only update the two line series for the first and second waste category
            //with the newly selected measurement unit.
            if (CSVLineGraph.SelectedItem == null)
            {
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
            }
            //If a CSV has already been imported, when updating the measurement unit,
            //we make sure to update the CSV line series as well to use the same newly selected measurement unit.
            else if (SeriesCollection.Count > 2)
            {
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
            else
            {
                string selectedCSV = CSVLineGraph.SelectedItem.ToString();
                UpdateGraphWC(startDate, endDate, selectedWC, selectedMU);
                UpdateGraphWC2(startDate, endDate, selectedWC2, selectedMU);
                UpdateGraphCSV(startDate, endDate, selectedWC, selectedMU, selectedCSV);
            }
        }

        //Added by Gamze
        private void resetGraph_Click(object sender, RoutedEventArgs e)
        {
            //The statistics page is re-loaded with its default values.
            MainWindow.mvvm.SwapPageContent("Statistics");
        }
    }
}
