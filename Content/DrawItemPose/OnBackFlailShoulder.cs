using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Over the shoulder. </summary>
    public class OnBackFlailShoulder : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.BackFlailShoulder;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.OffHand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {

            Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
            p.SetCompositeArmBack(enabled: true, backArm, MathHelper.Pi * -0.25f * p.direction);

            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.6f, 1f, p).RotateFaceForward(p, height, width);
            data.position += new Vector2(16, 6);
            data.rotation += (float)(Math.PI * -0.6f);
            return data.WithWaistOffset(p);
        }
    }
}
