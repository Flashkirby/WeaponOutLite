using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Hold like a minigun or chainsaw
    /// </summary>
    public class StanceMachineTool : OnBackUpright
    {
        public override int GetID() => DrawItemPoseID.StanceMachineTool;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.5f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }

            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmWaggle(t) * 0.5f;
            }

            if (bodyFrame == 0 && sheatheRotation == 0) {
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, (float)Math.PI * (-0.25f) * p.direction);
                return bodyFrame;
            }
            if (bodyFrame == 0 || bodyFrame >= 5) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;

                if (sheatheRotation > 0f) {
                    backArm = Player.CompositeArmStretchAmount.Quarter;
                    frontArm = Player.CompositeArmStretchAmount.None;
                }
                else {
                    backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                    frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                }

                if (bodyFrame != 5) {
                    // standing/moving
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.25f + sheatheRotation * 2) * p.direction);
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (0f - sheatheRotation) * p.direction);
                }
                else {
                    //jumping
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.3f + sheatheRotation * 2) * p.direction);
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (0f - sheatheRotation) * p.direction);
                }
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.1f, 0.5f, p);

            if (bodyFrame == 0 || (bodyFrame == 13 && !p.compositeBackArm.enabled)) {
                // Standing
                data.position += new Vector2(-8, 8);
                //data.rotation += (float)(Math.PI * 0.25f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(-7, 7);
                data.rotation -= (float)(Math.PI * 0.05f);
                data = data.WithWaistOffset(p);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(-7, 8);
                data.rotation -= (float)(Math.PI * 0.1f);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                data.position.X -= 16 * t;
                data.rotation -= 3f * t;
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data;
        }
    }
}
