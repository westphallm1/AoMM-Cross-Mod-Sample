using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleCustomPet
{
    /// <summary>
    /// Pet Projectile with partially custom AI that acts according to AoMM default behavior 
    /// most of the time, but overrides AoMM's behavior when no enemies are present.
    /// </summary>
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
            var maxSpeed = 8;
            var inertia = 12;
            // Get the AoMM calculated state for the projectile as a Dictionary<string, object>
            var stateDict = AmuletOfManyMinionsApi.GetState(this);

            // If AoMM is enabled, and the minion is in the "idle" state, override AoMM's AI
            // to maintain the non-cross-mod "hover directly over the head" behavior
            if(stateDict != null && (bool)stateDict["IsIdle"])
            {
                // Update travel speed based on the cross-mod calculated combat pet stats
                maxSpeed = (int)stateDict["MaxSpeed"];
                inertia = (int)stateDict["Inertia"];
                // AoMM typically overwrites any changes to position/velocity made in AI(), stop it from doing so
                // this frame.
                AmuletOfManyMinionsApi.ReleaseControl(this);
            }

            // Update the pet's velocity to move it above the player's head
            Vector2 idlePosition = Main.player[Projectile.owner].Top - new Vector2(0, 16);
            Vector2 newVelocity = idlePosition - Projectile.Center;
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
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleCustomPetBuff>()))
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
