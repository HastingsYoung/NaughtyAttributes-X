using System;

namespace NaughtyAttributes
{
    public sealed class ButtonEnableIfEditMode : ButtonAttribute
    {
        public ButtonEnableIfEditMode( string text ) : base( text, EButtonEnableMode.Editor )
        {
        }

        public ButtonEnableIfEditMode() : this( null )
        {
        }
    }
}