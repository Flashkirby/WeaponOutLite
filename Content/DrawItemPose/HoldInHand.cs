using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// General purpose.
    /// Can also be used for throwing weapons
    /// </summary>
    public class HoldInHand : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.HoldInHand;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Hand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.05f, 0.85f, p);

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
                data.rotation += (float)(Math.PI * 0.35f);
                data.position += new Vector2(
                    (-5f),
                    (5f));
                data = data.WithHandOffset(p);
            }
            else { // Grapple/Pulley
                data.color = Color.Transparent;
            }
            return data;
        }
    }
}
