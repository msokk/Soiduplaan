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
using System.Windows.Navigation;

namespace Soiduplaan
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
            DataContext = new SearchViewModel();

        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //MessageBox.Show(Data.loadJSON("generic.json"));
            //string selectedIndex = "";
            //if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            //{
            //    int index = int.Parse(selectedIndex);
            //    DataContext = App.ViewModel.Items[index];
            //}
        }
    }
}