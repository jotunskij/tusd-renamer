using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace tusd_renamer
{
    public class TusdEntry
    {
        public Upload Upload { get; set; }
    }

    public class Upload
    {
        public string ID { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        public Storage Storage { get; set; }
    }

    public class Storage
    {
        public string Path { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var stdin = Console.ReadLine();
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

            if (!entry.Upload.MetaData.ContainsKey("filename"))
            {
                Console.WriteLine("Json metadata does not contain filename");
                return;
            }

            try
            {
                var fileInfo = new FileInfo(entry.Upload.Storage.Path);
                var directory = fileInfo.Directory.FullName;
                var originalName = entry.Upload.MetaData["filename"];
                fileInfo.MoveTo(Path.Join(directory, originalName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when renaming file: {ex}");
            }
        }
    }
}
