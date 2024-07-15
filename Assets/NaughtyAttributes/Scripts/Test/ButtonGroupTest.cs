using System.Collections;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ButtonGroupTest : ButtonGroupBaseTest
    {
        [ButtonGroup("Group A", "ButtonFromChild_A", EButtonEnableMode.Always)]
        public void ButtonFromChild_A()
        {
        }

        [ButtonGroup("Group B", "ButtonFromChild_B", EButtonEnableMode.Always)]
        public void ButtonFromChild_B()
        {
        }
    }

    public class ButtonGroupBaseTest : MonoBehaviour
    {
        [ButtonGroup("Group A", "ButtonFromParent_A", EButtonEnableMode.Always)]
        public void ButtonFromParent_A()
        {
        }

        [ButtonGroup("Group B", "ButtonFromParent_B", EButtonEnableMode.Always)]
        public void ButtonFromParent_B()
        {
        }
    }
}