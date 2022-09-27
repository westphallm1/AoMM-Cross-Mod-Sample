using Terraria.ID;
using Terraria.ModLoader;

namespace AoMMCrossModSample.Projectiles
{
	/// <summary>
	/// Clone of the vanilla sapphire bolt, with appropriate metadata set to count as
	/// launched by a minion. Not used by any default AI, only added to AI via cross mod 
	/// integration.
	/// </summary>
	internal class SapphireBoltCloneProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SapphireBolt;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SapphireBolt);
			AIType = ProjectileID.SapphireBolt;
			Projectile.DamageType = DamageClass.Summon;
		}
	}
}
