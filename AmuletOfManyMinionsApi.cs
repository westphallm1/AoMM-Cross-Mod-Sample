using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AoMMCrossModSample
{

	/// <summary>
	/// Collection of utility methods that wrap the mod.Calls available from AoMM.
	/// </summary>
	public static class AmuletOfManyMinionsApi
	{
		/// <summary>
		/// Get the entire <key, object> mapping of the projectile's cross-mod exposed state, if it has one.
		/// Cross mod state variables are annotated with [CrossModProperty]
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		internal static Dictionary<string, object> GetState(ModProjectile proj)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return null; }
			return (Dictionary<string, object>) aomm.Call("GetState", proj);
		}

		/// <summary>
		/// Get the specified key from the projectile's cross-mod exposed state, if the key exists.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		/// <param name="key">The name of the property to read</param>
		internal static object GetStateValue(ModProjectile proj, string key)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return null; }
			return aomm.Call("GetStateValue", proj, key);
		}

		/// <summary>
		/// For the following frame, do not apply AoMM's pre-calculated position and velocity changes 
		/// to the projectile in PostAI(). Used to temporarily override behavior in fully managed minion AIs
		/// </summary>
		/// <param name="proj">The ModProjectile to release for this frame</param>
		internal static void ReleaseControl(ModProjectile proj)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("ReleaseControl", proj);
		}

		/// <summary>
		/// Register a read-only cross mod minion. AoMM will run its state calculations for this minion every frame,
		/// but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
		/// calculated state using mod.Call("GetState",this), and act on that state as it pleases.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="searchRange">The range (in pixels) over which the tactic enemy selection should search.</param>
		/// <returns></returns>
		internal static void RegisterInfoMinion(ModProjectile proj, ModBuff buff, int searchRange)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterInfoMinion", proj, buff, searchRange);
		}

		/// <summary>
		/// Register a read-only cross mod combat pet. AoMM will run its state calculations for this combat pet every frame,
		/// but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
		/// calculated state using mod.Call("GetState",this), and act on that state as it pleases.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this combat pet type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the pet</param>
		/// <returns></returns>
		internal static void RegisterInfoPet(ModProjectile proj, ModBuff buff)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterInfoPet", proj, buff);
		}

		/// <summary>
		/// Register a basic cross mod minion. AoMM will run its state calculations for this minion every frame,
		/// and take over its position and velocity while the pathfinding node is present.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="searchRange">
		/// The range (in pixels) over which the tactic enemy selection should search. AoMM will release the 
		/// minion from the pathfinding AI as soon as an enemy is detected in range.
		/// </param>
		/// <param name="travelSpeed">The speed at which the minion should travel while following the pathfinder</param>
		/// <param name="inertia">
		/// How quickly the minion should change directions while following the pathfinder. Higher values lead to
		/// slower turning.
		/// </param>
		internal static void RegisterPathfindingMinion(ModProjectile proj, ModBuff buff, int searchRange, int travelSpeed, int inertia)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterPathfindingMinion", proj, buff, searchRange, travelSpeed, inertia);
		}


		/// <summary>
		/// Register a basic cross mod combat pet. AoMM will run its state calculations for this minion every frame,
		/// and take over its position and velocity while the pathfinding node is present.
		/// The pet's movement speed and search range will automatically scale with the player's combat
		/// pet level.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// </param>
		internal static void RegisterPathfindingPet(ModProjectile proj, ModBuff buff)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterPathfindingPet", proj, buff);
		}

		/// <summary>
		/// Register a fully managed flying cross mod combat pet. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
		/// The pet's damage, movement speed, and search range will automatically scale with the player's combat
		/// pet level.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
		internal static void RegisterFlyingPet(ModProjectile proj, ModBuff buff, int? projType)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterFlyingPet", proj, buff, projType);
		}

		/// <summary>
		/// Register a fully managed flying cross mod minion. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
		/// <param name="searchRange">The range (in pixels) over which the tactic enemy selection should search.</param>
		/// <param name="travelSpeed">The speed at which the minion should travel while following the pathfinder</param>
		/// <param name="inertia">
		/// How quickly the minion should change directions while following the pathfinder. Higher values lead to
		/// slower turning.
		/// </param>
		internal static void RegisterFlyingMinion(ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterFlyingMinion", proj, buff, projType, searchRange, travelSpeed, inertia);
		}

		/// <summary>
		/// Register a fully managed grounded cross mod combat pet. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
		/// The pet's damage, movement speed, and search range will automatically scale with the player's combat
		/// pet level.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
		internal static void RegisterGroundedPet(ModProjectile proj, ModBuff buff, int? projType)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterGroundedPet", proj, buff, projType);
		}

		/// <summary>
		/// Register a fully managed grounded cross mod minion. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
		/// <param name="searchRange">The range (in pixels) over which the tactic enemy selection should search.</param>
		/// <param name="travelSpeed">The speed at which the minion should travel while following the pathfinder</param>
		/// <param name="inertia">
		/// How quickly the minion should change directions while following the pathfinder. Higher values lead to
		/// slower turning.
		/// </param>
		internal static void RegisterGroundedMinion(ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia)
		{
			if(!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
			aomm.Call("RegisterGroundedMinion", proj, buff, projType, searchRange, travelSpeed, inertia);
		}
	}
	
	/// <summary>
	/// Utility class for unpacking the Dictionary<string, object> returned by
	/// mod.Call("GetState", projectile). Also serves as documentation of the fields
	/// returned in the AoMM state.
	/// </summary>
	public class AoMMStateReader
    {
		/// <summary>
		/// Max travel speed for the minion. Updated automatically for pets, set in the 
		/// mod.Call for minions.
		/// </summary>
		public int MaxSpeed { get; set; }

		/// <summary>
		/// How quickly the minion should change directions while moving. Higher values lead to
		/// slower turning. Updated automatically for pets, set in the mod.Call for minions.
		/// </summary>
		public int Inertia { get; set; }

		/// <summary>
		/// The range (in pixels) over which the tactic enemy selection should search. Updated
		/// automatically for pets, set in the mod.Call for minions.
		/// </summary>
		public int SearchRange { get; set; }

		/// <summary>
		/// The position of the next bend in the pathfinding path, based on the minion's current
		/// position.
		/// </summary>
		public Vector2? NextPathfindingTaret { get; set; }

		/// <summary>
		/// The position of the end of the pathfinding path.
		/// </summary>
		public Vector2? PathfindingDestination { get; set; }

		/// <summary>
		/// The NPC selected as most relevant based on the minion's current tactic and search range.
		/// </summary>
		public NPC TargetNPC { get; set; }

		/// <summary>
		/// All possible NPC targets, ordered by proximity to the most relevant target.
		/// </summary>
		public List<NPC> PossibleTargetNPCs { get; set; }

		/// <summary>
		/// Whether this projectile is being treated as a combat pet.
		/// </summary>
		public bool IsPet { get; set; }

		/// <summary>
		/// The current combat pet level of the player the projectile belongs to.
		/// </summary>
		public int PetLevel { get; set; }

		/// <summary>
		/// The suggested originalDamage value for a combat pet based on the player's current combat pet level.
		/// </summary>
		public int PetDamage { get; set; }

		/// <summary>
		/// Whether AoMM expects the minion to be following the pathfinder on the current frame.
		/// </summary>
		public bool IsPathfinding { get; set; }

		/// <summary>
		/// Whether AoMM expects the minion to be attacking an enemy on the current frame.
		/// </summary>
		public bool IsAttacking { get; set; }

		/// <summary>
		/// Whether AoMM expects the minion to be idling on the current frame.
		/// </summary>
		public bool IsIdle { get; set; }

		public AoMMStateReader(Dictionary<string, object> stateDict)
        {
			// Use reflection to read the keys from the state dict into this object
			foreach (var property in GetType().GetProperties())
            {
				if(!stateDict.TryGetValue(property.Name, out var state))
                {
					continue;
                }
				property.SetValue(this, state);
            }
        }

	}

}
