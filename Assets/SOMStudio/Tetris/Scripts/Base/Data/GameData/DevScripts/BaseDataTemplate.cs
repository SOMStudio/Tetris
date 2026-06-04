using UnityEngine;

namespace Base.Data.GameData.DevScripts
{
    public abstract class BaseDataTemplate : ScriptableObject
    {
        public abstract BaseData Data { get; }
    }
}
