using Terraria;
using Terraria.DataStructures;

namespace WeaponOutLite.Common.GlobalDrawItemPose
{
    /// <summary>
    /// Modifies draw data position to follow the player's hand.
    /// </summary>
    public static class CommonWalkCycleOffset
    {
        /// <summary>
        /// Chain method that modifies drawData position to follow the offset of the player's hand.
        /// </summary>
        /// <param name="data">drawData</param>
        /// <param name="p">drawPlayer</param>
        /// <returns>Draw data with position offset</returns>
        public static DrawData WithHandOffset(this DrawData data, Player p) {
            // 7,8,9, 14,15,16
            if (p.bodyFrame.Y / p.bodyFrame.Height % 7 <= 2) { data.position.Y -= 2; }
            switch (p.bodyFrame.Y / p.bodyFrame.Height) {
                case 7:
                case 8:
                case 9:
                case 10:
                    data.position.X -= 2; break;
                case 14:
                case 17:
                    data.position.X += 2; break;
                case 15:
                case 16:
                    data.position.X += 4; break;
            }
            return data;
        }

        public static DrawData WithOffHandOffset(this DrawData data, Player p) {
            // 7,8,9, 14,15,16
            if (p.bodyFrame.Y / p.bodyFrame.Height % 7 <= 2) { data.position.Y -= 2; }
            switch (p.bodyFrame.Y / p.bodyFrame.Height) {
                case 7:
                case 8:
                case 9:
                case 10:
                    data.position.X += 2; break;
                case 14:
                case 17:
                    data.position.X -= 2; break;
                case 15:
                case 16:
                    data.position.X -= 4; break;
            }
            return data;
        }

        /// <summary>
        /// Chain method that modifies drawData position to follow the offset of the player's waist.
        /// </summary>
        /// <param name="data">drawData</param>
        /// <param name="p">drawPlayer</param>
        /// <returns>Draw data with position offset</returns>
        public static DrawData WithWaistOffset(this DrawData data, Player p) {
            switch (p.bodyFrame.Y / p.bodyFrame.Height) {
                case 7:
                case 8:
                case 9:
                case 14:
                case 15:
                case 16:
                    data.position.Y -= 2; break;
            }
            return data;
        }

        /// <summary>
        /// Provide a radian angle for walking frames, that imitates the motion of swinging the weapon,
        /// raising when held further forward and lowering when pulled back.
        /// <br/>Remember to apply <code>p.direction * p.gravDir</code> to the final rotation.
        /// </summary>
        /// <param name="FrameNum">The frame number of the player bodyFrame.
        ///                   <br/>p.bodyFrame.Y / p.bodyFrame.Height</param>
        /// <returns>Rotation in Radians from -1f to 1f</returns>
        public static float GetRotationDirect(int FrameNum) {
            //for body frames 6 - 19
            //furthest back 9
            //furthest forward 16
            float rot = 0;
            switch (FrameNum) {
                case 6: rot = -0.6f; break;
                case 7: rot = -0.8f; break;
                case 8: rot = -1; break;
                case 9: rot = -1; break;
                case 10: rot = -0.8f; break;
                case 11: rot = -0.4f; break;
                case 12: rot = 0; break;
                case 13: rot = 0.6f; break;
                case 14: rot = 0.8f; break;
                case 15: rot = 1; break;
                case 16: rot = 1; break;
                case 17: rot = 0.8f; break;
                case 18: rot = 0.4f; break;
                case 19: rot = 0; break;
            }

            return rot;
        }

        /// <summary>
        /// Provide a radian angle for walking frames, that imitates the motion of swinging the weapon,
        /// raising when held further forward and lowering when pulled back.
        /// <br/>Remember to apply <code>p.direction * p.gravDir</code> to the final rotation.
        /// </summary>
        /// <param name="p">drawPlayer to read bodyFrame from</param>
        /// <returns>Rotation in Radians from -1f to 1f</returns>
        public static float GetRotationDirect(Player p) {
            return GetRotationDirect(p.bodyFrame.Y / p.bodyFrame.Height);
        }

        /// <summary>
        /// Provide a radian angle for walking frames corrected for player direction and gravity, 
        /// that imitates the motion of swinging the weapon,
        /// raising when held further forward and lowering when pulled back.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Rotation in Radians from -1f to 1f</returns>
        public static float GetRotation(Player p) {
            return GetRotationDirect(p.bodyFrame.Y / p.bodyFrame.Height) * p.direction * p.gravDir;
        }
    }
}
