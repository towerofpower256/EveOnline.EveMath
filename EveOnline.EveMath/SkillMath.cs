using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveOnline.EveMath
{
    public static class SkillMath
    {
        /* Broken
        public static decimal SkillPointsNeededPerSkillLevel(int skillLevel, int skillRank)
        {
            return 2 * (2.5 * (skillLevel - 1) ) * 250 * skillRank
        }
        */

        public static decimal SkillpointsPerMinute(int primaryAttributeLevel, int secondaryAtributeLevel)
        {
            return primaryAttributeLevel + (secondaryAtributeLevel / 2);
        }
    }
}
