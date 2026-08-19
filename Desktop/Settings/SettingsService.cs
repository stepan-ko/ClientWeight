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
        private AppSettings? _original; // «Эталон» для сравнения и сброса

        public AppSettings Current => _current ??= Load();

        public SettingsService()
        {
            var baseDir = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.Create);

            var appDir = Path.Combine(baseDir, "ClientCW");
            Directory.CreateDirectory(appDir);
            _path = Path.Combine(appDir, "settings.json");

            // Если файла нет — создаём с дефолтными настройками
            if (!File.Exists(_path))
            {
                var defaultSettings = new AppSettings
                {
                    ModbusHost = "10.6.173.231",
                    ModbusLocalHost = "10.6.173.230",
                    ModbusPort = 502,
                    ModbusUnitId = 1,
                    ReconnectDelaySeconds = 5
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(defaultSettings, options);
                File.WriteAllText(_path, json);
            }

            // При старте сразу загружаем и запоминаем как «оригинал»
            _original = Clone(Current);
        }

        private AppSettings Load()
        {
            try
            {
                if (!File.Exists(_path)) return new AppSettings();

                var json = File.ReadAllText(_path);
                var loaded = JsonSerializer.Deserialize<AppSettings>(json);
                return loaded ?? new AppSettings();
            }
            catch
            {
                // При повреждённом файле — дефолтные настройки
                return new AppSettings();
            }
        }

        /// <summary>
        /// Сохраняет текущие настройки на диск и обновляет _original,
        /// чтобы «исходными» стали текущие значения.
        /// </summary>
        public void Save()
        {
            if (_current == null) return;

            var dir = Path.GetDirectoryName(_path);
            if (dir != null) Directory.CreateDirectory(dir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_path, JsonSerializer.Serialize(_current, options));

            // После сохранения «оригиналом» становятся текущие значения
            _original = Clone(_current);
        }

        /// <summary>
        /// Откатывает Current к значениям, которые были при загрузке (или после последнего Save).
        /// </summary>
        public void ResetToOriginal()
        {
            if (_original == null) return;

            Current.ModbusHost = _original.ModbusHost;
            Current.ModbusLocalHost = _original.ModbusLocalHost;
            Current.ModbusPort = _original.ModbusPort;
            Current.ModbusUnitId = _original.ModbusUnitId;
            Current.ReconnectDelaySeconds = _original.ReconnectDelaySeconds;
        }

        /// <summary>
        /// Возвращает true, если Current отличается от _original.
        /// </summary>
        public bool HasChanges()
        {
            if (_original == null || _current == null) return false;

            return Current.ModbusHost != _original.ModbusHost
                || Current.ModbusLocalHost != _original.ModbusLocalHost
                || Current.ModbusPort != _original.ModbusPort
                || Current.ModbusUnitId != _original.ModbusUnitId
                || Current.ReconnectDelaySeconds != _original.ReconnectDelaySeconds;
        }

        private static AppSettings Clone(AppSettings source)
        {
            return new AppSettings
            {
                ModbusHost = source.ModbusHost,
                ModbusLocalHost = source.ModbusLocalHost,
                ModbusPort = source.ModbusPort,
                ModbusUnitId = source.ModbusUnitId,
                ReconnectDelaySeconds = source.ReconnectDelaySeconds
            };
        }
    }

}
