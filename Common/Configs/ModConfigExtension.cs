using System.IO;
using Humanizer;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader.Config;
using static System.Net.Mime.MediaTypeNames;

namespace WeaponOutLite.Common.Configs
{
    public static class ModConfigExtension
    {
        /// <summary>
        /// Custom implementation that replicates internal method Terraria.ModLoader.Config.ConfigManager.Save()
        /// Tested with tModLoader 1.4.4.9
        /// </summary>
        public static void Save(this ModConfig config)
        {
            try
            {
                Directory.CreateDirectory(ConfigManager.ModConfigPath);
                string filename = config.Mod.Name + "_" + config.Name + ".json";
                string path = Path.Combine(ConfigManager.ModConfigPath, filename);
                string json = JsonConvert.SerializeObject(config, ConfigManager.serializerSettings);
                File.WriteAllText(path, json);
            }
            catch (IOException)
            {
                string error = $"WeaponOut: Failed to save config file {config.Name}. Disable weapon out quick swap setting in client config";
                System.Console.WriteLine(error);
                Main.NewText(error, Color.Red);
            }
            catch (JsonException)
            {
                string error = $"WeaponOut: Failed to serialize file {config.Name}. Disable weapon out quick swap setting in client config";
                System.Console.WriteLine(error);
                Main.NewText(error, Color.Red);
            }
        }
    }
}
