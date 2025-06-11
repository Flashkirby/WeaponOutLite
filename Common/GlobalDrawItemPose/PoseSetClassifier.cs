using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.ID;
using WeaponOutLite.Compatibility;
using static WeaponOutLite.ID.PoseStyleID;
using static WeaponOutLite.ID.DrawItemPoseID;

namespace WeaponOutLite.Common.GlobalDrawItemPose
{
    /// <summary>
    /// Class that determines the "default" post style for all items
    /// </summary>
    public static class PoseSetClassifier
    {
        /// <summary>
        /// Select an drawitempose to use from a pose style.
        /// </summary>
        public static IDrawItemPose SelectItemPose(Player p, Item item) {

            // Get group data for item pose
            GetItemPoseGroupData(item, out PoseGroup poseGroup, out IDrawItemPose drawItemPose);

            WeaponOutLite mod = WeaponOutLite.GetMod();
            var clientConfig = WeaponOutLite.ClientConfig;

            // Skip selection if an item pose is already active
            if (drawItemPose.GetID() != DrawItemPoseID.Unassigned) { return drawItemPose; }

            // Select the pose based on the client configured pose styles
            switch (poseGroup) {
                case PoseGroup.Item:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.SmallItemPose];
                    break;
                case PoseGroup.Ignore:
                    drawItemPose = mod.ItemPoses[(int)DrawItemPoseID.None];
                    break;
                case PoseGroup.LargeItem:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.LargeItemPose];
                    break;
                case PoseGroup.VanityItem:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.VanityItemPose];
                    break;
                case PoseGroup.Potion:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.PotionPose];
                    break;
                case PoseGroup.PowerTool:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.PowerToolPose];
                    break;
                case PoseGroup.Yoyo:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.YoyoPose];
                    break;
                case PoseGroup.SmallMelee:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.SmallMeleePose];
                    break;
                case PoseGroup.SmallTool:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.SmallToolPose];
                    break;
                case PoseGroup.LargeMelee:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.LargeMeleePose];
                    break;
                case PoseGroup.LargeTool:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.LargeToolPose];
                    break;
                case PoseGroup.Thrown:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.ThrownPose];
                    break;
                case PoseGroup.ThrownThin:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.ThrownThinPose];
                    break;
                case PoseGroup.Spear:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.SpearPose];
                    break;
                case PoseGroup.Rapier:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.RapierPose];
                    break;
                case PoseGroup.Flail:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.FlailPose];
                    break;
                case PoseGroup.Whips:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.WhipPose];
                    break;
                case PoseGroup.Bow:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.BowPose];
                    break;
                case PoseGroup.Repeater:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.RepeaterPose];
                    break;
                case PoseGroup.Pistol:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.PistolPose];
                    break;
                case PoseGroup.Gun:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GunPose];
                    break;
                case PoseGroup.GunManual:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GunManualPose];
                    break;
                case PoseGroup.Shotgun:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.ShotgunPose];
                    break;
                case PoseGroup.Launcher:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.LauncherPose];
                    break;
                case PoseGroup.Staff:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.StaffPose];
                    break;
                case PoseGroup.MagicBook:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.MagicBookPose];
                    break;
                case PoseGroup.MagicItem:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.MagicItemPose];
                    break;
                case PoseGroup.GiantItem:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GiantItemPose];
                    break;
                case PoseGroup.GiantWeapon:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GiantWeaponPose];
                    break;
                case PoseGroup.GiantBow:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GiantBowPose];
                    break;
                case PoseGroup.GiantGun:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GiantGunPose];
                    break;
                case PoseGroup.GiantMagic:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GiantMagicPose];
                    break;
                case PoseGroup.GiantDamaging:
                    drawItemPose = mod.ItemPoses[(int)clientConfig.GiantDamagingPose];
                    break;
            }
            return drawItemPose;
        }

        /// <summary>
        /// Fetch the posegroup to use for the item, taking into account any additional configuration options
        /// </summary>
        /// <param name="item">The item being classified</param>
        /// <param name="poseGroup">The pose group of the item, used to determined its DrawItemPose if DrawItemPoseID.Unassigned</param>
        /// <param name="drawItemPose">The specific DrawItemPose to use. If set, the poseGroup is skipped. </param>
        public static void GetItemPoseGroupData(Item item, out PoseGroup poseGroup, out IDrawItemPose drawItemPose)
        {
            WeaponOutLite mod = WeaponOutLite.GetMod();

            // Set initial pose style and item pose object
            poseGroup = PoseGroup.Unassigned;
            drawItemPose = mod.ItemPoses[DrawItemPoseID.Unassigned];

            // Set the item pose if its been set as a preferred, which skips pose group
            if (mod.priorityItemHoldPose.TryGetValue(item.type, out DrawItemPose itemPose))
            {
                drawItemPose = mod.ItemPoses[(int)itemPose];
            }

            // Read custom config for forced pose override
            ItemDrawOverrideData itemOverride = WeaponOutLite.ClientHoldOverride.FindStyleOverride(item.type);
            if (itemOverride != null) {
                // Found a forced pose in the config, so use this.
                drawItemPose = mod.ItemPoses[(int)itemOverride.ForceDrawItemPose];
                poseGroup = itemOverride.ForcePoseGroup;
            }

            // If the pose wasn't overwritten, and the style is not set, then figure out which one to use
            if (drawItemPose.GetID() == DrawItemPoseID.Unassigned
                && poseGroup == PoseGroup.Unassigned) {

                // If the item has been modded to use a custom pose, use that
                if (mod.customItemHoldStyles.Contains(item.type)) {
                    drawItemPose = mod.ItemPoses[DrawItemPoseID.Custom];
                }
                else {
                    // Otherwise, figure out which one to use
                    if (mod.priorityItemHoldGroups.TryGetValue(item.type, out poseGroup)) return;
                    else if (mod.compatibilityItemHoldGroups.TryGetValue(item.type, out poseGroup)) return;
                    else poseGroup = CalculateDrawStyleType(item);

                    //WeaponOutLite.TEXT_DEBUG += "\nclassifier " + item.useStyle + " = " + poseGroup;
                }
            }

            ////////////////////////////////////////////////////////////////////////
            //                                                                    //
            // debugging override                                                 //
            //                                                                    //
            ////////////////////////////////////////////////////////////////////////

            /*
            if (ThoriumMod.Found && item.ModItem?.Mod?.Name == "ThoriumMod")
            {
                if (item.ModItem?.Name == "Purify")
                {
                    poseGroup = PoseGroup.MagicBook;
                }
            }
            /**/

            /*
            if (Main.SmartCursorIsUsed) {
                poseGroup = PoseGroup.Unassigned;
                drawItemPose = mod.DrawStyle[DrawItemPoseID.HoldInHand];
            }
            /**/

            //poseGroup = PoseGroup.Unassigned;
            //drawItemPose = mod.DrawStyle[DrawItemPoseID.BackFlail];
            ////////////////////////////////////////////////////////////////////////
            //                                                                    //
            ////////////////////////////////////////////////////////////////////////
            return;
        }

        /// <summary>
        /// Given the player and item's state, what should they use (Defaults)
        /// </summary>
        private static PoseGroup CalculateDrawStyleType(Item item) {

            var itemTexture = TextureAssets.Item[item.type]?.Value;
            if(itemTexture == null)
            {
                return PoseGroup.Unassigned;
            }

            var itemFrames = 1;
            if (Main.itemAnimations[item.type] != null) {
                itemFrames = Main.itemAnimations[item.type].FrameCount;
            }
            float w = itemTexture.Width * item.scale;
            float h = itemTexture.Height * item.scale / itemFrames;

            var giantItemThreshold = WeaponOutLite.ClientConfig.GiantItemThreshold;
            var giantItemScale = WeaponOutLite.ClientConfig.GiantItemScalePercent / 100f;
            if (giantItemThreshold < Math.Max(w, h)) {
                w *= giantItemScale;
                h *= giantItemScale;
            }

            // ⚖ Giant Items
            if (giantItemThreshold < Math.Max(w, h)) {
                if (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed)) {
                    return PoseGroup.GiantWeapon;
                }
                else if (item.DamageType.CountsAsClass(DamageClass.Ranged) && h > w * 1.25f) {
                    return PoseGroup.GiantBow;
                }
                else if (item.DamageType.CountsAsClass(DamageClass.Ranged)) {
                    return PoseGroup.GiantGun;
                }
                else if (item.DamageType.CountsAsClass(DamageClass.Magic) || item.DamageType.CountsAsClass(DamageClass.Summon)) {
                    return PoseGroup.GiantMagic;
                }
                else if (item.damage > 0)
                {
                    return PoseGroup.GiantDamaging;
                }
                return PoseGroup.GiantItem;
            }

            // √◉ Yoyo, if it's in the set
            if (ItemID.Sets.Yoyo[item.type]) {
                return PoseGroup.Yoyo;
            }

            // 🔱 Spear, if it's in a set
            if (ItemID.Sets.Spears[item.type]) {
                return PoseGroup.Spear;
            }

            // 🥍 Staff, if it's in the set, exlcuding the beam sword which is not really a staff?
            if (Item.staff[item.type] && !item.DamageType.CountsAsClass(DamageClass.Melee)) {
                return PoseGroup.Staff;
            }

            // ➰ Whips, if it's in a set
            if (ProjectileID.Sets.IsAWhip[item.shoot]) {
                return PoseGroup.Whips;
            }

            if (item.damage > 0) {
                // ✨ Weird (usually magical) weapons, such as medusa head, blood thorn, books
                if (item.useStyle == ItemUseStyleID.HoldUp) {
                    return PoseGroup.MagicItem;
                }

                // 🤺🔱🛠🗡🔫 Various types of weirder weapon configurations, where the weeapon isn't rendered in use by default
                // Typically reserved for weapons that use projectiles for various logic, including rendering
                // Includes shortswords, flails, drills, spears, whips
                // Special endgame weapons such as celebration mk2 and phantasm use this method
                // Throwing weapons also fall under this category
                if (item.noUseGraphic) {
                    // 🤺 Shortswords and Rapiers. Thrust is only used by umbrellas
                    // Modded weapons may also use this
                    // Thorium's Charged Splasher uses this in a ranged weapon
                    if (!item.CountsAsClass(DamageClass.Ranged)) {
                        if (item.useStyle == ItemUseStyleID.Thrust || item.useStyle == ItemUseStyleID.Rapier)
                        {
                            // Applies a fix to attempt to help identify different types of swords. Default is On.
                            // Skip attempt to classify here if the melee effects mod integration is active
                            // Since it sets all melee swords to be this type, I would rather use normal poses
                            if (item.useStyle == ItemUseStyleID.Rapier &&
                                MeleeEffects.Found &&
                                WeaponOutLite.ClientConfig.ModIntegrationMeleeEffectsPlus)
                            {
                                // skip
                            }
                            else
                            {
                                return PoseGroup.Rapier;
                            }
                        }
                    }

                    // ➰ Whips are no graphic summon melee items
                    if (item.CountsAsClass(DamageClass.SummonMeleeSpeed) 
                        && item.noMelee)
                    {
                        return PoseGroup.Whips;
                    }

                    // Various shoot items can be categorised into melee weapons, unique (large) guns, and magic 
                    if (item.useStyle == ItemUseStyleID.Shoot)
                    {
                        if (item.CountsAsClass(DamageClass.Melee))
                        {
                            // 🛠 Powertools are basically, very wide.
                            // The shortest drill is the Nebula Drill (54 x 30, 1.8:1)
                            // Short modded drills include Thorium.IllumiteDrill (50 x 30, 1.6:1),
                            //
                            // Need to make sure we don't accidently put spears and such here:
                            // Most rectangular spears are actually taller (usually for halberds and glaives, eg. The Rotted Fork)
                            // A rare example of a wide spear is the Calamity.BansheeHook (120 x 108, 1.11: 1) 
                            if (h <= w * 0.8f)
                            {
                                return PoseGroup.PowerTool;
                            }
                            else
                            {
                                // nomelee melee weapons have a lot of different behaviours,
                                // it's quite the mess if ItemID.Sets aren't assigned properly.

                                // ♣️ Sleepy Octopod
                                // Modded weapons that act like it will often use the same sound
                                if (item.UseSound.HasValue && item.UseSound.Value == SoundID.DD2_MonkStaffSwing)
                                {
                                    return PoseGroup.Spear;
                                }

                                // √◉ Modded yoyos that are not marked as yoyos in the yoyo set
                                // They may still be yoyos if they feature all of the following attributes
                                if (item.channel
                                    && w == 30 && h == 26 && item.scale == 1f
                                    && item.noMelee && item.DamageType.Equals(DamageClass.MeleeNoSpeed)
                                    && item.channel && item.useAnimation == 25 && item.useTime == 25
                                    && item.UseSound.Equals(SoundID.Item1) && item.shootSpeed == 16f)
                                {
                                    return PoseGroup.Yoyo;
                                }

                                // Properly implemented flails and spears have this set 
                                var p = WeaponOutLayerRenderer.GetProjectile(item.shoot);
                                if (ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[item.shoot])
                                {
                                    if (p.ownerHitCheck)
                                    {
                                        // 🔱 Spears use owner check
                                        return PoseGroup.Spear;
                                    }
                                    else
                                    {
                                        // 🔨 Flails do not use owner hit check
                                        // Also uses netImportant but that's not as useful
                                        return PoseGroup.Flail;
                                    }
                                }

                                if (p.extraUpdates >= 1)
                                {
                                    // ➰ Vanilla whips are set up with extra updates for calculation
                                    // Modded whips that seek to emulate this probably have it set up
                                    return PoseGroup.Whips;
                                }

                                // Now we're in the realm of modded melee weapons
                                if (CalculateModdedWeaponDrawStyleType(item, p, w, h, out var poseGroup))
                                {
                                    return poseGroup;
                                }
                                else
                                {
                                // Still can't determine at this point, assume flail.
                                return PoseGroup.Flail;
                                }
                            }
                        }
                        // 🔫 Special graphic guns are wider than thrown weapons (eg. Celebration Mk2)
                        if (w > h && w > WeaponOutLite.ClientConfig.SmallGunThreshold) {
                            // quickfix for detecting shotguns, since they use this sound
                            if(item.UseSound == SoundID.Item36) {
                                return PoseGroup.Shotgun;
                            }
                            return item.autoReuse ? PoseGroup.Gun : PoseGroup.GunManual;
                        }
                        // ✨ Weird (usually magical) weapons, blaster, prism
                        if (item.CountsAsClass(DamageClass.Magic)) {
                            return PoseGroup.MagicItem;
                        }
                    }
                    
                    // 🔱 Big "throwing" weapons may as well be like spears, eg. Javelin.
                    if (w == h && w >= 40) {
                        return PoseGroup.Spear;
                    }

                    //WeaponOutLite.TEXT_DEBUG += $"\n{w / h} : ({w} {h})";  //todo remove
                    if (h > w * 1.25f && h >= 32 && !item.CountsAsClass(DamageClass.Melee)) {
                        // 🏹 Bow sprites are much taller than they are wide. No graphic bows include the Phantasm and Phantom Phoenix
                        // Also, excludes items smaller than 32, as even the smallest bows are at least 32px tall
                        // Examples of a wide bows would be:
                        // Calamity.Ultima (82 x 114, 1 : 1.39)
                        // Thorium.Destroyer's Rage (40 x 54, 1:1.35)
                        return PoseGroup.Bow;
                    }

                    // 🔪 Throwing weapons 
                    // such as boomerangs, knives, shurikens, and the shadow dagger
                    // include Grenades and Molotov Cocktails
                    // Thinner, upright weapons like knives fall under a separate adjustment category
                    // Also includes flat weapons, eg. Twisting Thunder from calamity mod
                    if (h >= w * 1.5f || w >= h * 1.5f) {
                        return PoseGroup.ThrownThin;
                    }

                    return PoseGroup.Thrown;
                }

                // ⚔ Melee Style items ⛏
                if (item.useStyle == ItemUseStyleID.Swing ||
                    item.useStyle == ItemUseStyleID.Thrust ||
                    item.useStyle == ItemUseStyleID.Rapier) {

                    // 🥍 Staff, because a lot of magic staff like weapons are in this category for some reason
                    if (item.noMelee && item.DamageType.CountsAsClass(DamageClass.Magic)) {
                        return PoseGroup.Staff;
                    }

                    // ⛏️
                    bool isTool = item.axe > 0 || item.pick > 0 || item.hammer > 0;

                    // 🔪 small swinging weapons
                    if (h + w <= WeaponOutLite.ClientConfig.SmallSwordThreshold * 2)
                    {
                        if (isTool) { return PoseGroup.SmallTool; }
                        return PoseGroup.SmallMelee;
                    }
                    // ⚔ big swinging weapons
                    else
                    {
                        if (isTool) { return PoseGroup.LargeTool; }
                        return PoseGroup.LargeMelee;

                    }
                }

                // 🔫🏹📔 Guns and Bows and Books
                if (item.useStyle == ItemUseStyleID.Shoot) {

                    // 🔫 Gun sprites are much wider than they are tall
                    // The "tallest" gun is the Pew-Matic Horn (28 x 26, 1.07:1)
                    if (w > h) {
                        // ⭕ small guns
                        if (w <= WeaponOutLite.ClientConfig.SmallGunThreshold) {
                            return PoseGroup.Pistol;
                        }

                        // ⚪ Big Guns
                        if (item.useAmmo == AmmoID.Arrow) {
                            return PoseGroup.Repeater;
                        }
                        else if (item.useAmmo == AmmoID.Rocket) {
                            return PoseGroup.Launcher;
                        }
                        else {
                            // quickfix for detecting shotguns, since they use this sound
                            if (item.UseSound == SoundID.Item36) {
                                return PoseGroup.Shotgun;
                            }
                            return item.autoReuse ? PoseGroup.Gun : PoseGroup.GunManual;
                        }
                    }
                    // 🏹 Bow sprites are much taller than they are wide (as opposed to guns)
                    // Examples of a wide bows would be:
                    // Calamity.Ultima (82 x 114, 1 : 1.39)
                    // Thorium.Destroyer's Rage (40 x 54, 1:1.35)
                    else {
                        // 📔 Books
                        if (item.CountsAsClass(DamageClass.Magic)) {
                            return PoseGroup.MagicBook;
                        }
                        else {
                            return PoseGroup.Bow;
                        }
                    }
                }
            }

            // 🧪 Potions
            if (item.healLife > 0 || item.healMana > 0 || (item.buffType != 0 && item.maxStack > 1)) {
                return PoseGroup.Potion;
            }

            // 💍 All equippable items / objects that affect a player visual slot go into this separate category.
            if (item.vanity || item.accessory ||
                item.dye != 0 || item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1 ||
                item.handOnSlot != -1 || item.handOffSlot != -1 || item.backSlot != -1 || item.frontSlot != -1 ||
                item.shoeSlot != -1 || item.waistSlot != -1 || item.wingSlot != -1 || item.shieldSlot != -1 ||
                item.neckSlot != -1 || item.faceSlot != -1 || item.balloonSlot != -1 || item.beardSlot != -1) {
                return PoseGroup.VanityItem;
            }

            // 📦 Big items such as paintings and crates, too wide to be held to the side
            // Excludes items that, while wide, are quite thin, such as the snaker charmer's flute, platforms
            if (w > 20 && h > 14) {
                return PoseGroup.LargeItem;
            }

            // 💎 Small hand held items, like blocks, bombs
            return PoseGroup.Item;
        }

        private static bool CalculateModdedWeaponDrawStyleType(Item item, Projectile p, float w, float h, out PoseGroup poseGroup)
        {
            poseGroup = PoseGroup.Unassigned;
            if (item.ModItem == null) return false;

            bool defaultToMeleeWeapon = false;

            // Redemption weapons are typically heavily modded melee weapons
            if (Redemption.Found) defaultToMeleeWeapon = true;


            if (defaultToMeleeWeapon)
            {
                // 🔪 small swinging weapons
                if (h + w <= WeaponOutLite.ClientConfig.SmallSwordThreshold * 2)
                { poseGroup = PoseGroup.SmallMelee; }
                // ⚔ big swinging weapons
                else
                { poseGroup = PoseGroup.LargeMelee; }
            }

            return poseGroup != PoseGroup.Unassigned;
        }
    }
}
