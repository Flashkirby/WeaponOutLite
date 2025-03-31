using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Floating in front of the player with hand out (like nebula arcanum)
    /// </summary>
    public class FloatingInFront : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.FloatingInFront;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            bodyFrame = 3;

            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            Vector2 spin = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 2f), -MathF.Cos(Main.GlobalTimeWrappedHourly * 2f));

            data.position += new Vector2(24f + width / 4f, 4f) + spin * 2f;
            data.position += (-p.velocity * 0.25f) * p.Directions; //momentum

            data.rotation += (float)(spin.X * -0.02f); // rotate 180 deg
            return data;
        }
    }
}
