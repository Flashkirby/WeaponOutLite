using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Special hold stance for lances, which are wielded differently to other spears.
    /// These are held at the base.
    /// </summary>
    public class JoustingLance : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.JoustingLance;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Hand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer)
        {
            if (bodyFrame == 0) return 6;
            if (bodyFrame == 5) return 10;
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.1f, 0.9f, p);

            if (bodyFrame == 0) { // Standing
                data.rotation += (float)(Math.PI * 0.5f);
                data.position += new Vector2(-6f, 8f);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation += (float)(Math.PI * -2.25f);
                data.position += new Vector2(
                    (-9),
                    (-7f));
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation += (float)(Math.PI * 0.3f);
                data.position += new Vector2(
                    (-6f),
                    (9f));
                data = data.WithHandOffset(p);
            }
            else if (p.IsMountPoseActive()) { // Mount
                float speedRotation = 0.25f;
                if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                    float maxSpeed = 3f;
                    speedRotation = Math.Clamp(p.velocity.X * p.direction, 0f, maxSpeed);
                    speedRotation = speedRotation / maxSpeed * 0.25f;
                }
                // 0.25f to 0.00f
                data.rotation -= (float)(Math.PI * (0.25f - speedRotation));
                data.position += new Vector2(
                    (8f),
                    (8f));
            }
            else { // Grapple/Pulley
                data.color = Color.Transparent;
            }
            return data;
        }
    }
}
