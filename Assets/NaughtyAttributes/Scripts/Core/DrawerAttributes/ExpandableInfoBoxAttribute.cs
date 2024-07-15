namespace NaughtyAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ExpandableInfoBoxAttribute : DrawerAttribute
    {
        public string ShortText { get; private set; }
        public string LongText { get; private set; }
        public EInfoBoxType Type { get; private set; }

        public ExpandableInfoBoxAttribute(string shortTxt, string longTxt, EInfoBoxType type = EInfoBoxType.Normal)
        {
            ShortText = shortTxt;
            LongText = longTxt;
            Type = type;
        }
    }
}