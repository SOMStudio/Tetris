using UnityEngine;

namespace Base.Input.Interfaces
{
    public interface IMouseInputHandler
    {
        Vector2 GetRawPosition();
        Vector2 GetInput(Vector2 relativePosition);
    }
}
