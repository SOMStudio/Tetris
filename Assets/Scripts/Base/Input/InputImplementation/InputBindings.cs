using System.Collections.Generic;
using UnityEngine;

namespace Base.Input.InputImplementation
{
    public class InputBindings
    {
        protected Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();

        public Dictionary<string, KeyCode> KeyBindings
        {
            get { return keyBindings; }
        }

        public InputBindings()
        {
            SetupBindings();
        }

        protected virtual void SetupBindings()
        {
        }
    }
}