using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HistoriansGuild.Helpers
{
    static class FileManager
    {
        const string AppDataFolder = "HistoriansGuild";

        public enum FileLocation
        {
            AppData,
            None
        }

        public static void SaveToJson<T>(T obj, string fileName, FileLocation location)
        {
            string filePath = Path.Combine(GetFileLocation(location), fileName);
            string jsonString = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

            if(!Directory.Exists(GetFileLocation(location)))
                Directory.CreateDirectory(GetFileLocation(location));

            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            File.WriteAllText(filePath, jsonString);
        }

        public static T? LoadFromJson<T>(string fileName, FileLocation location)
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(GetFileLocation(location), fileName);

            if (!File.Exists(filePath))
            {
                return default;
            }

            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        static string GetFileLocation(FileLocation location)
        {
            return location switch
            {
                FileLocation.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataFolder),
                _ => String.Empty,
            };
        }
    }
}
