using HistoriansGuild.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace HistoriansGuild.Models
{
    static class AppConfig
    {
        const string configSaveFileLocation = "appconfig.json";

        public static Config config;

        static AppConfig()
        {
            config = FileManager.LoadFromJson<Config>(configSaveFileLocation, FileManager.FileLocation.AppData) ?? new Config();
        }

        public static Repository AddRepository(string location, int userId)
        {
            if (config.Repositories.Any(r => r.Location == location))
                return config.Repositories.Find(r => r.Location == location)!;

            //Get the next available id as the current highest id + 1
            int id = config.Repositories.Count == 0 ? 0 : config.Repositories.Max(u => u.Id) + 1;

            var repo = new Repository(id, location, userId);

            config.Repositories.Add(repo);
            ApplyChanges();

            return repo;
        }

        public static void SetCurrentRepository(int repository)
        {
            config.CurrentRepository = repository;
            ApplyChanges();
        }

        public static User AddUser(string name, string email, string password)
        {
            if (config.Users.Any(u => u.Name == name))
                return config.Users.Find(u => u.Name == name)!;

            //Get the next available id as the current highest id + 1
            int id = config.Users.Count == 0 ? 0 : config.Users.Max(u => u.Id) + 1;

            var user = new User(id, name, email, password);

            config.Users.Add(user);
            ApplyChanges();

            return user;
        }

        public static void ApplyChanges()
        {
            SaveToJson();
        }

        static void SaveToJson()
        {
            FileManager.SaveToJson(config, configSaveFileLocation, FileManager.FileLocation.AppData);
        }
    }

    class Config
    {
        public List<User> Users { get; set; } = [];
        public List<Repository> Repositories { get; set; } = [];
        public int CurrentRepository { get; set; } = 0;
    }
}
