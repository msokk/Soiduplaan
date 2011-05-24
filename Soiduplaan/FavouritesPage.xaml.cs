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
    public partial class FavouritesPage : PhoneApplicationPage
    {
        public FavouritesPage()
        {
            InitializeComponent();
            DataContext = new FavouriteViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
    }
}