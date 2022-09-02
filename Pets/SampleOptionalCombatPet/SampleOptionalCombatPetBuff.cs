using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleOptionalCombatPet
{
    /// <summary>
    /// The non-combat pet version of the buff. This buff is not registered via a mod.Call, 
    /// so AoMM will not turn the pet into a combat pet if summoned via this buff.
    /// </summary>
    internal class SampleOptionalCombatPetBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.PetTurtle;

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
            DisplayName.SetDefault("Sample Grounded Pet");
            Description.SetDefault("Sample Grounded Pet");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            drawParams.DrawColor = Color.Purple;
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 2;
            int projType = ProjectileType<SampleOptionalCombatPetProjectile>();
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] == 0)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, default, projType, 0, 0, player.whoAmI);
            }
        }
    }

    /// <summary>
    /// The combat pet version of the buff. This buff *is* registered via a mod.Call, 
    /// so AoMM will turn the pet into a combat pet if summoned via this buff.
    /// </summary>
    internal class SampleOptionalCombatPetBuff_CombatVersion : SampleOptionalCombatPetBuff
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault(DisplayName.GetDefault() + " (AoMM Version)");
        }
    }

}
