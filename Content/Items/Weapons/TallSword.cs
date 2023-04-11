using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace WeaponOutLite.Content.Items.Weapons
{
	public class TallSword: ModItem
	{
        public override bool IsLoadingEnabled(Mod mod) => WeaponOutLite.DEBUG_TEST_ITEMS;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Giant's Cutter");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.Muramasa);
			Item.scale = 1f;
		}
	}
}
