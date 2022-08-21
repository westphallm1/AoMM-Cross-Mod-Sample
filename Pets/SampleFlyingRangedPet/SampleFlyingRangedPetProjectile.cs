using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleFlyingRangedPet
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleFlyingRangedPetProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ZephyrFish;
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

        public override void AI()
        {
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleFlyingRangedPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // make it red to distinguish from vanilla
            lightColor = Color.LightSkyBlue.MultiplyRGB(lightColor * 1.5f);
            return true;
        }
    }
}
