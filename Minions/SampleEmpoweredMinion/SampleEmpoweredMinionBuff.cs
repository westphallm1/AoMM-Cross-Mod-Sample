using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleEmpoweredMinion
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SampleEmpoweredMinionBuff : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_" + BuffID.Ravens;

		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			DisplayName.SetDefault("Sample Empowered Minion");
			Description.SetDefault("Sample Empowered Minion");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			drawParams.DrawColor = Color.SkyBlue;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 2;
			// Check for the counter minion here, rather than the base minion
			int projType = ProjectileType<SampleEmpoweredMinionCounterProjectile>();
			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] == 0)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}
