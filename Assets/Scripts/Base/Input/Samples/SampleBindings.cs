using Base.Input.InputImplementation;
using UnityEngine;

namespace Base.Input.Samples
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