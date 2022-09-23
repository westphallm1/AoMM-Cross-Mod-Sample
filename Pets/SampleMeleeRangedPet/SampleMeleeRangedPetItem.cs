using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleMeleeRangedPet
{
	internal class SampleMeleeRangedPetItem : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.LizardEgg;

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LizardEgg);
			Item.shoot = ProjectileType<SampleMeleeRangedPetProjectile>();
			Item.buffType = BuffType<SampleMeleeRangedPetBuff>();
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
			spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.LightBlue, 0, origin, scale, 0, 0);
			return false;
		}
	}
}
