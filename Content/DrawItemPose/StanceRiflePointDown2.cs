using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Hold face diagonally downwards
    /// </summary>
    public class StanceRiflePointDown2 : HoldRifleOffHandUpright
    {
        public override int GetID() => DrawItemPoseID.StanceRiflePointDown2;

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
                float sheatheRotation = DrawHelper.AnimEaseOutNormal(30, timer) * -0.4f;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * sheatheRotation * p.direction);

                if (t < 0.4f) {
                    bodyFrame = 17;
                }
            }
            else if(bodyFrame==0 || bodyFrame > 5){
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;

                if (bodyFrame == 0 || bodyFrame % 7 > 2) {
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.125f * p.direction);
                }
                else if(bodyFrame < 14) {
                    // inwards
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.05f * p.direction);
                }
                else {
                    // outwards
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.15f * p.direction);
                }


                if (bodyFrame == 0) bodyFrame = 6;
            }
            else if (bodyFrame == 5) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
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

            data = data.SetOrigin(0.25f, 0.25f, p);

            if (bodyFrame == 0 || bodyFrame > 5) {
                // Standing / Running
                data.position += new Vector2(2, -2).Round2(p);
                data = data.WithHandOffset(p);
                data.rotation += 0.5f;
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(-2, 2).Round2(p);
                data.rotation += 0.15f;
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            data.position.X += 8f * (float)Math.Sin(t * Math.PI);
            data = DrawHelper.LerpData(data, idleData, t);

            return data;
        }
    }
}
