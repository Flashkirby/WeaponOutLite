using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.ID;
using static WeaponOutLite.ID.PoseStyleID;

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

            GetItemPoseGroupData(item, out PoseGroup poseGroup, out IDrawItemPose drawItemPose);

            WeaponOutLite mod = WeaponOutLite.GetMod();
            var clientConfig = ModContent.GetInstance<WeaponOutClientConfig>();

            // Skip selection if an item pose is already active
            if (drawItemPose.GetID() != DrawItemPoseID.Unassigned) { return drawItemPose; }

            // Select the pose based on the client configured pose styles
            switch (poseGroup) {
                case PoseGroup.Item:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.SmallItemPose];
                    break;
                case PoseGroup.LargeItem:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.LargeItemPose];
                    break;
                case PoseGroup.Potion:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.PotionPose];
                    break;
                case PoseGroup.PowerTool:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.PowerToolPose];
                    break;
                case PoseGroup.Yoyo:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.YoyoPose];
                    break;
                case PoseGroup.SmallMelee:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.SmallMeleePose];
                    break;
                case PoseGroup.LargeMelee:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.LargeMeleePose];
                    break;
                case PoseGroup.Thrown:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.ThrownPose];
                    break;
                case PoseGroup.ThrownThin:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.ThrownThinPose];
                    break;
                case PoseGroup.Spear:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.SpearPose];
                    break;
                case PoseGroup.Rapier:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.RapierPose];
                    break;
                case PoseGroup.Flail:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.FlailPose];
                    break;
                case PoseGroup.Whips:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.WhipPose];
                    break;
                case PoseGroup.Bow:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.BowPose];
                    break;
                case PoseGroup.Repeater:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.RepeaterPose];
                    break;
                case PoseGroup.Pistol:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.PistolPose];
                    break;
                case PoseGroup.Gun:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GunPose];
                    break;
                case PoseGroup.GunManual:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GunManualPose];
                    break;
                case PoseGroup.Shotgun:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.ShotgunPose];
                    break;
                case PoseGroup.Launcher:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.LauncherPose];
                    break;
                case PoseGroup.Staff:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.StaffPose];
                    break;
                case PoseGroup.MagicBook:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.MagicBookPose];
                    break;
                case PoseGroup.MagicItem:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.MagicItemPose];
                    break;
                case PoseGroup.GiantItem:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GiantItemPose];
                    break;
                case PoseGroup.GiantWeapon:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GiantWeaponPose];
                    break;
                case PoseGroup.GiantBow:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GiantBowPose];
                    break;
                case PoseGroup.GiantGun:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GiantGunPose];
                    break;
                case PoseGroup.GiantMagic:
                    drawItemPose = mod.DrawStyle[(int)clientConfig.GiantMagicPose];
                    break;
            }
            return drawItemPose;
        }

        /// <summary>
        /// Fetch the posegroup to use for the item, taking into account any additional configuration options
        /// </summary>
        /// <returns></returns>
        public static void GetItemPoseGroupData(Item item, out PoseGroup poseGroup, out IDrawItemPose drawItemPose)
        {
            WeaponOutLite mod = WeaponOutLite.GetMod();
            var clientOverride = ModContent.GetInstance<WeaponOutClientHoldOverride>();

            // Set initial pose style and item pose object
            poseGroup = PoseGroup.Unassigned;
            drawItemPose = mod.DrawStyle[DrawItemPoseID.Unassigned];

            // Read custom config for forced pose override
            ItemDrawOverrideData itemOverride = clientOverride.FindStyleOverride(item.type);
            if (itemOverride != null) {
                // Found a forced pose in the config, so use this.
                drawItemPose = mod.DrawStyle[(int)itemOverride.ForceDrawItemPose];
                poseGroup = itemOverride.ForcePoseGroup;
            }

            // If the pose wasn't overwritten, and the style is not set, then figure out which one to use
            if (drawItemPose.GetID() == DrawItemPoseID.Unassigned
                && poseGroup == PoseGroup.Unassigned) {

                // If the item has been modded to use a custom pose, use that
                if (mod.customItemHoldStyles.Contains(item.type)) {
                    drawItemPose = mod.DrawStyle[DrawItemPoseID.Custom];
                }
                else {
                    // Otherwise, figure out which one to use
                    if (mod.customItemHoldGroups.Contains(item.type)) {
                        poseGroup = (PoseGroup)mod.customItemHoldGroups[item.type];
                    }
                    else {
                        poseGroup = CalculateDrawStyleType(item);
                    }
                    //WeaponOutLite.TEXT_DEBUG += "\nclassifier " + item.useStyle + " = " + poseGroup;
                }
            }

            ////////////////////////////////////////////////////////////////////////
            //                                                                    //
            // debugging override                                                 //
            //                                                                    //
            ////////////////////////////////////////////////////////////////////////
            /*
            if (Main.SmartCursorIsUsed) {
                poseGroup = PoseGroup.Unassigned;
                drawItemPose = mod.DrawStyle[DrawItemPoseID.HoldInHand];
            }
            */
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

            var itemTexture = TextureAssets.Item[item.type].Value;
            var itemFrames = 1;
            if (Main.itemAnimations[item.type] != null) {
                itemFrames = Main.itemAnimations[item.type].FrameCount;
            }
            float w = itemTexture.Width * item.scale;
            float h = itemTexture.Height * item.scale / itemFrames;

            var giantItemThreshold = ModContent.GetInstance<WeaponOutClientConfig>().GiantItemThreshold;
            var giantItemScale = ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
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
                return PoseGroup.GiantItem;
            }

            // √⭕ Yoyo, if it's in the set
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
                    // 🤺 Shortswords and Rapiers. Thrust is only used by umbrellas, but modded weapons may also use this.
                    if (item.useStyle == ItemUseStyleID.Thrust || item.useStyle == ItemUseStyleID.Rapier) {
                        // Skip attempt to classify here if the melee effects mod integration is active
                        // Since it sets all melee swords to be this type, I would rather use normal poses
                        if (item.useStyle == ItemUseStyleID.Rapier && 
                            WeaponOutLite.MeleeEffectsPlusModLoaded && 
                            ModContent.GetInstance<WeaponOutClientConfig>().ModIntegrationMeleeEffectsPlus) {
                            
                        }
                        else {
                            return PoseGroup.Rapier;
                        }

                    }

                    // Various shoot items can be categorised into melee weapons, unique (large) guns, and magic 
                    if (item.useStyle == ItemUseStyleID.Shoot) {
                        if (item.CountsAsClass(DamageClass.Melee)) {
                            // 🛠 Powertools are basically, very wide.
                            // The shortest drill is the Nebula Drill (54 x 30, 1.8:1)
                            // Short modded drills include Thorium.IllumiteDrill (50 x 30, 1.6:1),
                            if (h <= w * 0.8f) {
                                return PoseGroup.PowerTool;
                            }
                            else {
                                if (item.channel) {
                                    // Flails and jousting lances... though really more likely flails
                                    return PoseGroup.Flail;
                                }
                                else {
                                    // 🔱 Spears are diagonal sprites
                                    // Most rectangular spears are actually taller (usually for halberds and glaives, eg. The Rotted Fork)
                                    // A rare example of a wide spear is the Calamity.BansheeHook (120 x 108, 1.11: 1) 
                                    return PoseGroup.Spear;
                                }
                            }
                        }
                        // 🔫 Special graphic guns are wider than thrown weapons (eg. Celebration Mk2)
                        if (w > h && w > ModContent.GetInstance<WeaponOutClientConfig>().SmallGunThreshold) {
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

                    // ⛓ Whips are no graphic summon melee items
                    if (item.CountsAsClass(DamageClass.SummonMeleeSpeed)) {
                        return PoseGroup.Whips;
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
                    if (h >= w * 1.5f) {
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

                    // 🔪 small swinging weapons
                    if (h + w <= ModContent.GetInstance<WeaponOutClientConfig>().SmallSwordThreshold * 2) {
                        return PoseGroup.SmallMelee;
                    }
                    // ⚔ big swinging weapons
                    else {
                        return PoseGroup.LargeMelee;

                    }
                }

                // 🔫🏹📔 Guns and Bows and Books
                if (item.useStyle == ItemUseStyleID.Shoot) {

                    // 🔫 Gun sprites are much wider than they are tall
                    // The "tallest" gun is the Pew-Matic Horn (28 x 26, 1.07:1)
                    if (w > h) {
                        // ⭕ small guns
                        if (w <= ModContent.GetInstance<WeaponOutClientConfig>().SmallGunThreshold) {
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

            // 📦 Big items such as paintings and crates, too wide to be held to the side
            // Excludes items that, while wide, are quite thin, such as the snaker charmer's flute, platforms
            if (w > 20 && h > 14) {
                return PoseGroup.LargeItem;
            }

            // 💎 Small hand held items, like blocks, bombs
            return PoseGroup.Item;
        }
    }
}
