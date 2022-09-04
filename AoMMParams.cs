using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoMMCrossModSample
{
    /// <summary>
    /// Interface containing the names and types of the parameters used to determine the 
    /// behavior of managed minions and combat pets. These parameters are initially set in
    /// the registration mod.Call("RegisterXPet",...) or mod.Call("RegisterXMinion", ...).
    /// 
    /// An object that implements this interface can be populated directly with a projectile's
    /// current AoMM parameters using mod.Call("GetParamsDirect", projectile, paramsImpl).  
    /// 
    /// The AI parameters of an active projectile can be updated to match an object that implements
    /// this interface using mod.Call("UpdateParamsDirect", projectile, paramsImpl).  
    /// 
    /// An additional interface is provided below for parameters that are only relevant to minions,
    /// as they are updated automatically for combat pets based on the player's pet level.
    /// </summary>
    public interface IAoMMCombatPetParams
    {
        /// <summary>
        /// The projectile that the minion or pet fires. If null, the minion will use a
        /// melee attack.
        /// </summary>
        int? FiredProjectileId { get; set; }
    }

    /// <summary>
    /// Interface containing the names and types of the parameters used to determine the 
    /// behavior of managed minions and combat pets. These parameters are initially set in
    /// the registration mod.Call("RegisterXMinion",...).
    /// 
    /// An object that implements this interface can be populated directly with a projectile's
    /// current AoMM parameters using mod.Call("GetParamsDirect", projectile, paramsImpl).  
    /// 
    /// The AI parameters of an active projectile can be updated to match an object that implements
    /// this interface using mod.Call("UpdateParamsDirect", projectile, paramsImpl).  
    ///
    /// The values in this interface can only be updated for minions, as they are updated automatically
    /// for pets.
    /// </summary>
    public interface IAoMMParams : IAoMMCombatPetParams
    {
        /// <summary>
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning. 
        /// </summary>
        int Inertia { get; set; }

        /// <summary>
        /// Max travel speed for the minion.
        /// </summary>
        int MaxSpeed { get; set; }

        /// <summary>
        /// The range (in pixels) over which the tactic enemy selection should search.
        /// </summary>
        int SearchRange { get; set; }

        /// <summary>
        /// The projectile firing rate for the minion, if that minion fires a projectile. Only
        /// applies to projectile-firing minions. The attack speed of melee minions is derived
        /// from their movement speed.
        /// </summary>
        int AttackFrames { get; set; }
    }

    public class AoMMParamsImpl : IAoMMParams
    {
        public int Inertia { get; set; }
        public int MaxSpeed { get; set; }
        public int SearchRange { get; set; }
        public int AttackFrames { get; set; }
        public int? FiredProjectileId { get; set; }
    }
}
