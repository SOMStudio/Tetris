using UnityEngine;

namespace Base.Data.LevelListData.DevScripts
{
    public abstract class BaseLevelData
    {
        [Header("Base")]
        [SerializeField]
        private string nameScene;

        public string NameScene { get; internal set; }

        public abstract string GetStringTask();
    }
}
