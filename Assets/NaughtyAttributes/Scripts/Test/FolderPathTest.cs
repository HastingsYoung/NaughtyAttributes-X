using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class FolderPathTest : MonoBehaviour
    {
        [NaughtyAttributes.FolderPath("Assets/")]
        public string SomeRandomFolderPath = "Assets/NaughtyAttributes/Scripts/Test";
    }
}