using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleEmpoweredMinion
{
	/// <summary>
	/// Counter minion projectile used to track the "power level" of a single minion that scales with
	/// slots used, similar to the vanilla desert tiger and stardust dragon staves. 
	/// </summary>
	internal class SampleEmpoweredMinionCounterProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DeadlySphere;

		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.minionSlots = 1;
			Projectile.timeLeft = 5;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			int minionType = ProjectileType<SampleEmpoweredMinionProjectile>();
			// Keep alive while the buff is active
			if (player.HasBuff(BuffType<SampleEmpoweredMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			// Stay directly on top of the player
			Projectile.Center = player.Center;

			// if the player doesn't have an instance of the actual minion, summon one
			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[minionType] == 0)
			{
				Projectile spawned = Projectile.NewProjectileDirect(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					default,
					minionType,
					Projectile.damage,
					Projectile.knockBack,
					Projectile.owner);
				spawned.originalDamage = Projectile.originalDamage;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// Don't draw the counter minion
			return false;
		}
	}

	/// <summary>
	/// Minion projectile with cross-mod AI applied by a mod.Call. Uses GetParametersDirect and
	/// SetParametersDirect to scale the minion's power level based on the number of copies
	/// summoned.
	/// </summary>
	internal class SampleEmpoweredMinionProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Raven;

		private int? baseOriginalDamage;

		private int previousEmpowerCount;

		private bool hasSetActive;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.Raven];
			Main.projPet[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Raven);
			AIType = ProjectileID.Raven;
			Projectile.minionSlots = 0; // empowered minions don't count for slots, the counter type does
		}

		public override bool PreAI()
		{
			if ((!hasSetActive) && AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var modParams))
			{
				// GetParamsDirect uses reflection, which can have performance implications
				// As a best practice, only invoke it when needed
				hasSetActive = true;
				// By default, cross-mod AI is only applied to projectiles that are spawned by an 
				// item with a cross-mod registered `buffType`. Since this projectile is instead
				// spawned by another projectile (the counter minion), we must tell AoMM to apply
				// cross-mod AI explicitly by setting `IsActive = true`
				modParams.IsActive = true;
				// Need to manually apply params updates after updating
				AmuletOfManyMinionsApi.UpdateParamsDirect(this, modParams);
			}
			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			// Keep alive while the buff is active
			if (player.HasBuff(BuffType<SampleEmpoweredMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			// Need to cache the base original damage, since it's updated every frame
			baseOriginalDamage ??= Projectile.originalDamage;
			// Scale several factors based on the number of counter minions present
			int empowerCount = player.ownedProjectileCounts[ProjectileType<SampleEmpoweredMinionCounterProjectile>()];
			// With no cross mod enable, just update the damage
			Projectile.originalDamage = (int)baseOriginalDamage / 2 + (int)baseOriginalDamage / 6 * empowerCount;
			// With cross mod enabled, also update move speed/attack range when empower count changes
			if (empowerCount != previousEmpowerCount && AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var modParams))
			{
				// These values are roughly consistent with what's used in the base mod, be sure to cap
				// them at some reasonable minimum in case too many counter minions are summoned
				modParams.AttackFrames = Math.Max(30, 60 - 5 * empowerCount);
				modParams.MaxSpeed = Math.Min(16, 12 + empowerCount);
				modParams.Inertia = Math.Min(16, 12 + empowerCount);
				modParams.SearchRange = Math.Min(1200, 1000 + 50 * empowerCount);
				// Need to manually apply params updates after updating
				AmuletOfManyMinionsApi.UpdateParamsDirect(this, modParams);
			}
			previousEmpowerCount = empowerCount;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// change the light color to distinguish from vanilla
			lightColor = Color.SkyBlue.MultiplyRGB(lightColor * 1.5f);
			return true;
		}

	}
}
