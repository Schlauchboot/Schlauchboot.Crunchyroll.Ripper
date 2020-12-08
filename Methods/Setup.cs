using System;
using System.IO;

using Newtonsoft.Json;

using Schlauchboot.Ripper.Crunchyroll.Models;

namespace Schlauchboot.Ripper.Crunchyroll.Methods
{
    class Setup
    {
        public void CreateConfigTemplate()
        {
            var configObject = new Config()
            {
                outputPath = String.Empty,
                preferedAudioLang = String.Empty,
                preferedSubLang = String.Empty
            };
            string jsonConfig = JsonConvert.SerializeObject(configObject);
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Config\\config.json", jsonConfig);
        }

        public Config ReadConfig()
        {
            return JsonConvert
                .DeserializeObject<Config>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Config\\config.json"));
        }
    }
}
