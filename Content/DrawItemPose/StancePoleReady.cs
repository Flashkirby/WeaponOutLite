using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    public class StancePoleReady : OnBack
    {
        public override int GetID() => DrawItemPoseID.StancePoleReady;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.25f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }

            float t = DrawHelper.AnimLinearNormal(30, timer);
            if (t > 0 && bodyFrame != 5) {
                float sheatheRotation = DrawHelper.AnimArmRaiseLower(t, 1f) * 0.33f;

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                if (t > 0.2 && t < 0.8) frontArm = Player.CompositeArmStretchAmount.None;

                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * sheatheRotation * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.375f, 0.625f, p).RotateFaceForward(p, height, width);

            if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(-4, 14);
                data.rotation -= (float)(Math.PI * 1.75);
            }
            else if (bodyFrame > 5 ) {
                // Runndding
                data.position += new Vector2(1, 8);
                data = data.WithHandOffset(p);
                data.rotation -= (float)(Math.PI * 2f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(-8, -8);
                data.rotation -= (float)(Math.PI * 2.25f);
            }
            else if (p.IsMountPoseActive()) {
                data.position += new Vector2(13, 6);
                data.rotation -= (float)(Math.PI * 2.125f);
            }

            // Sheathing OnBack
            float t = DrawHelper.AnimEaseOutNormal(30, timer);
            if (t > 0)
            {
                data.position.X -= 8f * (float)Math.Sin(t * Math.PI);
                data.position.Y += 16f * (float)Math.Sin(t * 0.75f * Math.PI);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data;
        }
    }
}
