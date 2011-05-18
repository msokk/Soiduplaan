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
using System.Windows.Navigation;

namespace Soiduplaan
{
    public partial class NearbyPage : PhoneApplicationPage
    {
        public NearbyPage()
        {
            InitializeComponent();
            DataContext = new NearbyViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void PushPin_Click(object sender, MouseEventArgs e)
        {
            Pushpin p = sender as Pushpin;
            NavigationService.Navigate(new Uri("/StopPage.xaml?id=" + p.Tag.ToString(), UriKind.Relative));
        }
    }
}