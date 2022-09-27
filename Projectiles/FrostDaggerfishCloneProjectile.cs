using AoMMCrossModSample.Pets.SampleRapidFirePet;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AoMMCrossModSample.Projectiles
{
	/// <summary>
	/// Clone of the vanilla frost daggerfish, with appropriate metadata set to count as
	/// launched by a minion. Not used by any default AI, only added to AI via cross mod 
	/// integration. Used by two cross-mod minion AIs, with differing behavior based on
	/// which cross-mod minion it was spawned from. Spawn source is determined in `OnSpawn`,
	/// since cross-mod AI does not set any other custom flags on launched projectiles.
	/// </summary>
	internal class FrostDaggerfishCloneProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.FrostDaggerfish;

		private bool FromRapidFirePet;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.FrostDaggerfish);
			AIType = ProjectileID.FrostDaggerfish;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override void OnSpawn(IEntitySource source)
		{
			// AoMM cross-mod AI will set the projectile spawn source to the cross-mod minion that originated
			// the projectile, but otherwise uses a set of default parameters, including 0 in both ai slots.
			// To apply special behavior to the spawned projectile based on the cross-mod minion that spawned it,
			// check the source in OnSpawn
			FromRapidFirePet = source is EntitySource_Parent parentSource &&
				parentSource.Entity is Projectile parent &&
				parent.type == ModContent.ProjectileType<SampleRapidFirePetProjectile>();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (FromRapidFirePet)
			{
				lightColor = Color.Violet.MultiplyRGB(lightColor * 1.5f);
			}
			return true;
		}

	}
}
