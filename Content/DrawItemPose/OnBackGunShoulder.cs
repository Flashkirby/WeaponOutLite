using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> High on the back on the shoulders, for launchers etc. </summary>
    public class OnBackGunShoulder : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.BackGunShoulder;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.OffHand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {

            Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
            p.SetCompositeArmBack(enabled: true, backArm, MathHelper.Pi * -0.365f * p.direction);

            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            // 4 pixel height buffer offset, on the assumption that weapons using this are gun-like anyway
            // Pulling height inwards by 4 makes it focus more forwards (makes it wider in h/w ratio)
            data = DrawHelper.RotateFaceForward(data, p, height - 4, width).SetOrigin(0.25f, 0.75f, p);
            data.position += new Vector2(-12 + width / 4, 2 - height / 8).Round2(p.direction, 1);
            return data.WithWaistOffset(p);
        }
    }
}
