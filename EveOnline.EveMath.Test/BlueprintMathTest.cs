using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveOnline.EveMath;
using System.Collections.Generic;

namespace EveOnline.EveMath.Test
{
    [TestClass]
    public class BlueprintMathTest
    {
        public class BPResearchScenario
        {
            public string name;
            public double blueprintRank = 1;
            public double facilityModifier = 1;
            public int relevantSkillLevel = 0;
            public int advancedIndustrySkillLevel = 0;
            public double implantModifier = 1;

            public BPResearchScenarioEntry[] expectedResults;
        }

        public class BPResearchScenarioEntry
        {
            public BPResearchScenarioEntry(int StartLevel, int EndLevel, double ExpectedResults)
            {
                startLevel = StartLevel;
                endLevel = EndLevel;
                expectedResults = ExpectedResults;
            }
            public int startLevel;
            public int endLevel;
            public double expectedResults;
        }

        [TestMethod]
        public void BlueprintResearchTimeBasicMk2()
        {
            // Expected results calculated from http://www.eve-cost.eu/research

            List<BPResearchScenario> scenarios = new List<BPResearchScenario>();

            // Level 0 to 10, Rank 1 (T1 small ammo), nothing else
            scenarios.Add(new BPResearchScenario()
                {
                    name = "Rank 1 (T1 Small Ammo), nothing else",
                    blueprintRank = 1,
                    facilityModifier = 1,
                    relevantSkillLevel = 0,
                    advancedIndustrySkillLevel = 0,
                    implantModifier = 1,
                    expectedResults = new BPResearchScenarioEntry[]
                    {
                        new BPResearchScenarioEntry(0, 1, 105),
                        new BPResearchScenarioEntry(0, 2, 250),
                        new BPResearchScenarioEntry(0, 3, 595),
                        new BPResearchScenarioEntry(0, 4, 1414),
                        new BPResearchScenarioEntry(0, 5, 3360),
                        new BPResearchScenarioEntry(0, 6, 8000),
                        new BPResearchScenarioEntry(0, 7, 19000),
                        new BPResearchScenarioEntry(0, 8, 45255),
                        new BPResearchScenarioEntry(0, 9, 107700),
                        new BPResearchScenarioEntry(0, 10, 256000)
                    }
                }
            );

            // Level 5 to 10, Rank 1 (T1 small ammo), nothing else
            scenarios.Add(new BPResearchScenario()
                {
                    name = "Rank 1 (T1 Small Ammo), research mid way, nothing else",
                    blueprintRank = 1,
                    facilityModifier = 1,
                    relevantSkillLevel = 0,
                    advancedIndustrySkillLevel = 0,
                    implantModifier = 1,
                    expectedResults = new BPResearchScenarioEntry[]
                    {
                        new BPResearchScenarioEntry(5, 6, 4640),
                        new BPResearchScenarioEntry(5, 7, 15640),
                        new BPResearchScenarioEntry(5, 8, 41895),
                        new BPResearchScenarioEntry(5, 9, 104340),
                        new BPResearchScenarioEntry(5, 10, 252640)
                    }
                }
            );

            // Level 0 to 10, Rank 7.8 (T2 small modules), nothing else
            scenarios.Add(new BPResearchScenario() { 
                name = "Rank 7.8 (T2 small modules), nothing else",
                blueprintRank = 7.8,
                facilityModifier = 1,
                relevantSkillLevel = 0,
                advancedIndustrySkillLevel = 0,
                implantModifier = 1,
                expectedResults = new BPResearchScenarioEntry[]
                {
                    new BPResearchScenarioEntry(0, 1, 819),
                    new BPResearchScenarioEntry(0, 2, 1950),
                    new BPResearchScenarioEntry(0, 3, 4641),
                    new BPResearchScenarioEntry(0, 4, 11029.2),
                    new BPResearchScenarioEntry(0, 5, 26208),
                    new BPResearchScenarioEntry(0, 6, 62400),
                    new BPResearchScenarioEntry(0, 7, 148200),
                    new BPResearchScenarioEntry(0, 8, 352989),
                    new BPResearchScenarioEntry(0, 9, 840060),
                    new BPResearchScenarioEntry(0, 10, 1996800)
                }
            });

            // Level 0 to 10, Rank 7.8 (T2 small modules), relevant skill 0, adv industry 5
            scenarios.Add(new BPResearchScenario()
            {
                name = "Rank 7.8 (T2 small modules), adv industry 5",
                blueprintRank = 7.8,
                facilityModifier = 1,
                relevantSkillLevel = 0,
                advancedIndustrySkillLevel = 5,
                implantModifier = 1,
                expectedResults = new BPResearchScenarioEntry[]
                {
                    new BPResearchScenarioEntry(0, 1, 696),
                    new BPResearchScenarioEntry(0, 2, 1658),
                    new BPResearchScenarioEntry(0, 3, 3945),
                    new BPResearchScenarioEntry(0, 4, 9375),
                    new BPResearchScenarioEntry(0, 5, 22277),
                    new BPResearchScenarioEntry(0, 6, 53040),
                    new BPResearchScenarioEntry(0, 7, 125970),
                    new BPResearchScenarioEntry(0, 8, 300041),
                    new BPResearchScenarioEntry(0, 9, 714051),
                    new BPResearchScenarioEntry(0, 10, 1697280)
                }
            });




            // ============== Run them
            foreach (BPResearchScenario scenario in scenarios)
            {
                foreach (BPResearchScenarioEntry entry in scenario.expectedResults)
                {
                    double result = BlueprintMath.BlueprintResearchTime(
                        entry.startLevel,
                        entry.endLevel,
                        scenario.blueprintRank,
                        scenario.facilityModifier,
                        scenario.relevantSkillLevel,
                        scenario.advancedIndustrySkillLevel,
                        scenario.implantModifier);

                    Assert.AreEqual(
                        entry.expectedResults,
                        result,
                        1,
                        string.Format("Name: '{0}', from {1} - {2}", scenario.name, entry.startLevel, entry.endLevel)
                        );
                }
            }
        }
    }
}
