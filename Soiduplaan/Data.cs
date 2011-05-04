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

        public static string loadJSON(string filename)
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

        public static void UpdateData()
        {
            string[] filenames = { "generic.json" };

            for (int i = 0; i < filenames.Length; i++)
            {
                Download d = new Download(filenames[i]);
                d.Done += new Download.DownloadedEventHandler(UpdateData_Done);
            }
        }

        private static void UpdateData_Done(object sender, DownloadedEventArgs e)
        {
            saveFileToPhone(e.Filename, e.JSON);
        }
    }
}
