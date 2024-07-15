namespace NaughtyAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerializedCollectionAttribute : SpecialCaseDrawerAttribute
    {
        public string VariableName { get; private set; }
        
        public SerializedCollectionAttribute(string variableName)
        {
            VariableName = variableName;
        }
    }
}