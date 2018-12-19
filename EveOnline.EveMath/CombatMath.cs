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
            // How likely it is to hit the target with how fast the target is moving in relation to the ship doing the shooting
            double trackingTerm = 0.5 * ( (targetAngularVelocity * 40000) / (turretTrackingSpeed * targetSigRadius) );

            // How likely it is to hit the target at the distance it is
            // 100% within optimal range
            // about 50% at optimal + under half of falloff
            // about 6.5% @ optimal + over half of falloff
            // about 0.2% @ optimal + falloff
            double rangeTerm = 0.5 * ( (Math.Max(0, targetDistance - turretOptimalRange) ) / turretFalloff );

            return trackingTerm * rangeTerm;
        }

        /// <summary>
        /// Get a rough estimate of Effective Hit Points; the ship's hit points (shield, armor, or hull) with your resistance applied.
        /// </summary>
        /// <example>1000 shields, 50% EM res., 60% Therm res. 60% Exp res., 70% Kin res., 60% averaged shield resistance. 
        /// EffectiveHitPointsSimple(1000, 0.5, 0.6, 0.6, 0.7) = 1500 EHP</example>
        /// <param name="baseHP">The total hit points of shields, armor, or hull</param>
        /// <param name="resistEM">The EM resistance modifier</param>
        /// <param name="resistExp">The Explosive resistance modifier</param>
        /// <param name="resistTherm">The Thermal resistance modifier</param>
        /// <param name="resistKin">The Kinetic resistance modifier</param>
        /// <returns></returns>
        public static double EffectiveHitPoints(
            double baseHP,
            double resistEM,
            double resistExp,
            double resistTherm,
            double resistKin
            )
        {
            double averageResistance = (resistEM + resistExp + resistTherm + resistKin) / 4; //Get your average resistance
            return baseHP * (1 + averageResistance);
        }

        /// <summary>
        /// Get an estimate of Effective Hit Points; the ship's hit points (shield, armor, or hull) with your resistance applied.
        /// </summary>
        /// <param name="baseHP">The total hit points of shields, armor, or hull</param>
        /// <param name="resistEM">The EM resistance modifier</param>
        /// <param name="resistExp">The Explosive resistance modifier</param>
        /// <param name="resistTherm">The Thermal resistance modifier</param>
        /// <param name="resistKin">The Kinetic resistance modifier</param>
        /// <param name="attackEM">The percentage of incoming EM damage</param>
        /// <param name="attackExp">The percentage of incoming Explosive damage</param>
        /// <param name="attackTherm">The percentage of incoming Thermal damage</param>
        /// <param name="attackKin">The percentage of incoming Kinetic damage</param>
        /// <returns></returns>
        public static double EffectiveHitPoints(
            double baseHP,
            double resistEM,
            double resistExp,
            double resistTherm,
            double resistKin,
            double attackEM,
            double attackExp,
            double attackTherm,
            double attackKin
            )
        {
            return baseHP * 
                (1 + resistEM - attackEM) *
                (1 + resistExp - attackExp) *
                (1 + resistTherm - attackTherm) *
                (1 + resistKin - attackKin);
        }

        // https://forums.eveonline.com/t/targeting-time-locking-time-calculation/91133
        // =40000/(ScanRes*ASINH(SigRadius)^2)
        /// <summary>
        /// Calculate how long in seconds it will take for a ship to lock a target, in seconds.
        /// Due to how EVE's servers operate in 'ticks', time is rounded to the next second (e.g. 3.36s becomes 4s).
        /// </summary>
        /// <param name="shipScanResolution">The targetting resolution of the ship doing the targetting, in millimetres</param>
        /// <param name="targetSignatureRadius">The signature resolution of thie ship being targetted, in metres</param>
        /// <returns></returns>
        public static int LockTime(
            int shipScanResolution,
            int targetSignatureRadius
            )
        {
            // The C# Math library doesn't have a function for ArcSinH, so we'll calculate it ourselves
            // If you're doing this equation in Excel or anothe language which has asinh, just use that
            // https://www.codeproject.com/Articles/86805/An-introduction-to-numerical-programming-in-C
            // asinh(x) = log(x + sqrt(x2 + 1))
            double sigRadiusAsinh = Math.Log(targetSignatureRadius + Math.Sqrt(Math.Pow(targetSignatureRadius, 2) + 1));

            return (int)Math.Ceiling(40000 / (shipScanResolution * Math.Pow(sigRadiusAsinh, 2)));
        }

        #region EWarAndLogistics

        // Most of the EWar math comes from
        // https://wiki.eveuniversity.org/Electronic_warfare

        // Most of the logistics information comes from
        // https://wiki.eveuniversity.org/Guide_to_Logistics
        // https://forums-archive.eveonline.com/message/6306658/

        /// <summary>
        /// <para>Get the probability of an ECM jammer sucessfully jamming a target, based on the target's distance & sensor power vs the jammer's range and strength.</para>
        /// <para>When an ECM jammer cycles, it "rolls the die" against this probability of success.</para>
        /// </summary>
        /// <param name="jammerStrength">The ECM jammer's strength against the target's sensor type, after bonuses and range effects are applied.</param>
        /// <param name="targetSensorStrength">The target's sensor strength, after bonuses are applied.</param>
        /// <param name="jammerAccuracyFalloff">The jammer's effective accuracy falloff distance.</param>
        /// <param name="jammerOptimalRange">The jammer's optimal range.</param>
        /// <param name="targetDistance">The target's distance from the jamming ship.</param>
        /// <returns>The probabilty of the ECM jammer successfully jamming the target, as a number between 0 and 1.</returns>
        public static double EcmChanceToJam(
            double jammerStrength,
            double jammerOptimalRange,
            double jammerAccuracyFalloff,
            double targetSensorStrength,
            double targetDistance
            )
        {
            // Jammer strength vs the target's sensor strength
            double strengthTerm = jammerStrength / targetSensorStrength;

            //How strong or weak the jammer is because of range
            double rangeTerm = EWarAndLogisticsEffectiveness(jammerOptimalRange, jammerAccuracyFalloff, targetDistance);

            return 1 * strengthTerm * rangeTerm;
        }

        /// <summary>
        /// <para>Calculate the effectiveness modifier, depending on the module's range and the targets distance.</para>
        /// <para>This applies to electronic warfare modules (e.g. target painters, tracking disruptors, sensor dampeners) as well as logistics modules (e.g. remote repair, sensor booter).</para>
        /// <para>Note that it uses a similar calculation to turret range and accuracy, however angular velocity and signature radius has no effect.</para>
        /// <para>For ECM jmmers, use <seealso cref="EcmChanceToJam"./></para>
        /// </summary>
        /// <param name="moduleOptimalRange">The module's effective optimal range.</param>
        /// <param name="moduleAccuracyFalloff">The module's effective accuracy falloff distance.</param>
        /// <param name="targetDistance">The targets distance from the ship.</param>
        /// <returns>The effectiveness of the module, as a number between 0 and 1.</returns>
        public static double EWarAndLogisticsEffectiveness(
            double moduleOptimalRange,
            double moduleAccuracyFalloff,
            double targetDistance
            )
        {
            //How strong or weak the module's effect is because of range
            // 100% within optimal range
            // about 50% at optimal + under half of falloff
            // about 6.5% @ optimal + over half of falloff
            // about 0.2% @ optimal + falloff
            return 0.5 * ((Math.Max(0, targetDistance - moduleOptimalRange)) / moduleAccuracyFalloff);
        }
        

        #endregion
    }
}
