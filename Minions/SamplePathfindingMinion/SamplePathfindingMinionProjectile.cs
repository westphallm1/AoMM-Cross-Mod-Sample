using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SamplePathfindingMinion
{
	/// <summary>
	/// Minion Projectile with mostly custom AI that determines projectile behavior based on
	/// AoMM-provided state values, but relies on AoMM's default "follow the pathfinder"
	/// implementation.
	/// </summary>
	internal class SamplePathfindingMinionProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.PinkFairy;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.PinkFairy];
			Main.projPet[Type] = true;
		}

		public override void SetDefaults()
		{
			// Don't clone the AI of an existing projectile, set defaults manually
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.netImportant = true;
			Projectile.timeLeft = 10;
			DrawOriginOffsetX = -16; // projectile is much wider than its hitbox
		}

		public override void AI()
		{
			// Get the AoMM calculated state for the projectile as a Dictionary<string, object>
			// In a real mod, you would need to defensively program against these potentially being absent
			var maxSpeed = 12;
			var inertia = 18;
			Vector2 targetPosition;
			if (AmuletOfManyMinionsApi.TryGetStateDirect(this, out var aommState))
			{
				// If AoMM is enabled, determine a target position based on the AoMM state
				targetPosition = GetCrossModTargetPosition(aommState);
			}
			else
			{
				// Otherwise, use a simple algorithm to get the position of an enemy or the player
				targetPosition = GetBaseTargetPosition();
			}


			// Compute velocity, normalize to the max speed
			Vector2 newVelocity = targetPosition - Projectile.Center;
			if (newVelocity.LengthSquared() > maxSpeed * maxSpeed)
			{
				newVelocity.Normalize();
				newVelocity *= maxSpeed;
			}

			// Standard formula for moving with inertia
			Projectile.velocity = (Projectile.velocity * (inertia - 1) + newVelocity) / inertia;

			// Basic animation, loop through frames and face the direction of movement,
			// tilting slightly towards direction of movement
			Projectile.frameCounter++;
			Projectile.frame = (Projectile.frameCounter / 5) % Main.projFrames[Type];
			if (Math.Abs(Projectile.velocity.X) > 1)
			{
				Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
			}
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			// Keep alive while the buff is active
			if (Main.player[Projectile.owner].HasBuff(BuffType<SamplePathfindingMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			// Ensure Projectile.tileCollide is false, as AoMM may toggle this 
			Projectile.tileCollide = false;
		}

		private Vector2 GetCrossModTargetPosition(IAoMMState aommState)
		{
			// Simple Cross mod AI: Find where AoMM wants the projectile to go, then go there
			if (aommState.IsPathfinding && aommState.NextPathfindingTaret is Vector2 pathfindingTarget)
			{
				// Update the pet's velocity to move it towards the next pathfinding node
				return pathfindingTarget;
			}
			else if (aommState.IsAttacking && aommState.TargetNPC is NPC targetNPC)
			{
				// Update the pet's velocity to move it towards the target NPC
				return targetNPC.Center;
			}
			else
			{
				// If not attacking or pathfinding, return to above the player's head
				return Main.player[Projectile.owner].Top - new Vector2(0, 16);
			}

		}

		private Vector2 GetBaseTargetPosition()
		{
			// Basic AI: If an enemy is within line of sight, go towards it, otherwise go towards the player
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC possibleTarget = Main.npc[i];
				if (possibleTarget.CanBeChasedBy() && // only target enemies that can attack the player
				   Vector2.DistanceSquared(possibleTarget.Center, Projectile.Center) < 700 * 700 && // only attack enemies that are close by
				   Collision.CanHitLine(possibleTarget.Center, 1, 1, Projectile.Center, 1, 1))
				{
					return possibleTarget.Center;
				}
			}
			// otherwise, go above the player head
			return Main.player[Projectile.owner].Top - new Vector2(0, 16);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			// change the light color to distinguish from vanilla
			lightColor = Color.Tomato.MultiplyRGB(lightColor * 1.5f);
			return true;
		}

	}
}
