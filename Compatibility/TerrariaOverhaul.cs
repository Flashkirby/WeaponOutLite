using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Terraria Overhaul
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2811803870
    /// </summary>
    public static class TerrariaOverhaul
    {
        public static bool Found => ModLoader.HasMod("TerrariaOverhaul");
    }
}
