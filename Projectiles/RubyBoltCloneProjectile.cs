using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace AoMMCrossModSample.Projectiles
{
    /// <summary>
    /// Clone of the vanilla frost daggerfish, with appropriate metadata set to count as
    /// launched by a minion. Not used by any default AI, only added to AI via cross mod 
    /// intergration.
    /// </summary>
    internal class RubyBoltCloneProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RubyBolt);
            AIType = ProjectileID.RubyBolt;
            Projectile.DamageType = DamageClass.Summon;
        }
    }
}
