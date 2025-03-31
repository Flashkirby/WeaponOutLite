using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Floating around the player (back to front), pointing upwards
    /// </summary>
    public class FloatingOrbitAntiClockwiseFast : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.FloatingOrbitAntiClockwiseFast;

        public virtual short DrawDepth(Player p, Item i, int timer) {
            if (-MathF.Cos(Main.GlobalTimeWrappedHourly * 2f) < 0f) {
                return DrawDepthID.Back;
            }
            return DrawDepthID.Front;
        }

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            // data = DrawHelper.RotateFaceForward(data, p, height, width);

            Vector2 orbit = new Vector2(
                -MathF.Sin(Main.GlobalTimeWrappedHourly * 2f) * p.direction,
                -MathF.Cos(Main.GlobalTimeWrappedHourly * 2f) / 3f);

            data.position += new Vector2(0, 12f - Math.Max(height, width) / 2.5f) + orbit * 24f;
            data.position += (-p.velocity * 0.5f) * p.Directions; // momentum

            data.rotation += (float)(orbit.Y * p.direction * (64f / (8f + height + width)) * -0.5f);
            return data;
        }
    }
}
