using System;
using System.Linq;
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
using System.IO;
using System.Reflection;

namespace Soiduplaan
{
    public class CustomBinarySerializer
    {

        private List<PropertyInfo> serializableProperties = new List<PropertyInfo>();
        private Type serializableObjectType;

        public CustomBinarySerializer(Type objectType)
        {
            serializableObjectType = objectType;
            serializableProperties = GetMarkedProperties(objectType);
        }

        private List<PropertyInfo> GetMarkedProperties(Type type)
        {
            return (from property in type.GetProperties()
                    where property.GetCustomAttributes(true)
                    .Where((x) => x is System.Runtime.Serialization.DataMemberAttribute).Count() > 0
                    select property
                    ).ToList();
        }

        #region Write

        public void WriteObject(Stream stream, object graph)
        {
            if (stream == null || graph == null)
                return;

            BinaryWriter bw = new BinaryWriter(stream);

            foreach (PropertyInfo pi in serializableProperties)
            {
                var value = pi.GetValue(graph, null);

                if (pi.PropertyType == typeof(string))
                {
                    bw.Write(value as string ?? string.Empty);
                }
                else if (pi.PropertyType == typeof(List<int>))
                {
                    WriteIntegerList(bw, value as List<int>);
                }
            }
        }

        private void WriteIntegerList(BinaryWriter bw, List<int> list)
        {
            if (list == null || !list.Any())
            {
                bw.Write(0);
            }
            else
            {
                bw.Write(list.Count);
                list.ForEach(x => bw.Write(x));
            }
        }

        #endregion Write

        #region Read

        public object ReadObject(Stream stream)
        {
            if (stream == null)
                return null;

            BinaryReader br = new BinaryReader(stream);

            object deserializedObject = Activator.CreateInstance(serializableObjectType);

            foreach (PropertyInfo pi in serializableProperties)
            {
                if (pi.PropertyType == typeof(string))
                {
                    try
                    {
                        pi.SetValue(deserializedObject, br.ReadString(), null);
                    }
                    catch (EndOfStreamException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                else if (pi.PropertyType == typeof(List<int>))
                {
                    pi.SetValue(deserializedObject, ReadIntegerList(br), null);
                }
            }
            return deserializedObject;
        }

        private List<int> ReadIntegerList(BinaryReader br)
        {
            List<int> list = new List<int>();
            int count = br.ReadInt32();

            int index = count;
            while (index > 0)
            {
                list.Add(br.ReadInt32());
                index--;
            }
            return list;
        }

        #endregion Read

    }
}
