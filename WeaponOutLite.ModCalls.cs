using Terraria;
using Terraria.ModLoader;
using WeaponOutLite.ID;
using System;
using Terraria.DataStructures;
using WeaponOutLite.Common.Players;
using static WeaponOutLite.ID.DrawItemPoseID;
using static WeaponOutLite.ID.PoseStyleID;
using System.Collections.Specialized;

namespace WeaponOutLite
{
    partial class WeaponOutLite
    {
        public override object Call(params object[] args)
        {
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
                    // HidePlayerHeldItem(int playerWhoAmI)
                    case "HidePlayerHeldItem":
                        if (args[1] is int)
                        {
                            int whoAmI = (int)args[1];
                            if (whoAmI >= 0 && whoAmI < Main.player.Length)
                            {
                                if (Main.player[whoAmI].TryGetModPlayer<WeaponOutPlayerRenderer>(out var modPlayer))
                                {
                                    modPlayer.showHeldItemThisFrame = false;
                                }
                            }
                        }
                        return false;

                    // RegisterCustomItemStyle(int itemTypes)
                    // RegisterCustomItemStyle(Item item)
                    // RegisterCustomItemStyle(ModItem modItem)
                    // RegisterCustomItemStyle(int[] itemTypeArray)
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

                    // RegisterCustomPreDrawData(Func<Player, Item, DrawData, float, float, int, int, DrawData> customPreDrawData)
                    // preDrawFunc = (Player p, Item i, DrawData data, float h, float w, int bodyFrame, int timer) => { return (DrawData)data; }
                    case "RegisterCustomPreDrawData":
                        var funcPreDraw = args[1] as Func<Player, Item, DrawData, float, float, int, int, DrawData>;
                        if (funcPreDraw != null) {
                            RegisterCustomPreDrawData(funcPreDraw);
                            return true;
                        }
                        return false;

                    // RegisterCustomDrawDepth(Func<Player, Item, short, int, short> customDrawDepth)
                    // (Player player, Item i, short drawDepth, int timer) => { return (short)drawDepth }
                    case "RegisterCustomDrawDepth":
                        var funcDrawDepth = args[1] as Func<Player, Item, short, int, short>;
                        if (funcDrawDepth != null) {
                            RegisterCustomDrawDepth(funcDrawDepth);
                            return true;
                        }
                        return false;

                    // RegisterCustomUpdateIdleBodyFrame(Func<Player, Item, int, int, int> customUpdateIdleBodyFrame)
                    // (Player p, Item i, int bodyFrame, int timer) => { return (int)bodyFrame; }
                    case "RegisterCustomUpdateIdleBodyFrame":
                        var funcIdleBodyFrame = args[1] as Func<Player, Item, int, int, int>;
                        if (funcIdleBodyFrame != null) {
                            RegisterCustomUpdateIdleBodyFrame(funcIdleBodyFrame);
                            return true;
                        }
                        return false;

                    // RegisterItemHoldPose(int itemType, string drawItemPoseName)
                    // RegisterItemHoldPose(Item item, string drawItemPoseName)
                    // RegisterItemHoldPose(ModItem modItem, string drawItemPoseName)
                    case "RegisterItemHoldPose":
                        if (arg1IsInt) { RegisterItemHoldPose(arg1Int, (string)args[2]); return true; }
                        if (args[1] is Item) { RegisterItemHoldPose((Item)args[1], (string)args[2]); return true; }
                        else if (args[1] is ModItem) { RegisterItemHoldPose((ModItem)args[1], (string)args[2]); return true; }
                        return false;

                    // RegisterBow(int itemType)
                    // RegisterBow(Item item)
                    // RegisterBow(ModItem modItem)
                    // RegisterBow(Item itemTypeArray)
                    case "RegisterBow":
                        style = PoseStyleID.PoseGroup.Bow;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterFlail(int itemType)
                    // RegisterFlail(Item item)
                    // RegisterFlail(ModItem modItem)
                    // RegisterFlail(Item itemTypeArray)
                    case "RegisterFlail":
                        style = PoseStyleID.PoseGroup.Flail;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterGun(int itemType)
                    // RegisterGun(Item item)
                    // RegisterGun(ModItem modItem)
                    // RegisterGun(Item itemTypeArray)
                    case "RegisterGun":
                        style = PoseStyleID.PoseGroup.Gun;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterGunManual(int itemType)
                    // RegisterGunManual(Item item)
                    // RegisterGunManual(ModItem modItem)
                    // RegisterGunManual(Item itemTypeArray)
                    case "RegisterGunManual":
                        style = PoseStyleID.PoseGroup.GunManual;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterShotgun(int itemType)
                    // RegisterShotgun(Item item)
                    // RegisterShotgun(ModItem modItem)
                    // RegisterShotgun(Item itemTypeArray)
                    case "RegisterShotgun":
                        style = PoseStyleID.PoseGroup.Shotgun;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterItem(int itemType)
                    // RegisterItem(Item item)
                    // RegisterItem(ModItem modItem)
                    // RegisterItem(Item itemTypeArray)
                    case "RegisterItem":
                        style = PoseStyleID.PoseGroup.Item;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterLargeItem(int itemType)
                    // RegisterLargeItem(Item item)
                    // RegisterLargeItem(ModItem modItem)
                    // RegisterLargeItem(Item itemTypeArray)
                    case "RegisterLargeItem":
                        style = PoseStyleID.PoseGroup.LargeItem;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterVanityItem(int itemType)
                    // RegisterVanityItem(Item item)
                    // RegisterVanityItem(ModItem modItem)
                    // RegisterVanityItem(Item itemTypeArray)
                    case "RegisterVanityItem":
                        style = PoseStyleID.PoseGroup.VanityItem;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterLargeMelee(int itemType)
                    // RegisterLargeMelee(Item item)
                    // RegisterLargeMelee(ModItem modItem)
                    // RegisterLargeMelee(Item itemTypeArray)
                    case "RegisterLargeMelee":
                        style = PoseStyleID.PoseGroup.LargeMelee;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterLargeTool(int itemType)
                    // RegisterLargeTool(Item item)
                    // RegisterLargeTool(ModItem modItem)
                    // RegisterLargeTool(Item itemTypeArray)
                    case "RegisterLargeTool":
                        style = PoseStyleID.PoseGroup.LargeMelee;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterLauncher(int itemType)
                    // RegisterLauncher(Item item)
                    // RegisterLauncher(ModItem modItem)
                    // RegisterLauncher(Item itemTypeArray)
                    case "RegisterLauncher":
                        style = PoseStyleID.PoseGroup.Launcher;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterMagicBook(int itemType)
                    // RegisterMagicBook(Item item)
                    // RegisterMagicBook(ModItem modItem)
                    // RegisterMagicBook(Item itemTypeArray)
                    case "RegisterMagicBook":
                        style = PoseStyleID.PoseGroup.MagicBook;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterMagicItem(int itemType)
                    // RegisterMagicItem(Item item)
                    // RegisterMagicItem(ModItem modItem)
                    // RegisterMagicItem(Item itemTypeArray)
                    case "RegisterMagicItem":
                        style = PoseStyleID.PoseGroup.MagicItem;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterPistol(int itemType)
                    // RegisterPistol(Item item)
                    // RegisterPistol(ModItem modItem)
                    // RegisterPistol(Item itemTypeArray)
                    case "RegisterPistol":
                        style = PoseStyleID.PoseGroup.Pistol;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterPotion(int itemType)
                    // RegisterPotion(Item item)
                    // RegisterPotion(ModItem modItem)
                    // RegisterPotion(Item itemTypeArray)
                    case "RegisterPotion":
                        style = PoseStyleID.PoseGroup.Potion;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterPowerTool(int itemType)
                    // RegisterPowerTool(Item item)
                    // RegisterPowerTool(ModItem modItem)
                    // RegisterPowerTool(Item itemTypeArray)
                    case "RegisterPowerTool":
                        style = PoseStyleID.PoseGroup.PowerTool;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterRapier(int itemType)
                    // RegisterRapier(Item item)
                    // RegisterRapier(ModItem modItem)
                    // RegisterRapier(Item itemTypeArray)
                    case "RegisterRapier":
                        style = PoseStyleID.PoseGroup.Rapier;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterRepeater(int itemType)
                    // RegisterRepeater(Item item)
                    // RegisterRepeater(ModItem modItem)
                    // RegisterRepeater(Item itemTypeArray)
                    case "RegisterRepeater":
                        style = PoseStyleID.PoseGroup.Repeater;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterSmallMelee(int itemType)
                    // RegisterSmallMelee(Item item)
                    // RegisterSmallMelee(ModItem modItem)
                    // RegisterSmallMelee(Item itemTypeArray)
                    case "RegisterSmallMelee":
                        style = PoseStyleID.PoseGroup.SmallMelee;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterSmallTool(int itemType)
                    // RegisterSmallTool(Item item)
                    // RegisterSmallTool(ModItem modItem)
                    // RegisterSmallTool(Item itemTypeArray)
                    case "RegisterSmallTool":
                        style = PoseStyleID.PoseGroup.SmallTool;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterSpear(int itemType)
                    // RegisterSpear(Item item)
                    // RegisterSpear(ModItem modItem)
                    // RegisterSpear(Item itemTypeArray)
                    case "RegisterSpear":
                        style = PoseStyleID.PoseGroup.Spear;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterStaff(int itemType)
                    // RegisterStaff(Item item)
                    // RegisterStaff(ModItem modItem)
                    // RegisterStaff(Item itemTypeArray)
                    case "RegisterStaff":
                        style = PoseStyleID.PoseGroup.Staff;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterThrown(int itemType)
                    // RegisterThrown(Item item)
                    // RegisterThrown(ModItem modItem)
                    // RegisterThrown(Item itemTypeArray)
                    case "RegisterThrown":
                        style = PoseStyleID.PoseGroup.Thrown;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterThrownThin(int itemType)
                    // RegisterThrownThin(Item item)
                    // RegisterThrownThin(ModItem modItem)
                    // RegisterThrownThin(Item itemTypeArray)
                    case "RegisterThrownThin":
                        style = PoseStyleID.PoseGroup.ThrownThin;
                        if (arg1IsInt) { RegisterItemStyle(arg1Int, style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterWhips(int itemType)
                    // RegisterWhips(Item item)
                    // RegisterWhips(ModItem modItem)
                    // RegisterWhips(Item itemTypeArray)
                    case "RegisterWhips":
                        style = PoseStyleID.PoseGroup.Whips;
                        if (arg1IsInt) { RegisterItemStyle((short)args[1], style); return true; }
                        if (args[1] is Item) { RegisterItemStyle((Item)args[1], style); return true; }
                        else if (args[1] is ModItem) { RegisterItemStyle((ModItem)args[1], style); return true; }
                        else if (args[1] is int[]) { RegisterItemStyle((int[])args[1], style); return true; }
                        return false;

                    // RegisterYoyo(int itemType)
                    // RegisterYoyo(Item item)
                    // RegisterYoyo(ModItem modItem)
                    // RegisterYoyo(Item itemTypeArray)
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

        private void RegisterCustomPreDrawData(Func<Player, Item, DrawData, float, float, int, int, DrawData> customFunc)
        {
            if (!customPreDrawDataFuncs.Contains(customFunc)) {
                customPreDrawDataFuncs.Add(customFunc);
            }
        }

        private void RegisterCustomDrawDepth(Func<Player, Item, short, int, short> customFunc)
        {
            if (customDrawDepthFuncs.Contains(customFunc)) return;
            customDrawDepthFuncs.Add(customFunc);
        }

        private void RegisterCustomUpdateIdleBodyFrame(Func<Player, Item, int, int, int> customFunc)
        {
            if (customUpdateIdleBodyFrameFuncs.Contains(customFunc)) return;
            customUpdateIdleBodyFrameFuncs.Add(customFunc);
        }

        private void RegisterCustomItemStyle(int itemType)
        {
            if (customItemHoldStyles.Contains(itemType)) {
                Logger.Warn($"Item Type {itemType} has already been registered by a mod.");
                return;
            }
            customItemHoldStyles.Add(itemType);
        }

        /// <summary>
        /// Accessor for customItemHoldStyles, for fully customised DrawItemPose
        /// </summary>
        private void RegisterCustomItemStyle(Item item)
        {
            RegisterCustomItemStyle(item.type);
        }


        /// <summary>
        /// Accessor for customItemHoldStyles, for fully customised DrawItemPose
        /// </summary>
        private void RegisterCustomItemStyle(ModItem modItem)
        {
            RegisterCustomItemStyle(modItem.Item.type);
        }


        /// <summary>
        /// Accessor for customItemHoldStyles, for fully customised DrawItemPose
        /// </summary>
        private void RegisterCustomItemStyle(int[] itemTypeArray)
        {
            foreach (var itemType in itemTypeArray) {
                RegisterCustomItemStyle(itemType);
            }
        }


        /// <summary>
        /// List of item ids that should prioritise a pose group, used in PoseSetClassifier before other selections
        /// This is for setting up items that don't work with the standard automatic style selection
        /// </summary>
        private void RegisterItemStyle(int itemType, PoseStyleID.PoseGroup poseGroup)
        {
            if (priorityItemHoldGroups.ContainsKey(itemType)) {
                Logger.Warn($"Item Type {itemType} has already been registered by a mod.");
                return;
            }
            priorityItemHoldGroups.Add(itemType, poseGroup);
        }

        /// <summary>
        /// Accessor for customItemHoldGroups, for PoseGroup
        /// </summary>
        private void RegisterItemStyle(Item item, PoseStyleID.PoseGroup poseGroup)
        {
            priorityItemHoldGroups.Add(item.type, poseGroup);
        }


        /// <summary>
        /// Accessor for customItemHoldGroups, for PoseGroup
        /// </summary>
        private void RegisterItemStyle(ModItem modItem, PoseStyleID.PoseGroup poseGroup)
        {
            priorityItemHoldGroups.Add(modItem.Item.type, poseGroup);
        }


        /// <summary>
        /// Accessor for customItemHoldGroups, for PoseGroup
        /// </summary>
        private void RegisterItemStyle(int[] itemTypeArray, PoseStyleID.PoseGroup poseGroup)
        {
            foreach (var itemType in itemTypeArray) {
                priorityItemHoldGroups.Add(itemType, poseGroup);
            }
        }

        /// <summary>
        /// List of item ids that should prioritise a pose, used in PoseSetClassifier before other selections
        /// This is for advanced mod integrations looking to specify preferences/behaviours on specific items
        /// </summary>
        private bool RegisterItemHoldPose(int itemType, string itemPoseName)
        {
            if (Enum.TryParse(itemPoseName, true, out DrawItemPose pose)) {
                priorityItemHoldPose.Add(itemType, pose);
                return true;
            }
            else {
                Logger.Warn($"Mod Call RegisterItemHoldPose: Item Type {itemType} was assigned '" + itemPoseName + "' which doesn't exist.");
                return false;
            }
        }

        /// <summary>
        /// Accessor for customItemHoldPose, for manually set DrawItemPose
        /// </summary>
        private bool RegisterItemHoldPose(Item item, string itemPoseName)
        {
            return RegisterItemHoldPose(item.type, itemPoseName);
        }

        /// <summary>
        /// Accessor for customItemHoldPose, for manually set DrawItemPose
        /// </summary>
        private bool RegisterItemHoldPose(ModItem modItem, string itemPoseName)
        {
            return RegisterItemHoldPose(modItem.Item.type, itemPoseName);
        }
    }
}