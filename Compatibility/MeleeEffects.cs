using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// MeleeEffects
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2851052437
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2966457992
    /// </summary>
    internal static class MeleeEffects
    {
        public static bool Found => ModLoader.HasMod("MeleeWeaponEffects") || ModLoader.HasMod("MeleeEffects");
    }
}
