using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Data.GameLogic.DevScripts
{
    public abstract class BaseLogicScriptTemplate : ScriptableObject
    {
        public abstract BaseLogicScript Script { get; }
    }
}
