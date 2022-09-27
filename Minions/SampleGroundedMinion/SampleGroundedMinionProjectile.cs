using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleGroundedMinion
{
	/// <summary>
	/// Non-custom minion projectile with cross-mod AI applied by a mod.Call.
	/// </summary>
	internal class SampleGroundedMinionProjectile : ModProjectile
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
			Projectile.timeLeft = 60;
			AIType = ProjectileID.VampireFrog;
			// Adjust draw position
			DrawOffsetX = -64;
			DrawOriginOffsetY = -20;
		}

		// necessary for melee minions
		public override bool MinionContactDamage() => true;

		public override void AI()
		{
			Main.player[Projectile.owner].vampireFrog = false;
			// Keep alive while the buff is active
			if (Main.player[Projectile.owner].HasBuff(BuffType<SampleGroundedMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// change the light color to distinguish from vanilla
			lightColor = Color.SkyBlue.MultiplyRGB(lightColor * 1.5f);
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false; // do not die when colliding with a tile
		}

	}
}
