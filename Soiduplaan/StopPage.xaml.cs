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
using Microsoft.Phone.Shell;

namespace Soiduplaan
{
    public partial class StopPage : PhoneApplicationPage
    {
        public StopPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = Int32.Parse(NavigationContext.QueryString["id"]);
            DataContext = new StopViewModel(id);
        }

        private void addToFav(object sender, EventArgs e)
        {
            MessageBox.Show("Peatus on lemmikutesse lisatud!");
        }

        bool backward = false;
        private void changeDirection(object sender, EventArgs e)
        {
            backward = (backward) ? false : true;
            flipAnimationForward.Completed += new EventHandler(flipAnimationCompleted);
            flipAnimationForward.Begin();
        }

        void flipAnimationCompleted(object sender, EventArgs e)
        {
            //load new data
            ((StopViewModel)this.DataContext).changeDirection();
            flipAnimationBackward.Begin();
        }

        bool tomorrow = false;
        private void changeDay(object sender, EventArgs e)
        {
            ApplicationBarIconButton barButton = sender as ApplicationBarIconButton;
            if (tomorrow)
            {
                tomorrow = false;
                barButton.IconUri = new Uri("/Images/AppBar/appbar.next.rest.png", UriKind.Relative);
                barButton.Text = "homme";
                slideLeftOut.Completed += new EventHandler(slideAnimationCompleted);
                slideLeftOut.Begin();
            }
            else
            {
                tomorrow = true;
                barButton.IconUri = new Uri("/Images/AppBar/appbar.back.rest.png", UriKind.Relative);
                barButton.Text = "täna";
                slideRightOut.Completed += new EventHandler(slideAnimationCompleted);
                slideRightOut.Begin();
            }
        }

        void slideAnimationCompleted(object sender, EventArgs e)
        {
            ((StopViewModel)this.DataContext).changeDay();
            if (tomorrow)
            {
                slideLeftIn.Begin();
            }
            else
            {
                slideRightIn.Begin();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}