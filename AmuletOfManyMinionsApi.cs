using System.Collections.Generic;
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
        /// See AoMMState.cs for the names and types of the exposed state variables.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for</param>
        internal static Dictionary<string, object> GetState(ModProjectile proj)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return null; }
            return (Dictionary<string, object>)aomm.Call("GetState", proj);
        }

        /// <summary>
        /// Attempt to fill the projectile's cross-mod exposed state directly into a destination object.
        /// The returned object will contain all AoMM state variables automatically cast to the correct type 
        /// (see AoMMState.cs).
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for.</param>
        /// <param name="destination">The object to populate the projectile's cross mod state into.</param>
        /// <returns>True if AoMM is enabled and the projectile has an AoMM state attached, false otherwise.</returns>
        internal static bool TryGetStateDirect(ModProjectile proj, out IAoMMState destination)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) 
            { 
                destination = null;
                return false; 
            }
            destination = new AoMMStateImpl();
            aomm.Call("GetStateDirect", proj, destination);
            return destination != null;
        }

        /// <summary>
        /// Get the <key, object> mapping of the parameters used to control this projectile's
        /// cross-mod behavior. See AoMMParams.cs for the names and types of these parameters.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the behavior parameters for.</param>
        internal static Dictionary<string, object> GetParams(ModProjectile proj)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return null; }
            return (Dictionary<string, object>)aomm.Call("GetParams", proj);
        }

        /// <summary>
        /// Attempt to fill the projectile's cross-mod behavior parameters directly into a destination object.
        /// The returned object will contain all AoMM parameters automatically cast to the correct type 
        /// (see AoMMParams.cs).
        /// </summary>
        /// <param name="proj">The ModProjectile to access the behavior parameters for.</param>
        /// <param name="destination">The object to populate the projectile's behavior parameters into.</param>
        /// <returns>True if AoMM is enabled and the projectile has AoMM params attached, false otherwise.</returns>
        internal static bool TryGetParamsDirect(ModProjectile proj, out IAoMMParams destination)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) 
            { 
                destination = null;
                return false; 
            }
            destination = new AoMMParamsImpl();
            aomm.Call("GetParamsDirect", proj, destination);
            return destination != null;
        }

        /// <summary>
        /// Update the parameters used to control this projectile's cross mod behaviior by passing
        /// in a <key, object> mapping of new parameter values. See AoMMParams.cs for the names and 
        /// types of these parameters.
        /// </summary>
        /// <param name="proj">The ModProjectile to update the behavior parameters for.</param>
        /// <param name="update">A dictionary containing new behavior paramter values.</param>
        internal static void UpdateParams(ModProjectile proj, Dictionary<string, object> update)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
            aomm.Call("UpdateParams", proj, update);
        }

        /// <summary>
        /// Update the parameters used to control this projectile's cross mod behaviior by passing
        /// in an object that implements the correct paramter names and types. See AoMMParams.cs for 
        /// the names and types of these parameters.
        /// </summary>
        /// <param name="proj">The ModProjectile to update the behavior parameters for.</param>
        /// <param name="update">An object containing new behavior paramter values.</param>
        internal static void UpdateParamsDirect(ModProjectile proj, IAoMMParams update)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
            aomm.Call("UpdateParamsDirect", proj, update);
        }

        /// <summary>
        /// For the following frame, do not apply AoMM's pre-calculated position and velocity changes 
        /// to the projectile in PostAI(). Used to temporarily override behavior in fully managed minion AIs
        /// </summary>
        /// <param name="proj">The ModProjectile to release for this frame</param>
        internal static void ReleaseControl(ModProjectile proj)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
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
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
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
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
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
        /// Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
        /// </param>
        /// <param name="travelSpeed">
        /// The speed at which the minion should travel while following the pathfinder
        /// Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
        /// </param>
        /// <param name="inertia">
        /// How quickly the minion should change directions while following the pathfinder. Higher values lead to
        /// slower turning.
        /// Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.
        /// </param>
        internal static void RegisterPathfindingMinion(ModProjectile proj, ModBuff buff, int searchRange, int travelSpeed, int inertia)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
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
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
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
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
            aomm.Call("RegisterFlyingPet", proj, buff, projType);
        }

        /// <summary>
        /// Register a fully managed flying cross mod minion. AoMM will take over this projectile's 
        /// AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
        /// <param name="searchRange">
        /// The range (in pixels) over which the tactic enemy selection should search.
        /// Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
        /// </param>
        /// <param name="travelSpeed">
        /// The speed at which the minion should travel.
        /// Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
        /// </param>
        /// <param name="inertia">
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning.
        /// Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.
        /// </param>
        /// <param name="attackFrames">
        /// How frequently the minion should fire a projectile, if it fires a projectile.
        /// A good frequency depends on the amount of damage done, with somewhere around 45 frames
        /// for a high damage projectile and 15 frames for a low damage projectile.
        /// </param>
        internal static void RegisterFlyingMinion(
            ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia, int attackFrames = 30)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
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
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
            aomm.Call("RegisterGroundedPet", proj, buff, projType);
        }

        /// <summary>
        /// Register a fully managed grounded cross mod minion. AoMM will take over this projectile's 
        /// AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack.</param>
        /// <param name="searchRange">
        /// The range (in pixels) over which the tactic enemy selection should search.
        /// Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
        /// </param>
        /// <param name="travelSpeed">
        /// The speed at which the minion should travel.
        /// Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
        /// </param>
        /// <param name="inertia">
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning.
        /// Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.
        /// </param>
        /// <param name="attackFrames">
        /// How frequently the minion should fire a projectile, if it fires a projectile.
        /// A good frequency depends on the amount of damage done, with somewhere around 45 frames
        /// for a high damage projectile and 15 frames for a low damage projectile.
        /// </param>
        internal static void RegisterGroundedMinion(
            ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia, int attackFrames = 30)
        {
            if (!ModLoader.TryGetMod("AmuletOfManyMinions", out Mod aomm)) { return; }
            aomm.Call("RegisterGroundedMinion", proj, buff, projType, searchRange, travelSpeed, inertia, attackFrames);
        }
    }

}
