using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Resting on the back shoulder. A classic Guts pose. 
    /// </summary>
    public class StanceMeleeTwoHandBerserk : OnBack
    {
        public override int GetID() => DrawItemPoseID.StanceTwoHandBerserk;

        private bool CanUseBasePose(Player p, int timer) =>
            timer == 0 || p.shieldRaised;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.2f) {
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
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t);
                if (t > 0.5f) {
                    sheatheRotation = sheatheRotation - 0.5f;
                }
                else {
                    sheatheRotation *= 0.5f;
                }
            }

            Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
            p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.3f - sheatheRotation) * p.direction);

            //// Moving to rest pose
            //float animationTime = -(timer - WeaponOutLite.ClientConfig.CombatDelayTimerMax * 60f);
            //if (t == 0 && animationTime <= p.HeldItem.useAnimation * 1.5f) {
            //    var at = DrawHelper.AnimEaseOutNormal(p.HeldItem.useAnimation * 1.5f, animationTime);
            //    if(at > 0.5f) bodyFrame = 4;
            //    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.3f + 0.2f * at) * p.direction);
            //}
            

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.1f, 0.9f, p);

            data.position += new Vector2(14, 2);

            data.rotation += (float)(Math.PI * -0.5f);


            // Sheathing OnBack
            float t = DrawHelper.AnimOverEaseNormal(30, timer);
            if (t > 0f)
            {
                data.position.X += 16 * (float)Math.Sin(t * Math.PI);
                data.position.Y -= 24 * (float)Math.Sin(t * Math.PI * 0.5f);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
