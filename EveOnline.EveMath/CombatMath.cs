using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class CombatMath
    {
        /// <summary>
        /// Calculate the damage done by a missile
        /// https://wiki.eveuniversity.org/Missile_mechanics#Missile_damage_formula
        /// </summary>
        /// <param name="missileBaseDamage">The base damage of the missile, of a particular damage type</param>
        /// <param name="missileDamageReductionFactor">The Damage Reduction Factor of the missile</param>
        /// <param name="missileExplosionRadius">THe Explosion Radius of the missile</param>
        /// <param name="missileExplosionVelocity">The Explosion Velocity of the missile</param>
        /// <param name="targetSigRadius">The Signature Radius of the target</param>
        /// <param name="targetVelocity">The Velocity of the target</param>
        /// <returns>The final damage dealt by the missile impact</returns>
        public static double MissileDamageCalc(
            double missileBaseDamage,
            double missileDamageReductionFactor,
            double missileExplosionRadius,
            double missileExplosionVelocity,
            double targetSigRadius,
            double targetVelocity
            )
        {
            // To prevent doing damage greater than the missile's base damage, return the smallest out of:
            // 1 (full damage)
            // Sig Radius / Explosion Radius (signature radius is larger than the explosion radius)
            // The full calculation
            return missileBaseDamage * Math.Min(
                1, 
                Math.Min(
                    (targetSigRadius * missileExplosionRadius),
                    Math.Pow((targetSigRadius / missileExplosionRadius) * (missileExplosionVelocity / targetVelocity), missileDamageReductionFactor)
                    )
                );
        }

        /// <summary>
        /// Calculate the range of a missile.
        /// https://wiki.eveuniversity.org/Eve_Math#Missiles
        /// Note that EVE servers operate on 1-second intervals. A missile with a flight time of 12.3 seconds has
        /// a 70% chance of flying for 12 seconds and a 30% probability of flying for 13 seconds.
        /// </summary>
        /// <param name="missileFlightTime"></param>
        /// <param name="missileVelocity"></param>
        /// <returns></returns>
        public static double MissileFlightTimeCalc(double missileFlightTime, double missileVelocity)
        {
            return missileFlightTime * missileVelocity;
        }

        /// <summary>
        /// <para>Calculate the chance for a turret to hit something</para>
        /// <para>https://wiki.eveuniversity.org/Turret_Damage#Tracking</para>
        /// </summary>
        /// <param name="targetAngularVelocity">The angular velocity of the target, relative to the shooter</param>
        /// <param name="targetSigRadius">The target's signature radius</param>
        /// <param name="targetDistance">The target's distance from teh shooter</param>
        /// <param name="turretTrackingSpeed">The turret's tracking speed</param>
        /// <param name="turretOptimalRange">The turret's optimal range</param>
        /// <param name="turretFalloff">The turret's falloff range</param>
        /// <returns>The chance to hit (e.g. 0.5 = 50%)</returns>
        public static double TurretChanceToHit(
            double targetAngularVelocity,
            double targetSigRadius,
            double targetDistance,
            double turretTrackingSpeed,
            double turretOptimalRange,
            double turretFalloff)
        {
            double trackingTerm = 0.5 * ( (targetAngularVelocity * 40000) / (turretTrackingSpeed * targetSigRadius) );
            double rangeTerm = 0.5 * ( (Math.Max(0, targetDistance - turretOptimalRange) ) / turretFalloff );

            return trackingTerm * rangeTerm;
        }
    }
}
