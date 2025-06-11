using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Terraria Overhaul
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2811803870
    /// </summary>
    internal static class TerrariaOverhaul
    {
        public static bool Found => ModLoader.HasMod("TerrariaOverhaul");
    }
}
