using UnityEngine;

namespace Base.Data.GameLogic.DevScripts
{
    [CreateAssetMenu(fileName = "New GameLogicDataTemplate", menuName = "SOMStudio/Tetris/Create Game logic Data Template")]
    public abstract class BaseLogicScriptTemplate : ScriptableObject
    {
        public abstract BaseLogicScript Script { get; }
    }
}
