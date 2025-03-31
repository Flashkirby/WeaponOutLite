using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// General purpose, but in the offhand
    /// </summary>
    public class HoldInOffHand : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.HoldInOffHand;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.OffHand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer)
        {
            if (bodyFrame == 0) {
                /** For this particular use case, we won't be hiding the shield as this is the default pose for vanity items
                 *  ie. if the player wants to hide the shield while editing vanity they can do it themselves
                 **/
                //p.shield = -1; // Hide shield
                //p.cShield = -1; // Hide cosmetic shield

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.25f * p.direction);
            }
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.05f, 0.85f, p);

            if (bodyFrame == 0 || p.IsMountPoseActive()) { // Standing
                data.position += new Vector2(16f, 10f);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation += (float)(Math.PI * -0.5f);
                data.position += new Vector2(
                    (12),
                    (-6f));
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.position += new Vector2(
                    (8f),
                    (6f));
                data = data.WithOffHandOffset(p);
            }
            else { // Grapple/Pulley/Mount
                data.color = Color.Transparent;
            }
            return data;
        }
    }
}
