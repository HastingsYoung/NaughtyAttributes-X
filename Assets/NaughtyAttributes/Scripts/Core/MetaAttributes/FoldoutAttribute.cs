using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FoldoutAttribute : MetaAttribute, IGroupAttribute
    {
        public string Name { get; private set; }

        public int SortOrder { get; private set; }

        public FoldoutAttribute(string name, int sortOrder = 0)
        {
            Name = name;
            SortOrder = sortOrder;
        }
    }
}