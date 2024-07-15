namespace NaughtyAttributes
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FilePathAttribute : DrawerAttribute
    {
        public string DefaultParent = "Assets";
        public string Extensions = "txt";

        public FilePathAttribute()
        {
        }

        public FilePathAttribute(string parent, string extensions)
        {
            DefaultParent = parent;
            Extensions = extensions;
        }
    }
}