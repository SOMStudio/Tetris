using UnityEngine;

namespace Base.Data.GameLogic.DevScripts
{
    [CreateAssetMenu(fileName = "New GameLogicDataTemplate", menuName = "SOMStudio/Drop&Catch Data/Create Game logic Data Template")]
    public abstract class BaseLogicScriptTemplate : ScriptableObject
    {
        public abstract BaseLogicScript Script { get; }
    }
}
