using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.Compatibility;

namespace WeaponOutLite.Common.Players
{
    public class WeaponOutPlayerRenderer : ModPlayer
	{
		public int id;
		public int MyStat;
		public WeaponOutPlayerRenderer()
        {
			id = GetHashCode();
        }

        /// <summary>
        /// Value for player showing an item held
        /// </summary>
        public bool isShowingHeldItem = true;

		public bool showHeldItemThisFrame = true;

		/// <summary> 
		/// Many custom draw mods are modifying the arm. 
		/// If this value isn't the one we set, assume another mod is doing something here, and back off.
		/// </summary>
		internal float? expectedFrontArmThisFrame = null;

		/// <summary>
		/// Draw style of the currently held item. If null, the layer renderer won't run.
		/// </summary>
		public IDrawItemPose CurrentDrawItemPose;

		/// <summary>
		/// How many ticks before returning to relaxed pose.
		/// </summary>
		public int CombatDelayTimer;

		/// <summary>
		/// Does the player have showing items toggled on?
		/// If force show is enabled, this will always return true, but won't affect the internal value.
		/// get and set logic will fetch/assign based on config for local player, and send net updates if it changed
		/// </summary>
		public bool IsShowingHeldItem
		{
			get {
				if (ModContent.GetInstance<WeaponOutServerConfig>().EnableForcedWeaponOutVisuals) {
					return true;
                }
				if (Player == Main.LocalPlayer) {
					bool configShowHeldItem = ModContent.GetInstance<WeaponOutClientConfig>().ShowHeldItem;
					if (isShowingHeldItem != configShowHeldItem) {
						ModContent.GetInstance<WeaponOutClientConfig>().ShowHeldItem = isShowingHeldItem;
					}
					return isShowingHeldItem;
				}
				return isShowingHeldItem;
			}
            set {
				isShowingHeldItem = value;
				if (Player == Main.LocalPlayer) {
					ModContent.GetInstance<WeaponOutClientConfig>().ShowHeldItem = isShowingHeldItem;
					((WeaponOutLite)Mod).SendUpdateWeaponVisual(this);
				}
			}
		}

        /// <summary>
        /// Final checks for if the held item should be drawn this frame, eg. if attacking
        /// </summary>
        public bool DrawHeldItem
		{
			get {
                return IsShowingHeldItem && showHeldItemThisFrame &&
					(   // front arm hasn't been modified by another mod 
						(!Player.compositeFrontArm.enabled && expectedFrontArmThisFrame == null) ||
						(Player.compositeFrontArm.enabled && expectedFrontArmThisFrame == Player.compositeFrontArm.rotation)
					) &&
					!Main.gameMenu && // Not in game menu ie. select screen
					Player.active && // active player slot
					!Player.dead && // alive
					!Player.stoned && // unpetrified
					Player.itemAnimation <= 0; // not swinging
			}

        }

		/// <summary>
		/// Current body frame
		/// </summary>
		public int BodyFrameNum
		{
			get { return Player.bodyFrame.Y / Player.bodyFrame.Height; }
			set { Player.bodyFrame.Y = value * Player.bodyFrame.Height; }
		}

		/// <summary>
		/// Main variable for determining held item. For safe access both in menus and in game
		/// </summary>
		public Item HeldItem => Main.gameMenu && gameMenuItem != null ? gameMenuItem : Player.HeldItem;

		/// <summary>
		/// Item is not fully loaded during game menu draw. We wait for this to be loaded before we register it as a Held Item
		/// </summary>
		internal Item gameMenuItem;

        public override void OnEnterWorld()
        {
			if (Main.netMode != NetmodeID.SinglePlayer) {
				// Fixes BUG20231026A
				((WeaponOutLite)Mod).SendUpdateWeaponVisual(this);
			}
        }

		public override void ResetEffects()
        {
			expectedFrontArmThisFrame = null;
            showHeldItemThisFrame = true;

            // Mod Integrations
            ModCompatibleHideItem();
        }

		private void ModCompatibleHideItem()
		{
			var config = ModContent.GetInstance<WeaponOutClientConfig>();
			var item = HeldItem;

			// Terraria Overhaul Integration
			if (TerrariaOverhaul.Found && config.ModIntegrationTerrariaOverhaul)
			{
				// Basic implementation of ShouldForceUseAnim to prevent visual conflicts
				// https://github.com/Mirsario/TerrariaOverhaul/blob/dev/Common/EntityEffects/PlayerHoldOutAnimation.cs#L118

				if (item.noUseGraphic ||
					item.useStyle != ItemUseStyleID.Shoot)
				{
					// return
				}
				else
				{
					WeaponOutLite.GetMod().Call("HidePlayerHeldItem", Player.whoAmI);
					return;
				}
			}

			if (InsurgencyWeapons.Found && config.ModIntegrationInsurgencyWeapons)
			{
				// Insurgency has its own render system for weapons - but this can be disabled in its own config.
				if (InsurgencyWeapons.HasItem(item))
				{
					WeaponOutLite.GetMod().Call("HidePlayerHeldItem", Player.whoAmI);
					return;
				}
			}

			if (CalamityMod.Found && CalamityMod.HasItem(item))
			{
                // Something weird is happening with the way the animation is set during the charge attack.
				if (item.ModItem?.Name == "TheFinalDawn")
                {
                    bool attackFrames = BodyFrameNum >= 1 && BodyFrameNum <= 4;
					if (attackFrames)
                    {
                        WeaponOutLite.GetMod().Call("HidePlayerHeldItem", Player.whoAmI);
                        return;
                    }
                }
            }
		}


		public override void PostUpdate()
        {
            // This is for displaying items in-game
            if (ModContent.GetInstance<WeaponOutServerConfig>().EnableWeaponOutVisuals) {

                manageCombatTimer();

                if (DrawHeldItem && !Main.dedServ) {

                    if (Player == Main.LocalPlayer) {
                        manageHoldStyle();
                    }

                    manageBodyFrame();
                }
            }
        }

        public override void FrameEffects()
        {
			if (Main.gameMenu && ModContent.GetInstance<WeaponOutClientConfig>().EnableMenuDisplay) {
				// This is for displaying items in the menu. Pull this from the main menu once loaded
				if (gameMenuItem == null) {
					gameMenuItem = Player.HeldItem;
				}

				CombatDelayTimer = 0;

				CurrentDrawItemPose = PoseSetClassifier.SelectItemPose(Player, HeldItem);
				manageBodyFrame();
			}
			else {
				gameMenuItem = null;
            }
        }

		public override void OnHurt(Player.HurtInfo info)
		{
			if (ModContent.GetInstance<WeaponOutClientConfig>().CombatStanceWhenHurt)
			{
                manageCombatTimer(combatPoseTriggered: true);
            }
		}

        // Called on entering a server,with toWho=-1, fromWho=-1, newPlayer=true
        // Server receives toWho=-1, fromWho=0 (player id 0)
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			// Send a message out to the server when a sync is required, such as a new player joining the server
			MyStat = Main.rand.Next();
			if (fromWho == -1) {
				if(WeaponOutLite.DEBUG_MULTIPLAYER) Main.NewText($"Send update {Player.whoAmI} | to {toWho} | from {fromWho} | new {newPlayer}");
				((WeaponOutLite)Mod).SendUpdateWeaponVisual(this);
			}
		}

		public override void SaveData(TagCompound tag) {
			tag.Add("isShowingHeldItem", isShowingHeldItem);
		}

		public override void LoadData(TagCompound tag) {
			isShowingHeldItem = tag.GetBool("isShowingHeldItem");
		}

		/// <summary>
		/// Client and Server
		/// </summary>
		private void manageCombatTimer(bool combatPoseTriggered = false) {
			// Only process this line clientside, since every client can have different local configs
			if (Player == Main.LocalPlayer) {
				int combatDelayTimerMax = (int)(ModContent.GetInstance<WeaponOutClientConfig>().CombatDelayTimerMax * 60f);
                if (ModContent.GetInstance<WeaponOutClientConfig>().CombatStanceAlwaysOn) {
					combatDelayTimerMax = int.MaxValue;
                }

				if (combatPoseTriggered || Player.ItemAnimationActive) {
					// Set to max while using an item, or combat is triggered
					CombatDelayTimer = combatDelayTimerMax;
                }

				// As long as the timer is maxed out and the player is not using the item, send a net update
				if (CombatDelayTimer == combatDelayTimerMax && !Player.ItemAnimationActive)
                {
                    ((WeaponOutLite)Mod).SendUpdateCombatTimer(this);
                }
			}

			// Always tick down to 0
			if (!Player.ItemAnimationActive && !Player.CCed) {
				// Count down the combat timer before returning to "relaxed" state
				CombatDelayTimer = Math.Max(0, CombatDelayTimer - 1);

				// Move to relaxed state after 2 seconds sitting/sleeping (this is the default value before sleeping)
				if ((Player.sitting.isSitting || Player.sleeping.isSleeping) && CombatDelayTimer > 120) {
					CombatDelayTimer = 120;
                }
			}
		}

		/// <summary>
		/// General tasks to sort out before rendering the hold style.
		/// Assign the hold style of an item, if it is not set to the default.
		/// Detect item switching, and update the server with the new item and hold style.
		/// 
		/// The finalised hold style should be determined here.
		/// Local Client Only
		/// </summary>
        private void manageHoldStyle() {
            // Get configs etc.
            WeaponOutLite myMod = Mod as WeaponOutLite;

			CurrentDrawItemPose = PoseSetClassifier.SelectItemPose(Player, HeldItem);

            // After item switch, check disaparity between Player and Client to see if we need to send a held item update
            // Net update with the change in pose and timer
            if (Player.selectedItem != Main.clientPlayer.selectedItem
                && Player == Main.LocalPlayer
                && Main.netMode == NetmodeID.MultiplayerClient) {

                (myMod).SendUpdateWeaponVisual(this);
                (myMod).SendUpdateCombatTimer(this);
            }
        }

		/// <summary>
		/// Modify the player's body frame if there is an idle body frame set in the style.
		/// Clients Only
		/// </summary>
		private void manageBodyFrame()
		{
			//no item so nothing to show
			if (HeldItem == null || HeldItem.type == ItemID.None || HeldItem.holdStyle != 0) return;

			// BUG20231026A
			// ReceiveUpdateWeaponVisual is being called when new player B is set on client A, but B's value is null here
			// modPlayer.CurrentDrawItemPose = DrawStyle[holdStyleID] is being called properly...
			// But modifications to the modplayer in that method seem to be reverted by the time this method is called (all values reset to null)
			// tested IsCloneable and overriding Clone doesn't solve this.
			// 
			if (CurrentDrawItemPose == null)
			{
				//throw new Exception("This situation happens for existing clients when a new player joins a server. How to deal with this?");
				// Ignore this state. The next time a player changes their held item in MP, it will be re-synced anyway.
				return;
			}

			// Only if in a rest post
			// 0 = standing
			// 5 = falling
			// 6+ = running/flying

			bool idleFrames = BodyFrameNum == 0 || BodyFrameNum >= 5;
			bool idleCompositeArms = !Player.compositeFrontArm.enabled && !Player.compositeBackArm.enabled;

            if (idleFrames && idleCompositeArms)
			{
				BodyFrameNum = CurrentDrawItemPose.UpdateIdleBodyFrame(Player, HeldItem, BodyFrameNum, CombatDelayTimer);
			}

			// Monitor external modifications to compositeFrontArm to detect other mod overrides
			if (Player.compositeFrontArm.enabled)
			{ expectedFrontArmThisFrame = Player.compositeFrontArm.rotation; }
			else
			{ expectedFrontArmThisFrame = null; }
		}
    }
}