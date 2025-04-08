using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ModLoader.Config.UI;
using WeaponOutLite.Common.Players;
using WeaponOutLite.ID;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;

namespace WeaponOutLite.Common.Configs
{
	/// <summary>
	/// A config element that draws a mannequin and object based on the drawdata of a selected style
	/// </summary>
    public abstract class HoldStylePreviewElement : ConfigElement
	{
		public virtual float MinHeightPx => 64f;

		public override void OnBind() {
			base.OnBind();
			MinHeight = new StyleDimension(MinHeightPx, 0f);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			Rectangle hitbox = GetInnerDimensions().ToRectangle();
			Point drawCentre = hitbox.Center;
			int? holdStyleID = (MemberInfo.GetValue(Item) as int?);
			int itemType = PreviewItemType();
			bool drawArrow = ModContent.GetInstance<WeaponOutClientConfig>().BowDrawAmmo;

			// Create a new dummy player
			PlayerDrawSet dummyInfo = new PlayerDrawSet();
			dummyInfo.DrawDataCache = new List<DrawData>();
			dummyInfo.drawPlayer = new Player();

			// Load the selected preview item into their inventory
			dummyInfo.drawPlayer.inventory[0] = new Item();
			dummyInfo.drawPlayer.HeldItem.SetDefaults(itemType);
			Main.instance.LoadItem(itemType);

			// Load the composite body piece
			Main.instance.LoadArmorBody(ArmorIDs.Body.WoodBreastplate);
			Texture2D body = TextureAssets.ArmorBodyComposite[ArmorIDs.Body.WoodBreastplate].Value;
			Point bodySize = new Point(body.Width / 9, body.Height / 4);

			// And an arrow
			if (drawArrow) {
				dummyInfo.drawPlayer.inventory[1] = new Item();
				dummyInfo.drawPlayer.inventory[1].SetDefaults(ItemID.WoodenArrow);
				Main.instance.LoadItem(ItemID.WoodenArrow);
			}

			// Generate a basic DrawData for the preview item
			Texture2D itemTexture = TextureAssets.Item[itemType].Value;

			OverrideDrawItem(ref itemTexture, ref dummyInfo.drawPlayer.inventory[0]);

			DrawData itemData = new DrawData(
				itemTexture,
				drawCentre.ToVector2(),
				itemTexture.Bounds,
				Color.White,
				0f,
				itemTexture.Bounds.Center(),
				Vector2.One * dummyInfo.drawPlayer.HeldItem.scale * SetScale(),
				SpriteEffects.None,
				0);

			// Create a mod player to store the hold style and timer
			WeaponOutLite mod = WeaponOutLite.GetMod();
			WeaponOutPlayerRenderer modPlayer = new WeaponOutPlayerRenderer();
			try {
				modPlayer.CurrentDrawItemPose = mod.DrawStyle[(int)holdStyleID];
			}
			catch {
				modPlayer.CurrentDrawItemPose = mod.DrawStyle[DrawItemPoseID.Unassigned];
			}
			modPlayer.CombatDelayTimer = int.MaxValue;

			// Get body frame and depth
			var bodyFrame = modPlayer.CurrentDrawItemPose.UpdateIdleBodyFrame(dummyInfo.drawPlayer, dummyInfo.drawPlayer.HeldItem, 0, modPlayer.CombatDelayTimer);
			dummyInfo.drawPlayer.bodyFrame.Y = dummyInfo.drawPlayer.bodyFrame.Height * bodyFrame;
			var drawDepth = modPlayer.CurrentDrawItemPose.DrawDepth(dummyInfo.drawPlayer, dummyInfo.drawPlayer.HeldItem, modPlayer.CombatDelayTimer);

			// Call the draw process to get item data
			WeaponOutLayerRenderer.AddItemToDrawInfoCache(
				ref dummyInfo,
				itemData,
				dummyInfo.drawPlayer.HeldItem,
				modPlayer,
				drawArrow);

			// Draw the item if drawdepth is behind
			if (drawDepth == DrawDepthID.Back || drawDepth == DrawDepthID.OffHand) {
				foreach (var data in dummyInfo.DrawDataCache) {
					Main.spriteBatch.Draw(data.texture, data.position, data.sourceRect, data.color, data.rotation, data.origin, data.scale, data.effect, 0);
				}
			}

			// Load mannequin tile into memory
			Main.instance.LoadTiles(TileID.Mannequin);
			Texture2D mannequin = TextureAssets.Tile[TileID.Mannequin].Value;
			// Draw the mannequin
			Point mannequinOffset = drawCentre - new Point(48, 16);
			for (int tileY = 0; tileY < 3; tileY++) {
				for (int tileX = 2; tileX < 4; tileX++) {

					// skip front arm, because we'll be drawing it again later
					if (tileX == 2 && tileY == 1) continue;

					spriteBatch.Draw(
						mannequin,
						new Rectangle(
							mannequinOffset.X + tileX * 16,
							mannequinOffset.Y + tileY * 16, 16, 16),
						new Rectangle(tileX * 18, tileY * 18, 16, 16),
						Color.White);
				}
			}
			spriteBatch.Draw(
				body,
				mannequinOffset.ToVector2() + new Vector2(28, -12),
				new Rectangle(0, 0, bodySize.X, bodySize.Y),
				Color.White);

			// Draw the item if drawdepth is in the hand
			if (drawDepth == DrawDepthID.Hand) {
				foreach (var data in dummyInfo.DrawDataCache) {
					Main.spriteBatch.Draw(data.texture, data.position, data.sourceRect, data.color, data.rotation, data.origin, data.scale, data.effect, 0);
				}
			}


			// Preview the front arm
			Point frameCoord = new Point(2, 0);
			float rotation = 0f;
			if (dummyInfo.drawPlayer.compositeFrontArm.enabled) {
				rotation = dummyInfo.drawPlayer.compositeFrontArm.rotation;
				switch (dummyInfo.drawPlayer.compositeFrontArm.stretch) {
					case Player.CompositeArmStretchAmount.Full: frameCoord = new Point(7, 0); break;
					case Player.CompositeArmStretchAmount.ThreeQuarters: frameCoord = new Point(7, 1); break;
					case Player.CompositeArmStretchAmount.Quarter: frameCoord = new Point(7, 2); break;
					case Player.CompositeArmStretchAmount.None: frameCoord = new Point(7, 3); break;
				}
			}
            else {
				switch (dummyInfo.drawPlayer.bodyFrame.Y / dummyInfo.drawPlayer.bodyFrame.Height) {
					case 1: frameCoord.X += 1; break;
					case 2: frameCoord.X += 2; break;
					case 3: frameCoord.X += 3; break;
					case 4: frameCoord.X += 4; break;
					case 5: frameCoord.Y += 1; break;
					case 6:
					case 11:
					case 12:
					case 13:
					case 18:
					case 19:
						frameCoord = new Point(4, 1); break;
					case 7:
					case 8:
					case 9:
					case 10:
						frameCoord = new Point(3, 1); break;
					case 14:
					case 17:
						frameCoord = new Point(5, 1); break;
					case 15:
					case 16:
						frameCoord = new Point(6, 1); break;
				}
			}
			spriteBatch.Draw(
				body,
				mannequinOffset.ToVector2() + new Vector2(44, 16),
				new Rectangle(
					frameCoord.X * bodySize.X,
					frameCoord.Y * bodySize.Y,
					bodySize.X,
					bodySize.Y),
				Color.White,
				rotation,
				new Vector2(16, 28),
				1f,
				SpriteEffects.None, 0f);

			// Draw the item if drawdepth is front
			if (drawDepth == DrawDepthID.Front) {
				foreach (var data in dummyInfo.DrawDataCache) {
					Main.spriteBatch.Draw(data.texture, data.position, data.sourceRect, data.color, data.rotation, data.origin, data.scale, data.effect, 0);
				}
			}


		}

		/// <summary>
		/// The item to draw in this preview element
		/// </summary>
		/// <returns>ItemID of the item</returns>
		public abstract int PreviewItemType();

		/// <summary>
		/// Use this method to make final adjustments to the item texture before draw style is set up
		/// </summary>
		/// <param name="texture">texture of the item being drawn</param>
		/// <param name="item">item object being drawn</param>
		public virtual void OverrideDrawItem(ref Texture2D texture, ref Item item) { }

		/// <summary>
		/// Adjust the scale of the item being drawn
		/// </summary>
		/// <returns></returns>
		public virtual float SetScale() { return 1f; }
	}

	public class PreviewSmallItem : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.DirtBlock; }
	public class PreviewLargeItem : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Furnace; }
    public class PreviewVanityItem : HoldStylePreviewElement
    { public override int PreviewItemType() => ItemID.RedDye; }
    public class PreviewPotionItem : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.BottledWater; }
	public class PreviewSmallMelee : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.CopperBroadsword; }
    public class PreviewSmallTool : HoldStylePreviewElement
    { public override int PreviewItemType() => ItemID.IronPickaxe; }
    public class PreviewRapier : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.CopperShortsword; }
	public class PreviewSpear : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Spear; }
	public class PreviewFlail : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Mace; }
	public class PreviewYoyo : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.WoodYoyo; }
	public class PreviewPowerTool : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Drax; }
    public class PreviewLargeMelee : HoldStylePreviewElement
    { public override int PreviewItemType() => ItemID.BladeofGrass; }
    public class PreviewLargeTool : HoldStylePreviewElement
    { public override int PreviewItemType() => ItemID.ReaverShark; }
    public class PreviewThrown : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Shuriken; }
	public class PreviewThrownThin : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.ThrowingKnife; }
	public class PreviewBow : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.WoodenBow; }
	public class PreviewRepeater : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.CobaltRepeater; }
	public class PreviewPistol : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.FlintlockPistol; }
	public class PreviewGun : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Minishark; }
	public class PreviewGunManual : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Musket; }
	public class PreviewShotgun: HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.Boomstick; }
	public class PreviewLauncher : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.RocketLauncher; }
	public class PreviewStaff : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.AmethystStaff; }
	public class PreviewMagicBook : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.WaterBolt; }
	public class PreviewMagicItem : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.SharpTears; }
	public class PreviewWhip : HoldStylePreviewElement
	{ public override int PreviewItemType() => ItemID.BlandWhip; }
	public class PreviewGiantItem : HoldStylePreviewElement
	{
		public override float MinHeightPx => 128f;
		public override int PreviewItemType() => ItemID.AngelStatue;
		public override float SetScale() => ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
		public override void OverrideDrawItem(ref Texture2D texture, ref Item item) {
			texture = ModContent.Request<Texture2D>("WeaponOutLite/Assets/Textures/UI/GiantItem", AssetRequestMode.ImmediateLoad).Value;
		}
	}
	public class PreviewGiantMelee : HoldStylePreviewElement
	{
		public override float MinHeightPx => 128f;
		public override int PreviewItemType() => ItemID.WoodenSword;
		public override float SetScale() => ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
		public override void OverrideDrawItem(ref Texture2D texture, ref Item item) {
			texture = ModContent.Request<Texture2D>("WeaponOutLite/Assets/Textures/UI/GiantMelee", AssetRequestMode.ImmediateLoad).Value;
		}
	}
	public class PreviewGiantBow : HoldStylePreviewElement
	{
		public override float MinHeightPx => 128f;
		public override int PreviewItemType() => ItemID.WoodenBow;
		public override float SetScale() => ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
		public override void OverrideDrawItem(ref Texture2D texture, ref Item item) {
			texture = ModContent.Request<Texture2D>("WeaponOutLite/Assets/Textures/UI/GiantBow", AssetRequestMode.ImmediateLoad).Value;
		}
	}
	public class PreviewGiantGun : HoldStylePreviewElement
	{
		public override float MinHeightPx => 128f;
		public override int PreviewItemType() => ItemID.FlintlockPistol;
		public override float SetScale() => ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
		public override void OverrideDrawItem(ref Texture2D texture, ref Item item) {
			texture = ModContent.Request<Texture2D>("WeaponOutLite/Assets/Textures/UI/GiantGun", AssetRequestMode.ImmediateLoad).Value;
		}
	}
	public class PreviewGiantMagic : HoldStylePreviewElement
	{
		public override float MinHeightPx => 128f;
		public override int PreviewItemType() => ItemID.WandofSparking;
        public override float SetScale() => ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
		public override void OverrideDrawItem(ref Texture2D texture, ref Item item) {
			texture = ModContent.Request<Texture2D>("WeaponOutLite/Assets/Textures/UI/GiantMagic", AssetRequestMode.ImmediateLoad).Value;
		}
	}
	public class PreviewHeldItem : HoldStylePreviewElement
	{
		public override int PreviewItemType()
		{
			if (Main.LocalPlayer != null && Main.LocalPlayer.HeldItem != null) {
				return Main.LocalPlayer.HeldItem.type;
			}
			return ItemID.DirtBlock;
		}
	}
}
