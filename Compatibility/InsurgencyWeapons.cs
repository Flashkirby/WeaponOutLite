using Terraria;
using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// InsurgencyWeapons
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=3035273145
    /// </summary>
    internal static class InsurgencyWeapons
    {
        public static bool Found => ModLoader.HasMod("InsurgencyWeapons");

        public static bool HasItem(Item item)
        {
            return item.ModItem?.Mod?.Name == "InsurgencyWeapons";
        }
    }
}
