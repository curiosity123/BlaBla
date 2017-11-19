using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Common.Serialization
{
    public class XmlSerialization : ISerialization
    {
        public byte[] Serialize<T>(T item)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            xs.Serialize(stream, item);
            return stream.ToArray();
        }
        public T Deserialize<T>(byte[] data)
        {
            try
            {
                string s = Encoding.UTF8.GetString(data);
                MemoryStream memoryStream = new MemoryStream(data);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data corrupted!" + ex.ToString());
                return default(T);
            }
        }
    }

}
