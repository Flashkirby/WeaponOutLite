using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Held upright, resting on the ground
    /// </summary>
    public class StancePoleUpright : OnBack
    {
        public override int GetID() => DrawItemPoseID.StancePoleUpright;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.75f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }

            float t = DrawHelper.AnimLinearNormal(30, timer);
            if (t > 0) {
                float sheatheRotation = 0f;

                if (bodyFrame == 5) // jumping
                {
                    sheatheRotation = -1.25f + DrawHelper.AnimArmRaiseLower(t) * 1.25f;
                }
                else
                {
                    sheatheRotation = DrawHelper.AnimArmWaggle(t) * 0.3f;
                }

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Quarter;
                if (t > 0.2 && t < 0.8) frontArm = Player.CompositeArmStretchAmount.Full;

                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * sheatheRotation * p.direction);
                return bodyFrame;
            }
            if (bodyFrame == 0)
            {
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, 0f);
                bodyFrame = 6;
            }
            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            //data.origin = DrawHelper.NewOrigin(0.1f, 0.9f, data, p).Round2(p);
            data = data.SetOrigin(0.5f - 0.4f * (width / height), 0.9f, p).RotateFaceForward(p, height, width);

            if (bodyFrame == 0 || bodyFrame > 5) {
                // Idle/Running
                data.position += new Vector2(
                    -2, 
                    17);
                data = data.WithHandOffset(p);
                data.rotation -= (float)(Math.PI * 2.5f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(
                    0,
                    -2);
                data = data.WithHandOffset(p);
                data.rotation -= (float)(Math.PI * 2.75f);
            }
            else if (p.IsMountPoseActive()) { // Mount
                data = data.SetOrigin(0.25f, 0.75f, p);
                float speedRotation = 0.25f;
                if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                    float maxSpeed = 3f;
                    speedRotation = Math.Clamp(p.velocity.X * p.direction, 0f, maxSpeed);
                    speedRotation = speedRotation / maxSpeed * 0.25f;
                }
                // 0.25f to 0.00f
                data.rotation -= (float)(Math.PI * (2.5f - speedRotation));
                data.position += new Vector2(
                    (10f),
                    (6f));
            }

            // Sheathing OnBack
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if(t > 0)
            {
                data.position.X += 8f * (float)Math.Sin(t * Math.PI * 2f);
                data.position.Y -= 8f * (float)Math.Sin(t * 0.5f * Math.PI);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data;
        }
    }
}
