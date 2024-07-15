using System;

namespace NaughtyAttributes
{
    public sealed class ButtonEnableIfPlayMode : ButtonAttribute
    {
        public ButtonEnableIfPlayMode(string text) : base(text, EButtonEnableMode.Playmode)
        {
        }

        public ButtonEnableIfPlayMode() : this(null)
        {
        }
    }
}