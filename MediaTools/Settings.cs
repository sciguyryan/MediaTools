using System.Text.Json;

namespace MediaTools
{
    internal class Settings
    {
        private const string FileName = "settings.json";
        public bool ShowFolders { get; set; } = true;
        public bool ShowMediaInSubFolders { get; set; } = false;

        public void WriteSettings()
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText($".\\{FileName}", json);
        }

        public static Settings ReadSettings()
        {
            if (!File.Exists(FileName))
            {
                return new Settings();
            }

            var json = File.ReadAllText(FileName);
            var deserialized = JsonSerializer.Deserialize<Settings>(json);

            return deserialized ?? new Settings();
        }
    }
}
