using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Holding down and flipped, to look like the book is being read
    /// </summary>
    public class HoldSpellTome : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.HoldSpellTome;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Hand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer)
        {

            if (bodyFrame == 0) {
                bodyFrame = 4;
            }
            else if (bodyFrame == 5) {
                p.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -p.direction);
            }
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer)
        {

            data = data.SetOrigin(0.25f, 0.75f, p).ApplyFlip(p);
            data.rotation = -MathHelper.PiOver2;

            if (bodyFrame == 4) { // Standing
                data.position += new Vector2(9, 7);
            }
            else if (bodyFrame == 0) { // Standing (menu)
                data.position += new Vector2(4, 12);
                data.rotation += MathHelper.PiOver4 * 5f;
            }
            else if (bodyFrame == 5) { // Jumping
                data.position += new Vector2(12, 2);
                data.rotation -= MathHelper.PiOver4 / 2;
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation += MathHelper.PiOver4 * 5f;
                data.position += new Vector2(
                    (4f),
                    (10f));
                data = data.WithHandOffset(p);
            }
            else { // Grapple/Pulley/Mount
                data.color = Color.Transparent;
            }
            return data;
        }
    }
}
