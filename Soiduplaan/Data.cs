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
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Soiduplaan
{

    public class Data
    {
        private static void saveFileToPhone(string filename, string data)
        {
            StreamWriter writer = null;
            try
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = storage.OpenFile(filename, FileMode.Create, FileAccess.Write);

                writer = new StreamWriter(file);

                writer.Write(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }

            writer.Close();
        }

        private static string loadFromPhone(string filename)
        {
            string result = "";
            TextReader reader = null;
            try
            {

                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = storage.OpenFile(filename, FileMode.OpenOrCreate, FileAccess.Read);

                reader = new StreamReader(file);
                if (file.Length > 0)
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }
            reader.Close();

            return result;
        }

        public delegate void XmlFetchEventHandler(object sender, XmlFetchEventArgs e);
        public static event XmlFetchEventHandler Done;

        public class XmlFetchEventArgs : EventArgs
        {
            public readonly XDocument Xml;

            public XmlFetchEventArgs(string xml)
            {
                Xml = XDocument.Parse(xml);
            }
        }

        public static void fetchXML(string action, Dictionary<string, string> param)
        {
            param.Add("a", "p." + action);
            List<string> qParts = new List<string>();
            foreach (KeyValuePair<string, string> p in param)
            {
                qParts.Add(HttpUtility.UrlEncode(p.Key) + "=" + HttpUtility.UrlEncode(p.Value));
            }
            string query = string.Join("&", qParts.ToArray());

            string data = loadFromPhone(query);
            if (data == "")
            {
                Download d = new Download(param);
                d.Done += new Download.DownloadedEventHandler(d_Done);
            }
            else
            {
                Done(null, new XmlFetchEventArgs(data));
            }
        }

        static void d_Done(object sender, DownloadedEventArgs e)
        {
            Done(null, new XmlFetchEventArgs(e.Xml));
        }

        public static IsolatedStorageSettings Settings {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings;
            }
        }


        public static int getTodayBit(bool tom) {
            var dayInt = (int)DateTime.Now.DayOfWeek;
            if (tom)
                dayInt++;
            return (int)Math.Pow(2, dayInt);
        }
    }
}
