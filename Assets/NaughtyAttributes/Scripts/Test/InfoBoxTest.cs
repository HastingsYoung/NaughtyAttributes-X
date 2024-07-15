using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class InfoBoxTest : MonoBehaviour
    {
        [InfoBox("Normal", EInfoBoxType.Normal)]
        public int normal;

        public InfoBoxNest1 nest1;

        [ExpandableInfoBox("Some quick hint (Expand to find out more...)",
            "You've found the gem here! Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lobortis scelerisque fermentum dui faucibus in. ",
            EInfoBoxType.Normal)]
        public int something;
        
        [ExpandableInfoBox("Some quick hint 2 (Expand to find out more...)",
            "You've found the gem here! Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lobortis scelerisque fermentum dui faucibus in. ",
            EInfoBoxType.Normal)]
        public string somethingElse;
    }

    [System.Serializable]
    public class InfoBoxNest1
    {
        [InfoBox("Warning", EInfoBoxType.Warning)]
        public int warning;

        public InfoBoxNest2 nest2;
    }

    [System.Serializable]
    public class InfoBoxNest2
    {
        [InfoBox("Error", EInfoBoxType.Error)] public int error;
    }
}