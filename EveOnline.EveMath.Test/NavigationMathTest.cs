using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveOnline.EveMath;
using System.Collections.Generic;

namespace EveOnline.EveMath.Test
{
    // Test results are from the testing grid provided by EVE Online
    // https://www.eveonline.com/article/warp-drive-active

    [TestClass]
    public class NavigationMathTest
    {
        [TestMethod]
        public void WarpDurationFrigate()
        {
            var warpSpeed = 5;
            var subwarpSpeed = 200;

            var testCases = new WarpDurationCase[] {
                new WarpDurationCase()
                {
                    Distance = 150000,
                    ExpectedDuration = 7,
                    Description = "150 km"
                },
                new WarpDurationCase()
                {
                    Distance = 1000000000,
                    ExpectedDuration = 14,
                    Description = "1,000,000 km"
                },
                new WarpDurationCase()
                {
                    Distance = 1 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 18,
                    Description = "1 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 2 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 19,
                    Description = "2 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 5 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 19,
                    Description = "5 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 10 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 20,
                    Description = "10 AU"
                },
            };

            DoTestCases(testCases, warpSpeed, subwarpSpeed);
        }

        [TestMethod]
        public void WarpDurationCruiserIndustrial()
        {
            var warpSpeed = 3;
            var subwarpSpeed = 200;

            var testCases = new WarpDurationCase[] {
                new WarpDurationCase()
                {
                    Distance = 150000,
                    ExpectedDuration = 11,
                    Description = "150 km"
                },
                new WarpDurationCase()
                {
                    Distance = 1000000000,
                    ExpectedDuration = 23,
                    Description = "1,000,000 km"
                },
                new WarpDurationCase()
                {
                    Distance = 1 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 29,
                    Description = "1 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 2 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 30,
                    Description = "2 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 5 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 32,
                    Description = "5 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 10 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 33,
                    Description = "10 AU"
                },
            };

            DoTestCases(testCases, warpSpeed, subwarpSpeed);
        }

        [TestMethod]
        public void WarpDurationInterceptor()
        {
            var warpSpeed = 8;
            var subwarpSpeed = 200;

            var testCases = new WarpDurationCase[] {
                new WarpDurationCase()
                {
                    Distance = 150000,
                    ExpectedDuration = 6,
                    Description = "150 km"
                },
                new WarpDurationCase()
                {
                    Distance = 1000000000,
                    ExpectedDuration = 11,
                    Description = "1,000,000 km"
                },
                new WarpDurationCase()
                {
                    Distance = 1 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 14,
                    Description = "1 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 2 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 15,
                    Description = "2 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 5 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 15,
                    Description = "5 AU"
                },
                new WarpDurationCase()
                {
                    Distance = 10 * NavigationMath.AU_TO_M,
                    ExpectedDuration = 16,
                    Description = "10 AU"
                },
            };

            DoTestCases(testCases, warpSpeed, subwarpSpeed);
        }

        private void DoTestCases(
            WarpDurationCase[] testCases,
            decimal shipWarpSpeed,
            decimal subwarpSpeed
            )
        {
            foreach (var test in testCases)
            {
                var testResult = NavigationMath.WarpDuration(
                        shipWarpSpeed,
                        subwarpSpeed,
                        test.Distance
                        );
                Assert.AreEqual(
                    test.ExpectedDuration,
                    testResult,
                    string.Format("Failed test: {0}", test.Description)
                    );
            }
        }

        private class WarpDurationCase
        {
            public decimal Distance { get; set; }
            public int ExpectedDuration { get; set; }
            public string Description { get; set; }
        }
    }
}
