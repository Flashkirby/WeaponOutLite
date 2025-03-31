using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace WeaponOutLite.Common.Configs
{
	public class WeaponOutServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ServerSide")] 

		[DefaultValue(true)]
		public bool EnableWeaponOutVisuals;

		[DefaultValue(false)]
		public bool EnableForcedWeaponOutVisuals;
	}
}
