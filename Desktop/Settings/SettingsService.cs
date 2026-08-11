using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Desktop.Settings
{
    public class SettingsService
    {
        private readonly string _path;
        private AppSettings? _current;

        public AppSettings Current => _current ??= Load();

        public SettingsService()
        {
            var baseDir = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.Create);
            var appDir = Path.Combine(baseDir, "ClientCW");
            Directory.CreateDirectory(appDir);
            _path = Path.Combine(appDir, "settings.json");
        }

        private AppSettings Load()
        {
            try
            {
                if (!File.Exists(_path)) return new AppSettings();
                var json = File.ReadAllText(_path);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                // При повреждённом файле — дефолтные настройки
                return new AppSettings();
            }
        }

        public void Save()
        {
            if (_current == null) return;
            var dir = Path.GetDirectoryName(_path);
            if (dir != null) Directory.CreateDirectory(dir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_path, JsonSerializer.Serialize(_current, options));
        }
    }

}
