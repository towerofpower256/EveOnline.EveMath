using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class InventionMath
    {
        /// <summary>
        /// Calculate how long a blueprint copy job will take to complete
        /// </summary>
        /// <param name="baseCopyTime">The base copy time of the blueprint</param>
        /// <param name="copyRuns">How many copys this job will produce</param>
        /// <param name="runsPerCopy">How many licensed runs each copy will provide</param>
        /// <param name="timeModifier">Any time modifiers applicable (e.g. facility, implants, etc)</param>
        /// <returns>The time the copy job will take to complete, in minutes</returns>
        public static double BlueprintCopyTime(
            double baseCopyTime,
            int copys,
            int runsPerCopy,
            double timeModifier
            )
        {
            return baseCopyTime * copys * runsPerCopy * timeModifier;
        }

        /// <summary>
        /// Calculate the cost of a copy job
        /// </summary>
        /// <param name="baseJobCost">The base copy cost of the blueprint</param>
        /// <param name="systemCostIndex">The Cost Index of the system containing the copy facility</param>
        /// <param name="copys">How many copys the job will produce</param>
        /// <param name="runsPerCopy">How many licensed runs each copy will provide</param>
        /// <returns>The cost to submit the copy job</returns>
        public static double BlueprintCopyFee(
            double baseJobCost,
            double systemCostIndex,
            int copys,
            int runsPerCopy
            )
        {
            return baseJobCost * systemCostIndex * 0.02 * copys * runsPerCopy;
        }

        /// <summary>
        /// The seconds added 
        /// </summary>
        public static int[] BlueprintResearchLevelModifiers =
        {
            0, // Start at 1
            105,
            250,
            595,
            1414,
            3360,
            8000,
            19000,
            45255,
            107700,
            256000
        };

        /// <summary>
        /// Minutes it'll take to complete a research job, upgrading either the TE or ME rank of a blueprint from 
        /// <paramref name="currentResearchLevel"/> to <paramref name="targetResearchLevel"/>
        /// https://wiki.eveuniversity.org/Research#How_long_does_research_take.3F
        /// </summary>
        /// <param name="currentResearchLevel">The current TE or ME rank of the blueprint</param>
        /// <param name="targetResearchLevel">The TE or ME rank that you'd like to research up to in this job</param>
        /// <param name="blueprintRank">The (hidden) rank of the blueprint</param>
        /// <param name="baseResearchTime">The base research time of the blueprint</param>
        /// <param name="facilityModifier">The modifier granted by the facility</param>
        /// <param name="relevantSkillLevel">The skill level of Research for Time Effeciency or Metallurgy for Material Effeciency</param>
        /// <param name="advancedIndustrySkillLevel">The level of the Advanced Industry skill</param>
        /// <param name="implantModifier">Any reduction modifiers granted by implants</param>
        /// <returns>The time (in minutes) it'll take to complete a TE or ME research job</returns>
        public static double BlueprintResearchTime(
            int currentResearchLevel,
            int targetResearchLevel,
            int blueprintRank,
            double baseResearchTime,
            double facilityModifier,
            int relevantSkillLevel,
            int advancedIndustrySkillLevel,
            double implantModifier
            )
        {
            return baseResearchTime * facilityModifier *
                (1 - (0.05 * relevantSkillLevel)) * (1 - (0.03 * advancedIndustrySkillLevel)) * implantModifier *
                (BlueprintResearchLevelModifiers[targetResearchLevel] - BlueprintResearchLevelModifiers[currentResearchLevel]); // E.g. level 4 (595) - level 2 (250)
        }

        /// <summary>
        /// The cost for submitting a blueprint research job, upgrading either the TE or ME rank of a blueprint from
        /// <paramref name="currentResearchLevel"/> to <paramref name="targetResearchLevel"/>
        /// </summary>
        /// <param name="currentResearchLevel">The current TE or ME rank of the blueprint</param>
        /// <param name="targetResearchLevel">The TE or ME rank that you'd like to research up to in this job</param>
        /// <param name="blueprintRank">The (hidden) rank of the blueprint</param>
        /// <param name="baseJobCost">The base research cost of the blueprint</param>
        /// <param name="systemCostIndex">The System Cost Index</param>
        /// <returns></returns>
        public static double BlueprintResearchCost(
            int currentResearchLevel,
            int targetResearchLevel,
            int blueprintRank,
            double baseJobCost,
            double systemCostIndex
            )
        {
            return baseJobCost * (systemCostIndex * 0.02) *
                (BlueprintResearchLevelModifiers[targetResearchLevel] - BlueprintResearchLevelModifiers[currentResearchLevel]); // E.g. level 4 (595) - level 2 (250)
        }


        /// <summary>
        /// The chance of sucess when attempting invention, or creating a T2 blueprint
        /// https://wiki.eveuniversity.org/Invention
        /// </summary>
        /// <remarks>The <paramref name="baseChance"/> should be one of the below:
        /// All modules, rigs, ammo, and all intact Ancient Relics = 34%
        /// All Frigates, Destroyers, and all malfunctioning Ancient Relics = 30%
        /// All Cruisers, Battlecruisers, Mining Barges, and Industrials = 26%
        /// All Battleships and all wrecked Ancient Relics = 22%
        /// Freighters = 18%
        /// Tutorial perpetual motion unit = 100%
        /// </remarks>
        /// <param name="baseChance">The base success chance of the blueprint</param>
        /// <param name="relevantEncryptionSkill">Level of the relevant Encryption skill</param>
        /// <param name="relevantSkill1">Level of 1st relevant Science skill</param>
        /// <param name="relevantSkill2">Level of 2nd relevant Science skill</param>
        /// <param name="decryptorModifier">Any modifier added by the decryptor used</param>
        /// <returns></returns>
        public static double BlueprintInventionSuccessChance (
            double baseChance,
            int relevantEncryptionSkill,
            int relevantSkill1,
            int relevantSkill2,
            double decryptorModifier
            )
        {
            double skillModifier = 1 + (relevantEncryptionSkill / 40) + ((relevantSkill1 + relevantSkill2) / 30);
            return baseChance * skillModifier * decryptorModifier;
        }
        
        /// <summary>
        /// How long an invention job will take to complete
        /// </summary>
        /// <param name="baseInventionTime">The base invention time of the blueprint</param>
        /// <param name="facilityModifier">Any modifier granted by the research facility </param>
        /// <param name="advancedIndustryLevel">The level of the Advanced Industry skill, reduces time by 3% per level</param>
        /// <returns></returns>
        public static double BlueprintInventionTime(
            double baseInventionTime,
            double facilityModifier,
            int advancedIndustryLevel
            )
        {
            return baseInventionTime * facilityModifier * (1 - (0.03 * advancedIndustryLevel));
        }

        /// <summary>
        /// The cost of starting an invention job, or creating a T2 blueprint
        /// </summary>
        /// <param name="baseJobCost">The base job cost of the output blueprint</param>
        /// <param name="systemCostIndex">The System Cost Index</param>
        /// <param name="runs">How many runs or attempts in this job</param>
        /// <returns></returns>
        public static double BlueprintInventionFee (
            double baseJobCost,
            double systemCostIndex,
            int runs
            )
        {
            return baseJobCost * (systemCostIndex * 0.02) * runs;
        }
    }
}
