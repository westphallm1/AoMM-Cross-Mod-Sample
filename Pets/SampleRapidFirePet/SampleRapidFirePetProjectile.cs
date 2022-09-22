using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleRapidFirePet
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleRapidFirePetProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ZephyrFish;


        // Only need to update cross mod parameters once in this case, not every frame
        private bool hasSetCrossModParams;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.projFrames[ProjectileID.ZephyrFish];
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);
            AIType = ProjectileID.ZephyrFish;
        }

        public override bool PreAI()
        {
            // unset default buff
            Main.player[Projectile.owner].zephyrfish = false;
            return true;
        }

        // Update the travel speed and rate of fire multipliers to make this combat pet
        // move more quickly than an average combat pet and fire more quickly
        private void SetCrossModParams()
        {
            hasSetCrossModParams = true;
            if(AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var modParams))
            {
                // increase movement speed
                modParams.MaxSpeedScaleFactor = 1.1f;
                // increase rate of fire (decrease frames between firing projectiles)
                modParams.AttackFramesScaleFactor = 0.6f;
                AmuletOfManyMinionsApi.UpdateParamsDirect(this, modParams);
            }
        }

        public override void AI()
        {
            if(!hasSetCrossModParams)
            {
                SetCrossModParams();
            }

            if(AmuletOfManyMinionsApi.IsActive(this))
            {
                // Since we attack more rapidly than the combat pet, also decrease damage dealt
                // AoMM updates combat pet damage every frame, so update here every frame as well
                Projectile.originalDamage = 3 * Projectile.originalDamage / 4;
            }

            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleRapidFirePetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // make it red to distinguish from vanilla
            lightColor = Color.Violet.MultiplyRGB(lightColor * 1.5f);
            return true;
        }
    }
}
