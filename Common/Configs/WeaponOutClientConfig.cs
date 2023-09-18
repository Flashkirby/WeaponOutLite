using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using WeaponOutLite.Common.Players;
using WeaponOutLite.ID;

namespace WeaponOutLite.Common.Configs
{
    // Unless set as an auto property, mod config will always place public fields at the top.
    // This causes order issues as other write options, such as getters and [JsonIgnore][ShowDespiteJsonIgnore], will get pushed to the bottom.
    [Label("$Mods.WeaponOut.Config.ClientSideConfigLabel")]
	public class WeaponOutClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("$Mods.WeaponOut.Config.ClientSidePlayerHeader")]
		#region ClientSidePlayerHeader

		[Label("$Mods.WeaponOut.Config.ShowHeldItem.Label")]
		[Tooltip("$Mods.WeaponOut.Config.ShowHeldItem.Tooltip")]
		[DefaultValue(true)]
		public bool ShowHeldItem { get; set; } // This is controlled by WeaponOutPlayerRenderer.cs, but the option is here for people who historically may have issues with that button for whatever reason. See OnChanged for link to class.

		[Label("$Mods.WeaponOut.Config.CombatDelayTimerMax.Label")]
		[Tooltip("$Mods.WeaponOut.Config.CombatDelayTimerMax.Tooltip")]
		[Increment(0.5f)]
		[Range(0, 10f)]
		[DefaultValue(3f)]
		[Slider]
		public float CombatDelayTimerMax { get; set; }

		[Label("$Mods.WeaponOut.Config.CombatStanceAlwaysOn.Label")]
		[DefaultValue(false)]
		public bool CombatStanceAlwaysOn { get; set; }

		[Label("$Mods.WeaponOut.Config.SmallItemPose.Label")]
		[DrawTicks]
		[DefaultValue(PoseStyleID.ItemPoseID.Hold)]
		public PoseStyleID.ItemPoseID SmallItemPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewSmallItem))]
		public int SmallItemPosePV => (int)SmallItemPose;

		[Label("$Mods.WeaponOut.Config.LargeItemPose.Label")]
		[DrawTicks]
		[DefaultValue(PoseStyleID.LargeItemPoseID.Carry)]
		public PoseStyleID.LargeItemPoseID LargeItemPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewLargeItem))]
		public int LargeItemPosePV => (int)LargeItemPose;

		[Label("$Mods.WeaponOut.Config.PotionPose.Label")]
		[DrawTicks]
		[DefaultValue(PoseStyleID.PotionPoseID.HoldForward)]
		public PoseStyleID.PotionPoseID PotionPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewPotionItem))]
		public int PotionPosePV => (int)PotionPose;

		[Label("Melee Weapons")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.SmallMeleePoseID.Combat_Ready)]
		public PoseStyleID.SmallMeleePoseID SmallMeleePose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewSmallMelee))]
		public int SmallMeleePV => (int)SmallMeleePose;

		[Label("$Mods.WeaponOut.Config.LargeMeleePose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.LargeMeleePoseID.Combat_Two_Hand)]
		public PoseStyleID.LargeMeleePoseID LargeMeleePose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewLargeMelee))]
		public int LargeMeleePV => (int)LargeMeleePose;

		[Label("$Mods.WeaponOut.Config.RapierPose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.RapierPoseID.Combat_Hold)]
		public PoseStyleID.RapierPoseID RapierPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewRapier))]
		public int RapierPV => (int)RapierPose;

		[Label("$Mods.WeaponOut.Config.SpearPose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.SpearPoseID.Combat_Pole_Ready)]
		public PoseStyleID.SpearPoseID SpearPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewSpear))]
		public int SpearPV => (int)SpearPose;

		[Label("$Mods.WeaponOut.Config.YoyoPose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.YoyoPoseID.Hold)]
		public PoseStyleID.YoyoPoseID YoyoPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewYoyo))]
		public int YoyoPV => (int)YoyoPose;

		[Label("$Mods.WeaponOut.Config.YoyoHalfScale")]
		[DefaultValue(true)]
		public bool YoyoHalfScale { get; set; }

		[Label("$Mods.WeaponOut.Config.FlailPose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.FlailPoseID.Combat_Ready)]
		public PoseStyleID.FlailPoseID FlailPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewFlail))]
		public int FlailPV => (int)FlailPose;

		[Label("$Mods.WeaponOut.Config.WhipPose")]
		[DrawTicks]
		[SliderColor(104, 214, 255)]
		[DefaultValue(PoseStyleID.WhipPoseID.Hold)]
		public PoseStyleID.WhipPoseID WhipPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewWhip))]
		public int WhipPV => (int)WhipPose;

		[Label("$Mods.WeaponOut.Config.ThrownPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.ThrownPoseID.Combat_Upright)]
		public PoseStyleID.ThrownPoseID ThrownPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewThrown))]
		public int ThrownPV => (int)ThrownPose;

		[Label("$Mods.WeaponOut.Config.ThrownThinPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.ThrownPoseID.Hold)]
		public PoseStyleID.ThrownPoseID ThrownThinPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewThrownThin))]
		public int ThrownThinPV => (int)ThrownThinPose;

		[Label("$Mods.WeaponOut.Config.PowerToolPose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(PoseStyleID.PowerToolPoseID.Combat_Power_Tool)]
		public PoseStyleID.PowerToolPoseID PowerToolPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewPowerTool))]
		public int PowerToolPV => (int)PowerToolPose;

		[Label("$Mods.WeaponOut.Config.BowPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.BowPoseID.Combat_Hunter)]
		public PoseStyleID.BowPoseID BowPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewBow))]
		public int BowPV => (int)BowPose;

		[Label("$Mods.WeaponOut.Config.RepeaterPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.GunPoseID.Combat_Ready)]
		public PoseStyleID.GunPoseID RepeaterPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewRepeater))]
		public int RepeaterPV => (int)RepeaterPose;

		[Label("$Mods.WeaponOut.Config.BowDrawAmmo")]
		[DefaultValue(true)]
		public bool BowDrawAmmo { get; set; }

		[Label("$Mods.WeaponOut.Config.PistolPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.PistolPoseID.Hold)]
		public PoseStyleID.PistolPoseID PistolPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewPistol))]
		public int PistolPV => (int)PistolPose;

		[Label("$Mods.WeaponOut.Config.GunPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.GunPoseID.Combat_Hip_Ready)]
		public PoseStyleID.GunPoseID GunPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGun))]
		public int GunPV => (int)GunPose;

		[Label("$Mods.WeaponOut.Config.GunManualPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.GunPoseID.Combat_Bolt_Action)]
		public PoseStyleID.GunPoseID GunManualPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGunManual))]
		public int GunManualPV => (int)GunManualPose;

		[Label("$Mods.WeaponOut.Config.ShotgunPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.GunPoseID.Combat_Pump_Action)]
		public PoseStyleID.GunPoseID ShotgunPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewShotgun))]
		public int ShotgunPosePV => (int)ShotgunPose;

		[Label("$Mods.WeaponOut.Config.LauncherPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.LauncherPoseID.OffHand_Shoulder)]
		public PoseStyleID.LauncherPoseID LauncherPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewLauncher))]
		public int LauncherPV => (int)LauncherPose;

		[Label("$Mods.WeaponOut.Config.StaffPose")]
		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(PoseStyleID.StaffPoseID.PoleOffHand)]
		public PoseStyleID.StaffPoseID StaffPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewStaff))]
		public int StaffPV => (int)StaffPose;

		[Label("$Mods.WeaponOut.Config.MagicBookPose")]
		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(PoseStyleID.MagicBookPoseID.HoldTome)]
		public PoseStyleID.MagicBookPoseID MagicBookPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewMagicBook))]
		public int MagicBookPV => (int)MagicBookPose;

		[Label("$Mods.WeaponOut.Config.MagicItemPose")]
		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(PoseStyleID.MagicItemPoseID.Levitate_OffHand)]
		public PoseStyleID.MagicItemPoseID MagicItemPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewMagicItem))]
		public int MagicItemPV => (int)MagicItemPose;

		#endregion


		[Header("$Mods.WeaponOut.Config.ClientSideGraphicHeader")]
		#region ClientSideGraphicHeader

		[Label("$Mods.WeaponOut.Config.EnableWeaponPhysics.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableWeaponPhysics.Tooltip")]
		[DefaultValue(true)]
		public bool EnableWeaponPhysics { get; set; }

		[Label("$Mods.WeaponOut.Config.EnableSheathingAnim.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableSheathingAnim.Tooltip")]
		[DefaultValue(true)]
		public bool EnableSheathingAnim { get; set; }

		[Label("$Mods.WeaponOut.Config.EnableItemScaling.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableItemScaling.Tooltip")]
		[DefaultValue(true)]
		public bool EnableItemScaling { get; set; }

		[Label("$Mods.WeaponOut.Config.SmallSwordThreshold.Label")]
		[Tooltip("$Mods.WeaponOut.Config.SmallSwordThreshold.Tooltip")]
		[Increment(2)]
		[Range(0f, 64)]
		[DefaultValue(50f)]
		[Slider]
		[SliderColor(254, 159, 30)]
		public int SmallSwordThreshold { get; set; }

		[Label("$Mods.WeaponOut.Config.SmallGunThreshold.Label")]
		[Tooltip("$Mods.WeaponOut.Config.SmallGunThreshold.Tooltip")]
		[Increment(2)]
		[Range(0f, 64)]
		[DefaultValue(36f)]
		[Slider]
		[SliderColor(0, 242, 171)]
		public int SmallGunThreshold { get; set; }

		[Label("$Mods.WeaponOut.Config.GiantItemThreshold.Label")]
		[Tooltip("$Mods.WeaponOut.Config.GiantItemThreshold.Tooltip")]
		[Increment(2)]
		[Range(64, 256)]
		[DefaultValue(80f)]
		[Slider]
		public int GiantItemThreshold { get; set; } // For reference, the breaker blade's longest side is 80px * 1.1 scale

		[Label("$Mods.WeaponOut.Config.GiantItemScalePercent.Label")]
		[Tooltip("$Mods.WeaponOut.Config.GiantItemScalePercent.Tooltip")]
		[Increment(1f)]
		[Range(0f, 100f)]
		[DefaultValue(100f)]
		[Slider]
		public float GiantItemScalePercent { get; set; } // If still over the threshold, use the GIANT pose sets

		[Label("$Mods.WeaponOut.Config.GiantItemPose")]
		[DrawTicks]
		[DefaultValue(PoseStyleID.GiantItemPoseID.Carry)]
		public PoseStyleID.GiantItemPoseID GiantItemPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantItem))]
		public int GiantItemPV => (int)GiantItemPose;

		[Label("$Mods.WeaponOut.Config.GiantWeaponPose")]
		[DrawTicks]
		[SliderColor(254, 159, 30)] // and summon weapons!
		[DefaultValue(PoseStyleID.GiantMeleePoseID.Combat_Two_Hand_Berserk)]
		public PoseStyleID.GiantMeleePoseID GiantWeaponPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantMelee))]
		public int GiantMeleePV => (int)GiantWeaponPose;

		[Label("$Mods.WeaponOut.Config.GiantBowPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.GiantBowPoseID.Combat_Hunter)]
		public PoseStyleID.GiantBowPoseID GiantBowPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantBow))]
		public int GiantBowPV => (int)GiantBowPose;

		[Label("$Mods.WeaponOut.Config.GiantGunPose")]
		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(PoseStyleID.GiantGunPoseID.OffHand_Shoulder)]
		public PoseStyleID.GiantGunPoseID GiantGunPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantGun))]
		public int GiantGunPV => (int)GiantGunPose;

		[Label("$Mods.WeaponOut.Config.GiantMagicPose")] // and summon items!
		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(PoseStyleID.GiantMagicPoseID.Levitate_Aimed)]
		public PoseStyleID.GiantMagicPoseID GiantMagicPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[Label("$Mods.WeaponOut.Config.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantMagic))]
		public int GiantMagicPV => (int)GiantMagicPose;

		#endregion

		#region ClientSideExperimental

		[Label("$Mods.WeaponOut.Config.EnableMenuDisplay.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableMenuDisplay.Tooltip")]
		[DefaultValue(true)]
		public bool EnableMenuDisplay { get; set; }

		[Label("$Mods.WeaponOut.Config.EnableMeleeEffects.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableMeleeEffects.Tooltip")]
		[DefaultValue(false)]
		public bool EnableMeleeEffects { get; set; }

		[Label("$Mods.WeaponOut.Config.EnableProjSpears.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableProjSpears.Tooltip")]
		[DefaultValue(true)]
		public bool EnableProjSpears { get; set; }

		#endregion


		[Header("$Mods.WeaponOut.Config.ModIntegrationHeader")]
		#region ModIntegrations

		[Label("$Mods.WeaponOut.Config.ModIntegrationTerrariaOverhaul.Label")]
		[Tooltip("$Mods.WeaponOut.Config.ModIntegrationTerrariaOverhaul.Tooltip")]
		[DefaultValue(true)]

        public bool ModIntegrationTerrariaOverhaul { get; set; }

		#endregion

		public override void OnChanged() {
			// In the main menu, the local player is a default object with a blank name which cannot be made in-game
			if (Main.LocalPlayer.name == "") return;

			try {
				Main.LocalPlayer.GetModPlayer<WeaponOutPlayerRenderer>().isShowingHeldItem = this.ShowHeldItem;
			}
			catch {
				// No mod player currently assigned to a local player (ie. still in the main menu)
				return;
			}
		}

	}

}