using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleSlimePet
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleSlimePetProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.KingSlimePet;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.projFrames[ProjectileID.KingSlimePet];
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.KingSlimePet);
            AIType = ProjectileID.KingSlimePet;
            // This appears to be necessary for visual purposes
            DrawOriginOffsetY = -16;
            DrawOriginOffsetX = -8;
        }

        public override bool PreAI()
        {
            // unset default buff
            Main.player[Projectile.owner].petFlagKingSlimePet = false;
            return true;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleSlimePetBuff>()))
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
