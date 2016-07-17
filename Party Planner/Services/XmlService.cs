using System.IO;
using System.Xml.Serialization;
using Party_Planner.Interfaces;

namespace Party_Planner.Services
{
    public class XmlService : IXmlService
    {
        #region Methods
        public void Serialize<T>(T obj, string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.OpenWrite(path))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public T Deserialize<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.OpenRead(path))
            {
                return (T)(serializer.Deserialize(stream));
            }
        }
        #endregion
    }
}
