using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Soiduplaan
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
            DataContext = App.SearchViewModel;
            l1.SelectedIndex = -1;
            l2.SelectedIndex = -1;
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            searchPivot.SelectedIndex = Int32.Parse(NavigationContext.QueryString["tab"]);
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.SearchViewModel.Input = textBox1.Text;
        }

        private void showStop(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
            int id = Int32.Parse(g.Tag.ToString());
            NavigationService.Navigate(new Uri("/StopPage.xaml?id=" + id, UriKind.Relative));
        }

        private void showRoute(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
            int id = Int32.Parse(g.Tag.ToString());
            NavigationService.Navigate(new Uri("/RoutePage.xaml?id=" + id, UriKind.Relative));
        }

    }
}