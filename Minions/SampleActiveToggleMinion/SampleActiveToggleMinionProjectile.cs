using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleActiveToggleMinion
{
	/// <summary>
	/// Example custom minion projectile that toggles its own cross-mod AI on and off using
	/// `mod.Call("UpdateParameters",...)` based on the game state. This is not a
	/// particularly realistic use case, but demonstrates an advanced usage of the API.
	/// </summary>
	internal class SampleActiveToggleMinionProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.VampireFrog;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.VampireFrog];
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.VampireFrog);
			Projectile.netImportant = true;
			AIType = ProjectileID.VampireFrog;
			// Adjust draw position
			DrawOffsetX = -64;
			DrawOriginOffsetY = -20;
		}

		// necessary for melee minions
		public override bool MinionContactDamage() => true;

		public override bool PreAI()
		{
			if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var modParams))
			{
				// If an even number of minions are summoned and this minion is in an 
				// even minion slot, or if an odd number of minions are summoned and this minion
				// is in an odd minion slot, use cross mod AI. Otherwise, use default AI.
				bool hasCrossModParity = Projectile.minionPos % 2 == Main.player[Projectile.owner].ownedProjectileCounts[Type] % 2;
				modParams.IsActive = hasCrossModParity;
				AmuletOfManyMinionsApi.UpdateParamsDirect(this, modParams);
			}

			return true;
		}
		public override void AI()
		{
			Main.player[Projectile.owner].vampireFrog = false;
			// Keep alive while the buff is active
			if (Main.player[Projectile.owner].HasBuff(BuffType<SampleActiveToggleMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// change the light color to distinguish from vanilla
			lightColor = Color.Violet.MultiplyRGB(lightColor * 1.5f);
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false; // do not die when colling with a tile
		}

	}
}
