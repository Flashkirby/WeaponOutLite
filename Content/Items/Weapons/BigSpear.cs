using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace WeaponOutLite.Content.Items.Weapons
{
	public class BigSpear: ModItem
	{
        public override bool IsLoadingEnabled(Mod mod) => WeaponOutLite.DEBUG_TEST_ITEMS;

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Giant's Skewer");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.Spear);
			Item.scale = 1f;
		}
	}
}
