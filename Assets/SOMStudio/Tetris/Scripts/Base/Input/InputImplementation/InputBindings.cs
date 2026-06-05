using System.Collections.Generic;
using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Input.InputImplementation
{
    public class InputBindings
    {
        protected readonly Dictionary<string, KeyCode> keyBindings = new();

        public Dictionary<string, KeyCode> KeyBindings => keyBindings;

        public InputBindings()
        {
            SetupBindings();
        }

        protected virtual void SetupBindings()
        {
        }
    }
}