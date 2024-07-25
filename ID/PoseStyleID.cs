namespace WeaponOutLite.ID
{
    public static class PoseStyleID
    {
        public enum PoseGroup
        {
            Unassigned = 0,
            Item = 10,
            Potion = 20,
            LargeItem = 50,
            VanityItem = 60,
            PowerTool = 70,
            Thrown = 90,
            ThrownThin = 95,
            Whips = 100,
            SmallMelee = 120,
            LargeMelee = 140,
            Spear = 150,
            Flail = 155,
            Rapier = 160,
            Yoyo = 165,
            Bow = 200,
            Pistol = 220,
            Gun = 240,
            GunManual = 250,
            Shotgun = 255,
            Repeater = 260,
            Launcher = 280,
            Staff = 300,
            MagicBook = 310,
            MagicItem = 320,
            GiantItem = 900,
            GiantWeapon = 920,
            GiantBow = 940,
            GiantGun = 950,
            GiantMagic = 960,
        }

        public enum ItemPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            HoldTome = DrawItemPoseID.HoldSpellTome,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Carry = DrawItemPoseID.CarryInBothHands,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
        }

        public enum LargeItemPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Carry = DrawItemPoseID.CarryInBothHands,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Tail = DrawItemPoseID.StanceTwoHandTail,
        }

        public enum VanityItemPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            HoldTome = DrawItemPoseID.HoldSpellTome,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Carry = DrawItemPoseID.CarryInBothHands,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
        }

        public enum PotionPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            HoldTome = DrawItemPoseID.HoldSpellTome,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Carry = DrawItemPoseID.CarryInBothHands,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
        }

        public enum PowerToolPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            HoldOffHand = DrawItemPoseID.PoleOffHand,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Power_Tool = DrawItemPoseID.StanceMachineTool,
            Combat_Shoulder = DrawItemPoseID.StanceLauncherShoulder,
            Combat_Rifle_Hip = DrawItemPoseID.StanceRifleHipReady,
            Combat_Rifle_Up = DrawItemPoseID.StanceRifleHoldUp,
            Combat_Rifle_Down = DrawItemPoseID.StanceRiflePointDown,
            Combat_Rifle_OffHand = DrawItemPoseID.StanceRifleHoldUp,
            Combat_Pistol_OffHand = DrawItemPoseID.StancePistolHoldUp,
            Combat_Pistol = DrawItemPoseID.StancePistolCowboy,
        }

        public enum ThrownPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldReverse = DrawItemPoseID.HoldReversedInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
        }

        public enum WhipPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldReverse = DrawItemPoseID.HoldReversedInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Rapier = DrawItemPoseID.StanceRapierFootForward,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Berserk = DrawItemPoseID.StanceTwoHandBerserk,
            Combat_Two_Hand_Highlander = DrawItemPoseID.StanceTwoHandHighlander,
            Combat_Two_Hand_Brave = DrawItemPoseID.StanceTwoHandBrave,
            Combat_Two_Hand_Tail = DrawItemPoseID.StanceTwoHandTail,
        }

        public enum SmallMeleePoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldReverse = DrawItemPoseID.HoldReversedInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Rapier = DrawItemPoseID.StanceRapierFootForward,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Berserk = DrawItemPoseID.StanceTwoHandBerserk,
            Combat_Two_Hand_Highlander = DrawItemPoseID.StanceTwoHandHighlander,
            Combat_Two_Hand_Brave = DrawItemPoseID.StanceTwoHandBrave,
            Combat_Two_Hand_Tail = DrawItemPoseID.StanceTwoHandTail,
        }

        public enum LargeMeleePoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Berserk = DrawItemPoseID.StanceTwoHandBerserk,
            Combat_Two_Hand_Highlander = DrawItemPoseID.StanceTwoHandHighlander,
            Combat_Two_Hand_Brave = DrawItemPoseID.StanceTwoHandBrave,
            Combat_Two_Hand_Tail = DrawItemPoseID.StanceTwoHandTail,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
        }

        public enum YoyoPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Fencer = DrawItemPoseID.StanceRapierFootForward,
        }

        public enum SpearPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            PoleOffHand = DrawItemPoseID.PoleOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Pole_Ready = DrawItemPoseID.StancePoleReady,
            Combat_Pole_Upright = DrawItemPoseID.StancePoleUpright,
            Combat_Pole_Shoulder = DrawItemPoseID.StancePoleShoulder,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Fencer = DrawItemPoseID.StanceRapierFootForward,
        }

        public enum FlailPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackFlail,
            Waist = DrawItemPoseID.WaistSheathe,
            OffHand_Shoulder = DrawItemPoseID.BackFlailShoulder,
            Combat_Ready = DrawItemPoseID.StanceFlailReady,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
        }

        public enum RapierPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldReverse = DrawItemPoseID.HoldReversedInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Fencer = DrawItemPoseID.StanceRapierFootForward,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Berserk = DrawItemPoseID.StanceTwoHandBerserk,
            Combat_Two_Hand_Highlander = DrawItemPoseID.StanceTwoHandHighlander,
            Combat_Two_Hand_Brave = DrawItemPoseID.StanceTwoHandBrave,
            Combat_Two_Hand_Tail = DrawItemPoseID.StanceTwoHandTail,
        }

        public enum BowPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackBow,
            BackAlt = DrawItemPoseID.BackDownward,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceBowInHand,
            Combat_Hunter = DrawItemPoseID.StanceBowHunt,
        }

        public enum PistolPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            Combat_Offhand = DrawItemPoseID.StancePistolHoldUp,
            Combat_Cowboy = DrawItemPoseID.StancePistolCowboy,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Brave = DrawItemPoseID.StanceTwoHandBrave,
        }

        public enum GunPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackAlt = DrawItemPoseID.BackDownward,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand_Upright = DrawItemPoseID.HoldRifleOffHandUpright,
            OffHand_Shoulder = DrawItemPoseID.BackGunShoulder,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Combat_Hip_Ready = DrawItemPoseID.StanceRifleHipReady,
            Combat_Ready = DrawItemPoseID.StanceRiflePointDown,
            Combat_ReadyAlt = DrawItemPoseID.StanceRiflePointDown2,
            Combat_Offhand = DrawItemPoseID.StanceRifleHoldUp,
            Combat_Bolt_Action = DrawItemPoseID.StanceRifleBoltAction,
            Combat_Pump_Action = DrawItemPoseID.StanceRiflePumpAction,
            Combat_Cowboy = DrawItemPoseID.StancePistolCowboy,
            Combat_Power_Tool = DrawItemPoseID.StanceMachineTool,
        }
        public enum LauncherPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackAlt = DrawItemPoseID.BackDownward,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand_Upright = DrawItemPoseID.HoldRifleOffHandUpright,
            OffHand_Shoulder = DrawItemPoseID.StanceLauncherShoulder,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Combat_Hip_Ready = DrawItemPoseID.StanceRifleHipReady,
            Combat_Ready = DrawItemPoseID.StanceRiflePointDown,
            Combat_ReadyAlt = DrawItemPoseID.StanceRiflePointDown2,
            Combat_Offhand = DrawItemPoseID.StanceRifleHoldUp,
            Combat_Bolt_Action = DrawItemPoseID.StanceRifleBoltAction,
            Combat_Pump_Action = DrawItemPoseID.StanceRiflePumpAction,
            Combat_Cowboy = DrawItemPoseID.StancePistolCowboy,
            Combat_Power_Tool = DrawItemPoseID.StanceMachineTool,
        }

        public enum StaffPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            PoleOffHand = DrawItemPoseID.PoleOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Pole_Ready = DrawItemPoseID.StancePoleReady,
            Combat_Pole_Upright = DrawItemPoseID.StancePoleUpright,
            Combat_Pole_Shoulder = DrawItemPoseID.StancePoleShoulder,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Fencer = DrawItemPoseID.StanceRapierFootForward,
        }

        public enum MagicBookPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistHolster,
            Hold = DrawItemPoseID.HoldInHand,
            HoldTome = DrawItemPoseID.HoldSpellTome,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Floating_BackAlt = DrawItemPoseID.FloatingBackUpright,
            Orbit_Clockwise_Fast = DrawItemPoseID.FloatingOrbitClockwiseFast,
            Orbit_AntiClock_Fast = DrawItemPoseID.FloatingOrbitAntiClockwiseFast,
            Orbit_Clockwise_Slow = DrawItemPoseID.FloatingOrbitClockwiseSlow,
            Orbit_AntiClock_Slow = DrawItemPoseID.FloatingOrbitAntiClockwiseSlow,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
        }

        public enum MagicItemPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistHolster,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Orbit_Clockwise_Fast = DrawItemPoseID.FloatingOrbitClockwiseFast,
            Orbit_AntiClock_Fast = DrawItemPoseID.FloatingOrbitAntiClockwiseFast,
            Orbit_Clockwise_Slow = DrawItemPoseID.FloatingOrbitClockwiseSlow,
            Orbit_AntiClock_Slow = DrawItemPoseID.FloatingOrbitAntiClockwiseSlow,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
        }

        public enum GiantItemPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Carry = DrawItemPoseID.CarryInBothHands,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
        }

        public enum GiantMeleePoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            Waist = DrawItemPoseID.WaistSheathe,
            Hold = DrawItemPoseID.HoldInHand,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Two_Hand_Guard = DrawItemPoseID.StanceTwoHandGuard,
            Combat_Two_Hand_Berserk = DrawItemPoseID.StanceTwoHandBerserk,
            Combat_Two_Hand_Highlander = DrawItemPoseID.StanceTwoHandHighlander,
            Combat_Two_Hand_Brave = DrawItemPoseID.StanceTwoHandBrave,
            Combat_Two_Hand_Tail = DrawItemPoseID.StanceTwoHandTail,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
        }

        public enum GiantBowPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.BackBow,
            BackAlt = DrawItemPoseID.BackDownward,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Combat_Hold = DrawItemPoseID.StanceBowInHand,
            Combat_Hunter = DrawItemPoseID.StanceBowHunt,
        }

        public enum GiantGunPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackAlt = DrawItemPoseID.BackDownward,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand_Upright = DrawItemPoseID.HoldRifleOffHandUpright,
            OffHand_Shoulder = DrawItemPoseID.StanceLauncherShoulder,
            Floating_Back = DrawItemPoseID.FloatingBack,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Combat_Hip_Ready = DrawItemPoseID.StanceRifleHipReady,
            Combat_Ready = DrawItemPoseID.StanceRiflePointDown,
            Combat_ReadyAlt = DrawItemPoseID.StanceRiflePointDown2,
            Combat_Offhand = DrawItemPoseID.StanceRifleHoldUp,
            Combat_Bolt_Action = DrawItemPoseID.StanceRifleBoltAction,
            Combat_Pump_Action = DrawItemPoseID.StanceRiflePumpAction,
            Combat_Power_Tool = DrawItemPoseID.StanceMachineTool,
        }

        public enum GiantMagicPoseID
        {
            None = DrawItemPoseID.None,
            Back = DrawItemPoseID.Back,
            BackUpright = DrawItemPoseID.BackUpright,
            Waist = DrawItemPoseID.WaistSheathe,
            WaistAlt = DrawItemPoseID.WaistHolster,
            Hold = DrawItemPoseID.HoldInHand,
            HoldForward = DrawItemPoseID.HoldInHandFront,
            OffHand = DrawItemPoseID.HoldInOffHand,
            Levitate_Front = DrawItemPoseID.FloatingInFront,
            Levitate_OffHand = DrawItemPoseID.FloatingOffHand,
            Levitate_Aimed = DrawItemPoseID.FloatingOffHandAimed,
            Floating_Back = DrawItemPoseID.FloatingBackUpright,
            Floating_BackAlt = DrawItemPoseID.FloatingBack,
            Orbit_Clockwise_Fast = DrawItemPoseID.FloatingOrbitClockwiseFast,
            Orbit_AntiClock_Fast = DrawItemPoseID.FloatingOrbitAntiClockwiseFast,
            Orbit_Clockwise_Slow = DrawItemPoseID.FloatingOrbitClockwiseSlow,
            Orbit_AntiClock_Slow = DrawItemPoseID.FloatingOrbitAntiClockwiseSlow,
            Combat_Hold = DrawItemPoseID.StanceInHand,
            Combat_Pole_Ready = DrawItemPoseID.StancePoleReady,
            Combat_Pole_Upright = DrawItemPoseID.StancePoleUpright,
            Combat_Pole_Shoulder = DrawItemPoseID.StancePoleShoulder,
            Combat_Two_Hand = DrawItemPoseID.StanceTwoHand,
            Combat_Ready = DrawItemPoseID.StanceMeleeReady,
            Combat_Upright = DrawItemPoseID.StanceMeleeUpright,
            Combat_Raised = DrawItemPoseID.StanceMeleeRaised,
            Combat_Trail = DrawItemPoseID.StanceMeleeTrail,
            Combat_Fencer = DrawItemPoseID.StanceRapierFootForward,
        }
    }
}
