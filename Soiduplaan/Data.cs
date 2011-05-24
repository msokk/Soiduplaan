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

namespace Soiduplaan
{

    public class Data
    {
        private static Dictionary<string,string> cache = new Dictionary<string, string>();

        private static void saveFileToPhone(string filename, string data)
        {
            StreamWriter writer = null;
            try
            {
                cache[filename] = data;
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

        public static string loadJSON(string filename)
        {
            if (cache.ContainsKey(filename))
            {
                string result = "{}";

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
            else
            {
                if (cache.ContainsKey(filename))
                {
                    return cache[filename];
                }
                else
                {
                    return "{}";
                }
            }
        }

        private static string[] filenames = { "generic.json", "routes.json", "stops.json" };

        public static void UpdateData()
        {
            for (int i = 0; i < filenames.Length; i++)
            {
                Download d = new Download(filenames[i]);
                d.Done += new Download.DownloadedEventHandler(UpdateData_Done);
            }
        }

        public delegate void DoneHandler(object sender, EventArgs e);
        public static event DoneHandler Done;

        private static void UpdateData_Done(object sender, DownloadedEventArgs e)
        {
            saveFileToPhone(e.Filename, e.JSON);
            if (filenames[filenames.Length - 1] == e.Filename)
            {
                Done(null, new EventArgs());
            }
        }

        public static IsolatedStorageSettings Settings {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings;
            }
        }


    }
}
