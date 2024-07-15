namespace NaughtyAttributes.Test
{
    using UnityEngine;

    public class TabGroupTest : MonoBehaviour
    {
        [TabGroup("Integers")] public int int0;
        [TabGroup("Integers")] public int int1;

        [TabGroup("Floats")] public float float0;
        [TabGroup("Floats")] public float float1;

        [TabGroup("Sliders")] [MinMaxSlider(0, 1)]
        public Vector2 slider0;

        [TabGroup("Sliders")] [MinMaxSlider(0, 1)]
        public Vector2 slider1;

        public string str0;
        public string str1;

        [TabGroup] public Transform trans0;
        [TabGroup] public Transform trans1;
        
        [TabGroup("Priority", 10)]
        public int tabFirstPriority;
        [TabGroup("Priority", 5)]
        public int tabSecondPriority;
    }
}