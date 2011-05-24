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
using Microsoft.Phone.Controls.Maps;

namespace Soiduplaan
{
    public partial class NearbySmallPage : PhoneApplicationPage
    {
        public NearbySmallPage()
        {
            InitializeComponent();
            DataContext = new NearbyViewModel();
            RoutesList.SelectedItem = null;
        }

        private void map1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Pushpin p = sender as Pushpin;
            NavigationService.Navigate(new Uri("/NearbyPage.xaml", UriKind.Relative));
        }

        private void map1_MapPan(object sender, MapDragEventArgs e)
        {
            e.Handled = true;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
            int id = Int32.Parse(g.Tag.ToString());
            NavigationService.Navigate(new Uri("/StopPage.xaml?id=" + id, UriKind.Relative));
        }

    }
}