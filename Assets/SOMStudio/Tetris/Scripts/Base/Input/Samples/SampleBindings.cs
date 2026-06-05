using SOMStudio.Tetris.Scripts.Base.Input.InputImplementation;
using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Input.Samples
{
    public class SampleBindings : InputBindings
    {
        protected override void SetupBindings()
        {
            base.SetupBindings();
            keyBindings.Add("shoot", KeyCode.Mouse0);
        }
    }
}