using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Data.GameData.DevScripts
{
    public abstract class BaseDataTemplate : ScriptableObject
    {
        public abstract BaseData Data { get; }
    }
}
