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
using System.Collections.Generic;

namespace Soiduplaan
{
    public class DownloadedEventArgs : EventArgs
    {
        public readonly string Xml;

        public DownloadedEventArgs(string xml)
        {
            Xml = xml;
        }
    }
    public class Download
    {

        public delegate void DownloadedEventHandler(object sender, DownloadedEventArgs e);
        public event DownloadedEventHandler Done;

        public Download(Dictionary<string, string> param)
        {
            param.Add("t", "xml");
            UriBuilder ub = new UriBuilder();
            ub.Host = "soiduplaan.tallinn.ee";

            List<string> qParts = new List<string>();
            foreach (KeyValuePair<string, string> p in param)
            {
                qParts.Add(HttpUtility.UrlEncode(p.Key) + "=" + HttpUtility.UrlEncode(p.Value));
            }
            ub.Query = string.Join("&", qParts.ToArray());

            WebClient c = new WebClient();
            c.DownloadStringCompleted += new DownloadStringCompletedEventHandler(c_DownloadStringCompleted);
            c.DownloadStringAsync(ub.Uri);
        }

        void c_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //TODO: Handle errors
            }
            else
            {
                Done(null, new DownloadedEventArgs(e.Result));
            }
        }
    }
}
