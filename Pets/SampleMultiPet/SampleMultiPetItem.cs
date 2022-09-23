using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleMultiPet
{
	internal class SampleMultiPetItem : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.Seedling;

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Seedling);
			Item.shoot = ProjectileType<SampleMultiPetGroundedProjectile>();
			Item.buffType = BuffType<SampleMultiPetBuff>();
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
			spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.LimeGreen, 0, origin, scale, 0, 0);
			return false;
		}
	}
}
