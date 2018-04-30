using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class FittingMath
    {
        //https://wiki.eveuniversity.org/Eve_Math#Stacking_Penalties

        [Obsolete("Warning: the math here is a little off from what EVE Online uses.")]
        public static double StackPenaltyModifier(int stackLevel)
        {
            return 0.5 * Math.Pow(0.45 * (stackLevel - 1), 2);
        }

        /// <summary>
        /// Table of stacking penalties.
        /// </summary>
        public static double[] StackPenalty =
        {
            0.0, // First is always fully effective
            0.13, // ~87% effective
            0.43, // ~57% effective
            0.72, // ~28% effective
            0.90, // ~10% effective
            0.97 // ~3% effective
        };

        /// <summary>
        /// Calculate the bonus effectivity stacking penalty for a stacked item.
        /// E.g. the 3rd Ballistic Control System in a stack will only be X effective.
        /// </summary>
        /// <example>3rd Ballistic Control module 7% bonus): StackPenaltyCalc(0.07, 3) = 0.0399</example>
        /// <param name="bonus">The bonus that will incur the penalty</param>
        /// <param name="stackIndex">The bonus's location within the stack</param>
        /// <returns>The bonus, with the stacking penalty applied</returns>
        public static double StackPenaltyCalc(double bonus, int stackIndex)
        {
            return bonus * (1 - StackPenalty[stackIndex]);
        }

        /// <summary>
        /// Calculate the bonus sum with a stacking penalty applied.
        /// </summary>
        /// <example>3 Ballistic Control modules (7% bonus): SumPenaltyCalc(0.07, 3) = 0.1708</example>
        /// <param name="bonus">The bonus provided by each item / module</param>
        /// <param name="numberOfBonuses">The number of items / modules in the stack</param>
        /// <returns>The total sum bonus provided, with stacking penalty applied</returns>
        public static double StackPentaltyCalc(double bonus, int numberOfBonuses)
        {
            double r = 0;

            for (int i = 1; i <= numberOfBonuses; i++)
            {
                r += bonus * StackPenaltyCalc(bonus, i);
            }

            return r;
        }

        /// <summary>
        /// Calculate the total resistance for a damage type.
        /// https://wiki.eveuniversity.org/Eve_Math#Resistances
        /// </summary>
        /// <example>Base EM resistance (-10%), 1x Adaptive Invulnerability Field (-25% each): ResistanceCalc(new double[]{0.1, 0.5}) = 0.6 (40% resistance)</example>
        /// <param name="resistances">Array of decimal resistances (25% = 0.25)</param>
        /// <returns>Resistance modifier that'll be applied to incoming damage of that type</returns>
        public static double ResistanceCalc(double[] resistances)
        {
            double r = 0;
            for (int i = 0; i < resistances.Count(); i++)
            {
                if (i == 0)
                {
                    r = (1 - resistances[i]); // First one
                }
                else
                {
                    r = r * (1 - resistances[i]);
                }
            }

            return r;
        }

        /// <summary>
        /// Cumulative sum of applied bonuses.
        /// </summary>
        /// <example>Skill armor bonus (20%) and 1x Layered Plating (8%): PositiveBonusCalc(new double[]{1.20, 1.08}) = 1.296</example>
        /// <param name="bonuses">Array of bonuses of the same type</param>
        /// <returns>The sum of the positive bonuses</returns>
        public static double PositiveBonusCalc(double[] bonuses)
        {
            double r = 0;

            foreach (double bonus in bonuses)
            {
                r = r * bonus;
            }

            return r;
        }
    }
}
