using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Bows are slung on the back facing down. </summary>
    public class OnBackBow: IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.BackBow;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.5f, 0.5f, p).ApplyFlip(p);

            // Offset for large items to place them slightly further up and rotate to face the ground more
            var smallItemOffset = Math.Clamp(height, 20, 80);
            var smallItemOffsetR = 80 / smallItemOffset * 0.25f;

            data.position += new Vector2(-4, 0);
            data.rotation -= (float)(Math.PI * 1.1f + smallItemOffsetR);
            return data.WithWaistOffset(p);
        }
    }
}
