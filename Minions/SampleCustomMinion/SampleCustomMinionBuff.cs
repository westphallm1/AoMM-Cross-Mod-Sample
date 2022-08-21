using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleCustomMinion
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SampleCustomMinionBuff : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_" + BuffID.FairyBlue;

		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			DisplayName.SetDefault("Sample Custom Minion");
			Description.SetDefault("Sample Custom Minion");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			drawParams.DrawColor = Color.Violet;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 2;
			int projType = ProjectileType<SampleCustomMinionProjectile>();
			if(player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] == 0)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}
