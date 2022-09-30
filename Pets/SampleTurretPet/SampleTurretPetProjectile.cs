using AoMMCrossModSample.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleTurretPet
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SampleTurretPetProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.TikiSpirit;

		int framesSinceLastFiredProjectile;

		// Only need to update cross mod parameters once in this case, not every frame
		private bool hasSetCrossModParams;

		// Update the travel speed and rate of fire multipliers to make this combat pet
		// to have a longer firing cycle, since projectile firing is managed several times
		// within a cycle
		private void SetCrossModParams()
		{
			hasSetCrossModParams = true;
			if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var modParams))
			{
				// decrease rate of fire (increase frames between firing projectiles),
				// since we manually fire multiple projectiles in between AoMM's computed attack cycle
				modParams.AttackFramesScaleFactor = 2f;
				AmuletOfManyMinionsApi.UpdateParamsDirect(this, modParams);
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.TikiSpirit];
			Main.projPet[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.TikiSpirit);
			AIType = ProjectileID.TikiSpirit;
		}

		public override bool PreAI()
		{
			// unset default buff
			Main.player[Projectile.owner].tiki = false;
			// always reset AoMM-computed position changes, to maintain default tiki behavior
			// (hover by the player's head). ReleaseControl must be called before the projectile's
			// default AI runs, so put it at the end of PreAI
			AmuletOfManyMinionsApi.ReleaseControl(this);
			return true;
		}

		public override void AI()
		{
			if (!hasSetCrossModParams)
			{
				SetCrossModParams();
			}

			if(AmuletOfManyMinionsApi.IsActive(this))
			{
				CrossModAI();
			}

			if (Main.player[Projectile.owner].HasBuff(BuffType<SampleTurretPetBuff>()))
			{
				Projectile.timeLeft = 2;
			}
		}

		private void CrossModAI()
		{
			// Since we attack more rapidly than the basic combat pet, also decrease damage dealt
			// AoMM updates combat pet damage every frame, so update here every frame as well
			Projectile.originalDamage = 2 * Projectile.originalDamage / 3;

			// only perform other cross-mod tasks while AoMM is attacking, and in range to fire a projectile
			if(!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var modState) || !modState.IsInFiringRange)
			{
				return;
			}

			// Perform fully custom projectile firing AI: Attack on the frame that AoMM suggests to attack,
			// then also 12 and 24 frames after that
			// In the registration mod.Call, set firedProjectileId = 0 to prevent AoMM from spawning
			// a projectile with the default parameters
			if(modState.ShouldFireThisFrame) 
			{ 
				framesSinceLastFiredProjectile = 0; 
			}

			bool shouldFireThisFrame = framesSinceLastFiredProjectile % 12 == 0 && framesSinceLastFiredProjectile < 36;
			// ensure that this code only runs client side, and that an npc exists to attack
			if(shouldFireThisFrame && Main.myPlayer == Projectile.owner && modState.TargetNPC is NPC targetNpc)
			{
				Vector2 launchVector = targetNpc.Center - Projectile.Center;
				launchVector.Normalize();
				// fire faster the higher the player's pet level
				launchVector *= 1.5f * modState.MaxSpeed;
				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					launchVector,
					ProjectileType<SapphireBoltCloneProjectile>(),
					Projectile.damage,
					Projectile.knockBack,
					Projectile.owner);
			}
			framesSinceLastFiredProjectile++;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// add a tint to distinguish from vanilla
			lightColor = Color.LightSkyBlue.MultiplyRGB(lightColor * 1.5f);
			return true;
		}
	}
}
