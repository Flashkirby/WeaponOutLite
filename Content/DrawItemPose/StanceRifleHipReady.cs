using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Hold facing forward
    /// </summary>
    public class StanceRifleHipReady : OnBackDownward
    {
        public override int GetID() => DrawItemPoseID.StanceRifleHipReady;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.4f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            if (sheatheRotation > 0) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                if (sheatheRotation > 0.5f) backArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-sheatheRotation * 0.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Quarter;
                if (sheatheRotation > 0.75f) frontArm = Player.CompositeArmStretchAmount.Full;
                else if (sheatheRotation > 0.5f) frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-sheatheRotation * 1f) * p.direction);
            }
            else if(bodyFrame==0){
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.25f * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.25f * p.direction);
            }
            else if (bodyFrame == 5) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.1f * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0f * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            //data.origin = DrawHelper.NewOrigin(0.25f, 0.25f, data, p).Round2(p);
            data = data.SetOrigin(0.25f, 0.25f, p);

            if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(6, 2);
            }
            else if (bodyFrame > 5) {
                // Running
                data = data.WithHandOffset(p);
                float rot = 0;
                switch (bodyFrame) {
                    case 06: rot = -0.75f; break;
                    case 07: rot = -1f; break;
                    case 08: rot = -0.0f; break;
                    case 09: rot = 0.25f; break;
                    case 10: rot = 0.5f; break;
                    case 11: rot = 0.25f; break;
                    case 12: rot = 0f; break;
                    case 13: rot = -0.75f; break;
                    case 14: rot = -1f; break;
                    case 15: rot = 0f; break;
                    case 16: rot = 0.25f; break;
                    case 17: rot = 0.5f; break;
                    case 18: rot = 0.25f; break;
                    case 19: rot = 0f; break;
                }
                data.rotation += 0.05f * rot;
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(-2, 2);
                data.rotation -= 0.15f;
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            data.position.X += 8f * (float)Math.Sin(t * Math.PI);
            data = DrawHelper.LerpData(data, idleData, t);

            return data;
        }
    }
}
