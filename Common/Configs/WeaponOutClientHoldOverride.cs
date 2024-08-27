using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using WeaponOutLite.ID;
using WeaponOutLite.Common.GlobalDrawItemPose;
using Terraria.ModLoader;

namespace WeaponOutLite.Common.Configs
{
    public class WeaponOutClientHoldOverride : ModConfig
    {
        private HybridDictionary styleOverrideItemCache;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [DrawTicks]
        public PoseStyleID.PoseGroup CurrentPoseGroup
        {
            get {
                if (Main.LocalPlayer != null && Main.LocalPlayer.HeldItem != null) {
                    // Get pose group for display
                    PoseSetClassifier.GetItemPoseGroupData(Main.LocalPlayer.HeldItem, out PoseStyleID.PoseGroup currentPoseGroup, out _);
                    return currentPoseGroup;
                }
                return PoseStyleID.PoseGroup.Unassigned;
            }
        }

        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [DrawTicks]
        public DrawItemPoseID.DrawItemPose CurrentDrawItemPose
        {
            get {
                if (Main.LocalPlayer != null && Main.LocalPlayer.HeldItem != null) {
                    return (DrawItemPoseID.DrawItemPose)PoseSetClassifier.SelectItemPose(Main.LocalPlayer, Main.LocalPlayer.HeldItem).GetID();
                }
                return DrawItemPoseID.DrawItemPose.Unassigned;
            }
        }

        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [Label("$Mods.WeaponOutLite.Common.Preview")]
        [CustomModConfigItem(typeof(PreviewHeldItem))]
        public int CurrentItemPosePV => (int)CurrentDrawItemPose;


        private List<ItemDrawOverrideData> styleOverrideList;

        /// <summary>
        /// Note: This could have been done with a Dictionary(item,style),
        /// but I want to specify the behaviour of not allowing nulls and
        /// allowing duplicates, but only to accept the latest one.
        /// 
        /// Could also be done with a dictionary wrapper but ew sensible.
        /// </summary>
        public List<ItemDrawOverrideData> StyleOverrideList
        {
            get { return styleOverrideList; }
            set {
                styleOverrideList = CleanStyleOverrideListAndCache(value);
            }
        }

        /// <summary>
        /// Use the internal dicionary to fetch the StyleOverrideData, if one exists.
        /// </summary>
        /// <param name="itemType">item.type</param>
        /// <returns>StyelOverrideData or DEATH (null)</returns>
        public ItemDrawOverrideData FindStyleOverride(int itemType)
        {
            return styleOverrideItemCache[itemType] as ItemDrawOverrideData;
        }

        public override void OnLoaded()
        {
            styleOverrideItemCache = new HybridDictionary();
        }

        public override void OnChanged()
        {
            if (styleOverrideList == null) { //restore defaults 
                styleOverrideList = new List<ItemDrawOverrideData>();
                styleOverrideItemCache = new HybridDictionary();
            }
        }

        private List<ItemDrawOverrideData> CleanStyleOverrideListAndCache(List<ItemDrawOverrideData> list)
        {
            // If the cache is not ready yet, don't clean
            if (styleOverrideItemCache == null || list == null) return list;

            styleOverrideItemCache.Clear();
            List<ItemDrawOverrideData> cleanDataList = new List<ItemDrawOverrideData>();

            foreach (var data in list) {
                if (data == null || data.Item == null) { continue; }

                try {
                    cleanDataList.Add(data);
                    styleOverrideItemCache.Add(data.Item?.Type, data);
                }
                catch (ArgumentNullException e) { // key is null
                    cleanDataList.Remove(data);
                    Mod.Logger.Warn($"WeaponOut: Ignoring null Item. ({e.Message})");
                }
                catch (ArgumentException e) { // An entry with the same key already exists 
                    // remove old one, use newest in the list
                    ItemDrawOverrideData oldData = styleOverrideItemCache[data.Item.Type] as ItemDrawOverrideData;
                    cleanDataList.Remove(oldData);

                    styleOverrideItemCache.Remove(oldData.Item.Type);
                    styleOverrideItemCache.Add(data.Item.Type, data);

                    Mod.Logger.Warn($"WeaponOut: Item style already found for {data.Item.Mod}.{data.Item.Name}, using newest. ({e.Message})");
                }
            }

            return cleanDataList;
        }

        public WeaponOutClientHoldOverride()
        {
            // Set the default style datas
            styleOverrideList = new List<ItemDrawOverrideData>();

            // Example custom override for Terragrim
            styleOverrideList.Add(new ItemDrawOverrideData() {
                //ItemID.EmpressBlade
                Item = ItemDefinition.FromString("EmpressBlade"),
                ForceDrawItemPose = DrawItemPoseID.DrawItemPose.FloatingOffHandAimed
            });

            // Special hold for lances
            styleOverrideList.Add(new ItemDrawOverrideData() {
                Item = ItemDefinition.FromString("JoustingLance"),
                ForcePoseGroup = PoseStyleID.PoseGroup.Spear,
                ForceDrawItemPose = DrawItemPoseID.DrawItemPose.JoustingLance
            });
            styleOverrideList.Add(new ItemDrawOverrideData() {
                Item = ItemDefinition.FromString("ShadowJoustingLance"),
                ForcePoseGroup = PoseStyleID.PoseGroup.Spear,
                ForceDrawItemPose = DrawItemPoseID.DrawItemPose.JoustingLance
            });
            styleOverrideList.Add(new ItemDrawOverrideData() {
                Item = ItemDefinition.FromString("HallowJoustingLance"),
                ForcePoseGroup = PoseStyleID.PoseGroup.Spear,
                ForceDrawItemPose = DrawItemPoseID.DrawItemPose.JoustingLance
            });
        }
    }

    /// <summary>
    /// Selection data object for players to define the group, or pose, to override a weapon with.
    /// </summary>
    public class ItemDrawOverrideData
    {
        [DefaultValue(null)]
        public ItemDefinition Item;


        [DrawTicks]
        [DefaultValue(PoseStyleID.PoseGroup.Unassigned)]
        public PoseStyleID.PoseGroup ForcePoseGroup;

        [DrawTicks]
        [DefaultValue(DrawItemPoseID.DrawItemPose.Unassigned)]
        public DrawItemPoseID.DrawItemPose ForceDrawItemPose;

        public ItemDrawOverrideData()
        {
            // If the player is active and in-game, default to use what the player is holding
            if(Main.LocalPlayer != null && Main.LocalPlayer.HeldItem != null && Item == null) {
                Item = new ItemDefinition(Main.LocalPlayer.HeldItem.type);
            }
        }

        public override string ToString() {
            if(Item == null) return "-";
            string pre = $"[i/s1:{Item.Type}]  {Item.Name}";
            if (Item.Type == 0) {
                pre = Language.GetTextValue("Workshop.PreviewImagePathEmpty");
            }
            if (ForceDrawItemPose != DrawItemPoseID.DrawItemPose.Unassigned) {
                return pre + $" = {ForceDrawItemPose}";
            }
            if (ForcePoseGroup != PoseStyleID.PoseGroup.Unassigned) {
                return pre + $" = {ForcePoseGroup}";
            }
            return pre;
        }

        public override bool Equals(object obj) {
            if (obj is ItemDrawOverrideData other)
                return Item?.Type == other.Item?.Type
                    && ForcePoseGroup == other.ForcePoseGroup
                    && ForceDrawItemPose == other.ForceDrawItemPose;
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return new { Item?.Type, ForcePoseGroup , ForceDrawItemPose}.GetHashCode();
        }
    }
}
