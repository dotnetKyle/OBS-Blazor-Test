using Newtonsoft.Json;
using System.IO;

namespace BlazorOBS
{
    public class BlazorOBSUserSettings
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }

        public static void Save(string accessToken, string idToken, string scope, string tokenType)
        {
            var settings = new BlazorOBSUserSettings
            {
                AccessToken = accessToken,
                IdToken = idToken,
                Scope = scope,
                TokenType = tokenType
            };

            var path = BlazorOBSAppDataDirectory.GetFilePath("usersettings.json");

            var json = JsonConvert.SerializeObject(settings);

            File.WriteAllText(path, json);
        }

        public static BlazorOBSUserSettings Get()
        {
            var path = BlazorOBSAppDataDirectory.GetFilePath("usersettings.json");

            if (File.Exists(path) == false)
                return null;

            var json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<BlazorOBSUserSettings>(json);
        }
    }
}
