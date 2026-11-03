using Alfa_1_aars_eksamens_projekt.Views.Pages;
using System.ComponentModel;
using System.Windows.Controls;

namespace Alfa_1_aars_eksamens_projekt.ViewModels
{
    //Done by Keemon
    //The MainWindowViewModel implements the INotifiyPropertyChanged interface, in order to give notification, when our Page Content changes,
    //allowing us to swap between pages.
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //We have our PageContent property, which gets and set the PageContent, bound to the MainWindow frame.
        public Page PageContent { get; set; }
        //We define the PropertyChanged event handler which will handle the INotifyPropertyChanged event when a property is changed.
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            //We set the PageContent to load the login page when you open the application.
            Page loginPage = new LoginPage();
            PageContent = loginPage;
        }

        /// <summary>
        /// Allows for the swap of pages in the MainView Frame
        /// <para>Made by Keemon</para>
        /// </summary>
        /// <param name="name">The name of the page</param>
        public void SwapPageContent(string name)
        {
            //In the SwapPageContent method we have a switch that contains the name of all of our pages,
            //and using the OnPropertyChanged method on PageContent, we can swap between pages every time a new page name is set.
            switch (name)
            {
                case "LoginPage":
                    PageContent = new LoginPage();
                    break;
                case "CompanyProfile":
                    PageContent = new CompanyProfile();
                    break;
                case "LandingPage":
                    PageContent = new LandingPage();
                    break;
                case "RegisterCompanyProfile":
                    PageContent = new RegisterCompanyProfile();
                    break;
                case "WasteInformation":
                    PageContent = new WasteInformation();
                    break;
                case "Statistics":
                    PageContent = new Statistics();
                    break;
                default:
                    PageContent = new LoginPage();
                    break;
            }
            OnPropertyChanged("PageContent");
        }

        //This method invokes the PropertyChanged event whenever the PageContent value changes.
        protected void OnPropertyChanged(string pageContent)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pageContent));
        }

    }
}
