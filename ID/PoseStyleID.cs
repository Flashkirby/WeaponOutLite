using Terraria.Localization;
using WeaponOutLite.Content.DrawItemPose;

namespace WeaponOutLite.ID
{
    /// <summary>
    /// Class for IDs per pose group. 
    /// These are the groups that items can be assigned to for DrawItemPoseID.
    /// </summary>
    public static class PoseStyleID
    {
        public enum PoseGroup
        {
            Unassigned = 0,
            Item = 10,
            Potion = 20,
            LargeItem = 50,
            VanityItem = 60,
            PowerTool = 70,
            Thrown = 90,
            ThrownThin = 95,
            Whips = 100,
            SmallMelee = 120,
            LargeMelee = 140,
            Spear = 150,
            Flail = 155,
            Rapier = 160,
            Yoyo = 165,
            Bow = 200,
            Pistol = 220,
            Gun = 240,
            GunManual = 250,
            Shotgun = 255,
            Repeater = 260,
            Launcher = 280,
            Staff = 300,
            MagicBook = 310,
            MagicItem = 320,
            GiantItem = 900,
            GiantWeapon = 920,
            GiantBow = 940,
            GiantGun = 950,
            GiantMagic = 960,
        }

        public static string MapPoseGroupToString(PoseGroup poseGroup)
        {
            return Language.GetTextValue("Mods.WeaponOutLite.Configs.PoseGroup." + poseGroup.ToString() + ".Label");
        }
    }
}
