using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleEmpoweredMinion
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SampleEmpoweredMinionItem : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.DeadlySphereStaff;

		public override void SetStaticDefaults()
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DeadlySphereStaff);
			Item.damage = 45; // lower base damage since it gains more power per summon
							  // summon the counter minion rather than the empowered minion
			Item.shoot = ProjectileType<SampleEmpoweredMinionCounterProjectile>();
			Item.buffType = BuffType<SampleEmpoweredMinionBuff>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2);
			var proj = Projectile.NewProjectileDirect(source, Main.MouseWorld, default, type, damage, knockback, Main.myPlayer);
			proj.originalDamage = Item.damage;
			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.SkyBlue, 0, origin, scale, 0, 0);
			return false;
		}
	}
}
