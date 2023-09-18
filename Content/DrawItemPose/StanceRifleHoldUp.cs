using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Hold rifle upwards (offhand)
    /// </summary>
    public class StanceRifleHoldUp : OnBackDownward
    {
        public override int GetID() => DrawItemPoseID.StanceRifleHoldUp;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || (p.bodyFrame.Y != 0 && p.bodyFrame.Y < p.bodyFrame.Height * 4);

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.5f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Back;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            float t = DrawHelper.AnimLinearNormal(30, timer);
            if (t > 0) {
                float sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
                float rotation = (float)Math.PI * (-0.4f - sheatheRotation * 0.5f);
                if(t > 0.5f) {
                    // past halfway point, go from top all the way to bottom
                    rotation = (float)Math.PI * (-sheatheRotation * 0.9f);
                }

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                if (t > 0.5f) backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, rotation * p.direction);
            }
            else if (bodyFrame == 0 ) {
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, MathHelper.PiOver2 * -0.75f * p.direction);

            }
            else if (bodyFrame == 5) {
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, MathHelper.PiOver2 * -1f * p.direction);

            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.25f, 0.5f, p);

            if (bodyFrame == 0) { // Standing
                data.rotation = (float)(Math.PI * -0.375f);
                data.position += new Vector2(
                    13,
                    -2);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation = (float)(Math.PI * -0.5f);
                data.position += new Vector2(
                    12,
                    -4);
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation = (float)(Math.PI * -0.375);
                data.position += new Vector2(
                    9,
                    0);
                data = data.WithOffHandOffset(p);
            }
            else { // Grapple/Pulley/Mount
                data.color = Color.Transparent;
            }

            // Sheathing only
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                data.position.X -= 40 * t;
                data.position.Y -= 20 * t;
                data = DrawHelper.LerpData(data, idleData, t);
                return data;
            }

            return data;
        }
    }
}
