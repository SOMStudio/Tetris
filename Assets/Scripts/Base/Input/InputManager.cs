using System.Collections.Generic;
using Base.Input.InputImplementation;
using Base.Input.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Input
{
    public class InputManager : IInputManager
    {
        private InputBindings inputBindings;
        private IMouseInputHandler mouseInputHandler;
        private Dictionary<string, UnityAction> actionMapKeyUp = new Dictionary<string, UnityAction>();
        private Dictionary<string, UnityAction> actionMapKeyDown = new Dictionary<string, UnityAction>();
        
        public InputManager(InputBindings inputBindings)
        {
            this.inputBindings = inputBindings;
        }
        
        public InputManager(InputBindings inputBindings, IMouseInputHandler mouseInputHandler)
        {
            this.inputBindings = inputBindings;
            this.mouseInputHandler = mouseInputHandler;
        }

        public void AddActionToBindingKeyUp(string binding, UnityAction action)
        {
            actionMapKeyUp.Add(binding, action);
        }
        
        public void AddActionToBindingKeyDown(string binding, UnityAction action)
        {
            actionMapKeyDown.Add(binding, action);
        }

        public float GetAxis(string axisName)
        {
            return UnityEngine.Input.GetAxis(axisName);
        }

        public bool GetButton(string buttonName)
        {
            return UnityEngine.Input.GetButton(buttonName);
        }

        public Vector2 GetMouseVector(Vector2 relativePosition)
        {
            return mouseInputHandler.GetInput(relativePosition);
        }

        public void CheckForInput()
        {
            foreach (var kvp in inputBindings.KeyBindings)
            {
                if (UnityEngine.Input.GetKeyUp(kvp.Value))
                {
                    if (actionMapKeyUp.TryGetValue(kvp.Key, out var action))
                    {
                        action.Invoke();
                    }
                }
                else if (UnityEngine.Input.GetKeyDown(kvp.Value))
                {
                    if (actionMapKeyDown.TryGetValue(kvp.Key, out var action))
                    {
                        action.Invoke();
                    }
                }
            }
        }
    }
}