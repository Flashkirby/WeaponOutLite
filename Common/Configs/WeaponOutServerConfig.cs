using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace WeaponOutLite.Common.Configs
{

	[Label("$Mods.WeaponOut.Config.ServerSideConfigLabel")]
	public class WeaponOutServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("$Mods.WeaponOut.Config.ServerSideHeader")] 

		[Label("$Mods.WeaponOut.Config.EnableWeaponOutVisuals.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableWeaponOutVisuals.Tooltip")]
		[DefaultValue(true)]
		public bool EnableWeaponOutVisuals;

		[Label("$Mods.WeaponOut.Config.EnableForcedWeaponOutVisuals.Label")]
		[Tooltip("$Mods.WeaponOut.Config.EnableForcedWeaponOutVisuals.Tooltip")]
		[DefaultValue(false)]
		public bool EnableForcedWeaponOutVisuals;
	}
}
