namespace NaughtyAttributes
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FolderPathAttribute : DrawerAttribute
    {
        public string DefaultPath = "Assets";

        public FolderPathAttribute()
        {
        }

        public FolderPathAttribute(string path)
        {
            DefaultPath = path;
        }
    }
}