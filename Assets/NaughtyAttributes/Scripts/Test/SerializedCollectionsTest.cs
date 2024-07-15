namespace NaughtyAttributes.Test
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SerializedCollectionsTest : MonoBehaviour
    {
        [SerializedCollection("realHashSet")] public List<string> strSet;

        [SerializedCollection("realDictionary")]
        public List<string> strDict;

        private HashSet<string> realHashSet = new HashSet<string>()
        {
            "Apple",
            "Orange",
            "Pear"
        };

        private Dictionary<string, int> realDictionary = new Dictionary<string, int>()
        {
            ["Book"] = 10,
            ["Juice"] = 20
        };
    }
}