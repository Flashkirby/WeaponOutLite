using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Calamity Mod
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2824688072
    /// </summary>
    internal static class CalamityMod
    {
        public static bool Found => ModLoader.HasMod("CalamityMod");

        public static bool HasItem(Item item)
        {
            return item.ModItem?.Mod?.Name == "CalamityMod";
        }

        /// <summary>
        /// Call this in PostSetupContent for default weapon values.
        /// </summary>
        public static void PostSetupContent()
        {
            if (!Found) return;

            WeaponOutLite mod = WeaponOutLite.GetMod();
            Mod src = ModLoader.GetMod("CalamityMod");

            var modItems = src.GetContent<ModItem>();
            foreach (var i in modItems)
            {
                switch (i.Name)
                {
                    case "FracturedArk": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.SmallMelee); break;
                    case "BladecrestOathsword": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.SmallMelee); break;
                    case "PrismaticBreaker": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.SmallMelee); break;
                    case "CosmicDischarge": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Whips); break;
                    case "DevilsSunrise": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.LargeMelee); break;
                    case "YateveoBloom": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.ThrownThin); break;
                    case "YharimsCrystal": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.MagicItem); break;
                    case "Rancor": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.MagicItem); break;
                    case "CosmicRainbow": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Bow); break;
                    case "WarloksMoonFist": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.GiantMagic); break;
                    case "Cosmilamp": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.VanityItem); break;
                    case "EtherealSubjugator": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.VanityItem); break;
                    case "TheOldReaper": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Staff); break;
                    case "TheFinalDawn": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Staff); break;
                }
            }
        }
    }
}
