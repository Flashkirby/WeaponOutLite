using Terraria;
using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Thorium Mod
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2909886416
    /// </summary>
    internal static class ThoriumMod
    {
        public static bool Found => ModLoader.HasMod("ThoriumMod");
    }
}
