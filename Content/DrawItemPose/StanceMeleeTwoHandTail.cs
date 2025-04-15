using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// The tail stance, which has the sword ready in a way that can touch the floor
    /// </summary>
    public class StanceMeleeTwoHandTail : OnBack
    {
        public override int GetID() => DrawItemPoseID.StanceTwoHandTail;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(45, timer) > 0.2f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            float t = DrawHelper.AnimLinearNormal(45, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            if (bodyFrame == 0) {
                // standing
                if(p.legFrame.Y == 0) p.legFrame.Y = 9 * p.legFrame.Height;

                // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
                p.shield = -1; // Hide shield
                p.cShield = -1; // Hide cosmetic shield

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.125f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.15f - sheatheRotation) * p.direction);
            }
            else {
                if(p.velocity.Y == 0) {
                    // moving
                    Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (0.1f - sheatheRotation) * p.direction);
                }
                else {
                    // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
                    p.shield = -1; // Hide shield
                    p.cShield = -1; // Hide cosmetic shield

                    if (bodyFrame == 5) {
                        // falling
                        Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                        p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.1f - sheatheRotation * 1.5f) * p.direction);

                        Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                        p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.175f - sheatheRotation) * p.direction);
                    }
                    else {
                        //rising
                        Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                        p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.25f - sheatheRotation * 1.5f) * p.direction);

                        Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                        p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.25f - sheatheRotation) * p.direction);
                    }
                }
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            float t = DrawHelper.AnimOverEaseNormal(45, timer);
            data = data.SetOrigin(0.1f, 0.9f, p).RotateFaceForward(p, height, width);
            data.rotation -= MathHelper.PiOver4;


            if (bodyFrame == 0) {
                data.position += new Vector2(6, 10);
                data.rotation += (float)(Math.PI * 1.15f);
            }
            else if (bodyFrame >= 5 || p.IsMountPoseActive()) {
                if (p.velocity.Y == 0) {
                    float length = new Vector2(width, height).Length();

                    // Get a point somewhere around the end of the weapon
                    Vector2 weaponPoint = p.MountedCenter + new Vector2(-p.width / 2 - length * 0.8f, p.height / 2) * p.Directions;
                    //var d = Dust.NewDustDirect(weaponPoint + new Vector2(-2, -4), 0, 0, DustID.MinecartSpark, p.velocity.X, p.velocity.Y);
                    //d.noLight = true;


                    // Generate dust effects at the point. Generate more if based on velocity and weapon size
                    weaponPoint += new Vector2(8, 8) * p.Directions;
                    var tilePos = weaponPoint.ToTileCoordinates16();
                    var weaponDropHeight = 0f;
                    Tile tile = new Tile();

                    // With weapon physics, have the weapon drop if it's floating
                    if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                        for (int i = 0; i < length / 32; i++) {
                            int nextX = tilePos.X + i * p.direction;
                            int nextY = tilePos.Y + i * (int)p.gravDir;
                            try {
                                tile = Main.tile[nextX, nextY];
                                weaponDropHeight = i;
                                if (Collision.SolidTiles(nextX, nextX, nextY, nextY)) {
                                    tilePos = new Point16(nextX, nextY);
                                    break;
                                }
                            }
                            catch { } // in case this gets run in gameMenu
                        }
                        //var d = Dust.NewDustDirect(tilePos.ToWorldCoordinates() + new Vector2(-2, -4), 0, 0, DustID.TreasureSparkle, p.velocity.X, p.velocity.Y);
                        if (weaponDropHeight > 0) {
                            length /= 1 + weaponDropHeight;
                        }
                    }

                    switch (bodyFrame) {
                        case 7:
                        case 8:
                        case 9:
                        case 14:
                        case 15:
                        case 16:
                            length = (length + 8) * 0.75f; break;
                    }
                    float lengthScale = Math.Max((length - 20f) / 5f, 1);

                    if (timer > 30 && Main.rand.Next(64 * 2 / (int)Math.Max(lengthScale * Math.Abs(p.velocity.X), 4)) == 0) {
                        if (tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]) {
                            WorldGen.KillTile_MakeTileDust(tilePos.X, tilePos.Y, tile);
                        }
                    }

                    // How much to pull up the weapon by (bigger weapons need rotation, to a point)
                    float lengthOffset = 0.35f - (0.35f / lengthScale);

                    data.rotation += (float)(Math.PI * (0.88f + lengthOffset));

                    // 3.64 -> 2.7, so ranges from 0.1 -> -0.8
                    float handleOffset = data.rotation - 3.6f;
                    data.position += new Vector2(-4 + handleOffset * 6, 10 + handleOffset * 4);
                }
                else {
                    if (bodyFrame == 5) {
                        data.position += new Vector2(6, 12);

                        data.rotation += (float)(Math.PI * 1.2f);
                    }
                    else {
                        data.position += new Vector2(8, 8);

                        data.rotation += (float)(Math.PI * 1.05f);
                    }
                }
            }

            // Sheathing OnBack
            if (t > 0f)
            {
                data.position += new Vector2(26f, -18f) * t;
                data.position += new Vector2(0, (width + height) * -0.5f * t);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
