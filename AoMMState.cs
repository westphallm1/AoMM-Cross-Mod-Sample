using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace AoMMCrossModSample
{
    /// <summary>
    /// Interface containing the names and types of the variables in the AoMM state.
    /// An object that implements this interface can be populated directly with a projectile's
    /// current AoMM state using mod.Call("GetStateDirect", projectile, stateImpl).  
    /// </summary>
    public interface IAoMMState
    {
        /// <summary>
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning. Updated automatically for pets, set in the mod.Call for minions.
        /// </summary>
        int Inertia { get; set; }

        /// <summary>
        /// Whether AoMM expects the minion to be attacking an enemy on the current frame.
        /// </summary>
        bool IsAttacking { get; set; }

        /// <summary>
        /// Whether AoMM expects the minion to be idling on the current frame.
        /// </summary>
        bool IsIdle { get; set; }

        /// <summary>
        /// Whether AoMM expects the minion to be following the pathfinder on the current frame.
        /// </summary>
        bool IsPathfinding { get; set; }

        /// <summary>
        /// Whether this projectile is being treated as a combat pet.
        /// </summary>
        bool IsPet { get; set; }

        /// <summary>
        /// Max travel speed for the minion. Updated automatically for pets, set in the 
        /// mod.Call for minions.
        /// </summary>
        int MaxSpeed { get; set; }

        /// <summary>
        /// The position of the next bend in the pathfinding path, based on the minion's current
        /// position.
        /// </summary>
        Vector2? NextPathfindingTaret { get; set; }

        /// <summary>
        /// The position of the end of the pathfinding path.
        /// </summary>
        Vector2? PathfindingDestination { get; set; }

        /// <summary>
        /// The suggested originalDamage value for a combat pet based on the player's current combat pet level.
        /// </summary>
        int PetDamage { get; set; }

        /// <summary>
        /// The current combat pet level of the player the projectile belongs to.
        /// </summary>
        int PetLevel { get; set; }

        /// <summary>
        /// All possible NPC targets, ordered by proximity to the most relevant target.
        /// </summary>
        List<NPC> PossibleTargetNPCs { get; set; }

        /// <summary>
        /// The range (in pixels) over which the tactic enemy selection should search. Updated
        /// automatically for pets, set in the mod.Call for minions.
        /// </summary>
        int SearchRange { get; set; }

        /// <summary>
        /// The NPC selected as most relevant based on the minion's current tactic and search range.
        /// </summary>
        NPC TargetNPC { get; set; }
    }


    /// <summary>
    /// Utility class for accessing the AoMM state directly via
    /// mod.Call("GetStateDirect", projectile, stateImpl).  
    /// </summary>
    public class AoMMStateImpl : IAoMMState
    {
        public int MaxSpeed { get; set; }
        public int Inertia { get; set; }
        public int SearchRange { get; set; }
        public Vector2? NextPathfindingTaret { get; set; }
        public Vector2? PathfindingDestination { get; set; }
        public NPC TargetNPC { get; set; }
        public List<NPC> PossibleTargetNPCs { get; set; }
        public bool IsPet { get; set; }
        public int PetLevel { get; set; }
        public int PetDamage { get; set; }
        public bool IsPathfinding { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsIdle { get; set; }
    }

}
