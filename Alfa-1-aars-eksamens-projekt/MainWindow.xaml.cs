using Alfa_1_aars_eksamens_projekt.Classes;
using Alfa_1_aars_eksamens_projekt.ViewModels;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Alfa_1_aars_eksamens_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //We create an instance of the MainWindowViewModel in order to set the Data Context for the MainWindow,
        //which has the Frame of the MainWindow bound to our PageContent, which we control in the MainWindowViewModel.
        public static MainWindowViewModel mvvm = new MainWindowViewModel();
        //We define initial values for the current company ID and name, in order to
        //keep track of which company is logged in.
        public static int currentCompanyID = 0;
        public static string currentCompanyName = "";

        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            Log.CreateLog("Program opened ");
            DataContext = mvvm;
        }
    }
}
