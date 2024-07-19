using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.Common.Players;
using WeaponOutLite.Content.DrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Common
{
	/// <summary>
	/// Code for generating the draw data that is inserted in WeaponOutDrawLayers.cs
	/// Offsets and custom logic is derived from the IDrawItemStyle.cs class and the Content.DrawItemStyle namepsace
	/// </summary>
    public static class WeaponOutLayerRenderer
	{
		/// <summary>
		/// For logging errors once only per item, rather than spamming the player during a session
		/// </summary>
		private static HashSet<int> ItemLogOnceRecorder;

		/// <summary>
		/// Cache for special proj textures. DO NOT access before calling CreateItemProjectileSpearTexture, which sets the capacity
		/// </summary>
		public static List<Texture2D> ItemProjTextureCache { get; set; }

		/// <summary>
		/// Offset to place the item at a different Y centre, so that when *-2, the position will be consistent when the player is upside down.
		/// While this technically means the weapon is not placed directly in the centre,
		/// this means any further position changes from DrawItemStyle, when inverted, don't have any weird offsets when flipped.
		/// </summary>
		const int GravityOffset = -3;

        /// <summary>
        /// Initialised in main mod file Load().
        /// </summary>
        internal static void Load()
		{
            ItemLogOnceRecorder = new HashSet<int>();
            ItemProjTextureCache = new List<Texture2D>();
        }

        /// <summary>
        /// Initialised in main mod file Unload.
        /// </summary>
        internal static void Unload()
        {
			ItemLogOnceRecorder = null;
            ItemProjTextureCache = null;
        }

		/// <summary>
		/// Wrapper for adding the item and glow draw data to the draw set
		/// </summary>
		/// <param name="drawInfo">Draw set with item draw data</param>
		internal static void DrawPlayerItem(ref PlayerDrawSet drawInfo)
		{
			// Create basic draw data, centred on the player
			if (tryCreateBaseDrawData(drawInfo, out DrawData itemData)) {

				var heldItem = drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem];
				var modPlayer = drawInfo.drawPlayer.GetModPlayer<WeaponOutPlayerRenderer>();
				var bowDrawAmmo = ModContent.GetInstance<WeaponOutClientConfig>().BowDrawAmmo;

				try {
					AddItemToDrawInfoCache(ref drawInfo, itemData, heldItem, modPlayer, bowDrawAmmo);
				}
				catch (Exception ex) {
					int logType = -1;
					if (drawInfo.heldItem != null) {
						logType = drawInfo.heldItem.type;
					}
					if (!ItemLogOnceRecorder.Contains(logType)) {
						ItemLogOnceRecorder.Add(logType);
						WeaponOutLite.GetMod().Logger.Error($"WeaponOut: Exception in renderer {ex}");
						Main.NewText("WeaponOutLite: Exception in render for this item. Error has been logged.");
					}
				}
			}
		}

		public static void AddItemToDrawInfoCache(ref PlayerDrawSet drawInfo, DrawData itemData, Item heldItem, WeaponOutPlayerRenderer modPlayer, bool bowDrawAmmo) {
			// Don't draw if melee weapon projectiles are currently out (eg. boomerangs)
			if (heldItem.shoot != ProjectileID.None) {
				if (heldItem.CountsAsClass(DamageClass.Melee) && heldItem.noMelee) {
					foreach (Projectile p in Main.projectile) {
						if (!p.active) continue;
						if (p.owner == drawInfo.drawPlayer.whoAmI && p.CountsAsClass(DamageClass.Melee) && p.type == heldItem.shoot) {
							return;
						}
					}
				}
			}

			// Get the scaled height and width of the item to be drawn
			float height = (itemData.sourceRect?.Height ?? 0) * itemData.scale.X;
			float width = (itemData.sourceRect?.Width ?? 0) * itemData.scale.Y;

			var bodyFrame = drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height;

			// Apply giant scaling where applicable
			itemData.scale *= DrawHelper.GetGiantTextureScale(width * itemData.scale.X, height * itemData.scale.Y);

			// If enabled, shrink yoyos by 50%
			if (ModContent.GetInstance<WeaponOutClientConfig>().YoyoHalfScale && ItemID.Sets.Yoyo[heldItem.type]) {
				itemData.scale *= 0.66f;
			}

			// Origin pixel perfect centre and place in centre
			itemData.SetOrigin(0.5f, 0.5f, drawInfo.drawPlayer);

			// Save player centre
			var playerCentrePosition = itemData.position;

			// Modify dimensions being passed by scale
			height = (itemData.sourceRect?.Height ?? 0) * itemData.scale.Y;
			width = (itemData.sourceRect?.Width ?? 0) * itemData.scale.X;

			// Apply draw data from item style.
			var drawItemStyle = modPlayer.CurrentDrawItemPose;
			itemData = drawItemStyle.CalculateDrawData(
				itemData,
				drawInfo.drawPlayer,
				height,
				width,
				bodyFrame,
				modPlayer.CombatDelayTimer);

			// Apply draw data from custom mod hooks here, if applicable
			if (modPlayer.CurrentDrawItemPose.GetID() == DrawItemPoseID.Custom) {
				foreach (var CustomPreDrawData in WeaponOutLite.GetMod().customPreDrawDataFuncs) {
					itemData = CustomPreDrawData(drawInfo.drawPlayer, heldItem, itemData, height, width, bodyFrame, modPlayer.CombatDelayTimer);
				}
			}

			if (itemData.texture == null) {
				// Exit out without adding to the render list, if the texture is not set (aka, why bother)
				return;
            }

			// Flip position from player centre based on direction and gravity
			itemData.position = (itemData.position - playerCentrePosition)
				* drawInfo.drawPlayer.Directions + playerCentrePosition;

			// Flip origin based on sprite effects and directions
			float flipOriginX = itemData.effect.HasFlag(SpriteEffects.FlipHorizontally) ? -1 : 1;
			float flipOriginY = itemData.effect.HasFlag(SpriteEffects.FlipVertically) ? -1 : 1;
			flipOriginX *= drawInfo.drawPlayer.direction;
			flipOriginY *= drawInfo.drawPlayer.gravDir;
			if (flipOriginX < 0) {
				itemData.origin.X = (itemData.origin.X - itemData.sourceRect.Value.Width) * -1f;
			}
			if (flipOriginY < 0) {
				itemData.origin.Y = (itemData.origin.Y - itemData.sourceRect.Value.Height) * -1f;
			}

			// Re-angle offset so that rotation = 0 means the item face, instead of forwards.
			itemData.rotation += -MathHelper.PiOver2 * drawInfo.drawPlayer.direction * drawInfo.drawPlayer.gravDir
				// Then bring it back to face forwardss
				+ MathHelper.PiOver2;

			// Change rotation based on direction and gravity
			itemData.rotation *= drawInfo.drawPlayer.direction * drawInfo.drawPlayer.gravDir;

			//Add the weapon to the draw layers
			drawInfo.DrawDataCache.Add(itemData);
			if (tryCreateGlowLayerDrawData(drawInfo.drawPlayer, heldItem, itemData, out DrawData glowData)) {
				drawInfo.DrawDataCache.Add(glowData);
			}

			// For bows, potentially draw equipped arrow
			if (bowDrawAmmo && heldItem.useAmmo == AmmoID.Arrow) {
				Item ammo = FindAmmoVanillaOrNull(drawInfo.drawPlayer, heldItem.useAmmo);
				int projectileType = ApplyVanillaAmmoReplacement(heldItem.type, ammo?.shoot ?? 0);
                if (ammo != null && projectileType > 0) {
					// create the arrow
					if (tryCreateArrowDrawData(drawInfo.drawPlayer, projectileType, itemData, out DrawData arrowData)) {
						drawInfo.DrawDataCache.Add(arrowData);
						// and any glow layer
						if (tryCreateProjectileGlowLayerDrawData(drawInfo.drawPlayer, projectileType, arrowData, out DrawData arrowGlowData)) {
							drawInfo.DrawDataCache.Add(arrowGlowData);
						}
					}
				}
			}

			// Draw weapon effects
			if (ModContent.GetInstance<WeaponOutClientConfig>().EnableMeleeEffects) {
				try {
					if (modPlayer != null && heldItem != null && modPlayer.Player != null && modPlayer.CombatDelayTimer > 0) {
						const int MAX_STEPS = 4;
						const int INVERSE_FREQUENCY = 4;

						bool runThisFrame = Main.rand.NextBool(INVERSE_FREQUENCY);

						if (runThisFrame) {
							// so if 4, start at -2, up to 1
							// the idea is at each step, use (step and step + 1), to cover 4 rects over 5 points
							int step = (int)((long)Main.timeForVisualEffects % MAX_STEPS) - MAX_STEPS / 2;

							Vector2 corner = itemData.position - (itemData.origin * itemData.scale).RotatedBy(itemData.rotation);
							Vector2 centre = corner + new Vector2(width, height).RotatedBy(itemData.rotation) / 2f;
							float realRotation = itemData.rotation
								+ (itemData.effect.HasFlag(SpriteEffects.FlipVertically) ? MathHelper.Pi : MathHelper.PiOver2)
								+ (itemData.effect.HasFlag(SpriteEffects.FlipHorizontally) ? MathHelper.PiOver2 : 0);

							Vector2 quarterStep = new Vector2(width, height).RotatedBy(realRotation) / MAX_STEPS;

							Vector2 point1 = centre + Main.screenPosition + quarterStep * step;
							Vector2 point2 = centre + Main.screenPosition + quarterStep * (step + 1);

							Point rectTopLeft = new Point((int)Math.Min(point1.X, point2.X), (int)Math.Min(point1.Y, point2.Y));
							Point bounds = (point1 - point2).ToPoint();
							bounds = new Point(Math.Abs(bounds.X), Math.Abs(bounds.Y));

							Rectangle drawRectangle = new Rectangle(rectTopLeft.X, rectTopLeft.Y, bounds.X, bounds.Y);

							// uh ohhhh scary reflection ew
							MethodInfo method = modPlayer.Player.GetType().GetMethod("ItemCheck_EmitUseVisuals", BindingFlags.NonPublic | BindingFlags.Instance);
							method.Invoke(modPlayer.Player, new object[] { heldItem, drawRectangle });
						}
                    }
				}
				catch (Exception e) {
					Main.NewText("WeaponOutLite: Experimental feature failure, melee effects disabled");
					ModContent.GetInstance<WeaponOutClientConfig>().EnableMeleeEffects = false;
					new Exception("Something happened when trying to generate melee effects", e);
				}
			}
		}

		/// <summary>
		/// Attempt to find ammo slots - using default vanilla style behaviour (not taking into account mods)
		/// </summary>
		/// <param name="player"></param>
		/// <param name="useAmmo"></param>
		/// <returns></returns>
		private static Item? FindAmmoVanillaOrNull(Player player, int useAmmo) {
			// from Player.cs PickAmmo
			// 1. go through ammo slots
			for (int ammoPouch = 54; ammoPouch < 58; ammoPouch++) {
				Item check = player.inventory[ammoPouch];
				if (check.ammo == useAmmo && check.stack > 0) {
					return check;
				}
			}
			// 2. go through standard inventory
			for (int inventoryIndex = 0; inventoryIndex < 54; inventoryIndex++) {
				Item check = player.inventory[inventoryIndex];
				if (check.ammo == useAmmo && check.stack > 0) {
					return check;
				}
			}
			return null;
		}

		/// <summary>
		/// Swap out projectile types depending on the item. Various bows have custom code that replaces arrows on shoot.
		/// </summary>
		/// <param name="itemType"></param>
		/// <param name="projectileType"></param>
		/// <returns></returns>
		private static int ApplyVanillaAmmoReplacement(int itemType, int projectileType)
        {
			bool woodenArrow = projectileType == ProjectileID.WoodenArrowFriendly;
			switch (itemType) {
				case ItemID.DD2BetsyBow:
					return ProjectileID.DD2BetsyArrow;
				case ItemID.BloodRainBow:
					return ProjectileID.BloodArrow;
				case ItemID.FairyQueenRangedItem:
					return woodenArrow ? ProjectileID.FairyQueenRangedItemShot : projectileType;
				case ItemID.HellwingBow:
					return woodenArrow ? 0 : projectileType;
				case ItemID.Marrow:
					return ProjectileID.BoneArrow;
				case ItemID.MoltenFury:
					return woodenArrow ? ProjectileID.FireArrow : projectileType;
				case ItemID.IceBow:
					return ProjectileID.FrostArrow;
				case ItemID.DD2PhoenixBow:
					return woodenArrow ? ProjectileID.FireArrow : projectileType;
				case ItemID.PulseBow:
					return ProjectileID.MartianWalkerLaser; // Not actually the pulse bolt, but it looks close enough
				case ItemID.ShadowFlameBow:
					return ProjectileID.ShadowFlameArrow;
				case ItemID.BeesKnees:
					return woodenArrow ? ProjectileID.BeeArrow : projectileType;
			}
			return projectileType;
        }

		/// <summary>
		/// Defines the list of variables used for drawing, and checks against a series of conditions that determine if an item can be drawn.
		/// </summary>
		/// <param name="drawInfo"></param>
		/// <param name="drawPlayer"></param>
		/// <param name="modPlayer"></param>
		/// <param name="holdStyle"></param>
		/// <param name="heldItem"></param>
		/// <param name="itemTexture"></param>
		/// <returns></returns>
		public static bool CanDrawBaseDrawData(PlayerDrawSet drawInfo, 
			out Player drawPlayer, 
			out WeaponOutPlayerRenderer modPlayer, 
			out IDrawItemPose holdStyle, 
			out Item heldItem, 
			out Texture2D itemTexture) {

			drawPlayer = drawInfo.drawPlayer;
			modPlayer = drawPlayer.GetModPlayer<WeaponOutPlayerRenderer>();
			holdStyle = modPlayer.CurrentDrawItemPose;
			heldItem = null;
			itemTexture = null;

			// Don't draw when player doesn't meet standard draw conditions
			if ((!modPlayer.DrawHeldItem && !(Main.gameMenu && ModContent.GetInstance<WeaponOutClientConfig>().EnableMenuDisplay)) || holdStyle == null) return false;

			// Player player's held item
			heldItem = modPlayer.HeldItem;
			// no item so nothing to show, and items with predefined hold styles already have draw code/layers
			if (heldItem == null || heldItem.type == ItemID.None || heldItem.holdStyle != 0) return false;

			itemTexture = TextureAssets.Item[heldItem.type].Value;

			// Experimental projectile spear code
            if (ModContent.GetInstance<WeaponOutClientConfig>().EnableProjSpears && heldItem.shoot != 0) {

				// Check if this is a spear
				PoseSetClassifier.GetItemPoseGroupData(heldItem, out PoseStyleID.PoseGroup poseGroup, out _);
				bool isSpear = poseGroup == PoseStyleID.PoseGroup.Spear;

				// Don't actually load this texture until the game has done so - default to the item texture
				if(isSpear) {
					CreateItemProjectileSpearTexture(heldItem.type, heldItem.shoot);
					if(ItemProjTextureCache[heldItem.type] != null) {
						bool isTheTextureActuallyBigger = ItemProjTextureCache[heldItem.type].Width > itemTexture.Width
							 || ItemProjTextureCache[heldItem.type].Height > itemTexture.Height;
                        if (isTheTextureActuallyBigger) {
							itemTexture = ItemProjTextureCache[heldItem.type];
						}
					}
				}
			}

			// if no texture to item then can't render anything  ¯\_(ツ)_/¯
			if (itemTexture == null) return false;
			
			return true;
		}

		/// <summary>
		/// Provide a draw data with the centre of the weapon: 
		/// <br/> placed in the centre of the player, 
		/// <br/> flipped to match direction and gravity, 
		/// <br/> scaled to item size + hold style modifications, 
		/// <br/> no rotation,
		/// <br/> pre-animated (where applicable)
		/// </summary>
		/// <param name="drawInfo">Draw info from a PlayerDrawLayer</param>
		/// <returns>true if valid draw data was created</returns>
		internal static bool tryCreateBaseDrawData(PlayerDrawSet drawInfo, out DrawData data) {
			if (CanDrawBaseDrawData(drawInfo, 
				out Player drawPlayer, 
				out WeaponOutPlayerRenderer modPlayer, 
				out IDrawItemPose holdStyle, 
				out Item heldItem, 
				out Texture2D itemTexture)) {

				Rectangle sourceRect = calculateSourceRect(heldItem, itemTexture);

				//get draw location of player
				int drawX = (int)(drawPlayer.MountedCenter.X - Main.screenPosition.X);
				int drawY = (int)(drawPlayer.MountedCenter.Y - Main.screenPosition.Y + drawPlayer.gfxOffY) + GravityOffset;

                // -3 is to help with centering later (see + 6 from gravity flip)

				// Game menu cannot use the player center, use this position instead.
                if (Main.gameMenu && ModContent.GetInstance<WeaponOutClientConfig>().EnableMenuDisplay) {
					drawX = (int)(drawInfo.Position.X - Main.screenPosition.X) + 10;
					drawY = (int)(drawInfo.Position.Y - Main.screenPosition.Y) + 18;
				}

				var playerTile = drawPlayer.Center.ToTileCoordinates();
				if (drawPlayer.sitting.isSitting && PlayerSittingHelper.GetSittingTargetInfo(
					drawPlayer,
					playerTile.X, 
					playerTile.Y, 
					out int targetSeatDirection, 
					out Vector2 playerSittingPosition, 
					out Vector2 seatDownOffset, 
					out ExtraSeatInfo extraSeatInfo
					)) {

					drawX += (int)(seatDownOffset.X * targetSeatDirection);
					drawY += (int)(seatDownOffset.Y * 2 + GravityOffset);

					//WeaponOutLite.TEXT_DEBUG += $"{seatDownOffset}";
				}else if(drawPlayer.sleeping.isSleeping && PlayerSleepingHelper.GetSleepingTargetInfo(playerTile.X, playerTile.Y, out int targetSleepDirection, out Vector2 anchorPosition, out Vector2 visualoffset)) {
					drawX += (int)(visualoffset.X * targetSleepDirection);
					drawY += (int)(visualoffset.Y * 2 + GravityOffset);
					//WeaponOutLite.TEXT_DEBUG += visualoffset;
				}

				// get Scale
				float scale = ModContent.GetInstance<WeaponOutClientConfig>().EnableItemScaling? DrawHelper.SnapNearOne(heldItem.scale) : 1f;

				//get the lighting on the player's tile
				Color lighting = Lighting.GetColor(
						(int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f),
						(int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
				//get item alpha (like starfury) then player stealth and alpha (inviciblity etc.)
				var itemColour = heldItem.GetColor(lighting).ToVector4();
				var itemAlpha = heldItem.GetAlpha(lighting).ToVector4();
				var mix = itemColour + itemAlpha;
				lighting = drawPlayer.GetImmuneAlpha(new Color(mix.X,mix.Y,mix.Z,mix.W), 0) * drawPlayer.stealth;

				// Flip facing
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (drawPlayer.direction < 0) spriteEffects = SpriteEffects.FlipVertically;
				if (drawPlayer.gravDir < 0) {
                    drawY -= GravityOffset * 2;
                    spriteEffects = SpriteEffects.FlipHorizontally | spriteEffects;
                }

				// Create a base draw data from the values built above
				data = new DrawData(
						itemTexture,
						new Vector2(drawX, drawY),
						sourceRect,
						lighting,
						0f,
						sourceRect.Size() / 2, // centre origin
						scale,
						spriteEffects,
						0);

				// Item customiser integration (for whatever the 1.4 equivalent is)
				// https://github.com/gamrguy/ItemCustomizer
				//if (itemCustomizer != null) {
				//	data.shader = ItemCustomizerGetShader(itemCustomizer, heldItem);
				//}

				return true;
			}
			data = default;
			return false;
		}

		internal static Rectangle calculateSourceRect(Item item, Texture2D texture) {
			Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);

			//does the item have an animation? Since 1.4 vanilla items such as Fallen Star have multiple frames
			if (item != null && Main.itemAnimations[item.type] != null) {
				var itemAnimation = Main.itemAnimations[item.type];

				try {
					sourceRect = itemAnimation.GetFrame(texture);
				}
				catch {
					if (Main.time % 60 == 0) {
						WeaponOutLite.GetMod().Logger.Warn($"WeaponOut: Failed to animate item {item.Name}[{item.ModItem.FullName}]");
					}
					sourceRect = new Rectangle(0, 0, texture.Width, texture.Height / itemAnimation.FrameCount);
				}
			}
			return sourceRect;
		}

		internal static Rectangle calculateProjectileSourceRect(int projType, Texture2D texture) {
			Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);

			//does the projectile have an animation? Just use the first frame
			try {
				sourceRect = texture.Bounds;
				sourceRect.Height /= Main.projFrames[projType];
			}
			catch {
				if (Main.time % 60 == 0) {
					WeaponOutLite.GetMod().Logger.Warn($"WeaponOut: Failed to get projectile texture for type {projType}");
				}
				sourceRect = texture.Bounds;
			}
			return sourceRect;
		}

		/// <summary>
		/// Copy of draw data, with alphas set to reflect glow and stealth.
		/// </summary>
		/// <param name="data">Finished draw data</param>
		/// <param name="drawPlayer">The drawPlayer from drawInfo</param>
		internal static bool tryCreateGlowLayerDrawData(Player drawPlayer, Item item, DrawData data, out DrawData glowData) {
			// Technically glowmasks start at 0, but 0 is used for a projectile, and item.glowMask = -1 isn't set during the gameMenu
			if (item.glowMask > 0) {
				Color glowLighting = new Color(250, 250, 250, item.alpha);
				glowLighting = drawPlayer.GetImmuneAlpha(item.GetAlpha(glowLighting) * drawPlayer.stealth * data.color.A, 0);
				glowData = new DrawData(
				   TextureAssets.GlowMask[item.glowMask].Value,
				   data.position,
				   data.sourceRect,
				   glowLighting,
				   data.rotation,
				   data.origin,
				   data.scale,
				   data.effect,
				   0);
				return true;
			}
			glowData = default;
			return false;
		}

		internal static bool tryCreateProjectileGlowLayerDrawData(Player drawPlayer, int projectileType, DrawData data, out DrawData glowData) {
			// Get the glowmask from the proejctile

			Projectile p = new Projectile();
			p.SetDefaults(projectileType);

			if (p.glowMask != -1) {
				Color glowLighting = new Color(250, 250, 250, p.alpha);
				glowLighting = drawPlayer.GetImmuneAlpha(p.GetAlpha(glowLighting) * drawPlayer.stealth * data.color.A, 0);
				glowData = new DrawData(
				   TextureAssets.GlowMask[p.glowMask].Value,
				   data.position,
				   data.sourceRect,
				   glowLighting,
				   data.rotation,
				   data.origin,
				   data.scale,
				   data.effect,
				   0);
				return true;
			}
			glowData = default;
			return false;
		}

		internal static bool tryCreateArrowDrawData(Player drawPlayer, int projectileType, DrawData itemData, out DrawData arrowData) {
			arrowData = itemData;

			//var asset = Main.Assets.Request<Texture2D>(TextureAssets.Projectile[ammo.shoot].Name);
			Main.instance.LoadProjectile(projectileType);

			var itemTexture = TextureAssets.Projectile[projectileType].Value;

			// if no texture to item then can't render anything  ¯\_(ツ)_/¯
			if (itemTexture == null || !TextureAssets.Projectile[projectileType].IsLoaded) return false;

			// rotation direction relative to player
			float directionGrav = drawPlayer.direction * drawPlayer.gravDir;
			float flipOriginY = itemData.effect.HasFlag(SpriteEffects.FlipVertically) ? -1 : 1;
			float flippedSprite = flipOriginY != drawPlayer.direction ? -1 : 1;
			if (flippedSprite < 0) {
				if (arrowData.effect.HasFlag(SpriteEffects.FlipVertically)) {
					arrowData.effect = SpriteEffects.None;
				}
				else {
					arrowData.effect = SpriteEffects.FlipVertically;
				}
			}
			arrowData.effect = arrowData.effect | SpriteEffects.FlipHorizontally;

			// change texture and recentre
			arrowData.texture = itemTexture;
			arrowData.sourceRect = calculateProjectileSourceRect(projectileType, arrowData.texture);
			arrowData.origin = new Vector2(
				itemTexture.Width / 2, 
				(int)(itemTexture.Height * (0.5f - 0.175f * drawPlayer.direction) / 2) * 2 + 1); // base of arrow head (projectile sprite faces upwards)

			// Place in left X centre Y of the parent item. This ends up matching 1/8 along the arrow with 1/8th across the bow
			// Generally speaking, that will match up with most sprites
			Vector2 originRawOffset = -itemData.origin * itemData.scale + (itemData.texture.Size() * itemData.scale * new Vector2(0.5f + 0.375f * drawPlayer.gravDir, 0.5f)).Round(drawPlayer.gravDir, 1);
			// also needs offsetting by scale, and also reset the projectile scale to 1, to be safe
			arrowData.scale = Vector2.One;

			if (itemData.sourceRect.Value.Width < itemData.sourceRect.Value.Height) {
                //// Bow alignment Y (To directly middle, and align X)
                if (itemData.sourceRect.Value.Height % 4 == 0) originRawOffset.Y += directionGrav;
				if (itemData.sourceRect.Value.Width % 4 != originRawOffset.X % 2) originRawOffset.X += drawPlayer.gravDir;
            }
			else {
				originRawOffset.Y -= 2 * drawPlayer.direction * flippedSprite;
				if (itemData.sourceRect.Value.Height % 4 == 0) originRawOffset.Y -= directionGrav * 1;
			}

			arrowData.position += new Vector2(
				(float)Math.Cos(arrowData.rotation) * originRawOffset.X +
				(float)Math.Sin(-arrowData.rotation) * originRawOffset.Y,
				(float)Math.Sin(arrowData.rotation) * originRawOffset.X +
				(float)Math.Cos(arrowData.rotation) * originRawOffset.Y);

            arrowData.rotation += MathHelper.PiOver2 * directionGrav;

			Projectile p = new Projectile();
			p.SetDefaults(projectileType);
			if (p.alpha > 0) arrowData.color.A = (byte)p.alpha;

			if (projectileType == ProjectileID.JestersArrow ||
				projectileType == ProjectileID.HolyArrow ||
				projectileType == ProjectileID.FireArrow ||
				projectileType == ProjectileID.FrostburnArrow ||
				projectileType == ProjectileID.CursedArrow ||
				projectileType == ProjectileID.IchorArrow ||
				projectileType == ProjectileID.ShimmerArrow) {
				arrowData.color = new Color(256, 256, 256, (byte)p.alpha);
			}
			if(projectileType == ProjectileID.FairyQueenRangedItemShot) {
				arrowData.origin = arrowData.texture.Size() / 2;
				arrowData.rotation += MathHelper.PiOver2 * directionGrav;
				p.ai[1] = drawPlayer.miscCounterNormalized * 12f;
				arrowData.color = p.GetFairyQueenWeaponsColor();
				arrowData.color = new Color(
					arrowData.color.R / 4 + 192,
					arrowData.color.G / 4 + 192,
					arrowData.color.B / 4 + 192, 0);
			}
			if (projectileType == ProjectileID.MartianWalkerLaser) {
				arrowData.color = new Color(192, 192, 256, 0);
			}

			return true;
		}

		internal static void CreateItemProjectileSpearTexture(int itemType, int projectileType)
		{
			// First check if the appropriate cached texture already exists
			// Populate inbetween space with empty textures
			if(ItemProjTextureCache.Capacity <= itemType) {
				ItemProjTextureCache.Capacity = itemType + 1;
				ItemProjTextureCache.AddRange(new Texture2D[ItemProjTextureCache.Capacity - ItemProjTextureCache.Count]);
            }

			//
			if (WeaponOutLite.DEBUG_EXPERIMENTAL && Main.inventorySortMouseOver) { ItemProjTextureCache[itemType] = null; }
			//

			if (ItemProjTextureCache[itemType] != null) {
				return;
			}

			// Don't actually load this texture until the game has done so - default to the item texture
			if (Main.IsGraphicsDeviceAvailable && TextureAssets.Projectile[projectileType].IsLoaded) {
				if (WeaponOutLite.DEBUG_EXPERIMENTAL) {
					string text = $"Generating New {itemType}";
					if (Main.dedServ) { System.Console.WriteLine(text); } else { Main.NewText(text); }
				}
				Texture2D baseTexture = TextureAssets.Projectile[projectileType].Value;

				// Flip projectile texture horizontally to match item rotation
				// This is because internally, spear projectiles go from bottom right to top left.
				// This operation is somewhat expensive, so we only want to run it once and then cache the result
				// It may also have odd interations if multiple frames are present
				try {
					Texture2D rotatedItemTexture = new Texture2D(Main.instance.GraphicsDevice, baseTexture.Width, baseTexture.Height);

					// If the texture is not squarish, it's probably not something we need to flip. All spear textures are squares
					Color[] data = new Color[baseTexture.Width * baseTexture.Height];
					Color[] rotatedData = new Color[data.Length];
					baseTexture.GetData(data);

					// Check how much of the graphic the spear has in this axes
					int totalLength = Math.Min(baseTexture.Width, baseTexture.Height);
					int coverageAlongLength = 0;
					for (int i = 0; i < totalLength; i++) {
						coverageAlongLength += data[i * baseTexture.Width + i].A > 0 ? 1 : 0;
					}

					// If the spear has pixels along at least 75% of this diagonal, it's probably flippable.
					// System.Console.WriteLine($"WeaponOutLite Spear flipper ({itemType}): For {totalLength}px, coverage is {coverageAlongLength}px ({(int)(coverageAlongLength * 100 / totalLength)}%)");
					if (coverageAlongLength > totalLength * 0.75f) {
						// Create a horizontally flipped texture
						var x = baseTexture.Width - 1;
						var y = 0;
						for (int i = 0; i < data.Length; i++) {
							rotatedData[i] = data[x + y * baseTexture.Width];
							x -= 1;
							if (x < 0) {
								x = baseTexture.Width - 1;
								y += 1;
							}
						}
						rotatedItemTexture.SetData(rotatedData);

						ItemProjTextureCache[itemType] = rotatedItemTexture;
                    }
                    else {
						ItemProjTextureCache[itemType] = baseTexture;
					}
				}
				catch (Exception e) {
					Main.NewText("WeaponOutLite: Experimental feature failure, proj spears temporarily disabled");
					ModContent.GetInstance<WeaponOutClientConfig>().EnableProjSpears = false;
					new Exception("Something happened when trying to rotate spear texture", e);
				}
			}
		}
	}
}