using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// A placeholder style. This should always be transformed into something else, and should not be rendered on its own.
    /// </summary>
    public class Unassigned: IDrawItemPose
    {
        public int GetID() => DrawItemPoseID.Unassigned;

        public short DrawDepth(Player p, Item i, int timer) => 0;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            // Should never see this in normal conditions - useful for debugging though
            float x = (Main.mouseX - Main.screenWidth / 2);
            float y = (Main.mouseY - Main.screenHeight / 2);
            data.position += new Vector2(
                x * p.direction,
                y * p.gravDir);

            if (Main.SmartCursorIsUsed) {
                data.rotation = (float)(Main.GlobalTimeWrappedHourly * 2f) * p.direction * p.gravDir;
            }
            return data;
        }
    }
}
