using System;
using System.IO;

namespace BlazorOBS
{
    public static class BlazorOBSAppDataDirectory
    {
        public static string GetFilePath(string filename, bool fileMustExist = false)
        {
            var roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var appRoamingDirectory = Path.Combine(roaming, "BlazorOBS");

            if (Directory.Exists(appRoamingDirectory) == false)
                throw new Exception($"The app roaming directory does not exist at {appRoamingDirectory}.");

            var filePath = Path.Combine(appRoamingDirectory, filename);

            if (fileMustExist && File.Exists(filePath) == false)
                throw new FileNotFoundException($"A required file is missing at: {filePath}. You may need to repair your installation of Blazor OBS.");

            return filePath;
        }
    }
}
