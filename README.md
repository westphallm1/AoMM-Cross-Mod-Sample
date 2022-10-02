# Amulet of Many Minions Cross Mod Sample

## Overview
This repository contains a small example mod that utilizes [Amulet of Many Minions' (AoMM)](https://github.com/westphallm1/tModLoader_Minions) cross-mod
API. This API allows minions and pets added by other mods to utilize AoMM's pathfinding tools,
select enemies to attack based on AoMM's target selection tactics, and, in the case of pets, automatically
scale in damage and travel speed based on the player's combat pet level.

## Requirements
* Make sure your build.txt has `loadAfter = AmuletOfManyMinions` added to it to ensure proper mod load order, as some calls depend on that order.

* Make sure you do nothing unconventional regarding spawning of your pets - Always apply your buff before the pet projectile spawns.

## Mod Structure

The sample mod's files are located in the following sub-directories. Each file demonstrates
a different portion of the cross mod API:

### General cross-mod utilities

* `AmuletOfManyMinionsApi.cs`: Contains all the things you need in your mod - convenience wrappers for every `mod.Call` option in AoMM, helper classes and interfaces, as well as documentation for those. Documentation is also provided below in the README.
    * IAoMMState: An Interface that matches the value names and types returned by AoMM's state-getter call 
  (`mod.Call("GetStateDirect", versionString, projectile, output)`), as well as documentation of those fields, and a basic implementation of that interface (AoMMStateImpl). Documentation for the fields in the interface is also provided below in the README.
    * IAoMMParams: An Interface that matches the value names and types returned by AoMM's params-getter call 
  (`mod.Call("GetParamsDirect", versionString, projectile, destination)`), as well as documentation of those fields, and a basic implementation of that interface (AoMMParamsImpl). Documentation for the fields in the interface is also provided below in the README.

* `AoMMCrossModSample.cs`: Contains all of the `mod.Call`s used to register the sample minions and pets for cross-mod AI.

### Cross-mod Combat Pet examples

* `Pets/SampleGroundedPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Turtle pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by `mod.Call("RegisterGroundedPet", ...)`.

* `Pets/SampleFlyingPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Zephyr Fish pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by the `mod.Call("RegisterFlyingPet", ...)`.

* `Pets/SampleSlimePet/`: Contains the boilerplate code for a pet that behaves like the vanilla Slime Prince pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by the `mod.Call("RegisterSlimePet", ...)`.

* `Pets/SampleWormPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Destroyer Lite pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by the `mod.Call("RegisterWormPet", ...)`.

* `Pets/SampleMultiPet/`: Contains the boilerplate code for a pet item that spawns multiple pet projectiles. Contains a small amount
  of cross-mod specific code, which scales the dual combat pets' damage down slightly to bring them more into balance with
  single projectile combat pets. The cross-mod combat pet behavior is managed by adding a `mod.Call("RegisterGroundedPet", ...)`
  for each type of pet projectile that the pet item spawns.

* `Pets/SampleCustomPet/`: Contains the code for a non-pet projectile that is turned into a combat pet via 
  `mod.Call("RegisterFlyingPet", ...)`. Also includes a basic example of using `mod.Call("GetState",...)` to act on
  AoMM state variables retrieved as a dictionary.

* `Pets/SampleOptionalCombatPetProjectile/`: Contains the code for a pet projectile with two pet buffs, one of which is registered with 
  `mod.Call(...)` and one of which isn't. The pet will behave as a regular pet when summoned via the non-registered buff, and as a combat 
  pet when summoned via the registered buff. This can be used to achieve an effect similar to the " (AoMM Version)" of pet items in the 
  base mod.

* `Pets/SampleMeleeRangedPet/`: Contains the code for a pet projectile that alternates between melee and ranged behavior depending
  on the player's combat pet level. Uses `mod.Call("GetStateDirect", ...)` to determine the player's combat pet level each frame, then
  `mod.Call("GetParamsDirect", ...)` and `mod.Call("UpdateParamsDirect", ...)` to toggle between melee and ranged behavior.

* `Pets/SampleRapidFirePet/`: Contains the code for a pet projectile that moves and attacks more quickly than the
  default cross-mod AI. Uses `mod.Call("UpdateParamsDirect", ...)` once when the projectile spawns to set custom scale factors
  for travel speed and attack rate. The cross-mod AI then scales its own behavior parameters, determined by the player's combat
  pet level, by these scale factors.

* `Pets/SampleTurretPet/`: Contains code demonstrating an advanced use case for a pet projectile that implements custom behavior 
  by overriding certain behaviors of the managed flying cross-mod AI. Uses `mod.Call("RegisterFlyingPet", ...)` to initially register 
  the projectile as a managed flying pet, then a combination of `mod.Call("ReleaseControl",...)` and `mod.Call("GetStateDirect",...)` to
  implement custom movement and projectile launching behavior on top of the default flying cross-mod AI.

### Cross-mod Minion examples

* `Minions/SampleGroundedMinion/`: Contains the boilerplate code for a minion that behaves like the vanilla vampire frog pet. No cross-mod 
  specific code exists in this directory. The cross-mod minion behavior is managed entirely by `mod.Call("RegisterGroundedMinion", ...)`.

* `Minions/SamplePathfindingMinion/`: Contains the code for a minion that implements custom behavior based on AoMM's state variables,
  while using AoMM's default pathfinder-following code. Also includes an example of using `mod.Call("GetStateDirect", ...)` to access
  AoMM's state variables directly as an object.

* `Minions/SampleCustomMinion/`: Contains the code for a minion that implements custom behavior based on AoMM's state variables,
  while implementing custom code for following the pathfinder. Also includes an example of using `mod.Call("GetStateDirect", ...)` 
  to access AoMM's state variables directly as an object.

* `Minions/SampleEmpoweredMinion/`: Contains the code for a minion that behaves like Abigail or the Desert tiger, summoning a single minion
  that gets more powerful with each minion slot used. Uses `mod.Call("GetParamsDirect", ...)` and `mod.Call("UpdateParamsDirect", ...)` to 
  dynamically update the minion's travel speed, search range, and attack rate based on the number of copies summoned. Also contains an
  example of manually enabling cross-mod AI with the `IsActive` flag in `UpdateParamsDirect`, since the specific spawn conditions for 
  the empowered minion prevent AoMM from enabling it by default.

* `Minions/SampleActiveToggleMinion/`: Contains the code for a minion that behaves like the vanilla vampire pet frog, and switches
  between its default AI and the managed grounded cross mod AI based on the count of minions summoned. This is not a particularly
  practical scenario for cross mod AI behavior, but demonstrates an advanced feature of the cross mod AI.

### Miscellaneous examples

* `Projectiles/`: Contains clones of a few vanilla projectiles, with their `SetDefaults` updated to properly set the flags for a
  minion-shot projectile. These projectiles are assigned as minion attacks in several `mod.Calls` (see below). 

* `Projectiles/FrostDaggerfishCloneProjectile.cs`: Example of using `ModProjectile.OnSpawn(IEntitySource source)` to alter 
  projectile behavior based on a cross-mod minion's state. The projectile spawning code for cross-mod minions is fairly 
  inflexible, so any adjustments must be made after the projectile is spawned.
