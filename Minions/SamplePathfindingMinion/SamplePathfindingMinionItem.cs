using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SamplePathfindingMinion
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SamplePathfindingMinionItem : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.FairyBell;

        public override void SetStaticDefaults()
        {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.FlinxStaff);
			Item.damage = 32;
			Item.shoot = ProjectileType<SamplePathfindingMinionProjectile>();
			Item.buffType = BuffType<SamplePathfindingMinionBuff>();
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
			spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.Red, 0, origin, scale, 0, 0);
			return false;
		}
	}
}
