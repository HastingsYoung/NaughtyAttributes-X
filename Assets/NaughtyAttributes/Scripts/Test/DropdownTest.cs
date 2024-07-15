namespace NaughtyAttributes.Test
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DropdownTest : MonoBehaviour
    {
        [Dropdown("intValues")] public int intValue;

#pragma warning disable 414
        private int[] intValues = new int[] { 1, 2, 3 };
#pragma warning restore 414

        [Dropdown("GetIntIds")]
        public int IntId;

        [Dropdown("GetStrIds")]
        public string StringId;
        // public DropdownNest1 nest1;

        public DropdownList<int> GetIntIds() => NaughtyIdExt.GetConstIdsInt<DropdownTestDropdownIdInt>();
        public DropdownList<string> GetStrIds() => NaughtyIdExt.GetConstIdsString<DropdownTestDropdownIdStr>();
    }

    public class DropdownTestDropdownIdInt : INaughtyDropdownId<int>
    {
        public const int NORMAL = 0;
        public const int WARNING = 1;
        public const int DANGER = 2;
    }

    public class DropdownTestDropdownIdStr : INaughtyDropdownId<string>
    {
        public const string AAA = "AAA";
        public const string BBB = "BBB";
        public const string CCC = "CCC";
    }

    // [System.Serializable]
    // public class DropdownNest1
    // {
    //     [Dropdown("StringValues")] public string stringValue;
    //
    //     private List<string> StringValues
    //     {
    //         get { return new List<string>() { "A", "B", "C" }; }
    //     }
    //
    //     public DropdownNest2 nest2;
    // }
    //
    // [System.Serializable]
    // public class DropdownNest2
    // {
    //     [Dropdown("GetVectorValues")] public Vector3 vectorValue;
    //
    //     private DropdownList<Vector3> GetVectorValues()
    //     {
    //         return new DropdownList<Vector3>()
    //         {
    //             { "Right", Vector3.right },
    //             { "Up", Vector3.up },
    //             { "Forward", Vector3.forward }
    //         };
    //     }
    // }
}