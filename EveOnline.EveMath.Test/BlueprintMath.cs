using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveOnline.EveMath;

namespace EveOnline.EveMath.Test
{
    [TestClass]
    public class BlueprintMath
    {
        private struct BlueprintResearchScenario
        {
            int currentResearchLevel;
            int targetResearchLevel;
            int blueprintRank;
            double baseResearchTime;
            double facilityModifier;
            int relevantSkillLevel;
            int advancedIndustrySkillLevel;
            double implantModifier;
        }

        [TestMethod]
        public void BlueprintResearchTime()
        {

        }
    }
}
