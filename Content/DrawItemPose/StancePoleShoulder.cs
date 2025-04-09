using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    public class StancePoleShoulder : OnBack
    {
        public override int GetID() => DrawItemPoseID.StancePoleShoulder;

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

            float t = DrawHelper.AnimLinearNormal(30, timer);
            if (t > 0) {
                float sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * -1f;

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;

                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * sheatheRotation * p.direction);
                return bodyFrame;
            }

            if (bodyFrame == 0) {
                // Visually no change in the back hand, but this helps recognise when bodyframe 12 = standingstill
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, 0f);
                return 10;
            }
            if(bodyFrame == 5) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.5f * p.direction);
            }
            else if (bodyFrame > 5)
            {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Quarter;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.25f * p.direction);
            }
            

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }
            Main.NewText($"width={width / height}");
            data = data.SetOrigin(0.5f - 0.4375f + 0 * (height / height), 0.5f + 0.4375f + 0* (width / height), p).ApplyFlip(p).RotateFaceForward(p, height, width);


            if (bodyFrame == 0 || (bodyFrame == 10 && p.compositeBackArm.enabled)) {
                // Standing
                data.position += new Vector2(6, 20);
                data.rotation += (float)(Math.PI * -0.675f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(16, 18);
                data = data.WithWaistOffset(p);
                data.rotation += (float)(Math.PI * -0.75f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(18, 8);
                data.rotation += (float)(Math.PI * -0.825f);
            }
            else if (p.IsMountPoseActive()) {
                data.position += new Vector2(14, 22);
                data.rotation += (float)(Math.PI * -0.675f);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                data.position.X += width * t;
                data.position.Y -= 128 * t;
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data;
        }
    }
}