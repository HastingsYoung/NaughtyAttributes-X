namespace NaughtyAttributes.Editor
{
    using UnityEngine;
    public static class EditorPathUtil
    {
        public static string ConvertPathRelativeToAssets(string absPath)
        {
            if (absPath.StartsWith(Application.dataPath))
            {
                return "Assets" + absPath.Substring(Application.dataPath.Length);
            }
            return absPath;
        }
    }
}