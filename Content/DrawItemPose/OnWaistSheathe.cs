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
    public class OnWaistSheathe : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.WaistSheathe;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            // Face forward
            data = data.SetOrigin(0.5f - 0.3f * (width / height), 0.8f, p).ApplyFlip(p).RotateFaceForward(p, height, width);

            float minSize = 64;
            var smallItemOffset = Math.Max(new Vector2(width, height).Length(), minSize) / minSize; // 1 small increasing with size relative to minsize
            var smallItemOffsetR = 0.1f / smallItemOffset;

            data.rotation += (float)(Math.PI * (1f - smallItemOffsetR)); // rotate up to 180 deg

            data.position += new Vector2(
                4 + (8 / smallItemOffset),
                4 + (4 / smallItemOffset));

            // Motion determined by fall speed, or forward running speed
            if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                var motion = p.velocity.Y == 0 ? (p.velocity.X * p.direction) : (p.velocity.Y * p.gravDir);
                var motionR = -Math.Clamp(motion * 0.02f, -0.39f, 0.39f);
                data.rotation -= (float)(motionR); // rotate up to 180 deg
            }

            return data.WithWaistOffset(p);
        }
    }
}
