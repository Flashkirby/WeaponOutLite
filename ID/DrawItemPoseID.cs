using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.Content.DrawItemPose;

namespace WeaponOutLite.ID
{
    /// <summary>
    /// Static class for setting the NetIDs for poses. 
    /// </summary>
    public static class DrawItemPoseID
    {
        /// <summary>
        /// Create dictionary of net id keys to styles. Call this once on mod loading
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, IDrawItemPose> LoadPoses() {
            // Search through the mod assembly for all classes assignable to IDrawItemPose
            string drawItemPoseNameSpace = typeof(None).Namespace; // WeaponOutLite.Content.DrawItemPose
            var drawItemPoseClasses = from type in Assembly.GetExecutingAssembly().GetTypes()
                                      where type.IsClass && typeof(IDrawItemPose).IsAssignableFrom(type)
                                      && type.Namespace == drawItemPoseNameSpace
                                      select type;

            // For each class found, create an object and register it
            var drawStyle = new Dictionary<int, IDrawItemPose>();
            foreach (Type poseStyle in drawItemPoseClasses) {
                drawStyle.RegisterStyle((IDrawItemPose)Activator.CreateInstance(poseStyle));
            }
            return drawStyle;
        }

        /// <summary></summary>
        /// <param name="dict">Dictionary to register style to</param>
        /// <param name="style">the drawitem pose to add</param>
        /// <returns>The dictionary passed in, with the style registered to it. </returns>
        /// <exception cref="Exception">Throw error, usually if I forgot to change the net id of the pose. </exception>
        private static Dictionary<int, IDrawItemPose> RegisterStyle(this Dictionary<int, IDrawItemPose> dict, IDrawItemPose style) {
            try {
                dict.Add(style.GetID(), style);
            }
            catch (ArgumentException e) {
                var existing = dict[style.GetID()];
                throw new Exception("Failed to load " + style.GetType().Name + " ()" + style.GetID() + ", already set by " + existing.GetType().Name);
            }
            return dict;
        }

        // enum for forced styles
        public enum DrawItemPose
        {
            Default = DrawItemPoseID.Unassigned,
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            WaistSheathe = DrawItemPoseID.WaistSheathe,
            WaistHolster = DrawItemPoseID.WaistHolster,
            HoldInHand = DrawItemPoseID.HoldInHand,
            StanceInHand = DrawItemPoseID.StanceInHand,
            HoldReversedInHand = DrawItemPoseID.HoldReversedInHand,
            HoldInHandFront = DrawItemPoseID.HoldInHandFront,
            HoldInOffHand = DrawItemPoseID.HoldInOffHand,
            CarryInBothHands = DrawItemPoseID.CarryInBothHands,
            StanceMachineTool = DrawItemPoseID.StanceMachineTool,
            StanceMeleeReady = DrawItemPoseID.StanceMeleeReady,
            StanceMeleeTrail = DrawItemPoseID.StanceMeleeTrail,
            StanceMeleeRaised = DrawItemPoseID.StanceMeleeRaised,
            StanceMeleeUpright = DrawItemPoseID.StanceMeleeUpright,
            StanceTwoHand = DrawItemPoseID.StanceTwoHand,
            StanceTwoHandBrave = DrawItemPoseID.StanceTwoHandBrave,
            StanceTwoHandTail = DrawItemPoseID.StanceTwoHandTail,
            StanceTwoHandHighlander = DrawItemPoseID.StanceTwoHandHighlander,
            StanceTwoHandBerserk = DrawItemPoseID.StanceTwoHandBerserk,
            StanceTwoHandGuard = DrawItemPoseID.StanceTwoHandGuard,
            StanceRapierFootForward = DrawItemPoseID.StanceRapierFootForward,
            PoleOffHand = DrawItemPoseID.PoleOffHand,
            StancePoleReady = DrawItemPoseID.StancePoleReady,
            StancePoleShoulder = DrawItemPoseID.StancePoleShoulder,
            StancePoleUpright = DrawItemPoseID.StancePoleUpright,
            JoustingLance = DrawItemPoseID.JoustingLance,
            BackFlail = DrawItemPoseID.BackFlail,
            BackFlailShoulder = DrawItemPoseID.BackFlailShoulder,
            StanceFlailReady = DrawItemPoseID.StanceFlailReady,
            BackBow = DrawItemPoseID.BackBow,
            StanceBowInHand = DrawItemPoseID.StanceBowInHand,
            StanceBowHunt = DrawItemPoseID.StanceBowHunt,
            StancePistolHoldUp = DrawItemPoseID.StancePistolHoldUp,
            StancePistolCowboy = DrawItemPoseID.StancePistolCowboy,
            BackDownward = DrawItemPoseID.BackDownward,
            HoldRifleOffHandUpright = DrawItemPoseID.HoldRifleOffHandUpright,
            StanceRifleHipReady = DrawItemPoseID.StanceRifleHipReady,
            StanceRiflePointDown = DrawItemPoseID.StanceRiflePointDown,
            StanceRiflePointDown2 = DrawItemPoseID.StanceRiflePointDown2,
            StanceRifleHoldUp = DrawItemPoseID.StanceRifleHoldUp,
            StanceRifleBoltAction = DrawItemPoseID.StanceRifleBoltAction,
            StanceRiflePumpAction = DrawItemPoseID.StanceRiflePumpAction,
            BackGunShoulder = DrawItemPoseID.BackGunShoulder,
            StanceLauncherShoulder = DrawItemPoseID.StanceLauncherShoulder,
            FloatingBack = DrawItemPoseID.FloatingBack,
            FloatingBackUpright = DrawItemPoseID.FloatingBackUpright,
            FloatingInFront = DrawItemPoseID.FloatingInFront,
            FloatingOffHandAimed = DrawItemPoseID.FloatingOffHandAimed,
            FloatingOffHand = DrawItemPoseID.FloatingOffHand,
            FloatingOrbitAntiClockwiseSlow = DrawItemPoseID.FloatingOrbitAntiClockwiseSlow,
            FloatingOrbitClockwiseSlow = DrawItemPoseID.FloatingOrbitClockwiseSlow,
            FloatingOrbitAntiClockwiseFast = DrawItemPoseID.FloatingOrbitAntiClockwiseFast,
            FloatingOrbitClockwiseFast = DrawItemPoseID.FloatingOrbitClockwiseFast,
        }

        public const int Custom     = -1;
        public const int Unassigned = 0;
        public const int None       = 1;

        public const int Back               = 00010;
        public const int BackUpright        = 00020;
        public const int WaistSheathe       = 00060;
        public const int WaistHolster       = 00070;
        
        public const int HoldInHand         = 01000;
        public const int StanceInHand       = 01002;
        public const int HoldReversedInHand = 01005;
        public const int HoldInHandFront    = 01010;
        public const int HoldInOffHand      = 01020;
        public const int CarryInBothHands   = 01100;
        public const int StanceMachineTool  = 01110;

        public const int StanceMeleeReady           = 10000;
        public const int StanceMeleeTrail        = 10010;
        public const int StanceMeleeRaised          = 10020;
        public const int StanceMeleeUpright         = 10030;

        public const int StanceTwoHand              = 11000;
        public const int StanceTwoHandBrave         = 11010;
        public const int StanceTwoHandTail          = 11020;
        public const int StanceTwoHandHighlander    = 11030;
        public const int StanceTwoHandBerserk       = 11040;
        public const int StanceTwoHandGuard         = 11050;

        public const int StanceRapierFootForward    = 12000;

        public const int PoleOffHand                = 13000;
        public const int StancePoleReady            = 13010;
        public const int StancePoleShoulder         = 13020;
        public const int StancePoleUpright          = 13030;
        public const int JoustingLance              = 13101;

        public const int BackFlail                  = 14000;
        public const int BackFlailShoulder          = 14010;
        public const int StanceFlailReady           = 14020;

        public const int BackBow                    = 20000;
        public const int StanceBowInHand            = 20010;
        public const int StanceBowHunt              = 20020;
        public const int StancePistolHoldUp         = 21000;
        public const int StancePistolCowboy         = 21010;
        public const int BackDownward               = 21110;
        public const int HoldRifleOffHandUpright    = 21120;
        public const int StanceRifleHipReady        = 21130;
        public const int StanceRiflePointDown       = 21140;
        public const int StanceRiflePointDown2      = 21150;
        public const int StanceRifleHoldUp          = 21160;
        public const int StanceRifleBoltAction      = 21170;
        public const int StanceRiflePumpAction      = 21180;
        public const int BackGunShoulder            = 22000;
        public const int StanceLauncherShoulder     = 22010;

        public const int FloatingBack                   = 30000;
        public const int FloatingBackUpright            = 30001;
        public const int FloatingInFront                = 30010;
        public const int FloatingOffHandAimed           = 30020;
        public const int FloatingOffHand            = 30025;
        public const int FloatingOrbitAntiClockwiseSlow = 30030;
        public const int FloatingOrbitClockwiseSlow     = 30031;
        public const int FloatingOrbitAntiClockwiseFast = 30032;
        public const int FloatingOrbitClockwiseFast     = 30033;
    }
}
