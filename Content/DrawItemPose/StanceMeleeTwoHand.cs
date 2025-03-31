using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Two hand forward to hold steady
    /// </summary>
    public class StanceMeleeTwoHand : OnBack
    {
        public override int GetID() => DrawItemPoseID.StanceTwoHand;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley || p.shieldRaised;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(30, timer) > 0.2f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            if (bodyFrame == 0 && t < 0.3f) {
                if(p.legFrame.Y == 0) p.legFrame.Y = 9 * p.legFrame.Height;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.3f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.2f - sheatheRotation) * p.direction);
            }
            else {

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.15f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.1f - sheatheRotation) * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.1f, 0.9f, p);

            if (bodyFrame == 0) {
                data.position += new Vector2(8, 10);
            }
            else if (bodyFrame >= 5) {
                data.position += new Vector2(4, 13);

                data.rotation += (float)(Math.PI * 0.15f);

                if (p.IsMountPoseActive()) {
                    data.position += new Vector2(2, -8);
                }
            }
            else if (p.IsMountPoseActive()) {
                data.position += new Vector2(10, 4);
            }

            // Sheathing
            float t = DrawHelper.AnimOverEaseOutNormal(30, timer);
            data.position.X += 16f * (float)Math.Sin(t * Math.PI);
            data.position.Y -= height / 2 * (float)Math.Sin(t * Math.PI);
            if (t > 1f / 2f) {
                // flip item at the halfway point
                data = data.ApplyFlip(p);
                data.rotation -= MathHelper.PiOver2;
                data = DrawHelper.LerpData(data, idleData, t);
            }
            else {
                idleData.rotation += MathHelper.PiOver2;
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
