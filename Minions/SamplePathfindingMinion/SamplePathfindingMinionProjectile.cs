using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
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

		private IAoMMState aommState { get; set; } = new AoMMStateImpl();

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.PinkFairy];
			Main.projPet[Type] = true;
		}

		public override void SetDefaults()
		{
			// Don't clone the AI of an existing projectile, set defaults manually
			Main.projPet[Type] = true;
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
			AmuletOfManyMinionsApi.GetStateDirect(this, aommState);
			var maxSpeed = 12;
			var inertia = 18;
			Vector2 newVelocity;

			// Simple AI: Find where AoMM wants the projectile to go, then go there
			// Do not need to account for pathfinding, as it is handled by AoMM
			if (aommState.IsAttacking && aommState.TargetNPC is NPC targetNPC)
            {
				// Update the pet's velocity to move it towards the target NPC
				newVelocity = targetNPC.Center - Projectile.Center;
            } else 
            {
				// Update the pet's velocity to move it towards the player
				Vector2 idlePosition = Main.player[Projectile.owner].Top - new Vector2(0, 16);
				newVelocity = idlePosition - Projectile.Center;
            }
            if(newVelocity.LengthSquared() > maxSpeed * maxSpeed)
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
			if(Math.Abs(Projectile.velocity.X) > 1)
            {
				Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
            }
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			// Keep alive while the buff is active
			if(Main.player[Projectile.owner].HasBuff(BuffType<SamplePathfindingMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			// Ensure Projectile.tileCollide is false, as AoMM may toggle this 
			Projectile.tileCollide = false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// change the light color to distinguish from vanilla
			lightColor = Color.Tomato.MultiplyRGB(lightColor * 1.5f);
			return true;
		}

	}
}
