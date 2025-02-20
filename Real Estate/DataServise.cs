using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Xml;
namespace Real_Estate
{
    public class DataService
    {
        private const string FilePath = "properties.json";

        public List<Property> LoadProperties()
        {
            if (!File.Exists(FilePath)) return new List<Property>();
            return JsonConvert.DeserializeObject<List<Property>>(File.ReadAllText(FilePath));
        }

        public void SaveProperties(List<Property> properties)
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(properties, Newtonsoft.Json.Formatting.Indented));
        }
    }
}