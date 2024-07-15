using System;
using UnityEngine;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IndicatorAttribute : DrawerAttribute
    {
        public EColor TrueColor { get; private set; }
        public EColor FalseColor { get; private set; }
        public IndicatorAttribute(EColor color1, EColor color2)
        {
            TrueColor = color1;
            FalseColor = color2;
        }
        public IndicatorAttribute(EColor color1) : this(color1, EColor.Red)
        {
        }

        public IndicatorAttribute() : this(EColor.Green)
        {
        }
    }
}
