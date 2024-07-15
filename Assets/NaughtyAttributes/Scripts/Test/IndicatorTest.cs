namespace NaughtyAttributes.Test
{
    using UnityEngine;

    public class IndicatorTest : MonoBehaviour
    {
        [Indicator] public bool Passed = true;
        [Indicator] public bool Failed = false;
    }
}