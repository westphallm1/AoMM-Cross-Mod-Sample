using AoMMCrossModSample.Minions.SampleActiveToggleMinion;
using AoMMCrossModSample.Minions.SampleCustomMinion;
using AoMMCrossModSample.Minions.SampleEmpoweredMinion;
using AoMMCrossModSample.Minions.SampleGroundedMinion;
using AoMMCrossModSample.Minions.SamplePathfindingMinion;
using AoMMCrossModSample.Pets.SampleCustomPet;
using AoMMCrossModSample.Pets.SampleFlyingRangedPet;
using AoMMCrossModSample.Pets.SampleGroundedPet;
using AoMMCrossModSample.Pets.SampleMeleeRangedPet;
using AoMMCrossModSample.Pets.SampleMultiPet;
using AoMMCrossModSample.Pets.SampleOptionalCombatPet;
using AoMMCrossModSample.Pets.SampleRapidFirePet;
using AoMMCrossModSample.Pets.SampleSlimePet;
using AoMMCrossModSample.Projectiles;
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
                GetInstance<SampleFlyingRangedPetProjectile>(), 
                GetInstance<SampleFlyingRangedPetBuff>(), 
                ProjectileType<FrostDaggerfishCloneProjectile>());

            // Register a projectile with vanilla pet AI as a slime-style cross mod combat pet with a ranged attack
            // The final boolean paramter determines whether the slime pet will bounce at all times, or only when
            // moving
            AmuletOfManyMinionsApi.RegisterSlimePet(
                GetInstance<SampleSlimePetProjectile>(), GetInstance<SampleSlimePetBuff>(), ProjectileType<SapphireBoltCloneProjectile>(), false);

            // Apply combat pet AI to a projectile that is not a clone of a vanilla pet
            // This pet's AI also performs some small custom actions based on AoMM state
            AmuletOfManyMinionsApi.RegisterFlyingPet(
                GetInstance<SampleCustomPetProjectile>(), GetInstance<SampleCustomPetBuff>(), null, false);

            // Apply combat pet AI to a projectile with multiple summoning buffs, so that it will be a
            // regular pet when summoned with one buff and a combat pet when summoned with the other
            AmuletOfManyMinionsApi.RegisterGroundedPet(
                GetInstance<SampleOptionalCombatPetProjectile>(), GetInstance<SampleOptionalCombatPetBuff_CombatVersion>(), null);

            // Apply combat pet AI to a projectile that variably acts as a melee or ranged pet,
            // depending on the player's combat pet level. Uses GetStateDirect to determine pet level,
            // then GetParamsDirect and UpdateParamsDirect to dynamically update the fired projectile.
            AmuletOfManyMinionsApi.RegisterGroundedPet(
                GetInstance<SampleMeleeRangedPetProjectile>(), GetInstance<SampleMeleeRangedPetBuff>(), null);


            // Register two different combat pet projectiles to the same cross-mod buff
            // Spawning of both projectiles from the same buff must be handled from the buff itself
            AmuletOfManyMinionsApi.RegisterGroundedPet(
                GetInstance<SampleMultiPetGroundedProjectile>(), GetInstance<SampleMultiPetBuff>(), null);
            AmuletOfManyMinionsApi.RegisterFlyingPet(
                GetInstance<SampleMultiPetFlyingProjectile>(), GetInstance<SampleMultiPetBuff>(), null);

            // Register a combat pet that uses SetParameters to adjust the default scaling of attack speed
            // with pet level
            AmuletOfManyMinionsApi.RegisterFlyingPet(
                GetInstance<SampleRapidFirePetProjectile>(), GetInstance<SampleRapidFirePetBuff>(), ProjectileType<FrostDaggerfishCloneProjectile>());
        }

        private static void RegisterMinions()
        {
            // Register a projectile with vanilla minion AI as a grounded cross mod minion with ranged attack.
            // Need to manually specify shot projectile, search range, travel speed, movement inertia, and 
            // attack rate
            AmuletOfManyMinionsApi.RegisterGroundedMinion(
                GetInstance<SampleGroundedMinionProjectile>(),
                GetInstance<SampleGroundedMinionBuff>(),
                ProjectileType<RubyBoltCloneProjectile>(),
                800, 8, 12, 25);

            // Register a custom minion that acts on AoMM's state variables, with a search range of 800 pixels
            AmuletOfManyMinionsApi.RegisterInfoMinion(
                GetInstance<SampleCustomMinionProjectile>(), GetInstance<SampleCustomMinionBuff>(), 800);

            // Register a custom minion that acts on AoMM's state variables, but uses the default AoMM pathfinder
            AmuletOfManyMinionsApi.RegisterPathfindingMinion(
                GetInstance<SamplePathfindingMinionProjectile>(), GetInstance<SamplePathfindingMinionBuff>(), 800, 12, 18);

            // Register a managed minion that updates AoMM's behavior params based on the number of copies summoned,
            // to mimic an "empowered" minion such as Abigail or the Desert Tiger
            AmuletOfManyMinionsApi.RegisterFlyingMinion(
                GetInstance<SampleEmpoweredMinionProjectile>(), 
                GetInstance<SampleEmpoweredMinionBuff>(), 
                ProjectileType<RubyBoltCloneProjectile>(),
                800, 12, 18);

            // Register a projectile with vanilla minion AI as a grounded cross mod minion that manually
            // determines whether it should use cross mod AI based on the game state
            AmuletOfManyMinionsApi.RegisterGroundedMinion(
                GetInstance<SampleActiveToggleMinionProjectile>(), 
                GetInstance<SampleActiveToggleMinionBuff>(), 
                ProjectileType<RubyBoltCloneProjectile>(),
                800, 8, 12, 25);
        }
    }
}