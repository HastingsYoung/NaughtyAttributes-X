using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ClassAvatarAttribute : ClassAttribute, IGroupAttribute
    {
        public string Name { get; private set; }
        public string Message { get; private set; }
        public string Path
        {
            get
            {
                return "Icons/" + Name + ".png";
            }
        }

        public ClassAvatarAttribute(string name, string message = "")
        {
            Name = name;
            Message = message;
        }
    }
}