using UnityEngine;

namespace Base.Data.LevelListData.DevScripts
{
    public abstract class BaseLevelListDataTemplate : ScriptableObject
    {
        public abstract BaseLevelData[] Data { get; }
    }
}
