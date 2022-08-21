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

namespace AoMMCrossModSample.Pets.SampleCustomPet
{
	// Code largely adapted from tModLoader Sample Mod
	internal class SampleCustomPetProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Wisp;
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.Wisp];
			Main.projPet[Type] = true;
		}

		public override void SetDefaults()
		{
			// Don't clone the AI of an existing projectile, set defaults manually
			Main.projPet[Type] = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.netImportant = true;
			Projectile.timeLeft = 10;
			DrawOriginOffsetY = -24; // projectile is much taller than its hitbox
		}

		public override void AI()
		{
			// Get the AoMM calculated state for the projectile
			// In a real mod, you would need to defensively program against these potentially being null
			var stateDict = AmuletOfManyMinionsApi.GetState(this);
			var state = new AoMMStateReader(stateDict);

			// If the projectile is idling (not attacking or pathfinding), release it from
			// AoMM's control and make it hover directly over the player's head
			if(state.IsIdle)
            {
				// AoMM typically overwrites any changes to position/velocity made in AI(), stop it from doing so
				// this frame.
				AmuletOfManyMinionsApi.ReleaseControl(this);

				// Update the pet's velocity to move it towards the player
				Vector2 idlePosition = Main.player[Projectile.owner].Top - new Vector2(0, 16);
				Vector2 newVelocity = idlePosition - Projectile.Center;
				if(newVelocity.LengthSquared() > state.MaxSpeed * state.MaxSpeed)
                {
					newVelocity.Normalize();
					newVelocity *= state.MaxSpeed;
                }

				// Standard formula for moving with inertia
				Projectile.velocity = (Projectile.velocity * (state.Inertia - 1) + newVelocity) / state.Inertia;
            }

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
			if(Main.player[Projectile.owner].HasBuff(BuffType<SampleCustomPetBuff>()))
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

	}
}
