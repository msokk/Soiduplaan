﻿using System;
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


namespace Soiduplaan
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Device.Location.GeoCoordinate gc = CurrentLocation.Current.Location;
        }

        private void favouriteTile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FavouritesPage.xaml", UriKind.Relative));
        }

        private void routeTile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml?tab=0", UriKind.Relative));
        }

        private void stopTile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml?tab=1", UriKind.Relative));
        }

        private void nearbyTile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NearbySmallPage.xaml", UriKind.Relative));
        }

        private void OnSearchKeyPress(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void showInformation(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InformationPage.xaml", UriKind.Relative));
        }
    }
}