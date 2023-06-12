using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleMultiPet
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SampleMultiPetBuff : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_" + BuffID.PetSapling;

		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			drawParams.DrawColor = Color.LimeGreen;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 2;
			int projType = ProjectileType<SampleMultiPetGroundedProjectile>();
			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] == 0)
			{
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, default, projType, 0, 0, player.whoAmI);
			}
			// Spawn both pet types from the same buff
			int projType2 = ProjectileType<SampleMultiPetFlyingProjectile>();
			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType2] == 0)
			{
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, default, projType2, 0, 0, player.whoAmI);
			}
		}
	}

}
