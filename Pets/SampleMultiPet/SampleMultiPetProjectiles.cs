using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleMultiPet
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleMultiPetGroundedProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Sapling;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.projFrames[ProjectileID.Sapling];
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Sapling);
            AIType = ProjectileID.Sapling;
            // This appears to be necessary for visual purposes
            DrawOriginOffsetY = -4;
            DrawOffsetX = -16;
        }

        public override bool PreAI()
        {
            // unset default buff
            Main.player[Projectile.owner].sapling = false;
            return true;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleMultiPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // make it red to distinguish from vanilla
            lightColor = Color.LimeGreen.MultiplyRGB(lightColor * 1.5f);
            return true;
        }
    }

    // Second pet spawned from the same pet item/buff
    internal class SampleMultiPetFlyingProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.TikiSpirit;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.projFrames[ProjectileID.TikiSpirit];
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.TikiSpirit);
            AIType = ProjectileID.TikiSpirit;
            // This appears to be necessary for visual purposes
            DrawOriginOffsetY = -8;
        }

        public override bool PreAI()
        {
            // unset default buff
            Main.player[Projectile.owner].tiki = false;
            return true;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleMultiPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // make it red to distinguish from vanilla
            lightColor = Color.LimeGreen.MultiplyRGB(lightColor * 1.5f);
            return true;
        }
    }
}
