using UnityEngine;
using UnityEngine.Events;

namespace Base.Input.Interfaces
{
    public interface IInputManager
    {
        void AddActionToBindingKeyUp(string binding, UnityAction action); 
        void AddActionToBindingKeyDown(string binding, UnityAction action);
        float GetAxis(string axisName);
        bool GetButton(string buttonName);
        Vector2 GetMouseVector(Vector2 relativePosition);
        void CheckForInput();
    }
}

