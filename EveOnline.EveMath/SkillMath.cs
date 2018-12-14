using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class SkillMath
    {
        // The points required to level up a skill now uses a static matrix of how many skillpoints are required according to the next level and the skill's rank, which can be found here:
        // https://eveonline-third-party-documentation.readthedocs.io/en/latest/formulas/skillpoints.html
        public static readonly int[,] RequiredSkillPointsTable =
        {
            {250, 1414, 8000, 45254, 256000}, // Rank 1
            {500, 2828, 16000, 90509, 512000}, // Rank 2
            {750, 4242, 24000, 135764, 768000}, // Rank 3
            {1000, 5656, 32000, 181019, 1024000}, // Rank 4
            {1250, 7071, 40000, 226274, 1280000}, // Rank 5
            {1500, 8485, 48000, 271529, 1536000}, // Rank 6
            {1750, 9899, 56000, 316783, 1792000}, // Rank 7
            {2000, 11313, 64000, 362038, 2048000}, // Rank 8
            {2250, 12727, 72000, 407293, 2304000}, // Rank 9
            {2500, 14142, 80000, 452548, 2560000}, // Rank 10
            {2750, 15556, 88000, 497803, 2816000}, // Rank 11
            {3000, 16970, 96000, 543058, 3072000}, // Rank 12
            {3250, 18384, 104000, 588312, 3328000}, // Rank 13
            {3500, 19798, 112000, 633567, 3584000}, // Rank 14
            {3750, 21213, 120000, 678822, 3840000}, // Rank 15
            {4000, 22627, 128000, 724077, 4096000}, // Rank 16
        };

        /// <summary>
        /// Calculate how many skill points will be requred to train a skill to a new level.
        /// </summary>
        /// <param name="skillRank">The skill's rank</param>
        /// <param name="currentSkillLevel">The current level the skill has been trained to</param>
        /// <param name="targetSkillLevel">The desired level to train the skill to</param>
        /// <returns>The total number of skill points required to level up the skill to the desired level</returns>
        public static int RequiredSkillPoints(int skillRank, int currentSkillLevel, int targetSkillLevel)
        {
            if (skillRank < 1 || skillRank > 16)
                throw new ArgumentOutOfRangeException("skillRank", "The skill rank must be a number between 1 and 16");

            if (currentSkillLevel < 0 || currentSkillLevel > 5)
                throw new ArgumentOutOfRangeException("currentSkillLevel", "The current skill level must be a number between 0 and 5");

            if (targetSkillLevel < 0 || targetSkillLevel > 5)
                throw new ArgumentOutOfRangeException("targetSkillLevel", "The target skill level must be a number between 0 and 5");

            int totalSkillPointsRequired = 0;

            // Loop through the "Skill Points Required To Upskill" table, summing up how many skillpoints it will
            // take to train a skill skill from level A to level B.
            // E.g. if training a Rank 2 skill from level 2 to level 5, it'd be:
            // Level 3 (16,000) + Level 4 (90,509) + Level 5 (512,000) = 618,509 skill points required
            for (var i=currentSkillLevel+1; i <= targetSkillLevel; i++)
            {
                totalSkillPointsRequired += RequiredSkillPointsTable[skillRank, i];
            }

            return totalSkillPointsRequired;
        }

        /// <summary>
        /// Calculate the rate that skill points will be generated each minute, depending on the required attributes of the skill being trained.
        /// </summary>
        /// <param name="primaryAttributeLevel">The primary attribute of the skill being trained</param>
        /// <param name="secondaryAtributeLevel">The secondary attribute of the skill being trained</param>
        /// <returns>The amount of skill points generated per minute</returns>
        public static decimal SkillpointsPerMinute(int primaryAttributeLevel, int secondaryAtributeLevel)
        {
            return primaryAttributeLevel + (secondaryAtributeLevel / 2);
        }
    }
}
