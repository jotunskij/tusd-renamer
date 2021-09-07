using System;
using System.IO;
using Newtonsoft.Json;

namespace tusd_renamer
{
    public class TusdEntry
    {
        [JsonProperty(PropertyName = "Upload")]
        public Upload Upload { get; set; }
    }

    public class Upload
    {
        [JsonProperty(PropertyName = "MetaData")]
        public MetaData MetaData { get; set; }
        [JsonProperty(PropertyName = "Storage")]
        public Storage Storage { get; set; }
    }

    public class Storage
    {
        [JsonProperty(PropertyName = "Path")]
        public string Path { get; set; }
    }

    public class MetaData
    {
        [JsonProperty(PropertyName = "filename")]
        public string filename { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string s;
            string stdin = "";
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while ((s = Console.ReadLine()) != null)
            {
                // Remove line comments
                if (s.Trim().StartsWith("//"))
                    continue;
                // Trim utf-8 bom
                stdin += s.Trim('\uFEFF', '\u200B');
            }

            TusdEntry entry;
            try
            {
                entry = JsonConvert.DeserializeObject<TusdEntry>(stdin);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when deserializing json: {ex}");
                return;
            }

            if (entry == null)
            {
                Console.WriteLine("Json deserialization returned null object");
                return;
            }

            try
            {
                var fileInfo = new FileInfo(entry.Upload.Storage.Path);
                var directory = fileInfo.Directory.FullName;
                var originalName = entry.Upload.MetaData.filename;
                fileInfo.MoveTo(Path.Join(directory, originalName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when renaming file: {ex}");
            }
        }
    }
}
