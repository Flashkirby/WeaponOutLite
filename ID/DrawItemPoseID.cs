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
    /// This is where all base poses need to be set.
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

        /// <summary>
        /// Internal/Network mapping for draw item poses. See IDrawItemPose GetID()
        /// </summary>
        public enum DrawItemPose
        {
            Unassigned = DrawItemPoseID.Unassigned,
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            WaistSheathe = DrawItemPoseID.WaistSheathe,
            WaistHolster = DrawItemPoseID.WaistHolster,
            HoldInHand = DrawItemPoseID.HoldInHand,
            StanceInHand = DrawItemPoseID.StanceInHand,
            HoldReversedInHand = DrawItemPoseID.HoldReversedInHand,
            HoldInHandFront = DrawItemPoseID.HoldInHandFront,
            HoldSpellTome = DrawItemPoseID.HoldSpellTome,
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
            StancePoleTwoHand = DrawItemPoseID.StancePoleTwoHand,
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
            FloatingBackAimed = DrawItemPoseID.FloatingBackAimed,
            FloatingInFront = DrawItemPoseID.FloatingInFront,
            FloatingOffHandAimed = DrawItemPoseID.FloatingOffHandAimed,
            FloatingOffHand = DrawItemPoseID.FloatingOffHand,
            FloatingOrbitAntiClockwiseSlow = DrawItemPoseID.FloatingOrbitAntiClockwiseSlow,
            FloatingOrbitClockwiseSlow = DrawItemPoseID.FloatingOrbitClockwiseSlow,
            FloatingOrbitAntiClockwiseFast = DrawItemPoseID.FloatingOrbitAntiClockwiseFast,
            FloatingOrbitClockwiseFast = DrawItemPoseID.FloatingOrbitClockwiseFast,
        }


        /// <summary>
        /// A more human readable version of DrawItemPose, for the default config. This is what the selected group allows access to.
        /// This is basically everything except for Unassigned, and any other weird specific hold poses reserved for client override, etc.
        /// </summary>
        public enum LabelledItemPose
        {
            /* Back & Waist */
            //Unassigned = DrawItemPoseID.Unassigned,
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            BackDownward = DrawItemPoseID.BackDownward,
            BackBow = DrawItemPoseID.BackBow,
            Waist = DrawItemPoseID.WaistSheathe,
            WaistHolster = DrawItemPoseID.WaistHolster,
            /* Front */
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            HoldReverse = DrawItemPoseID.HoldReversedInHand,
            HoldTome = DrawItemPoseID.HoldSpellTome,
            /* Offhand */
            OffHand = DrawItemPoseID.HoldInOffHand,
            OffHandFlailShoulder = DrawItemPoseID.BackFlailShoulder,
            OffHandPole = DrawItemPoseID.PoleOffHand,
            OffHandShoulder = DrawItemPoseID.BackGunShoulder,
            OffHandUpright = DrawItemPoseID.HoldRifleOffHandUpright,
            /* Two hand */
            TwoHandCarry = DrawItemPoseID.CarryInBothHands,
            TwoHandJoustingLance = DrawItemPoseID.JoustingLance,

            /* Levitate */
            FloatingBack = DrawItemPoseID.FloatingBack,
            FloatingBackUpright = DrawItemPoseID.FloatingBackUpright,
            FloatingBackAimed = DrawItemPoseID.FloatingBackAimed,
            FloatingFront = DrawItemPoseID.FloatingInFront,
            FloatingOffHand = DrawItemPoseID.FloatingOffHand,
            FloatingAimed = DrawItemPoseID.FloatingOffHandAimed,
            OrbitClockwiseFast = DrawItemPoseID.FloatingOrbitClockwiseFast,
            OrbitAntiClockFast = DrawItemPoseID.FloatingOrbitAntiClockwiseFast,
            OrbitClockwiseSlow = DrawItemPoseID.FloatingOrbitClockwiseSlow,
            OrbitAntiClockSlow = DrawItemPoseID.FloatingOrbitAntiClockwiseSlow,

            // Combat Front  */
            CombatHold = DrawItemPoseID.StanceInHand,
            CombatReady = DrawItemPoseID.StanceMeleeReady,
            CombatUpright = DrawItemPoseID.StanceMeleeUpright,
            CombatRaised = DrawItemPoseID.StanceMeleeRaised,
            CombatTrail = DrawItemPoseID.StanceMeleeTrail,
            CombatRapier = DrawItemPoseID.StanceRapierFootForward,
            CombatPoleUpright = DrawItemPoseID.StancePoleUpright,
            CombatPoleReady = DrawItemPoseID.StancePoleReady,
            CombatFlailReady = DrawItemPoseID.StanceFlailReady,
            CombatBowHold = DrawItemPoseID.StanceBowInHand,
            CombatPistol = DrawItemPoseID.StancePistolCowboy,
            /* Combat Offhand  */
            CombatOffHandPoleShoulder = DrawItemPoseID.StancePoleShoulder,
            CombatOffHandPistol = DrawItemPoseID.StancePistolHoldUp,
            CombatOffHandShoulder = DrawItemPoseID.StanceLauncherShoulder,
            /* Combat Twohand */
            CombatTwoHand = DrawItemPoseID.StanceTwoHand,
            CombatTwoHandBrave = DrawItemPoseID.StanceTwoHandBrave,
            CombatTwoHandTail = DrawItemPoseID.StanceTwoHandTail,
            CombatTwoHandHighlander = DrawItemPoseID.StanceTwoHandHighlander,
            CombatTwoHandBerserk = DrawItemPoseID.StanceTwoHandBerserk,
            CombatTwoHandGuard = DrawItemPoseID.StanceTwoHandGuard,
            CombatTwoHandPoleTwoHand = DrawItemPoseID.StancePoleTwoHand,
            CombatTwoHandPowerTool = DrawItemPoseID.StanceMachineTool,
            CombatTwoHandBow = DrawItemPoseID.StanceBowHunt,
            CombatTwoHandRifleHip = DrawItemPoseID.StanceRifleHipReady,
            CombatTwoHandRifleUp = DrawItemPoseID.StanceRifleHoldUp,
            CombatTwoHandRifleDown = DrawItemPoseID.StanceRiflePointDown,
            CombatTwoHandRifleDown2 = DrawItemPoseID.StanceRiflePointDown2,
            CombatTwoHandBoltAction = DrawItemPoseID.StanceRifleBoltAction,
            CombatTwoHandPumpAction = DrawItemPoseID.StanceRiflePumpAction,
        }

        // Default values
        public const int Custom             = -1;
        public const int Unassigned         = 0;
        public const int None               = 1;

        // Back & Waist
        public const int Back               = 00010;
        public const int BackDownward       = 00020;
        public const int BackUpright        = 00030;
        public const int BackFlail          = 00040;
        public const int BackBow            = 00050;
        public const int WaistSheathe   = 00500;
        public const int WaistHolster   = 00510;
        // Front
        public const int HoldInHand         = 10000;
        public const int HoldInHandFront    = 10010;
        public const int HoldReversedInHand = 10020;
        public const int HoldSpellTome      = 10030;
        // Offhand
        public const int HoldInOffHand              = 20000;
        public const int BackFlailShoulder          = 20010;
        public const int PoleOffHand                = 20020;
        public const int BackGunShoulder            = 20030;
        public const int HoldRifleOffHandUpright    = 20040;
        // Two hand
        public const int CarryInBothHands       = 30000;
        public const int JoustingLance          = 30050;

        // Levitate
        public const int FloatingBack                   = 40000;
        public const int FloatingBackUpright            = 40010;
        public const int FloatingBackAimed              = 40020;
        public const int FloatingInFront                = 40100;
        public const int FloatingOffHand                = 40110;
        public const int FloatingOffHandAimed           = 40120;
        public const int FloatingOrbitClockwiseFast         = 41000;
        public const int FloatingOrbitAntiClockwiseFast     = 41001;
        public const int FloatingOrbitClockwiseSlow         = 41010;
        public const int FloatingOrbitAntiClockwiseSlow     = 41011;

        // Combat Front 
        public const int StanceInHand               = 50000;
        public const int StanceMeleeReady           = 50050;
        public const int StanceMeleeUpright         = 50060;
        public const int StanceMeleeRaised          = 50070;
        public const int StanceMeleeTrail           = 50080;
        public const int StanceRapierFootForward    = 50100;
        public const int StancePoleReady            = 50200;
        public const int StancePoleUpright          = 50210;
        public const int StanceFlailReady           = 50220;
        public const int StanceBowInHand            = 50300;
        public const int StancePistolCowboy         = 50400;
        // Combat Offhand 
        public const int StancePoleShoulder         = 60200;
        public const int StancePistolHoldUp         = 60400;
        public const int StanceLauncherShoulder     = 60450;
        // Combat Twohand
        public const int StanceTwoHand              = 70000;
        public const int StanceTwoHandBrave         = 70010;
        public const int StanceTwoHandTail          = 70020;
        public const int StanceTwoHandHighlander    = 70030;
        public const int StanceTwoHandBerserk       = 70040;
        public const int StanceTwoHandGuard         = 70050;
        public const int StancePoleTwoHand          = 70200;
        public const int StanceMachineTool          = 70250;
        public const int StanceBowHunt              = 70300;
        public const int StanceRifleHipReady        = 70400;
        public const int StanceRifleHoldUp          = 70410;
        public const int StanceRiflePointDown       = 70420;
        public const int StanceRiflePointDown2      = 70430;
        public const int StanceRifleBoltAction      = 70440;
        public const int StanceRiflePumpAction      = 70450;

    }
}
