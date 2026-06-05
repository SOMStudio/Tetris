using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Input.Interfaces
{
    public interface IMouseInputHandler
    {
        Vector2 GetRawPosition();
        Vector2 GetInput(Vector2 relativePosition);
    }
}
