using Terraria.ID;
using Terraria.ModLoader;

namespace AoMMCrossModSample.Projectiles
{
	/// <summary>
	/// Clone of the vanilla ruby bolt, with appropriate metadata set to count as
	/// launched by a minion. Not used by any default AI, only added to AI via cross mod 
	/// integration.
	/// </summary>
	internal class RubyBoltCloneProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RubyBolt;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.RubyBolt);
			AIType = ProjectileID.RubyBolt;
			Projectile.DamageType = DamageClass.Summon;
		}
	}
}
