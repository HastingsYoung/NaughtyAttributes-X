using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class FilePathTest : MonoBehaviour
    {
        [NaughtyAttributes.FilePath("Assets/", "cs")]
        public string SomeRandomScriptPath = "Assets/NaughtyAttributes/Scripts/Test";
    }
}