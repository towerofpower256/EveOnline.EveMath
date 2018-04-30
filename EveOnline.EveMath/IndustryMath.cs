using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class IndustryMath
    {
        /// <summary>
        /// Calculate the item reprocessing rate / modifier for a facility.
        /// </summary>
        /// <param name="facilityModifier">The base modifier at the station / facility</param>
        /// <param name="scrapMetalProcessingSkillLevel">The trained skill level of Scrap Metal Reprocessing</param>
        /// <param name="stationTax">The tax rate of the station</param>
        /// <returns></returns>
        public static double ReprocessingRate(
            double facilityModifier,
            double scrapMetalProcessingSkillLevel,
            double stationTax
            )
        {
            return facilityModifier * (1 + 0.02 * scrapMetalProcessingSkillLevel) * (1 - stationTax);
        }

        /// <summary>
        /// Calculate the ore reprocessing rate / modifier for a facility.
        /// </summary>
        /// <param name="facilityModifier">The base modifier at the station / facility</param>
        /// <param name="stationTax">The tax rate of the station</param>
        /// <param name="reprocessingLevel">The level of the skill Reprocessing</param>
        /// <param name="reprocessingEfficiencyLevel">The level of the skill Reprocessing Effeciency</param>
        /// <param name="oreSpecificSkillLevel">The level of the ore-specific refining skill</param>
        /// <param name="implantModifier">Any bonuses granted by implants</param>
        /// <returns></returns>
        public static double OreProcessingRate(
            double facilityModifier,
            double stationTax,
            int reprocessingLevel,
            int reprocessingEfficiencyLevel,
            int oreSpecificSkillLevel,
            double implantModifier
            )
        {
            return facilityModifier *
                (1 + 0.03 * reprocessingLevel) *
                (1 + 0.02 * reprocessingEfficiencyLevel) *
                (1 + 0.02 * oreSpecificSkillLevel) *
                implantModifier *
                (1 - stationTax);
        }

        /// <summary>
        /// Calculate the station reprocessing / refining tax rate
        /// </summary>
        /// <param name="corporationStanding">Your effective standing with the owner corporation of the station</param>
        /// <returns></returns>
        public static double StationTax(double corporationStanding)
        {
            return 0.05 - (0.0075 * corporationStanding);
        }

        

        /// <summary>
        /// Calculate the fee to submit a manufacturing job
        /// </summary>
        /// <param name="baseJobCost">The base run cost of the item</param>
        /// <param name="systemCostIndex">The Cost Index of the system containing the manufacturing factility</param>
        /// <param name="runs">How many runs this job will complete</param>
        /// <returns>The cost to submit the manufacturing job</returns>
        public static double ManufactureJobFee(
            double baseJobCost,
            double systemCostIndex,
            int runs
            )
        {
            return baseJobCost * systemCostIndex * runs;
        }

        /// <summary>
        /// Calculate the required materials per run per material
        /// </summary>
        /// <param name="baseQuantity">The base quantity of a particular material required by the blueprint, per run</param>
        /// <param name="runs">How many runs this job will complete</param>
        /// <param name="materialModifier">The product of the ME modifier and the Facility's material modifier</param>
        /// <returns></returns>
        public static double ManufactureJobMaterial(
            double baseQuantity,
            int runs,
            double materialModifier
            )
        {
            return
                Math.Max(runs, //Minumim material count should be at least the number of runs. E.g. if building 3 shuttles, always cost at least 3 tritanium.
                    Math.Ceiling( Math.Round( runs * baseQuantity * materialModifier, 2 ) ) // Round the result to 2 decimal places, then round up to an integer
                );
        }

        /// <summary>
        /// Calculate how long manufacturing job will take to complete, in minutes
        /// </summary>
        /// <param name="baseProductionTime">The blueprint's base time each run will take</param>
        /// <param name="timeModifier">The time modifier is a product of the TE modifier and the facility's modifier</param>
        /// <param name="skillModifier">The product of all the modifiers provided by the relevant skills, usually in T2 manufacturing</param>
        /// <param name="runs"></param>
        /// <returns></returns>
        public static double ManufactureJobTime(
            double baseProductionTime,
            double timeModifier,
            double skillModifier,
            int runs
            )
        {
            return baseProductionTime * timeModifier * skillModifier * runs;
        }

        /// <summary>
        /// Calculate the manufacture time modifier granted by a character's skills and implants
        /// https://wiki.eveuniversity.org/Manufacturing
        /// </summary>
        /// <param name="industrySkill">The rank of the Industry skill, 4% per level</param>
        /// <param name="advancedIndustrySkill">The rank of the Advanced Industry skill, 3% per level</param>
        /// <param name="t2Skill">The rank of the relevant T2 Manufacturing skill, 1% per level</param>
        /// <param name="implantModifier">Any modifiers granted by implants</param>
        /// <returns>The sumarised modifier granted</returns>
        public static double ManufactureSkillModifier(
            int industrySkill,
            int advancedIndustrySkill = 0,
            int t2Skill = 0,
            double implantModifier = 1
            )
        {
            return 1 * (1 - (0.04 * industrySkill)) * (1 - (0.03 * advancedIndustrySkill)) * (1 - (0.01 * t2Skill)) * implantModifier;
        }

        /// <summary>
        /// Calculates the materials modifier granted by a blueprint's Material Effeciency rating, which is 1% per level up to 10%.
        /// https://wiki.eveuniversity.org/Industry
        /// </summary>
        /// <param name="meRating"></param>
        /// <returns>The materials modifier granted by the ME rating</returns>
        public static double MaterialEffeciencyModifier(int meRating)
        {
            if (meRating < 0) meRating = 0;
            if (meRating > 10) meRating = 10;

            return 1 - (meRating * 0.01);
        }

        /// <summary>
        /// Calculates the time effeciency modifier granted by a blueprint's Time Effeciency rating, which is 2% per level up to 20%.
        /// https://wiki.eveuniversity.org/Industry
        /// </summary>
        /// <param name="teRating"></param>
        /// <returns>The time modifier granted by the TE rating</returns>
        public static double TimeEffeciencyModifier(int teRating)
        {
            if (teRating < 0) teRating = 0;
            if (teRating > 10) teRating = 10;

            return 1 - (teRating * 0.02);
        }


    }
}
