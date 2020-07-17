using System;
using System.IO;
using System.Xml.Serialization;

namespace SmRecipeModifier.Core
{

    public class Configuration
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "SmRecipeModifier.cfg");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));

        public string GameDataPath { get; set; } = string.Empty;
        public string AccentName { get; set; } = "Orange";
        public bool EnableDarkMode { get; set; } = true;
        public bool EnableDeveloperAnalytics { get; set; } = true;

        public void Save()
        {
            var stream = new FileStream(Source, FileMode.Create);
            Serializer.Serialize(stream, this);
            stream.Close();
        }
        
        public void Reset()
        {
            if (File.Exists(Source))
                File.Delete(Source);
        }

        public static Configuration Load()
        {
            if (!File.Exists(Source))
                return new Configuration();
            var stream = new FileStream(Source, FileMode.Open);
            var result =  Serializer.Deserialize(stream) as Configuration;
            stream.Close();
            return result;
        }

    }

}