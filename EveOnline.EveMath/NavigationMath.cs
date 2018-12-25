using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class NavigationMath
    {
        /// <summary>
        /// Number of meters (M) in a an astronomical unit (AU).
        /// </summary>
        public const long AU_TO_M = 149597870700;
        
        /// <summary>
        /// Number of meters (M) in a light year (LY).
        /// </summary>
        public const long LY_TO_M = 9460730472580800;

        /// <summary>
        /// <para>Determine if a system is within range, measured by light years.</para>
        /// <para>Useful for caculating if a system is within jump range or remote scanning range.</para>
        /// <para>Note that the XYZ coordinates for systems in EVE are accurate down to the meter, and are typically very large numbers.</para>
        /// </summary>
        /// <param name="currentSystemX">The X part of the current system's coordinates.</param>
        /// <param name="currentSystemY">The Y part of the current system's coordinates.</param>
        /// <param name="currentSystemZ">The Z part of the current system's coordinates.</param>
        /// <param name="targetSystemX">The X part of the target system's coordinates.</param>
        /// <param name="targetSystemY">The Y part of the target system's coordinates.</param>
        /// <param name="targetSystemZ">The Z part of the target system's coordinates.</param>
        /// <param name="lightYearRange">The range of whatever you're trying to do, in light years.</param>
        /// <returns>Returns if the target system is within range of whatever you're trying to do from the current system's position.</returns>
        public static bool IsSystemWithinRange(
            decimal currentSystemX,
            decimal currentSystemY,
            decimal currentSystemZ,
            decimal targetSystemX,
            decimal targetSystemY,
            decimal targetSystemZ,
            decimal lightYearRange
            )
        {
            // Convert the range from ligth years to meters.
            decimal rangeInM = lightYearRange * LY_TO_M;

            // Get the distance from the current system to the target system.
            // Remember, these numbers are going to be really big!
            decimal distance = Math.Abs(currentSystemX - targetSystemX)
                + Math.Abs(currentSystemY - targetSystemY)
                + Math.Abs(currentSystemZ - targetSystemZ);

            return (distance <= rangeInM); // Is the distance between the 2 systems less than or equal to the range.
        }

        // https://eveonline-third-party-documentation.readthedocs.io/en/latest/formulas/combat.html
        // "The ship alignment time (talign) depends on the ship’s inertia modifier (i) and the ships mass (m)"
        // Basically, it'll calculate how long it'll take you from a full stop to entering warp.
        // This does not take into account if you're already pointing in that direction, that should usually push you straight into warp.

        // EveUni says "Base Time to Warp is essentially the time needed for this ship to align and accelerate
        //    until it reaches 75% of its top speed and goes to warp. The time displayed here is the base calculated time
        //     with no account for any warp related skills, modules or any other effects."

        /// <summary>
        /// Calculate the time it would take to align for a warp from a complete stand-still. Note that things are different if you're already pointing the direction that you want to warp.
        /// </summary>
        /// <param name="shipInertiaModifier">The ship's effective inertial modifier, taking into accounts modules, skills, etc.</param>
        /// <param name="shipMass">The ship's effective mass, taking into accounts modules, skills, etc.</param>
        /// <returns>The time in seconds it would take to align before warping.</returns>
        public static double AlignToWarpTime(double shipInertiaModifier, double shipMass)
        {
            return (Math.Log(2) * shipInertiaModifier * shipMass) / 500000;
        }

        #region WarpCalculations

        // Calculate how long a warp trip will take
        // 1. Ship enters warp and accelerates to maximum warp speed
        // 2. Ship travels at maximum warp speed (if distance allows accelertaion, could decelerate before reaching max speed)
        // 3. Ship decelerates and exits warp

        // https://wiki.eveuniversity.org/Warp_time_calculation
        // https://wiki.eveuniversity.org/Warp_mechanics
        // https://www.eveonline.com/article/warp-drive-active

        // Total warp time = acceleration phase + cruising phase + deceleration phase

        // For the acceleration, the accelaration speed is the max warp speed
        // For the deceleration, the deceleration speed is the max warp speed divided by 3, or 2, whichever is larger
        //     That max of 2 is to stop ludicrusly fast ships going from warp speed to subwarp speed 
        //     in less time than the server can handle.

        /// <summary>
        /// Calculate how long a warp will take, from entering warp to exiting warp.
        /// </summary>
        /// <param name="shipMaxWarpSpeedAu">The ship's maximum warp speed in AU/s</param>
        /// <param name="shipMaxSubwarpSpeed">The ship's maximum subwarp speed, which will determine the speed it will exit warp at</param>
        /// <param name="warpDistance">The distance of the position that the ship is warping to, in metres</param>
        /// <returns>The time a warp will take in seconds</returns>
        public static decimal WarpDuration(
            decimal shipMaxWarpSpeedAu,
            decimal shipMaxSubwarpSpeed,
            decimal warpDistance
            )
        {
            // The warp acceleration speed will always be the ship's max warp speed (AU/s)
            decimal warpAccelerationSpeed = shipMaxWarpSpeedAu;

            // The warp deceleration speed will always be the ship's max warp speed devided by 3 AU/s, or 2 AU/s, whatever is larger
            // This is to prevent fast warping ships going from "very far away" to "locking down your battleship"
            //    faster than the server can handle.
            decimal warpDecelerationSpeed = Math.Min(shipMaxWarpSpeedAu / 3, 2);

            // Get the max warp speed in meters per second, we'll need it later
            decimal shipMaxWarpSpeedMetres = shipMaxWarpSpeedAu * AU_TO_M;

            // This is the speed in m/s that the ship will exit warp upon reaching this speed.
            // Apparently, all ships exit warp when their speed drops below 100 m/s
            decimal warpDropoutSpeed = Math.Min(shipMaxSubwarpSpeed / 2, 100);

            // Distance required to accelerate to maximum warp speed (is this right? this doesn't feel right.)
            decimal accelerationDistance = AU_TO_M;

            // Distance required to descelerate from warp, in metres
            decimal descelerationDistance = shipMaxWarpSpeedMetres / warpDecelerationSpeed;

            // Minimum distance to reach maximum warp speed.
            // Anything below this is a "short warp" and this ship won't be able to reach its maximum warp speed.
            decimal minimumDistance = accelerationDistance + descelerationDistance;

            // Cruise Time is the amount of time in seconds that the ship will cruise at its maximum warp speed.
            decimal cruiseTime = 0;

            // Is the warp distance shorter than the minimum distance to reach maximum speed?
            // Is this going to be a "short warp"?
            if (minimumDistance > warpDistance)
            {
                // Short warp
                // The ship won't reach its maximum warp speed, it'll reach this top speed
                shipMaxWarpSpeedMetres = warpDistance * warpAccelerationSpeed * warpDecelerationSpeed / (warpAccelerationSpeed + warpDecelerationSpeed);

            }
            else
            {
                // Not a short warp, will involve cruising at maximum warp speed
                // (Warp distance - acceleration and deceleration distant) / maximum warp speed per second
                cruiseTime = (warpDistance - minimumDistance) / shipMaxWarpSpeedMetres;
            }

            // Time in seconds that the ship will spend accelerating
            // C#'s Math function doesn't do decimal / super large numbers, so we push it down to a double, and then back to a decimal
            decimal accelerationTime = (decimal)(Math.Log((double)shipMaxWarpSpeedMetres / (double)warpAccelerationSpeed) / (double)warpAccelerationSpeed);

            // Time in seconds that the ship will spend decelerating
            decimal decelerationTime = (decimal)(Math.Log((double)shipMaxWarpSpeedMetres / (double)warpDropoutSpeed) / (double)warpDecelerationSpeed);

            // The total time in seconds that the ship will be in warp
            decimal totalWarpTime = accelerationTime + cruiseTime + decelerationTime;

            // Due to how the server works in ticks, ceiling the result to the next tick
            return Math.Ceiling(totalWarpTime);
        }

        #endregion
    }
}
