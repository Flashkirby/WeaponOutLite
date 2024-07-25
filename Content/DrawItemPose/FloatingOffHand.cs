using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Floating and pointing forwards, with back hand held out.
    /// Same as FloatingPointForward but because it's a book, no additional rotation and slightly lower
    /// </summary>
    public class FloatingOffHand : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.FloatingOffHand;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer)
        {
            // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            if (bodyFrame == 0) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.25f * p.direction);
            }
            else {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.175f * p.direction);
            }
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            Vector2 spin = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.5f, -MathF.Cos(Main.GlobalTimeWrappedHourly * 2f));
            data.position += new Vector2(
                6f + Math.Max(height, width) / 3f,
                0f - (height + width) / 8f) + spin * 2f;
            data.position += (-p.velocity * 0.5f) * p.Directions; //momentum

            data.rotation += (float)(spin.Y * 0.02f);

            return data;
        }
    }
}
