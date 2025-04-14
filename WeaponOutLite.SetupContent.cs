using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent;
using WeaponOutLite.Compatibility;

namespace WeaponOutLite
{
    /// <summary>
    /// Mod Calls examples in practice. This is for items that MUST use a custom style as defined by the modder.
    /// For more flexible styles
    /// </summary>
    partial class WeaponOutLite : Mod
    {
        public override void PostSetupContent() {
          
            CalamityMod.PostSetupContent();
            MetroidMod.PostSetupContent();

            // Get the WeaponOutLite mod
            Mod weaponOutLite = ModLoader.GetMod("WeaponOutLite");

            // Register these items to prioritise a specific hold style
            if (!(bool)weaponOutLite.Call("RegisterItem", new int[] {
                ItemID.BladedGlove,
                ItemID.ChainKnife,
                ItemID.Ruler,
            })) { throw new ArgumentException("RegisterItem ModCall Failed"); }
            if (!(bool)weaponOutLite.Call("RegisterSpear", new int[] {
                ItemID.JoustingLance,
                ItemID.ShadowJoustingLance,
                ItemID.HallowJoustingLance,
                ItemID.MonkStaffT1, // Sleepy Octopod
                ItemID.MonkStaffT3, // Sky Fury
            })) { throw new ArgumentException("RegisterSpear ModCall Failed"); }

            if (!(bool)weaponOutLite.Call("RegisterFlail", new int[] {
                ItemID.Flairon,
                ItemID.ChainGuillotines,
            })) { throw new ArgumentException("RegisterFlail ModCall Failed"); }

            if (!(bool)weaponOutLite.Call("RegisterSmallMelee", new int[] {
                ItemID.Terragrim,
                ItemID.Arkhalis,
            })) { throw new ArgumentException("RegisterSmallMelee ModCall Failed"); }

            if (!(bool)weaponOutLite.Call("RegisterLargeMelee", new int[] {
                ItemID.BrokenHeroSword,
                ItemID.Zenith,
            })) { throw new ArgumentException("RegisterLargeMelee ModCall Failed"); }

            if (!(bool)weaponOutLite.Call("RegisterPistol", new int[] {
                ItemID.ConfettiGun,
                ItemID.Revolver,
                ItemID.VenusMagnum,
            })) { throw new ArgumentException("RegisterPistol ModCall Failed"); }

            if (!(bool)weaponOutLite.Call("RegisterGun", ItemID.CoinGun )) { throw new ArgumentException("RegisterGun ModCall Failed"); }
            if (!(bool)weaponOutLite.Call("RegisterWhips", ItemID.SolarEruption)) { throw new ArgumentException("RegisterWhips ModCall Failed"); }

            if (!(bool)weaponOutLite.Call("RegisterItemHoldPose", ItemID.NebulaBlaze, "None") || 
                !(bool)weaponOutLite.Call("RegisterItemHoldPose", ItemID.BouncingShield, "None")
            ) { throw new ArgumentException("RegisterItemHoldPose ModCall Failed"); }

            // Register these items to use Custom Holdstyles. The other functions will not call for an item unless it is specified here first.
            if (!(bool)weaponOutLite.Call("RegisterCustomItemStyle", new int[] {
                ItemID.TheAxe,
                ItemID.PortalGun,
                ItemID.ChargedBlasterCannon, ItemID.AleThrowingGlove,
            })) { throw new ArgumentException("RegisterCustomItemStyle ModCall Failed"); }

            // Draw data, called after the item has been centred on the player in the draw layer
            Func<Player, Item, DrawData, float, float, int, int, DrawData> weaponOutLiteCustomDrawData = (Player p, Item i, DrawData data, float h, float w, int bodyFrame, int timer) => {
                bool walkDisplacement = false;

                //if (i.type == ItemID.NebulaBlaze || i.type == ItemID.BouncingShield) {
                //    data.color = Color.Transparent;
                //}

                if (i.type == ItemID.TheAxe) {
                    // Rotate clockwise 180
                    data.rotation += (float)Math.PI;
                    // move right 4 and 10 down
                    data.position += new Vector2(4, 10);

                    // offset Y when body frame is raised
                    if (bodyFrame >=7 && bodyFrame % 7 <= 2) { data.position.Y -= 2; }
                }

                if (i.type == ItemID.PortalGun) {
                    var projAsset = TextureAssets.Projectile[ProjectileID.PortalGun];
                    if (Main.IsGraphicsDeviceAvailable && TextureAssets.Projectile[ProjectileID.PortalGun].IsLoaded) {
                        data.texture = projAsset.Value;
                        data.sourceRect = data.texture.Bounds;
                        data.origin = new Vector2(data.texture.Width * 0.5f, data.texture.Height * 0.875f);
                    }
                    else {
                        data.texture = null;
                    }
                    data.rotation += (float)Math.PI / 2;
                    //data.rotation = (float)Main.GlobalTimeWrappedHourly * 4f;



                    if (bodyFrame == 0) { // Standing
                        // Recentre to 
                        data.position += new Vector2(-6, 9);
                    }
                    else if (bodyFrame > 5) { // Walk Cycle
                        data.position += new Vector2(-2,7);
                        walkDisplacement = true;
                    }
                    else if (bodyFrame == 5 || bodyFrame == 1) { // Jump
                        data.rotation += (float)Math.PI * -0.5f;
                        data.position += new Vector2(-11, -4);
                    }
                    else if (bodyFrame == 2) { // Grappling Up
                        data.rotation += (float)Math.PI * -0.25f;
                        data.position += new Vector2(0, 2);
                    }
                    else if (bodyFrame == 3) { // Grappling Mid
                        data.position += new Vector2(2, 3);
                    }
                    else if (bodyFrame == 4) { // Grappling Down
                        data.rotation += (float)Math.PI * 0.25f;
                        data.position += new Vector2(0, 3);
                    }
                }

                if (i.type == ItemID.ChargedBlasterCannon || 
                    i.type == ItemID.AleThrowingGlove) {

                    // Ale glove is rotated at a 45 degree angle
                    if (i.type == ItemID.AleThrowingGlove) data.rotation += (float)Math.PI * 0.25f;

                    if (bodyFrame == 0) {
                        data.rotation += (float)Math.PI * 0.5f;
                        data.position += new Vector2(-7, 6 + h / 2);
                    }
                    else if (bodyFrame > 5) {
                        data.position += new Vector2(-10 + w / 2, 7);
                        walkDisplacement = true;
                    }
                    else if (bodyFrame == 5 || bodyFrame == 1) {
                        data.rotation += (float)Math.PI * -0.5f;
                        data.position += new Vector2(-9, -2 - h / 2);
                    }
                    else if (bodyFrame == 2) {
                        data.rotation += (float)Math.PI * -0.25f;
                        data.position += new Vector2(5, -7);
                    }
                    else if (bodyFrame == 3) {
                        data.position += new Vector2(9, 5);
                    }
                    else if (bodyFrame == 4) {
                        data.rotation += (float)Math.PI * 0.25f;
                        data.position += new Vector2(5, 11);
                    }
                }
                if (walkDisplacement) {
                        if (bodyFrame % 7 <= 2) { data.position.Y -= 2; }
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
                }
                return data;
            };

            // Body frame, called in PostUpdate
            Func<Player, Item, int, int, int> weaponOutLiteCustomIdleBodyFrame = (Player p, Item i, int bodyFrame, int timer) => {
                if (i.type == ItemID.TheAxe) {
                    p.shield = -1; // Hide shield
                    p.cShield = -1; // Hide cosmetic shield

                    Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.3f * p.direction);
                    
                    Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.1f * p.direction);
                }
                return bodyFrame;
            };

            // Draw depth, used to define the layer the item should be drawn on relative to the player
            // 1 = In front of player
            // 0 = In front of player, behind front arm
            // -1 = Behind player, in front of back arm
            // -2 = Behind player
            Func<Player, Item, short, int, short> weaponOutLiteCustomDrawDepth = (Player player, Item i, short drawDepth, int timer) => {
                if (i.type == ItemID.TheAxe) return 0; // Draw the axe in hand
                if (i.type == ItemID.PortalGun) return 1; // Draw over hand
                if (i.type == ItemID.ChargedBlasterCannon || i.type == ItemID.AleThrowingGlove) return 1; // Draw over hand
                return drawDepth;
            };

            // Add the custom style calls. This will call on all items where the draw style has been set as a Custom Holdstyle
            if (!(bool)weaponOutLite.Call("RegisterCustomUpdateIdleBodyFrame", weaponOutLiteCustomIdleBodyFrame)) { throw new ArgumentException("RegisterCustomUpdateIdleBodyFrame ModCall Failed"); }
            if (!(bool)weaponOutLite.Call("RegisterCustomDrawDepth", weaponOutLiteCustomDrawDepth)) { throw new ArgumentException("RegisterCustomDrawDepth ModCall Failed"); }
            if (!(bool)weaponOutLite.Call("RegisterCustomPreDrawData", weaponOutLiteCustomDrawData)) { throw new ArgumentException("RegisterCustomPreDrawData ModCall Failed"); }
        }
    }
}