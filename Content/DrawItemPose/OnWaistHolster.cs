using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Item hang from waist, but facing forwards like a holster, rather than backwards like a sheathe </summary>
    public class OnWaistHolster : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.WaistHolster;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            // Face forward
            data = DrawHelper.RotateFaceForward(data, p, height, width);

            // Move position to waist
            data.position += new Vector2(
                (-2 + (width) / 8f),
                (28 - (height + width) / 4.5f)).Round2(p.direction, 1);

            // Motion determined by fall speed, or forward running speed
            if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                var motion = p.velocity.Y == 0 ? (p.velocity.X * p.direction) : (-p.velocity.Y * p.gravDir);
                var motionR = -Math.Clamp(motion * 0.02f, -0.39f, 0.39f);
                data.rotation -= (float)(motionR); // rotate up to 180 deg
            }

            return data.WithWaistOffset(p);
        }
    }
}
