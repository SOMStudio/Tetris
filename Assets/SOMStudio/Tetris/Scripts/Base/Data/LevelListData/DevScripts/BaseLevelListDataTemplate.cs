using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Data.LevelListData.DevScripts
{
    public abstract class BaseLevelListDataTemplate : ScriptableObject
    {
        public abstract BaseLevelData[] Data { get; }
    }
}
