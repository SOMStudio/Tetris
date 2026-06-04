using System;
using Base.Data.GameData.DevScripts;
using Base.Data.LevelListData.DevScripts;
using UnityEngine;

namespace Base.Data.GameLogic.DevScripts
{
    public abstract class BaseLogicScript
    {
        public abstract void InitGameRules(BaseGameController gameController, BaseDataTemplate gameData);

        public abstract void InitLevelRules(BaseLevelManager levelManager, BaseLevelData levelData = null);

        public abstract void CheckLocalTask(GameObject obj, EventArgs args = null);

        public abstract void CheckGlobalTask();
    }
}
