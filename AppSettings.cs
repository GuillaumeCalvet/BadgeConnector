using Microsoft.Extensions.Configuration;
using System.IO;

namespace BadgeConnector
{
    public class AppSettings
    {
        public bool IsFirstRun { get; set; }

        public AppSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            configuration.GetSection("AppSettings").Bind(this);
        }

        public void Save()
        {
            var json = File.ReadAllText("appsettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["AppSettings"]["IsFirstRun"] = IsFirstRun;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("appsettings.json", output);
        }
    }
}