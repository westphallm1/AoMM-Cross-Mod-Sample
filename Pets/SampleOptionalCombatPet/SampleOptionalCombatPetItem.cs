using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleOptionalCombatPet
{
	/// <summary>
	/// The non-combat pet version of the item. Applies a pet buff that is not registered via a mod.Call, 
	/// so AoMM will not turn the pet into a combat pet if summoned via this item.
	/// </summary>
	internal class SampleOptionalCombatPetItem : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.Seaweed;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sample Optional Combat Pet Item");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Seaweed);
			Item.shoot = ProjectileType<SampleOptionalCombatPetProjectile>();
			Item.buffType = BuffType<SampleOptionalCombatPetBuff>();
		}


		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600);
			}
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.Purple, 0, origin, scale, 0, 0);
			return false;
		}
	}

	/// <summary>
	/// The combat pet version of the item. Applies a pet buff that *is* registered via a mod.Call, 
	/// so AoMM will turn the pet into a combat pet if summoned via this item.
	/// </summary>
	internal class SampleOptionalCombatPetItem_CombatVersion : SampleOptionalCombatPetItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(DisplayName.GetDefault() + " (AoMM Version)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.buffType = BuffType<SampleOptionalCombatPetBuff_CombatVersion>();
		}

		// Add a recipe that allows crafting from the regular version
		public override void AddRecipes()
		{
			// Only enable this item if AmuletOfManyMinions is enabled
			if (ModLoader.HasMod("AmuletOfManyMinions"))
			{
				// add reciprocal recipes for this and the regular version, crafted at a demon altar
				int otherType = ItemType<SampleOptionalCombatPetItem>();
				Recipe.Create(Type).AddIngredient(otherType).AddTile(TileID.DemonAltar).Register();
				Recipe.Create(otherType).AddIngredient(Type).AddTile(TileID.DemonAltar).Register();
			}
		}
	}
}
