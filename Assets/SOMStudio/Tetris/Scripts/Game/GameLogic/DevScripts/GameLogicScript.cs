using System;
using SOMStudio.Tetris.Scripts.Base;
using SOMStudio.Tetris.Scripts.Base.Data.GameData.DevScripts;
using SOMStudio.Tetris.Scripts.Base.Data.GameLogic.DevScripts;
using SOMStudio.Tetris.Scripts.Base.Data.LevelListData.DevScripts;
using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Game.GameLogic.DevScripts
{
    [Serializable]
    public class GameLogicScript : BaseLogicScript
    {
        private SOMStudio.Tetris.Scripts.Game.GameData.DevScripts.GameData gameData;
        private LevelManager levelManager;

        private BaseGameController gameController;
        
        private int taskScore;

        public override void InitGameRules(BaseGameController gameControllerSet, BaseDataTemplate gameDataSet)
        {
            gameController = gameControllerSet;
            
            gameData = (SOMStudio.Tetris.Scripts.Game.GameData.DevScripts.GameData)gameDataSet.Data;
        }
        
        public override void InitLevelRules(BaseLevelManager levelManagerSet, BaseLevelData levelDataNew = null)
        {
            levelManager = (LevelManager)levelManagerSet;

            UserManager.Instance.SetHealth(1, true);
            UserManager.Instance.SetScore(0, true);
            UserManager.Instance.SetWave(1, true);
        }

        public override void CheckLocalTask(GameObject obj, EventArgs args = null)
        {
            EvenArgsReward argsReward = (EvenArgsReward) args;
            int bonusRes = CalculateBonusCount(argsReward.countLine);

            UserManager.Instance.AddScore(bonusRes);

            gameController.CheckLocalTask(bonusRes);
        }

        private int CalculateBonusCount(int countLineReduce)
        {
            return (countLineReduce + Mathf.FloorToInt(countLineReduce / 2)) * gameData.BonusForItem; ;
        }

        public override void CheckGlobalTask()
        {
        }
    }
}
