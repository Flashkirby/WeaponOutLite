using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Hip forward pistol hold -> backward spin to holster
    /// </summary>
    public class StancePistolCowboy : OnWaistHolster
    {
        public override int GetID() => DrawItemPoseID.StancePistolCowboy;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.5f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;

            if (t > 0) {
                // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
                p.shield = -1; // Hide shield
                p.cShield = -1; // Hide cosmetic shield

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                if (sheatheRotation > 0.5f) backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-sheatheRotation * 0.75f) * p.direction);
            }
            else if (bodyFrame == 0) {
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, 0f);
                bodyFrame = 17;
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data.origin = DrawHelper.NewOrigin(0.25f, 0.5f, data, p).Round2(p);

            // Sheathing only
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                data.rotation = (float)(Math.PI * 4.125f);
                data.position += new Vector2(6f + t * 48f, 10f - t * 86f);
                data = DrawHelper.LerpData(data, idleData, t);
                return data;
            }

            if (bodyFrame == 0 || (bodyFrame == 17 && p.compositeBackArm.enabled)) { // Standing
                data.rotation = (float)(Math.PI * 0.125f);
                data.position += new Vector2(
                    0f, 
                    6f);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation = (float)(Math.PI * -0.25f);
                data.position += new Vector2(
                    (-10f),
                    (-8f));
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation = (float)(Math.PI * 0.25f);
                data.position += new Vector2(
                    (-4f),
                    (6f));
                data = data.WithHandOffset(p);
            }
            else { // Grapple/Pulley
                data.color = Color.Transparent;
            }

            return data;
        }
    }
}
