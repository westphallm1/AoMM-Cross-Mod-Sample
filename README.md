# Amulet of Many Minions Cross Mod Sample

## Overview
This repository contains a small example mod that utilizes Amulet of Many Minions' (AoMM) cross-mod
API. This API allows minions and pets added by other mods to utilize AoMM's pathfinding tools,
select enemies to attack based on AoMM's target selection tactics, and, in the case of pets, automatically
scale in damage and travel speed based on the player's combat pet level.

## Mod Structure

The sample mod's files are located in the following sub-directories. Each file demonstrates
a different portion of the cross mod API:

* `AmuletOfManyMinionsApi.cs`: Contains convenience wrappers for every `mod.Call` option in AoMM, as well
  as documentation for those calls. Documentation is also provided below in the README.

* `AoMMState.cs`: Contains an Interface that matches the value names and types returned by AoMM's state-getter call 
  (`mod.Call("GetStateDirect", projectile, output)`), as well as documentation of those fields, and a basic implementation of that interface. 
  Documentation for the fields in the interface is also provided below in the README.

* `AoMMCrossModSample.cs`: Contains all of the `mod.Call`s used to register the sample minions and pets for cross-mod AI.

* `Pets/SampleGroundedPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Turtle pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by `mod.Call("RegisterGroundedPet", ...)`.

* `Pets/SampleFlyingPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Zephyr Fish pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by the `mod.Call("RegisterFlyingPet", ...)`.

* `Pets/SampleCustomPet/`: Contains the code for a non-pet projectile that is turned into a combat pet via 
  `mod.Call("RegisterFlyingPet", ...)`. Also includes a basic example of using `mod.Call("GetState",...)` to act on
  AoMM state variables retrieved as a dictionary.

* `Minions/SampleGroundedMinion/`: Contains the boilerplate code for a minion that behaves like the vanilla vampire frog pet. No cross-mod 
  specific code exists in this directory. The cross-mod minion behavior is managed entirely by `mod.Call("RegisterGroundedMinion", ...)`.

* `Minions/SamplePathfindingMinion/`: Contains the code for a minion that implements custom behavior based on AoMM's state variables,
  while using AoMM's default pathfinder-following code. Also includes an example of using `mod.Call("GetStateDirect", ...)` to access
  AoMM's state variables directly as an object.

* `Minions/SampleCustomMinion/`: Contains the code for a minion that implements custom behavior based on AoMM's state variables,
  while implementing custom code for following the pathfinder. Also includes an example of using `mod.Call("GetStateDirect", ...)` 
  to access AoMM's state variables directly as an object.

* `Projectiles/`: Contains clones of a few vanilla projectiles, with their `SetDefaults` updated to properly set the flags for a
	minion-shot projectile. These projectiles are assigned as minion attacks in several `mod.Calls` (see below).

## Mod.Call Documentation

AoMM provides the following mod.Calls:

* `mod.Call("GetState", ModProjectile proj) -> Dictionary<string, object>`  
  Get the entire <key, object> mapping of the projectile's cross-mod exposed state.
  * `proj`: The active instance of the projectile whose state should be retrieved

* `mod.Call("GetStateDirect", ModProjectile proj, object destination) -> void`  
	Fill the projectile's cross-mod exposed state directly into a destination object.
	The destination object should either explicitly or implicitly implement IAoMMState (see AoMMState.cs).
  * `proj`: The active instance of the projectile whose state should be retrieved
  * `destination`: An object that implements IAoMMState. Its fields will be overridden with the projectile's AoMM managed state via reflection.

* `mod.Call("ReleaseControl", ModProjectile proj) -> void`  
	For the following frame, do not apply AoMM's pre-calculated position and velocity changes 
	to the projectile in PostAI(). Used to temporarily override behavior in fully managed minion AIs.
  * `proj`: The active instance of the projectile whose state should be retrieved

* `mod.Call("RegisterInfoMinion", ModProjectile proj, ModBuff buff, int searchRange) -> void`  
	Register a read-only cross mod minion. AoMM will run its state calculations for this minion every frame,
	but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
	calculated state using `mod.Call("GetState",this)`, and act on that state in any way.
  * `proj`: The singleton content instance of the ModProjectile (as retrieved via `ModContent.GetInstance`)
  * `buff`: The singleton content instance of the ModBuff (as retrieved via `ModContent.GetInstance`) that's applied when the minion is summoned. 
  * `searchRange`: The range (in pixels) over which the tactic enemy selection should search.

* `mod.Call("RegisterInfoPet", ModProjectile proj, ModBuff buff) -> void`  
	Register a read-only cross mod combat pet. AoMM will run its state calculations for this combat pet every frame,
	but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
	calculated state using `mod.Call("GetState",this)`, and act on that state in any way.
  * `proj`: The singleton content instance of the ModProjectile (as retrieved via `ModContent.GetInstance`)
  * `buff`: The singleton content instance of the ModBuff (as retrieved via `ModContent.GetInstance`) that's applied when the minion is summoned. 

* `mod.Call("RegisterPathfindingMinion", ModProjectile proj, ModBuff buff, int searchRange, int travelSpeed, int inertia) -> void`  
	Register a basic cross mod minion. AoMM will run its state calculations for this minion every frame,
	and take over its position and velocity while the pathfinder is present.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `searchRange`: 
		The range (in pixels) over which the tactic enemy selection should search.
		Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
	* `travelSpeed`: 
		The speed at which the minion should travel while following the pathfinder.
		Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
	* `inertia`: 
		How quickly the minion should change directions while following the pathfinder. 
		Higher values lead to slower turning.
		Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.

* `mod.Call("RegisterPathfindingPet", ModProjectile proj, ModBuff buff) -> void`  
	Register a basic cross mod combat pet. AoMM will run its state calculations for this minion every frame,
	and take over its position and velocity while the pathfinding node is present.
	The pet's movement speed and search range will automatically scale with the player's combat
	pet level.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion

* `mod.Call("RegisterFlyingPet", ModProjectile proj, ModBuff buff, int? projType) -> void`  
	Register a fully managed flying cross mod combat pet. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
	The pet's damage, movement speed, and search range will automatically scale with the player's combat
	pet level.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack

* `mod.Call("RegisterFlyingMinion", ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia) -> void`  
	Register a fully managed flying cross mod minion. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack
	* `searchRange`: 
		The range (in pixels) over which the tactic enemy selection should search.
		Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
	* `travelSpeed`: 
		The speed at which the minion should travel.
		Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
	* `inertia`: 
		How quickly the minion should change directions. Higher values lead to slower turning.
		Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.

* `mod.Call("RegisterGroundedPet", ModProjectile proj, ModBuff buff, int? projType) -> void`
	Register a fully managed grounded cross mod combat pet. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
	The pet's damage, movement speed, and search range will automatically scale with the player's combat
	pet level.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack

* `mod.Call("RegisterGroundedMinion", ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia) -> void`  
	Register a fully managed grounded cross mod minion. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack
	* `searchRange`: 
		The range (in pixels) over which the tactic enemy selection should search.
		Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
	* `travelSpeed`: 
		The speed at which the minion should travel.
		Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
	* `inertia`: 
		How quickly the minion should change directions. Higher values lead to slower turning.
		Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.

## Amulet of Many Minions cross-mod State Documentation

While a minion is registered to AoMM via `mod.Call("RegisterX",...)`, AoMM will perform a variety of state
calculations each frame in that ModProjectile's `PreAI` hook. These values can be retrieved as a dictionary
via `mod.Call("GetState", projectile)`, or copied directly into an object that contains any subset of the
state properties via `mod.Call("GetStateDirect", projectile, destination)`. The state values that are calculated,
and their types, are documented below:

* `int Inertia`: 
  How quickly the minion should change directions while moving. Higher values lead to
  slower turning. Updated automatically for pets, set in the "Register..." `mod.Call` for minions.

* `bool IsAttacking`: Whether AoMM has determined that an enemy is available for the minion to attack. True
when a nonzero number of enemy NPCs are found within range of the minion for its current tactic, and the minion is
not in the middle of following the pathfinder.

* `bool IsIdle`: Whether AoMM has determined the minion should idle by the player's head. True when no enemies are detected, 
and the pathfinder is not present.

* `bool IsPathfinding`: Whether AoMM has determined that the minion should be following the pathfinder. True while the pathfinder
is present, and the minion has either not completed the pathfinding route, or has completed the route but has no enemies available to attack.

* `bool IsPet`: Whether the minion is being treated as a combat pet.

* `int MaxSpeed`: Max travel speed for the minion. Updated automatically for pets, set in the "Register..." `mod.Call` for minions.

* `int MaxSpeed`: Max travel speed for the minion. Updated automatically for pets, set in the `mod.Call` for minions.

* `Vector2? NextPathfindingTarget`: The position of the next bend in the pathfinding path, based on the minion's current position.
`null` if the pathfinder is not present.

* `Vector2? PathfindingDestination`: The position of the end of the pathfinding path. `null` if the pathfinder is not present.

* `int PetDamage`: The suggested originalDamage value for a combat pet based on the player's current combat pet level.

* `int PetLevel`: The current combat pet level of the player the projectile belongs to.

* `List<NPC> PossibleTargetNPCs`: All possible NPC targets, ordered by proximity to the most relevant target.

* `int SearchRange`: The range (in pixels) over which the tactic enemy selection should search. 
Updated automatically for pets, set in the "Register..." `mod.Call` for minions.

* `NPC TargetNPC`: The NPC selected as most relevant based on the minion's current tactic and search range.
`null` if none are available.