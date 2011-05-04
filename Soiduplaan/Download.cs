using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Soiduplaan
{
    public class DownloadedEventArgs : EventArgs
    {
        public readonly string JSON;
        public readonly string Filename;

        public DownloadedEventArgs(string json, string filename)
        {
            JSON = json;
            Filename = filename;
        }
    }
    public class Download
    {

        private Uri[] mirrors = { new Uri("http://sokk.ee/soiduplaan/"),
                               new Uri("http://byte.net.ee/soiduplaan/") };

        private int defaultMirror = 1;

        private string fileName = "";

        public delegate void DownloadedEventHandler(object sender, DownloadedEventArgs e);
        public event DownloadedEventHandler Done;

        public Download(string name)
        {
            fileName = name;
            int mirrorIndex = defaultMirror;
            WebClient c = new WebClient();

            Uri current = mirrors[mirrorIndex];
            c.DownloadStringCompleted += new DownloadStringCompletedEventHandler(c_DownloadStringCompleted);

            c.DownloadStringAsync(new Uri(current.ToString() + fileName));
        }

        void c_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //TODO: Handle mirrors here
            }
            else
            {
                Done(null, new DownloadedEventArgs(e.Result, fileName));
            }
        }
    }
}
