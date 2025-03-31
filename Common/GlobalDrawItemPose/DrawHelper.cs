using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;
using Microsoft.Xna.Framework.Graphics;
using WeaponOutLite.Common.Configs;

namespace WeaponOutLite.Content.DrawItemPose
{
    internal static class DrawHelper
	{
		public static float AnimArmWaggle(float normal) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			return -(float)Math.Sin(2f * Math.Pow(normal, 2) * Math.PI) * 0.5f;
		}

		/// <summary>
		/// Passes the normal through a sine function
		/// </summary>
		/// <returns>Value from 0 to 1 to 0</returns>
		public static float AnimArmRaiseLower(float normal, float speed = 1f) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			return (float)Math.Sin(Math.Pow(normal, speed) * Math.PI);
		}

		/// <summary>
		/// linear (no curve, 0 to 1)
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimLinearNormal(float duration, float timer) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			var sheathing = Math.Clamp(1f - timer / duration, 0f, 1f);
			return sheathing;
		}
		/// <summary>
		/// ease out ease in = start 0 fast, slow down, then accelerate (towards 1)
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimEaseOutEaseInNormal(float duration, float timer) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			var sheathing = Math.Clamp(1f - timer / duration, 0f, 1f);
			var sheathingCurve = (float)Math.Pow(sheathing - 0.5f, 3) * 4f + 0.5f;
			return sheathingCurve;
		}
		/// <summary>
		/// ease out ease in = start 0 slow, speed up, then slow again (towards 1)
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimEaseInEaseOutNormal(float duration, float timer) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			var sheathing = Math.Clamp(1f - timer / duration, 0f, 1f);
			var sheathingCurve = 0.5f - 0.5f * (float)Math.Cos(sheathing * Math.PI);
			return sheathingCurve;
		}
		/// <summary>
		/// Ease out = start 0 fast, slow down to 1
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimEaseOutNormal(float duration, float timer) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			var sheathing = Math.Clamp(1f - timer /duration, 0f, 1f);
			var sheathingCurve = 1 + (float)Math.Pow(sheathing - 1f, 3);
			return sheathingCurve;
		}
		/// <summary>
		/// Ease in = start 0 slow, speed up to 1. 
		/// 0.5 - 0.5 cos(x pi)
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimEaseInNormal(float duration, float timer) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			var sheathing = 1f - Math.Min(timer, duration) / (float)(duration);
			var sheathingCurve = (float)Math.Pow(sheathing, 3);
			return sheathingCurve;
		}
		/// <summary>
		/// Ease in = start 0 linear to 1.14, then back to 1. 
		/// 0.5 - 0.5 * cos(x * pi) +   0.2 - 0.2 * cos(2x * pi)
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimOverEaseOutNormal(float duration, float timer) {
			if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
			var sheathing = 1f - Math.Min(timer, duration) / (float)(duration);
			var sheathingCurve = 0.675f - 0.5f * (float)Math.Cos(sheathing * Math.PI) - 0.175f * (float)Math.Cos(2 * sheathing * Math.PI);
			return (float)sheathingCurve;
		}

		/// <summary>
		/// Ease in = start 0 speeds up to 1.281, then back to 1. 
		/// 
		/// </summary>
		/// <returns>Returns a value from 0 to 1, as timer reaches 0</returns>
		public static float AnimOverEaseNormal(float duration, float timer) {
            if (!ModContent.GetInstance<WeaponOutClientConfig>().EnableSheathingAnim) return 0f;
            var sheathing = 1f - Math.Min(timer, duration) / (float)(duration);
			// var sheathingCurve = (Math.Cos(sheathing * 1.5f * Math.PI) / (30 * Math.Pow(sheathing, 2) + 1)); // 1 - cos 2xpi * 1/(30x^2 + 1) * 1.032
			var sheathingCurve = 0.55f - 0.55f * (float)Math.Cos(sheathing * 1.2 * Math.PI);
			return (float)sheathingCurve;
        }

        /// <summary>
        /// Lerp origin, position, and rotation properties from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="amount"></param>
        /// <returns>DrawData lerped</returns>
        public static DrawData LerpData(DrawData start, DrawData end, float amount) {
			DrawData mix = start;
			mix.origin.X = MathHelper.Lerp(start.origin.X, end.origin.X, amount);
			mix.origin.Y = MathHelper.Lerp(start.origin.Y, end.origin.Y, amount);
			mix.position.X = MathHelper.Lerp(start.position.X, end.position.X, amount);
			mix.position.Y = MathHelper.Lerp(start.position.Y, end.position.Y, amount);
			mix.rotation = MathHelper.Lerp(start.rotation, end.rotation, amount);
			return mix;
		}

		/// <summary>
		/// Shortcut for muiltiplying vector by direction for X and gravDir for Y
		/// </summary>
		public static Vector2 ApplyDirection(this Vector2 value, Player p) {
			return new Vector2(value.X * p.direction, value.Y * p.gravDir);
        }

		public static bool IsMountPoseActive(this Player player) {
			return player.mount.Active && player.bodyFrame.Y >= player.bodyFrame.Height && player.bodyFrame.Y < 5 * player.bodyFrame.Height;

		}

		public static float Round2(this float value) {
			return (float)(int)Math.Round(value / 2f) * 2f;
		}

		/// <summary>Obsolete, use overload with player</summary>
		[Obsolete]
		public static Vector2 Round(this Vector2 value) {
			return new Vector2((float)(int)Math.Round(value.X), (float)(int)Math.Round(value.Y));
		}
		public static Vector2 Round(this Vector2 value, float xDir, float yDir) {
			var xRounding = xDir < 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToPositiveInfinity;
			var yRounding = yDir < 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToPositiveInfinity;
			return new Vector2((int)Math.Round(value.X, xRounding), (int)Math.Round(value.Y, yRounding));
		}

		/// <summary>Proper "2pixel perfect" rounding, without special offset alignment. Use this for positional setting only. </summary>
		public static Vector2 Round2(this Vector2 value) {
			return new Vector2((float)(int)Math.Round(value.X / 2f) * 2f, (float)(int)Math.Round(value.Y / 2f) * 2f);
		}

		/// <summary>
		/// Proper "2pixel perfect" rounding, taking into account rounding preference on flipped x and y axes.
		/// Use this for origin setting
		/// </summary>
		/// <param name="value"></param>
		/// <param name="xDir"></param>
		/// <param name="yDir"></param>
		/// <returns></returns>
		public static Vector2 Round2(this Vector2 value, float xDir, float yDir) {
			var xRounding = xDir < 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToPositiveInfinity;
			var yRounding = yDir < 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToPositiveInfinity;
			return new Vector2((float)(int)Math.Round(value.X / 2f, xRounding) * 2f, (float)(int)Math.Round(value.Y / 2f, yRounding) * 2f);
		}

		public static Vector2 Round2(this Vector2 value, Player p) {
			var xRounding = p.direction < 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToPositiveInfinity;
			var yRounding = p.gravDir < 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToPositiveInfinity;
			return new Vector2((float)(int)Math.Round(value.X / 2f, xRounding) * 2f, (float)(int)Math.Round(value.Y / 2f, yRounding) * 2f);
		}

		public static Vector2 NewOrigin(float originX, float originY, Rectangle? sourceRect, float xDir, float yDir) {
			var size = sourceRect?.Size() ?? default(Vector2);
			var centre = new Vector2(0.5f, 0.5f);
			var originOffset = new Vector2(0.5f - originX, 0.5f - originY);
			var offsetDirection = new Vector2(xDir, yDir);
			return size * (centre - originOffset * offsetDirection);
		}

		[Obsolete]
		public static Vector2 NewOrigin(float originX, float originY, DrawData data, Player player) {
			return NewOrigin(originX, originY, data.sourceRect, player.direction, player.gravDir);
		}

		/// <summary>
		/// Place the origin at the middle of a pixel given originX and originY as normals (0 to 1 = 0 to max)
		/// </summary>
		/// <param name="data"></param>
		/// <param name="originX"></param>
		/// <param name="originY"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public static DrawData SetOrigin(this DrawData data, float originX, float originY, Player player) {
			return SetOrigin(data, originX, originY, player.direction, player.gravDir);
		}

		/// <summary>
		/// Place the origin at the middle of a pixel given originX and originY as normals (0 to 1 = 0 to max)
		/// </summary>
		/// <param name="data"></param>
		/// <param name="originX"></param>
		/// <param name="originY"></param>
		/// <returns></returns>
		public static DrawData SetOrigin(this DrawData data, float originX, float originY, float xDir, float yDir) {
			data.origin = NewOrigin(originX, originY, data.sourceRect, xDir, yDir).Round2(xDir, yDir);
			//- player.Directions;
			//data.position -= player.Directions;

			// For items with even boundaries
			if (data.origin.Y % 2 == 0) {
				data.origin.Y -= yDir; // pick the pixel above the split
				data.position.Y += 1;
			}
			if (data.origin.X % 2 == 0) {
				data.origin.X += xDir; // pick the pixel left of the split
				data.position.X -= 1;
			}
			return data;
		}

		/// <summary>
		/// Clamp scale to 1, if it is near enough.
		/// Because sometimes an item at 1.05 scale just doesn't look good.
		/// </summary>
		/// <param name="scale">DrawData scale</param>
		/// <returns>Modified DrawData</returns>
		public static Vector2 SnapNearOne(Vector2 scale, float near = 0.1f, float to = 1f) {
			if (Math.Abs(scale.X - to - 0.001f) <= near) scale.X = to;
			if (Math.Abs(scale.Y - to - 0.001f) <= near) scale.Y = to;
			return scale;
		}

		public static float SnapNearOne(float scale, float near = 0.1f, float to = 1f) {
			if (Math.Abs(scale - to - 0.001f) <= near) scale = to;
			return scale;
		}

		public static float GetHandleLength(float handleRatioX, float handleRatioY, float width, float height) {
			var size = new Vector2(width, height);
			var ratio = new Vector2(handleRatioX, handleRatioY);
			var handleVector = size * ratio;
			return handleVector.Length() / 4f;
		}

		/// <summary>
		/// CALL AFTER SPRITE FLIPPING, BEFORE ROTATION. Rotates the default item draw data to point upwards and to the right.
		/// Useful for homogenising sprites of different sizes so they face the same direction.
		/// </summary>
		/// <returns>DrawData with rotation</returns>
		public static DrawData RotateFaceForward(this DrawData data, Player player, float height, float width) {
			float rotationOffset = data.GetRotateFaceForwardRotationOffset(player, height, width);
			data.rotation = (float)(Math.PI * (0.25 + rotationOffset)); // rotate up to 45 deg to face top right, then turn forwards.
			return data;
		}

		/// <summary>
		/// Find the amount to rotate an object anticlockwise or clockwise so it faces the default direction (Top Right)
		/// </summary>
		/// <returns>Value between -0.25f (for wide items) to 0.25f (for tall items).</returns>
		public static float GetRotateFaceForwardRotationOffset(this DrawData data, Player player, float height, float width) {
			float rotationOffset;
			// Rotate clockwise by this much. A tall item should rotate clockwise up to 45, and a wide item should do the reverse.
			// Reduce ratio sensitivity for smaller items (require more extreme value to bend)
			var inverseRatioSensitivity = (height < 32 || height < 32) ? 1f : 0.5f;
			rotationOffset = (height - width) / Math.Min(height, width) * inverseRatioSensitivity;
			rotationOffset = Math.Clamp(rotationOffset, -1f, 1f);
			if (data.effect.HasFlag(SpriteEffects.FlipVertically) == player.direction > 0) {
				rotationOffset = -rotationOffset - 2f;
			}
			return rotationOffset / 4f;
		}

		public static DrawData ApplyFlip(this DrawData data, float xDir, float yDir) {
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (xDir > 0) spriteEffects = SpriteEffects.FlipVertically;
			if (yDir < 0) {
				spriteEffects = SpriteEffects.FlipHorizontally | spriteEffects;
			}
			data.effect = spriteEffects;

			// When flipping an even number, pick the opposite pixel. See set origins for more details
			if (data.sourceRect.Value.Height % 4 == 0) data.position.Y += 2;
			return data;
		}

		/// <summary>
		/// Apply flip, run this after setting origin but before rotation
		/// </summary>
		/// <param name="data">drawdata of the item</param>
		/// <param name="player">player holding item</param>
		/// <returns>drawdata</returns>
		public static DrawData ApplyFlip(this DrawData data, Player player) {
			return data.ApplyFlip(player.direction, player.gravDir);
		}

		/// <summary>
		/// Modifies the scale according to the configs if the item surpasses the "giant" threshold"
		/// </summary>
		public static float GetGiantTextureScale(float width, float height) {
			var threshold = ModContent.GetInstance<WeaponOutClientConfig>().GiantItemThreshold;
			if (threshold < Math.Max(width, height)) {
				return ModContent.GetInstance<WeaponOutClientConfig>().GiantItemScalePercent / 100f;
			}
			return 1f;
		}

	}
}
