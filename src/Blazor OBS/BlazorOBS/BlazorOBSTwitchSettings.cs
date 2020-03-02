using Newtonsoft.Json;
using System.IO;

namespace BlazorOBS
{
    public class BlazorOBSTwitchSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public static BlazorOBSTwitchSettings Get()
        {
            var settingsPath = BlazorOBSAppDataDirectory.GetFilePath("serverSettings.json", true);

            var json = File.ReadAllText(settingsPath);

            return JsonConvert.DeserializeObject<BlazorOBSTwitchSettings>(json);
        }
    }
}
