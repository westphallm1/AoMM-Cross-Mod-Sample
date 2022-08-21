using AoMMCrossModSample.Minions.SampleCustomMinion;
using AoMMCrossModSample.Minions.SampleGroundedMinion;
using AoMMCrossModSample.Minions.SamplePathfindingMinion;
using AoMMCrossModSample.Pets.SampleCustomPet;
using AoMMCrossModSample.Pets.SampleFlyingRangedPet;
using AoMMCrossModSample.Pets.SampleGroundedPet;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample
{
    public class AoMMCrossModSample : Mod
    {
        public override void PostSetupContent()
        {
            RegisterPets();
            RegisterMinions();
        }

        private static void RegisterPets()
        {
            // Register a projectile with vanilla pet AI as a grounded cross mod combat pet with melee attack
            AmuletOfManyMinionsApi.RegisterGroundedPet(
                GetInstance<SampleGroundedPetProjectile>(), GetInstance<SampleGroundedPetBuff>(), null);

            // Register a projectile with vanilla pet AI as a flying cross mod combat pet. To switch
            // a grounded or flying combat pet to ranged attack style, pass in a non-null 3rd parameter
            // to the mod.Call
            AmuletOfManyMinionsApi.RegisterFlyingPet(
                GetInstance<SampleFlyingRangedPetProjectile>(), GetInstance<SampleFlyingRangedPetBuff>(), ProjectileID.FrostDaggerfish);

            // Apply combat pet AI to a projectile that is not a clone of a vanilla pet
            // This pet's AI also perform some small custom actions based on AoMM state
            AmuletOfManyMinionsApi.RegisterFlyingPet(
                GetInstance<SampleCustomPetProjectile>(), GetInstance<SampleCustomPetBuff>(), null);

        }

        private static void RegisterMinions()
        {
            // Register a projectile with vanilla minion AI as a grounded cross mod minion with ranged attack.
            // Need to manually specify shot projectile, search range, and travel speed
            AmuletOfManyMinionsApi.RegisterGroundedMinion(
                GetInstance<SampleGroundedMinionProjectile>(), GetInstance<SampleGroundedMinionBuff>(), ProjectileID.RubyBolt, 800, 8, 12);

            // Register a custom minion that acts on AoMM's state variables, with a search range of 800 pixels
            AmuletOfManyMinionsApi.RegisterInfoMinion(
                GetInstance<SampleCustomMinionProjectile>(), GetInstance<SampleCustomMinionBuff>(), 800);

            // Register a custom minion that acts on AoMM's state variables, but uses the default AoMM pathfinder
            AmuletOfManyMinionsApi.RegisterPathfindingMinion(
                GetInstance<SamplePathfindingMinionProjectile>(), GetInstance<SamplePathfindingMinionBuff>(), 800, 12, 18);
        }
    }
}