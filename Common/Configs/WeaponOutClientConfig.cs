using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using WeaponOutLite.Common.Players;
using WeaponOutLite.Compatibility;
using WeaponOutLite.ID;

namespace WeaponOutLite.Common.Configs
{
    // Unless set as an auto property, mod config will always place public fields at the top.
    // This causes order issues as other write options, such as getters and [JsonIgnore][ShowDespiteJsonIgnore], will get pushed to the bottom.
	public class WeaponOutClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("PlayerHeader")]
		#region ClientSidePlayerHeader

		[DefaultValue(true)]
		public bool ShowHeldItem { get; set; } // This is controlled by WeaponOutPlayerRenderer.cs, but the option is here for people who historically may have issues with that button for whatever reason. See OnChanged for link to class.

		[Increment(0.5f)]
		[Range(0, 10f)]
		[DefaultValue(3f)]
		[Slider]
		public float CombatDelayTimerMax { get; set; }

		[DefaultValue(false)]
		public bool CombatStanceAlwaysOn { get; set; }

        [DefaultValue(true)]
        public bool CombatStanceWhenHurt { get; set; }

        [DrawTicks]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.Hold)]
		public DrawItemPoseID.LabelledItemPose SmallItemPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewSmallItem))]
		public int SmallItemPosePV => (int)SmallItemPose;


		[DrawTicks]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.TwoHandCarry)]
		public DrawItemPoseID.LabelledItemPose LargeItemPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewLargeItem))]
		public int LargeItemPosePV => (int)LargeItemPose;


        [DrawTicks]
        [DefaultValue(DrawItemPoseID.LabelledItemPose.OffHand)]
        public DrawItemPoseID.LabelledItemPose VanityItemPose { get; set; }
        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [LabelKey("$Mods.WeaponOutLite.Common.Preview")]
        [CustomModConfigItem(typeof(PreviewVanityItem))]
        public int VanityItemPosePV => (int)VanityItemPose;


		[DrawTicks]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.HoldForward)]
		public DrawItemPoseID.LabelledItemPose PotionPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewPotionItem))]
		public int PotionPosePV => (int)PotionPose;


        [DrawTicks]
        [DefaultValue(DrawItemPoseID.LabelledItemPose.CombatReady)]
        public DrawItemPoseID.LabelledItemPose SmallToolPose { get; set; }
        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [LabelKey("$Mods.WeaponOutLite.Common.Preview")]
        [CustomModConfigItem(typeof(PreviewSmallTool))]
        public int SmallToolPV => (int)SmallToolPose;


        [DrawTicks]
        [DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandBerserk)]
        public DrawItemPoseID.LabelledItemPose LargeToolPose { get; set; }
        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [LabelKey("$Mods.WeaponOutLite.Common.Preview")]
        [CustomModConfigItem(typeof(PreviewLargeTool))]
        public int LargeToolPV => (int)LargeToolPose;


        [DrawTicks]
        [SliderColor(254, 159, 30)]
        [DefaultValue(DrawItemPoseID.LabelledItemPose.CombatReady)]
        public DrawItemPoseID.LabelledItemPose SmallMeleePose { get; set; }
        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        [LabelKey("$Mods.WeaponOutLite.Common.Preview")]
        [CustomModConfigItem(typeof(PreviewSmallMelee))]
        public int SmallMeleePV => (int)SmallMeleePose;


        [DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHand)]
		public DrawItemPoseID.LabelledItemPose LargeMeleePose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewLargeMelee))]
		public int LargeMeleePV => (int)LargeMeleePose;


        [DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatHold)]
		public DrawItemPoseID.LabelledItemPose RapierPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewRapier))]
		public int RapierPV => (int)RapierPose;


		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatPoleReady)]
		public DrawItemPoseID.LabelledItemPose SpearPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewSpear))]
		public int SpearPV => (int)SpearPose;


		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.Hold)]
		public DrawItemPoseID.LabelledItemPose YoyoPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewYoyo))]
		public int YoyoPV => (int)YoyoPose;

		[DefaultValue(true)]
		public bool YoyoHalfScale { get; set; }


		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatFlailReady)]
		public DrawItemPoseID.LabelledItemPose FlailPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewFlail))]
		public int FlailPV => (int)FlailPose;


		[DrawTicks]
		[SliderColor(104, 214, 255)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.Hold)]
		public DrawItemPoseID.LabelledItemPose WhipPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewWhip))]
		public int WhipPV => (int)WhipPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatUpright)]
        public DrawItemPoseID.LabelledItemPose ThrownPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewThrown))]
		public int ThrownPV => (int)ThrownPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.Hold)]
		public DrawItemPoseID.LabelledItemPose ThrownThinPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewThrownThin))]
		public int ThrownThinPV => (int)ThrownThinPose;


		[DrawTicks]
		[SliderColor(254, 159, 30)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandPowerTool)]
		public DrawItemPoseID.LabelledItemPose PowerToolPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewPowerTool))]
		public int PowerToolPV => (int)PowerToolPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandBow)]
        public DrawItemPoseID.LabelledItemPose BowPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewBow))]
		public int BowPV => (int)BowPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandRifleDown)]
        public DrawItemPoseID.LabelledItemPose RepeaterPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewRepeater))]
		public int RepeaterPV => (int)RepeaterPose;

		[DefaultValue(true)]
		public bool BowDrawAmmo { get; set; }


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.Hold)]
		public DrawItemPoseID.LabelledItemPose PistolPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewPistol))]
		public int PistolPV => (int)PistolPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandRifleHip)]
        public DrawItemPoseID.LabelledItemPose GunPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGun))]
		public int GunPV => (int)GunPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandBoltAction)]
		public DrawItemPoseID.LabelledItemPose GunManualPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGunManual))]
		public int GunManualPV => (int)GunManualPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandPumpAction)]
		public DrawItemPoseID.LabelledItemPose ShotgunPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewShotgun))]
		public int ShotgunPosePV => (int)ShotgunPose;


		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.OffHandShoulder)]
        public DrawItemPoseID.LabelledItemPose LauncherPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewLauncher))]
		public int LauncherPV => (int)LauncherPose;


		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.OffHandPole)]
        public DrawItemPoseID.LabelledItemPose StaffPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewStaff))]
		public int StaffPV => (int)StaffPose;


		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.HoldTome)]
		public DrawItemPoseID.LabelledItemPose MagicBookPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewMagicBook))]
		public int MagicBookPV => (int)MagicBookPose;


		[DrawTicks]
		[SliderColor(254, 127, 230)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.FloatingOffHand)]
		public DrawItemPoseID.LabelledItemPose MagicItemPose { get; set; }
		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewMagicItem))]
		public int MagicItemPV => (int)MagicItemPose;

		#endregion


		[Header("LocalGraphics")]
        #region LocalGraphics

		[DefaultValue(true)]
		public bool EnableWeaponPhysics { get; set; }

		[DefaultValue(true)]
		public bool EnableSheathingAnim { get; set; }

		[DefaultValue(true)]
		public bool EnableItemScaling { get; set; }

		[Increment(2)]
		[Range(0f, 64)]
		[DefaultValue(50f)]
		[Slider]
		[SliderColor(254, 159, 30)]
		public int SmallSwordThreshold { get; set; }

		[Increment(2)]
		[Range(0f, 64)]
		[DefaultValue(36f)]
		[Slider]
		[SliderColor(0, 242, 171)]
		public int SmallGunThreshold { get; set; }

		[Increment(2)]
		[Range(64, 256)]
		[DefaultValue(80f)]
		[Slider]
		public int GiantItemThreshold { get; set; } // For reference, the breaker blade's longest side is 80px * 1.1 scale

		[Increment(1f)]
		[Range(0f, 100f)]
		[DefaultValue(100f)]
		[Slider]
		public float GiantItemScalePercent { get; set; } // If still over the threshold, use the GIANT pose sets

		[DrawTicks]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.TwoHandCarry)]
		public DrawItemPoseID.LabelledItemPose GiantItemPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[CustomModConfigItem(typeof(PreviewGiantItem))]
		public int GiantItemPV => (int)GiantItemPose;

		[DrawTicks]
		[SliderColor(254, 159, 30)] // and summon weapons!
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandBerserk)]
		public DrawItemPoseID.LabelledItemPose GiantWeaponPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantMelee))]
		public int GiantMeleePV => (int)GiantWeaponPose;

		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandBow)]
		public DrawItemPoseID.LabelledItemPose GiantBowPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantBow))]
		public int GiantBowPV => (int)GiantBowPose;

		[DrawTicks]
		[SliderColor(0, 242, 171)]
		[DefaultValue(DrawItemPoseID.LabelledItemPose.CombatTwoHandRifleUp)]
		public DrawItemPoseID.LabelledItemPose GiantGunPose { get; set; }

		[JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantGun))]
		public int GiantGunPV => (int)GiantGunPose;

        [DrawTicks]
        [SliderColor(254, 127, 230)]
        [DefaultValue(DrawItemPoseID.LabelledItemPose.FloatingAimed)]
        public DrawItemPoseID.LabelledItemPose GiantMagicPose { get; set; } // and summon items!

        [JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantMagic))]
		public int GiantMagicPV => (int)GiantMagicPose;

        [DrawTicks]
        [DefaultValue(DrawItemPoseID.LabelledItemPose.OffHandPole)]
        public DrawItemPoseID.LabelledItemPose GiantDamagingPose { get; set; } // for things like calamity throwing weapons

        [JsonIgnore][ShowDespiteJsonIgnore]
		[LabelKey("$Mods.WeaponOutLite.Common.Preview")]
		[CustomModConfigItem(typeof(PreviewGiantDamaging))]
		public int GiantDamagingPosePV => (int)GiantDamagingPose;

        #endregion

        /*
		 Experimental because the code in here may have issues when introduced to modded content
		 */
        #region ClientSideExperimental

        [DefaultValue(true)]
		public bool EnableMenuDisplay { get; set; }

		[DefaultValue(true)]
		public bool EnableProjSpears { get; set; }

        [DefaultValue(true)]
        public bool EnableProjYoyos { get; set; }


        [JsonIgnore]
        /* The idea here was to emulate the light and dust effects from various melee weapons 
         * but there are so many different implementations that it's unlikely to work any time soon. So ignore hide setting for now */
        [DefaultValue(false)]
		public bool EnableMeleeEffects { get; set; }

		#endregion


		[Header("ModIntegrationHeader")]
		#region ModIntegrations

		// Toggles
		[DefaultValue(true)]
        public bool ModIntegrationTerrariaOverhaul { get; set; }

        [DefaultValue(true)]
        public bool ModIntegrationMeleeEffectsPlus { get; set; }

        [DefaultValue(true)]
        public bool ModIntegrationInsurgencyWeapons { get; set; }

        // Manual changes required / Unsupported
        [JsonIgnore][ShowDespiteJsonIgnore]
        public bool ModIntegrationOverhaulGunAnimations { get { return false; } }

        [JsonIgnore][ShowDespiteJsonIgnore]
        public bool ModIntegrationArmamentDisplay { get { return false; } }

        [JsonIgnore][ShowDespiteJsonIgnore]
        public bool ModIntegrationCoolerItemVisualEffect { get { return false; } }

		// Automatic
        [JsonIgnore][ShowDespiteJsonIgnore]
        public bool ModIntegrationVibrantReverie { get { return VibrantReverie.Found; } }

        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        public bool ModIntegrationThoriumMod { get { return ThoriumMod.Found; } }

        [JsonIgnore][ShowDespiteJsonIgnore]
        public bool ModIntegrationArmamentDisplayLite { get { return WeaponDisplayLite.Found; } }

        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        public bool ModIntegrationCalamityMod { get { return CalamityMod.Found; } }

        [JsonIgnore]
        [ShowDespiteJsonIgnore]
        public bool ModIntegrationMetroidMod { get { return MetroidMod.Found; } }

        #endregion

        public override void OnChanged() {
			// In the main menu, the local player is a default object with a blank name which cannot be made in-game
			if (Main.LocalPlayer.name == "") return;

			if (Main.LocalPlayer.TryGetModPlayer<WeaponOutPlayerRenderer>(out var modPlayer))
			{
                modPlayer.isShowingHeldItem = this.ShowHeldItem;
            }
			else
			{
                // No mod player currently assigned to a local player (ie. still in the main menu)
                return;
            }
		}

	}

}