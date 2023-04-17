using Terraria;
using Terraria.ModLoader;
using WeaponOutLite.ID;
using System;
using Terraria.DataStructures;
using WeaponOutLite.Common.Players;

namespace WeaponOutLite
{
    partial class WeaponOutLite
    {
        public override object Call(params object[] args) {
            // Make sure the call doesn't include anything that could potentially cause exceptions.
            if (args is null) {
                throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
            }

            if (args.Length == 0) {
                throw new ArgumentException("Arguments cannot be empty!");
            }

            if (args[0] is string method) {
                PoseStyleID.PoseGroup style;
                bool arg1IsInt = int.TryParse(args[1].ToString(), out int arg1Int);

                switch (method) {
                    case "HidePlayerHeldItem":
                        if (args[1] is int) {
                            int whoAmI = (int)args[1];
                            if (whoAmI >= 0 && whoAmI < Main.player.Length) {
                                Main.player[whoAmI].GetModPlayer<WeaponOutPlayerRenderer>().showHeldItemThisFrame = false;
                            }
                        }
                        return false;
                    case "RegisterCustomItemStyle":
                        if (arg1IsInt) {
                            RegisterCustomItemStyle(arg1Int);
                            return true;
                        }
                        else
                        if (args[1] is Item) {
                            RegisterCustomItemStyle((Item)args[1]);
                            return true;
                        }
                        else
                        if (args[1] is ModItem) {
                            RegisterCustomItemStyle((ModItem)args[1]);
                            return true;
                        }
                        else
                        if (args[1] is int[]) {
                            RegisterCustomItemStyle((int[])args[1]);
                            return true;
                        }
                        return false;
                    case "RegisterCustomPreDrawData":
                        var funcPreDraw = args[1] as Func<Player, Item, DrawData, float, float, int, int, DrawData>;
                        if (funcPreDraw != null) {
                            RegisterCustomPreDrawData(funcPreDraw);
                            return true;
                        }
                        return false;
                    case "RegisterCustomDrawDepth":
                        var funcDrawDepth = args[1] as Func<Player, Item, short, int, short>;
                        if (funcDrawDepth != null) {
                            RegisterCustomDrawDepth(funcDrawDepth);
                            return true;
                        }
                        return false;
                    case "RegisterCustomUpdateIdleBodyFrame":
                        var funcIdleBodyFrame = args[1] as Func<Player, Item, int, int, int>;
                        if (funcIdleBodyFrame != null) {
                            RegisterCustomUpdateIdleBodyFrame(funcIdleBodyFrame);
                            return true;
                        }
                        return false;
                    case "RegisterBow":
                        style = PoseStyleID.PoseGroup.Bow;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterFlail":
                        style = PoseStyleID.PoseGroup.Flail;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterGun":
                        style = PoseStyleID.PoseGroup.Gun;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterGunManual":
                        style = PoseStyleID.PoseGroup.GunManual;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterItem":
                        style = PoseStyleID.PoseGroup.Item;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterLargeItem":
                        style = PoseStyleID.PoseGroup.LargeItem;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterLargeMelee":
                        style = PoseStyleID.PoseGroup.LargeMelee;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterLauncher":
                        style = PoseStyleID.PoseGroup.Launcher;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterMagicBook":
                        style = PoseStyleID.PoseGroup.MagicBook;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterMagicItem":
                        style = PoseStyleID.PoseGroup.MagicItem;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterPistol":
                        style = PoseStyleID.PoseGroup.Pistol;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterPotion":
                        style = PoseStyleID.PoseGroup.Potion;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterPowerTool":
                        style = PoseStyleID.PoseGroup.PowerTool;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterRapier":
                        style = PoseStyleID.PoseGroup.Rapier;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterRepeater":
                        style = PoseStyleID.PoseGroup.Repeater;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterSmallMelee":
                        style = PoseStyleID.PoseGroup.SmallMelee;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterSpear":
                        style = PoseStyleID.PoseGroup.Spear;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterStaff":
                        style = PoseStyleID.PoseGroup.Staff;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterThrown":
                        style = PoseStyleID.PoseGroup.Thrown;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterThrownThin":
                        style = PoseStyleID.PoseGroup.ThrownThin;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterWhips":
                        style = PoseStyleID.PoseGroup.Whips;
                        if (arg1IsInt) { RegisterItemStyle((short)args[1], style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                    case "RegisterYoyo":
                        style = PoseStyleID.PoseGroup.Yoyo;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;
                }
                throw new ArgumentException($"No Mod Call method '{method}' exists!");
            }
            throw new ArgumentException("First argument must be a string!");
        }

        private void RegisterCustomPreDrawData(Func<Player, Item, DrawData, float, float, int, int, DrawData> customFunc) {
            if (!customPreDrawDataFuncs.Contains(customFunc)) {
                customPreDrawDataFuncs.Add(customFunc);
            }
        }

        private void RegisterCustomDrawDepth(Func<Player, Item, short, int, short> customFunc) {
            if (customDrawDepthFuncs.Contains(customFunc)) return;
            customDrawDepthFuncs.Add(customFunc);
        }

        private void RegisterCustomUpdateIdleBodyFrame(Func<Player, Item, int, int, int> customFunc) {
            if (customUpdateIdleBodyFrameFuncs.Contains(customFunc)) return;
            customUpdateIdleBodyFrameFuncs.Add(customFunc);
        }

        private void RegisterCustomItemStyle(int itemType) {
            if (customItemHoldStyles.Contains(itemType)) {
                Logger.Warn($"Item Type {itemType} has already been registered by a mod.");
                return;
            }
            customItemHoldStyles.Add(itemType);
        }

        private void RegisterCustomItemStyle(Item item) {
            RegisterCustomItemStyle(item.type);
        }

        private void RegisterCustomItemStyle(ModItem modItem) {
            RegisterCustomItemStyle(modItem.Item.type);
        }

        private void RegisterCustomItemStyle(int[] itemTypeArray) {
            foreach (var itemType in itemTypeArray) {
                RegisterCustomItemStyle(itemType);
            }
        }


        private void RegisterItemStyle(int itemType, PoseStyleID.PoseGroup poseGroup) {
            if (customItemHoldGroups.Contains(itemType)) {
                Logger.Warn($"Item Type {itemType} has already been registered by a mod.");
                return;
            }
            customItemHoldGroups.Add(itemType, poseGroup);
        }

        private void RegisterItemStyle(Item item, PoseStyleID.PoseGroup poseGroup) {
            customItemHoldGroups.Add(item.type, poseGroup);
        }

        private void RegisterItemStyle(ModItem modItem, PoseStyleID.PoseGroup poseGroup) {
            customItemHoldGroups.Add(modItem.Item.type, poseGroup);
        }

        private void RegisterItemStyle(int[] itemTypeArray, PoseStyleID.PoseGroup poseGroup) {
            foreach (var itemType in itemTypeArray) {
                customItemHoldGroups.Add(itemType, poseGroup);
            }
        }
    }
}