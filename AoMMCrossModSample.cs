using AoMMCrossModSample.Pets.SampleCustomPet;
using AoMMCrossModSample.Pets.SampleFlyingRangedPet;
using AoMMCrossModSample.Pets.SampleGroundedPet;
using AoMMCrossModSample.Pets.SampleMeleeRangedPet;
using AoMMCrossModSample.Pets.SampleMultiPet;
using AoMMCrossModSample.Pets.SampleOptionalCombatPet;
using AoMMCrossModSample.Pets.SampleRapidFirePet;
using AoMMCrossModSample.Pets.SampleSlimePet;
using AoMMCrossModSample.Pets.SampleTurretPet;
using AoMMCrossModSample.Projectiles;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample
{
	public class AoMMCrossModSample : Mod
	{
		public override void PostSetupContent()
		{
			RegisterPets();
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
			AmuletOfManyMinionsApi.RegisterSlimePet(
				GetInstance<SampleSlimePetProjectile>(), GetInstance<SampleSlimePetBuff>(), ProjectileType<SapphireBoltCloneProjectile>());

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

			// Register a flying combat pet that uses mod.Calls to implement a number of custom movement
			// and projectile firing behaviors. `projType = 0` is used to specify that the projectile
			// firing behavior should be managed in-mod, rather than by AoMM
			AmuletOfManyMinionsApi.RegisterFlyingPet(
				GetInstance<SampleTurretPetProjectile>(), GetInstance<SampleTurretPetBuff>(), 0, false);
		}
	}
}